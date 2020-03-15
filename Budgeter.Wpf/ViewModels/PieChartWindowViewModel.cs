using Budgeter.Wpf.Entities;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.Wpf.ViewModels
{
    public class PieChartWindowViewModel : INotifyPropertyChanged
    {
        
        public PieChartWindowViewModel(List<TransactionRecord> transactionRecords)
        {
            PlotModel = new OxyPlot.PlotModel();
            TransactionRecords = transactionRecords;
            Task.Run(PlotData);
        }

        public Task PlotData()
        {
            var spentTotal = TransactionRecords.Sum(t => Math.Abs(t.Amount));
            TransactionSlices = from record in TransactionRecords group record by record.MccGroup into trxSlice select new PieSlice(trxSlice.Key, trxSlice.Sum(trx => Convert.ToDouble(trx.Amount)));
            PlotModel.Series.Add(new PieSeries() { ItemsSource = TransactionSlices, StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 });
            return Task.CompletedTask;
        }

        private Func<IGrouping<string, TransactionRecord>, decimal, double> GetPercentage = (trxSlice, total) =>
        {
            var percent = trxSlice.Sum(trx => Math.Abs(trx.Amount)) / total;
            return Convert.ToDouble(percent * 100);
        };
        public OxyPlot.PlotModel PlotModel { get; set; }
        public IEnumerable<PieSlice> TransactionSlices { get; set; }
        public List<TransactionRecord> TransactionRecords { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
