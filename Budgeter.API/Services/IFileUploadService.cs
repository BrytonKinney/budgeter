using Budgeter.API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.API.Services
{
    public interface IFileUploadService
    {
        Task<IEnumerable<TransactionRecord>> ParseSpreadsheetAsync(IFormFile file);
    }
}
