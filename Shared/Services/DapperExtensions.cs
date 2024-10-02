using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Shared.Services
{
    public static class DapperExtensions
    {
        public static DataTable AsTableValuedParameter<T>(this IEnumerable<T> enumerable, string typeName)
        {
            var dataTable = new DataTable();
            dataTable.TableName = typeName;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties) {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(
            property.PropertyType) ?? property.PropertyType);
            }
            foreach (var item in enumerable) {
                var row = dataTable.NewRow();
                foreach (var property in properties) {
                    var val = property.GetValue(item);
                    row[property.Name] = val;
                }
                dataTable.Rows.Add(row);
            }
            var parameter = new SqlParameter("@" + typeName, SqlDbType.Structured);
            parameter.TypeName = typeName;
            parameter.Value = dataTable;
            return dataTable;
        }
    }
}
