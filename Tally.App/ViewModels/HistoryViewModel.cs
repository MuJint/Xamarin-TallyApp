using System;
using System.Collections.Generic;
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
