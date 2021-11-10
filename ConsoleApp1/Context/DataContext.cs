using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Npgsql;
using LinqToDB.Mapping;
using System.Linq;
using ConsoleApp1.Entities;
using ConsoleApp1.Enums;
using LinqToDB.EntityFrameworkCore;
using LinqToDB;
using LinqToDB.DataProvider.PostgreSQL;

namespace ConsoleApp1.Context
{
    public class DataContext : DbContext
    {
        private readonly DbContextOptions options;

        public DataContext(DbContextOptions options) : base(options)
        {
            this.options = options;
        }

        public DbSet<Workitem> Workitems { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Edge> Edges { get; set; }
        
        static DataContext()
        {
            var nameTranslator = NpgsqlConnection.GlobalTypeMapper.DefaultNameTranslator;

            NpgsqlConnection.GlobalTypeMapper
                .MapEnum<EdgeType>();

            MappingSchema.Default.SetDefaultFromEnumType(typeof(EdgeType), typeof(string));
            MappingSchema.Default.SetConverter<EdgeType, string>(c => nameTranslator.TranslateMemberName(c.ToString()));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.HasPostgresEnum<EdgeType>();
            builder.HasPostgresExtension("uuid-ossp");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
            => builder.LogTo(Console.WriteLine);

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ModifyEntitiesBeforeSaveChanges();
            var res = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            return res;
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ModifyEntitiesBeforeSaveChanges();
            var res = await base.SaveChangesAsync(cancellationToken);
            return res;
        }

        public override int SaveChanges()
        {
            ModifyEntitiesBeforeSaveChanges();
            var res = base.SaveChanges();
            return res;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ModifyEntitiesBeforeSaveChanges();
            var res = base.SaveChanges(acceptAllChangesOnSuccess);
            return res;
        }

        public IQueryable<GraphPath> SearchGraph2(Node root, EdgeType[] types, int depth = 99999, int limit = 2000000000)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));

            var nameTranslator = NpgsqlConnection.GlobalTypeMapper.DefaultNameTranslator;
            var typesStr = string.Join("','", types.Select(t => nameTranslator.TranslateMemberName(t.ToString())));

            var sql = $"select * from search_graph('{root.OwnerId}', array['{typesStr}']::edge_type[], {depth}, {limit})";
            return this.CreateLinqToDbContext().FromSql<GraphPath>(sql);
        }

        
        public IQueryable<GraphPath> SearchGraph3(Node root, EdgeType[] types, int depth = 99999, int limit = 2000000000)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));

            var graphSte = this.CreateLinqToDbContext().GetCte<GraphPath>(search =>
                (
                    (from edge in Edges
                     where edge.FromId == root.OwnerId
                         && types.Contains(edge.Type)
                     select new GraphPath
                     {
                         FromId = edge.FromId,
                         Path = Sql.Ext.PostgreSQL().NewArray(edge.FromId),
                         ToId = edge.ToId,
                         Type = edge.Type,
                         Depth = 1
                     })
                    .OrderByDescending(e => e.Type)
                    .Take(limit)
                )
                .Concat
                (
                    (from edge in Edges
                     from sg in search
                     where edge.FromId == sg.ToId
                         && !Sql.Ext.PostgreSQL().ArrayContains(sg.Path, edge.FromId)
                         && types.Contains(edge.Type)
                         && sg.Depth <= depth
                     select new GraphPath
                     {
                         FromId = edge.FromId,
                         Path = Sql.Ext.PostgreSQL().ArrayAppend(sg.Path, edge.FromId),
                         ToId = edge.ToId,
                         Type = edge.Type,
                         Depth = sg.Depth + 1
                     })
                    .OrderByDescending(e => e.Type)
                    .Take(limit)
                )
            );

            return graphSte.Select(x => new GraphPath
            {
                Depth = x.Depth,
                FromId = x.FromId,
                Path = Sql.Ext.PostgreSQL().ArrayAppend(x.Path, x.ToId),
                Type = x.Type,
                ToId = x.ToId
            });
        }
        private void ModifyEntitiesBeforeSaveChanges()
        {
            var inverseEdgesToAdd = new List<Edge>();
            var inverseEdgesToRemove = new List<Edge>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Edge edge)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            var inversEdge = edge.CreateInverse();
                            if (Edges.Find(inversEdge.FromId, inversEdge.ToId, inversEdge.Type) is null)
                                inverseEdgesToAdd.Add(inversEdge);
                            break;
                        case EntityState.Deleted:
                            var inverseEdge = Edges.Find(edge.ToId, edge.FromId, edge.Type);
                            if (inverseEdge != null)
                                inverseEdgesToRemove.Add(inverseEdge);
                            break;
                        default: break;
                    }
                }
            }
            Edges.AddRange(inverseEdgesToAdd);
            Edges.RemoveRange(inverseEdgesToRemove);
        }
    }
}

