using LinqToDB.Linq;
using LinqToDB.SqlQuery;

namespace LinqToDB.DataProvider.PostgreSQL
{
    public static class PostgreSQLExtensions
    {
        [Sql.Extension("{item} <> ALL({array})", ServerSideOnly = true, CanBeNull = false, Precedence = Precedence.Additive)]
        public static bool ArrayContains<T>(this IPostgreSQLExtensions? ext, [ExprParameter] T[] array, [ExprParameter] T item)
        {
            throw new LinqException($"'{nameof(ArrayContains)}' is server-side method.");
        }
        [Sql.Extension("{array} || {item}", ServerSideOnly = true, CanBeNull = false, Precedence = Precedence.Additive)]
        public static T[] ArrayAppendItem<T>(this IPostgreSQLExtensions? ext, [ExprParameter] T[] array, [ExprParameter] T item)
        {
            throw new LinqException($"'{nameof(ArrayAppendItem)}' is server-side method.");
        }
        [Sql.Extension("ARRAY[{item}]", ServerSideOnly = true, CanBeNull = false, Precedence = Precedence.Additive)]
        public static T[] NewArray<T>(this IPostgreSQLExtensions? ext, [ExprParameter] T item)
        {
            throw new LinqException($"'{nameof(NewArray)}' is server-side method.");
        }
    }
}
