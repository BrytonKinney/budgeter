using System;
using System.Collections.Generic;
using System.Text;

namespace Budgeter.FileParsers
{
    public interface IFileParser
    {
        System.Threading.Tasks.Task<IEnumerable<T>> ParseFileAsync<T>(string filePath) where T : new();
        System.Threading.Tasks.Task<IEnumerable<T>> ParseFileAsync<T>(System.IO.Stream stream) where T : new();
    }
}
