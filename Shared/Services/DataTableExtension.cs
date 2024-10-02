using Shared.OtherModels.DataService;
using System.Data;

namespace Shared.Services
{
    public static class DataTableExtension
    {
        public static void RemoveUnwantedColumns(this DataTable dataTable, List<string> columnsToKeep)
        {
            List<DataColumn> columnsToRemove = new List<DataColumn>();

            // Iterate through the columns in the DataTable
            foreach (DataColumn column in dataTable.Columns) {
                // Check if the column is in the list of columns to keep
                if (!columnsToKeep.Contains(column.ColumnName)) {
                    // If not, add it to the list of columns to remove
                    columnsToRemove.Add(column);
                }
            }

            // Remove the columns that are not in the columns to keep list
            foreach (DataColumn column in columnsToRemove) {
                dataTable.Columns.Remove(column);
            }
        }
        public static void RemoveUnwantedColumns(this DataTable dataTable, List<KeyValue> columnsToKeepInKeyValuePairs, bool setValueAsName = false)
        {
            List<DataColumn> columnsToRemove = new List<DataColumn>();
            var columnsToKeep = columnsToKeepInKeyValuePairs.Select(item=> item.Key).ToArray();

            // Iterate through the columns in the DataTable
            foreach (DataColumn column in dataTable.Columns) {
                // Check if the column is in the list of columns to keep
                if (!columnsToKeep.Contains(column.ColumnName)) {
                    // If not, add it to the list of columns to remove
                    columnsToRemove.Add(column);
                }
            }

            // Remove the columns that are not in the columns to keep list
            foreach (DataColumn column in columnsToRemove) {
                dataTable.Columns.Remove(column);
            }

            if (setValueAsName) {
                foreach (DataColumn column in dataTable.Columns) {
                    var columnDisplayName = columnsToKeepInKeyValuePairs.ToList().FirstOrDefault(item => item.Key == column.ColumnName).Value;
                    if (columnDisplayName != null) {
                        column.ColumnName = columnDisplayName;
                    }
                }
            }
        }
        public static DataTable AddColumns(string[] columns)
        {
            DataTable dataTable = new DataTable();

            foreach (var column in columns)
            {
                dataTable.Columns.Add(column);
            }

            return dataTable;
        }

        public static DataTable AddColumns(this DataTable dataTable,string[] columns, int addRow=0)
        {
            foreach (var column in columns)
            {
                dataTable.Columns.Add(column);
            }

            if(addRow > 0)
            {
                for (int i = 0; i < addRow; i++)
                {
                    DataRow dr = dataTable.NewRow();
                    for (int j = 0; j < columns.Length; j++)
                    {
                        dr[j] = "";
                    }
                    dataTable.Rows.Add(dr);
                }
            }
            return dataTable;
        }
    }
}
