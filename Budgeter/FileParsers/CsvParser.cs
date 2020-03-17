using Budgeter.FileParsers.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Budgeter.FileParsers
{
    public class CsvParser : AbstractFileParser
    {
        protected override async IAsyncEnumerable<T> ReadFromStreamAsync<T>(System.IO.Stream stream)
        {
            var recordType = typeof(T);
            var properties = Serializer.GetProperties(recordType).ToDictionary(pi => pi.GetCustomAttribute<CsvColumnAttribute>().ColumnName);
            using (var sr = new StreamReader(stream))
            {
                if (sr.EndOfStream)
                    sr.BaseStream.Position = 0;
                int columnIter = 0;
                var headerLine = await sr.ReadLineAsync();
                var headerColumns = headerLine.Split("\",\"").Select(s => RemoveQuotations(s));
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    var lineValues = line.Split("\",\"").Select(s => RemoveQuotations(s));
                    var record = new T();
                    foreach (var column in headerColumns)
                    {
                        if (columnIter > lineValues.Count() - 1)
                            BindValue(record, properties[column], string.Empty);
                        else if (properties.ContainsKey(column))
                            BindValue(record, properties[column], lineValues.ElementAt(columnIter));
                        columnIter++;
                    }
                    columnIter = 0;
                    yield return record;
                }
            }
        }

        private void HandleEmptyRecord<T>(T record, PropertyInfo memberToBind)
        {
            if (memberToBind.PropertyType == typeof(string))
                memberToBind.SetValue(record, string.Empty);
            else if (memberToBind.PropertyType == typeof(DateTime))
                memberToBind.SetValue(record, DateTime.MinValue);
        }

        private void BindValue<T>(T record, PropertyInfo memberToBind, string value)
        {
            if (value == string.Empty)
                HandleEmptyRecord(record, memberToBind);
            else
                memberToBind.SetValue(record, Convert.ChangeType(value, memberToBind.PropertyType));
        }

        private string RemoveQuotations(string record)
        {
            if (record.StartsWith('"') || record.StartsWith('\''))
                record = record.Remove(0, 1);
            else if (record.StartsWith(",\"") || record.StartsWith(",'"))
                record = record.Remove(0, 2);
            if (record.EndsWith('"') || record.EndsWith('\''))
                record = record.Remove(record.Length - 1, 1);
            else if (record.EndsWith("\",") || record.EndsWith("',"))
                record = record.Remove(record.Length - 2, 2);
            return record;
        }
    }
}
