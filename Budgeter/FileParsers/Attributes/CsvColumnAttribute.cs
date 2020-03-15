using System;
using System.Collections.Generic;
using System.Text;

namespace Budgeter.FileParsers.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvColumnAttribute : Attribute
    {
        public CsvColumnAttribute(string columnName, string displayName = "")
        {
            ColumnName = columnName;
            if (!string.IsNullOrEmpty(displayName))
                DisplayName = displayName;
            else
                DisplayName = columnName;
        }

        public CsvColumnAttribute(int index, string displayName = "")
        {
            Index = index;
            DisplayName = displayName;
        }

        public string ColumnName { get; }
        public int Index { get; }
        public string DisplayName { get; }
    }
}
