using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LSystems.Backend
{
    /// <summary>
    /// Polyline wrapper class
    /// </summary>
    public class Polyline : IDrawable
    {
        /// <summary>
        /// List of points for polyline
        /// </summary>
        public List<PointF> Points { get; private set; }

        /// <summary>
        /// Polyline pen
        /// </summary>
        public Pen Pen { get; set; }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="pen">Polyline pen</param>
        /// <param name="points">Polyline points (optional)</param>
        public Polyline(Pen pen = null, List<PointF> points = null)
        {
            if (pen != null)
                Pen = pen;
            else
                Pen = new Pen(Color.Black);

            if (points != null)
                Points = points;
            else
                Points = new List<PointF>();
        }

        /// <summary>
        /// Add next point to polyline
        /// </summary>
        /// <param name="point">Point</param>
        public void Add(PointF point)
        {
            Points.Add(point);
        }

        /// <summary>
        /// Paint object to given graphics
        /// </summary>
        /// <param name="graphics">Graphics instance</param>
        public void Paint(Graphics graphics)
        {
            graphics.DrawLines(Pen, Points.ToArray());
        }

        /// <summary>
        /// Destructor.
        /// Dispose of graphics objects (Pen)
        /// </summary>
        ~Polyline()
        {
            if (Pen != null)
                Pen.Dispose();
        }
    }
}
