using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LSystems.Base
{
    /// <summary>
    /// url: http://www.japf.fr/2009/09/how-to-close-a-view-from-a-viewmodel/
    /// </summary>
    public class ApplicationWindowBase : Window
    {
        public ApplicationWindowBase()
        {
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.OnDataContextChanged);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IRequestCloseViewModel)
            {
                // if the new datacontext supports the IRequestCloseViewModel we can use
                // the event to be notified when the associated viewmodel wants to close
                // the window
                ((IRequestCloseViewModel)e.NewValue).RequestClose += (s, ev) => this.Close();
            }
        }
    }
}
