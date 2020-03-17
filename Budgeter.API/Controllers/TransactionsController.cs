using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Budgeter.API.Models;
using Budgeter.API.Requests;
using Budgeter.API.Responses;
using Budgeter.API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Budgeter.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly IDataService _dataService;
        private readonly IFileUploadService _fileUploadService;
        public TransactionsController(ILogger<TransactionsController> logger, IDataService dataService, IFileUploadService fileUploadService)
        {
            _logger = logger;
            _dataService = dataService;
            _fileUploadService = fileUploadService;
        }

        [EnableCors]
        [HttpGet]
        [Route("expenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var allExpenses = _dataService.GetAllExpensesAsync();
            return Ok(await allExpenses);
        }

        [EnableCors]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PagedTransactionRequest request)
        {
            var response = _dataService.GetTransactionsAsync(request);
            var count = _dataService.GetTotalTransactionCountAsync(request.IgnorePayments);
            var trxResp = new TransactionsResponse() { Data = await response, Total = await count, Skip = request.Skip, Take = request.Take };
            return Ok(trxResp);
        }

        [EnableCors]
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile transactions)
        {
            var results = await _fileUploadService.ParseSpreadsheetAsync(transactions);
            await _dataService.UploadTransactions(results);
            return Ok();
        }
    }
}
