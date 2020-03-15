using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Budgeter.FileParsers.Attributes;

namespace Budgeter.FileParsers
{
    public class CsvParser : AbstractFileParser
    {
        public override async Task<IEnumerable<T>> ParseFileAsync<T>(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                return await YieldFromStreamAsync<T>(fs);
            }
        }

        public override async Task<IEnumerable<T>> ParseFileAsync<T>(Stream stream)
        {
            return await YieldFromStreamAsync<T>(stream);
        }


        private async Task<IEnumerable<T>> YieldFromStreamAsync<T>(System.IO.Stream stream) where T : new()
        {
            var recordResults = new List<T>(128);
            await foreach (var result in ReadFromStreamAsync<T>(stream))
            {
                recordResults.Add(result);
            }
            return recordResults;
        }
        private async IAsyncEnumerable<T> ReadFromStreamAsync<T>(System.IO.Stream stream) where T : new()
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
#if DEBUG
                int recordIter = 0;
#endif
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
#if DEBUG
                    recordIter++;
#endif
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
