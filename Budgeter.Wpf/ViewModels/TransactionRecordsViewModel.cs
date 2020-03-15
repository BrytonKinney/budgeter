using Budgeter.FileParsers;
using Budgeter.Wpf.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeter.Wpf.ViewModels
{
    public class TransactionRecordsViewModel : INotifyPropertyChanged
    {
        private IFileParser _fileParser;
        private List<TransactionRecord> _trxRecords;
        private bool _isBusy;

        public TransactionRecordsViewModel(IFileParser fileParser)
        {
            _fileParser = fileParser;
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }
        public List<TransactionRecord> TransactionRecordsList
        {
            get => _trxRecords; 
            set
            {
                _trxRecords = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransactionRecordsList)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public async Task GetRecordsAsync(string filePath)
        {
            var recordsTask = _fileParser.ParseFileAsync<TransactionRecord>(filePath);
            IsBusy = true;
            TransactionRecordsList = (await recordsTask).ToList();
            IsBusy = false;
        }
    }
}
