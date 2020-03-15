using Budgeter.API.Models;
using Budgeter.FileParsers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.API.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileParser _fileParser;
        private const string UPLOAD_DIRECTORY = "../data/";
        public FileUploadService(IFileParser fileParser)
        {
            _fileParser = fileParser;
        }

        public async Task<IEnumerable<TransactionRecord>> ParseSpreadsheetAsync(IFormFile upload)
        {
            var fileStream = upload.OpenReadStream();
            using(var localFs = File.Create(Path.Combine(UPLOAD_DIRECTORY, Path.GetRandomFileName())))
            {
                await fileStream.CopyToAsync(localFs);
                return await _fileParser.ParseFileAsync<TransactionRecord>(fileStream);
            }
        }
    }
}
