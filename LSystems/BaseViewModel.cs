using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LSystems.Base
{

    /// <summary>
    /// Base ViewModel class
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged, IRequestCloseViewModel
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);

            PropertyChangedEventHandler changed = PropertyChanged;

            if (changed != null)
                changed(this, e);
        }

        #endregion  // Implementation of INotifyPropertyChanged

        #region Implementation of IRequestCloseViewModel

        public event EventHandler RequestClose;

        protected void InvokeRequestClose()
        {
            EventHandler evnt = RequestClose;

            if (evnt != null)
                evnt(this, null);
        }

        #endregion // Implementation of IRequestCloseViewModel
    }
}
