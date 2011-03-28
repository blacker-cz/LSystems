using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LSystems.Backend
{
    /// <summary>
    /// Public interface for drawable objects (Polyline, etc.)
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Paint object to given graphics
        /// </summary>
        /// <param name="graphics">Graphics instance</param>
        void Paint(Graphics graphics);
    }
}
