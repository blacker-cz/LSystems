using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSystems.Base
{
    /// <summary>
    /// url: http://www.japf.fr/2009/09/how-to-close-a-view-from-a-viewmodel/
    /// </summary>
    public interface IRequestCloseViewModel
    {
        event EventHandler RequestClose;
    }
}
