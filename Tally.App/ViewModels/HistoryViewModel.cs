using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Tally.App.Models;
using Tally.App.ViewModel;

namespace Tally.App.ViewModels
{
    public class HistoryViewModel : NotifyPropertyChanged
    {
        public HistoryViewModel()
        {

        }

        #region Identity

        #endregion

        #region Property

        public ObservableCollection<ExpenseRecord> ExpenseRecordsDetails { get; set; }=new ObservableCollection<ExpenseRecord>();

        private History history;
        public History History
        {
            get => history;
            set => SetProperty(ref history, value);
        }
        #endregion

        #region Method

        #endregion
    }
}
