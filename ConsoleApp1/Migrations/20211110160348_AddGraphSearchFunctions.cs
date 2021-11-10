using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp1.Migrations
{
    public partial class AddGraphSearchFunctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
migrationBuilder.Sql(@"
create or replace function search_graph(IN ""root_id"" uuid,
                                        IN ""types"" edge_type[] default array[]::edge_type[],
                                        IN ""depth"" int default 99999,
                                        IN ""limit"" int8 default 2000000000,
                                        out ""Path"" uuid[],
                                        out ""FromId"" uuid,
                                        out ""ToId"" uuid,
                                        out ""Type"" edge_type,
                                        out ""Depth"" int)
    returns setof record as
$$
declare
    sql text;
begin
    sql := format($_$ WITH RECURSIVE search( ""FromId"", ""ToId"", ""Type"", ""Depth"", ""Path"" ) AS
   (
      select
         ""FromId"",
         ""ToId"",
         ""Type"",
         ""Depth"",
         ""Path""
      from
         (
            SELECT
               g.""FromId"",
               g.""ToId"",
               g.""Type"",
               1 ""Depth"",
               ARRAY[g.""FromId""] ""Path""
            FROM
               ""Edges"" AS g
            WHERE
               g.""FromId"" = '%s'::uuid
               AND g.""Type""=any(string_to_array('%s', ',')::edge_type[])
            ORDER BY
               g.""Type"" desc limit %s
         )
         t
      UNION ALL
      select
         ""FromId"",
         ""ToId"",
         ""Type"",
         ""Depth"",
         ""Path""
      from
         (
            SELECT
               g.""FromId"",
               g.""ToId"",
               g.""Type"",
               sg.""Depth"" + 1 ""Depth"",
               ""Path"" || g.""FromId"" ""Path""
            FROM
               ""Edges"" AS g,
               search AS sg
            WHERE
               g.""FromId"" = sg.""ToId""
               AND
               (
                  g.""FromId"" <> ALL(sg.""Path"")
               )
               AND sg.""Depth"" <= %s
               AND g.""Type""=any(string_to_array('%s', ',')::edge_type[])
            ORDER BY
               g.""Type"" desc limit %s
         )
         t
   )
   SELECT
      ""Path"" || ""ToId"", ""FromId"", ""ToId"", ""Type"", ""Depth""
   FROM
      search;
$_$, ""root_id"", array_to_string(""types"", ','), ""limit"", ""depth"", array_to_string(""types"", ','), ""limit"");
    return query execute sql;
end;
$$ language plpgsql strict;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
