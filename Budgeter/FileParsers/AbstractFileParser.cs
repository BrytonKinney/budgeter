using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Budgeter.FileParsers
{
    public abstract class AbstractFileParser : IFileParser
    {
        public abstract Task<IEnumerable<T>> ParseFileAsync<T>(string filePath) where T : new();
        public abstract Task<IEnumerable<T>> ParseFileAsync<T>(System.IO.Stream stream) where T : new();

        protected T CreateInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
