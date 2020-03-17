using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Budgeter.FileParsers
{
    public abstract class AbstractFileParser : IFileParser
    {
        #region Public functions

        /// <summary>
        /// Parse a file from a file path and return an <see cref="IEnumerable{T}"/> containing the results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ParseFileAsync<T>(string filePath) where T : new()
        {
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                return await YieldFromStreamAsync<T>(fs);
            }
        }

        /// <summary>
        /// Parse a file from a stream and return an <see cref="IEnumerable{T}"/> containing the results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ParseFileAsync<T>(Stream stream) where T : new()
        {
            return await YieldFromStreamAsync<T>(stream);
        }

        #endregion

        #region Default implementations

        /// <summary>
        /// Lazy enumeration yielding result sets from the stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<T>> YieldFromStreamAsync<T>(System.IO.Stream stream) where T : new()
        {
            var recordResults = new List<T>(128);
            await foreach (var result in ReadFromStreamAsync<T>(stream))
            {
                recordResults.Add(result);
            }
            return recordResults;
        }
        
        #endregion

        #region Abstract functions

        /// <summary>
        /// Read from the supplied stream and return <see cref="IAsyncEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected abstract IAsyncEnumerable<T> ReadFromStreamAsync<T>(System.IO.Stream stream) where T : new();

        #endregion
    }
}
