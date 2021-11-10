using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Context;
using ConsoleApp1.Entities;
using ConsoleApp1.Enums;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ConsoleApp1
{
    class Program
    {
        static async  Task Main(string[] args)
        {
            LinqToDBForEFTools.Initialize();
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=127.0.0.1;Port=5432;Database=test;User Id=postgres;Password=LtdjxrfCGthcbrfvb;CommandTimeout=30;", b =>
                {
                    b.MigrationsAssembly(typeof(DataContext).Assembly.FullName);
                    b.UseNodaTime();
                    b.SetPostgresVersion(13, 2);
                });

            var context = new DataContext(optionsBuilder.Options);
            await context.Database.MigrateAsync().ConfigureAwait(false);

            var conn = (NpgsqlConnection)context.Database.GetDbConnection();
            conn.Open();
            conn.ReloadTypes();
            
            var wi1 = new Workitem()
            {
                Caption = "wi1",
            };

            var wi2 = new Workitem()
            {
                Caption = "w2",
                Node = new WorkitemNode()
                {
                    Edges = new List<Edge>()
                    {
                        new Edge()
                        {
                            To = wi1.Node,
                            Type = ConsoleApp1.Enums.EdgeType.Parrent
                        },
                    }
                }
            };
            
            context.Workitems.AddRange(new []{wi1, wi2});
            await context.SaveChangesAsync();
            
            
            var types = new[] { EdgeType.Parrent, EdgeType.Relation };
            var query = from node in context.Nodes
                let graph = context.SearchGraph3(node, types, 10, 10)
                where graph.Any()
                select new {node, graph = graph.ToList()};

            var list = await query.ToLinqToDB().ToListAsync();
        }
    }
}