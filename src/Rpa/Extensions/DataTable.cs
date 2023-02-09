using Rpa.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rpa.Extensions
{
    public static class DataTableExtensions
    {
        #region Methods - Public

        public static DataTable ConvertColumns(this DataTable dt, Dictionary<string, Type> dataTableColumns)
        {
            DataTable dtCloned = dt.Clone();

            foreach (var dataTableColumn in dataTableColumns)
            {
                dtCloned.Columns[dataTableColumn.Key].DataType = dataTableColumn.Value;
            }

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    dtCloned.ImportRow(row);
                }
                catch (ArgumentException)
                {
                    foreach (var dataTableColumn in dataTableColumns)
                    {
                        dynamic changedObj = Convert.ChangeType(dataTableColumn.Value.GetDefault(), dataTableColumn.Value);
                        row[dataTableColumn.Key] = changedObj;
                    }
                    dtCloned.ImportRow(row);
                }
            }

            return dtCloned;
        }

        public static DataTable AppendTable(this DataTable dtSource, DataTable dtTarget)
        {
            foreach (DataColumn column in dtSource.Columns)
            {
                if (!dtTarget.Columns.Contains(column.ColumnName))
                    throw new Exception($"Column '{column}' does not in the given DataTable columns!");

                var givenColumn = dtTarget.Columns[column.ColumnName];
                if (givenColumn.DataType != column.DataType)
                    throw new Exception($"Column '{column}' has type of '{column.DataType.FullName}', but given column type is '{givenColumn.DataType.FullName}'!");
            }

            dtSource.Merge(dtTarget);

            return dtSource;
        }
        public static bool IsAnyColumnExist(this DataTable dt, params string[] columnsNames)
        {
            return dt.IsAnyColumnExist(columnsNames.ToList());
        }
        public static bool IsAnyColumnExist(this DataTable dt, List<string> columnsNames)
        {
            foreach (string columnName in columnsNames)
            {
                if (dt.Columns.Contains(columnName))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<T> ToClass<T>(this DataTable dt) where T : new()
        {
            var result = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                result.Add(row.DataRowToClass<T>());
            }

            return result;
        }
        public static T ToObject<T>(this DataRow row) where T : class
        {
            var result = Activator.CreateInstance<T>();

            foreach (PropertyInfo propertyInfo in result.GetType().GetProperties())
            {
                var isIgnored = propertyInfo.GetCustomAttribute<IgnoreColumn>() != null;

                if (propertyInfo.CanWrite && !isIgnored)
                {
                    var columnName = propertyInfo.Name;
                    foreach (DataColumn tableColumn in row.Table.Columns)
                    {
                        if (tableColumn.ColumnName == columnName)
                        {
                            propertyInfo.SetValue(result,
                                propertyInfo.PropertyType.IsSimpleType()
                                    ? row[tableColumn.ColumnName]
                                    : row[tableColumn.ColumnName].ToString().FromJson(propertyInfo.PropertyType));//If not primitive we will json it
                        }
                    }
                }
            }

            return result;
        }
        public static DataRow ToDataRow<T>(this T obj, DataTable ofDataTable) where T : class
        {
            var result = ofDataTable.NewRow();

            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                var isIgnored = propertyInfo.GetCustomAttribute<IgnoreColumn>() != null;

                if (propertyInfo.CanWrite && !isIgnored)
                {
                    var columnName = propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name ?? propertyInfo.Name;
                    var val = propertyInfo.GetValue(obj);

                    result[columnName] =
                        propertyInfo.PropertyType.IsSimpleType()
                            ? val
                            : val.ToJson(); //If not primitive we will json it
                }
            }

            return result;
        }
        public static List<T> ToList<T>(this DataTable dt) where T : class
        {
            var result = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                result.Add(row.ToObject<T>());
            }

            return result;
        }

        public static DataTable ToDataTable<T>(this IList<T> list, string tableName = "Table1", params string[] excludedColumns) where T : class
        {
            return list.AsEnumerable().ToDataTable(tableName, excludedColumns);
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, string tableName = "Table1", params string[] excludedColumns) where T : class
        {
            list = list ?? new List<T>();

            var result = !list.Any()
                ? Activator.CreateInstance<T>().InitDataTable(tableName)
                : list.First().InitDataTable(tableName, excludedColumns);

            foreach (var obj in list)
            {
                var newRow = result.NewRow();

                foreach (PropertyInfo propertyInfo in list.First().GetType().GetProperties())
                {
                    var isIgnored = propertyInfo.GetCustomAttribute<IgnoreColumn>() != null;

                    if (propertyInfo.CanRead && !isIgnored
                                             && (excludedColumns == null || excludedColumns.All(c => c != propertyInfo.Name)))
                    {
                        var columnName = propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name ?? propertyInfo.Name;
                        var val = propertyInfo.GetValue(obj);

                        if (val != null)
                        {
                            if (val.GetType().IsSimpleType())
                            {
                                newRow[columnName] = val;
                            }
                            else
                            {
                                newRow[columnName] = val.ToJson();
                            }
                        }
                    }
                }

                result.Rows.Add(newRow);
            }

            return result;
        }

        #endregion

        #region Methods - Private

        private static DataTable InitDataTable<T>(this T item, string tableName = "Table1", params string[] excludedColumns)
            where T : class
        {
            if (typeof(T) == typeof(DataRow))
                throw new Exception($"{typeof(T).FullName} cannot be used with this method! You must use CopyToDataTable instead.");

            if (item == null)
                return new DataTable(tableName);

            var result = new DataTable(tableName);
            foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
            {
                var isIgnored = propertyInfo.GetCustomAttribute<IgnoreColumn>() != null;

                if (propertyInfo.CanRead && !isIgnored
                                         && (excludedColumns == null ||
                                             excludedColumns.All(c => c != propertyInfo.Name)))
                {
                    var columnName = propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name ?? propertyInfo.Name;

                    result.Columns.Add(columnName,
                        propertyInfo.PropertyType.IsSimpleType()
                            ? propertyInfo.PropertyType
                            : typeof(string)); //If not primitive we will json it
                }
            }

            return result;
        }
        private static T DataRowToClass<T>(this DataRow row) where T : new()
        {
            // create a new object
            var item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        private static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                try
                {
                    // find the property for the column
                    PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                    // if exists, set the value
                    if (p != null && row[c] != DBNull.Value && p.CanWrite)
                    {
                        if (p.PropertyType.IsEnum)
                        {
                            p.SetValue(item, Enum.Parse(p.PropertyType, row[c].ToString()));
                        }
                        //else if (p.PropertyType == typeof(DateTime))
                        //{
                        //    p.SetValue(item, DateTime.Parse(row[c].ToString(), CultureInfo.CurrentCulture));
                        //}
                        else
                        {
                            p.SetValue(item, Convert.ChangeType(row[c], p.PropertyType), null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"ColumnName: {c.ColumnName} | Value: {row[c]}", ex);
                }
            }
        }

        #endregion
    }
}