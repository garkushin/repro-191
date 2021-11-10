using System;
using ConsoleApp1.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

namespace ConsoleApp1.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:edge_type", "relation,parrent,children,like,liked_by,viewed,viewed_by")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "GraphPath",
                columns: table => new
                {
                    Path = table.Column<Guid[]>(type: "uuid[]", nullable: true),
                    FromId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<EdgeType>(type: "edge_type", nullable: false),
                    Depth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Edges",
                columns: table => new
                {
                    FromId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<EdgeType>(type: "edge_type", nullable: false),
                    CreatedDate = table.Column<Instant>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edges", x => new { x.FromId, x.ToId, x.Type });
                    table.ForeignKey(
                        name: "FK_Edges_Nodes_FromId",
                        column: x => x.FromId,
                        principalTable: "Nodes",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Edges_Nodes_ToId",
                        column: x => x.ToId,
                        principalTable: "Nodes",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workitems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Caption = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<Instant>(type: "timestamp", nullable: false),
                    UpdatedDate = table.Column<Instant>(type: "timestamp", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workitems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workitems_Nodes_Id",
                        column: x => x.Id,
                        principalTable: "Nodes",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Edges_ToId",
                table: "Edges",
                column: "ToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Edges");

            migrationBuilder.DropTable(
                name: "GraphPath");

            migrationBuilder.DropTable(
                name: "Workitems");

            migrationBuilder.DropTable(
                name: "Nodes");
        }
    }
}
