using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LSystems.Backend
{
    /// <summary>
    /// Class for saving system state
    /// </summary>
    class State
    {
        /// <summary>
        /// Turtle instance
        /// </summary>
        public Turtle Turtle { get; private set; }

        /// <summary>
        /// Drawing pen
        /// </summary>
        public Pen Pen { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="turtle">Turtle instance</param>
        /// <param name="pen">Drawing pen (optional)</param>
        public State(Turtle turtle, Pen pen = null)
        {
            Turtle = turtle;

            if (pen != null)
                Pen = pen;
            else
                Pen = new Pen(Color.Black);
        }
    }
}
