using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace LSystems.Backend
{
    /// <summary>
    /// Class for rendering IDrawable elements
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// Underlying bitmap
        /// </summary>
        private Bitmap _bitmap;

        /// <summary>
        /// Preview bitmap
        /// </summary>
        private Bitmap _preview;

        /// <summary>
        /// Internal flag for regenerating preview image
        /// </summary>
        private bool _valid;

        /// <summary>
        /// Rendered image width
        /// </summary>
        public const int WIDTH = 1600;

        /// <summary>
        /// Rendered image height
        /// </summary>
        public const int HEIGHT = 1200;

        /// <summary>
        /// Preview image width
        /// </summary>
        public const int PREVIEW_WIDTH = 480;

        /// <summary>
        /// Preview image height
        /// </summary>
        public const int PREVIEW_HEIGHT = 360;

        /// <summary>
        /// Constructor
        /// </summary>
        public Renderer()
        {
            _bitmap = new Bitmap(WIDTH, HEIGHT, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _preview = new Bitmap(PREVIEW_WIDTH, PREVIEW_HEIGHT, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Clear(Color.White);

            _valid = false;
        }

        /// <summary>
        /// Getter for preview image
        /// </summary>
        public Image Preview
        {
            get
            {
                if (!_valid)
                {
                    using (Graphics g = Graphics.FromImage(_preview))
                    {
                        g.DrawImage(_bitmap, 0, 0, PREVIEW_WIDTH, PREVIEW_HEIGHT);
                        g.Dispose();
                    }
                    _valid = true;
                }

                return _preview;
            }
        }

        /// <summary>
        /// Clear bitmap
        /// </summary>
        /// <param name="background">Background color</param>
        public void Clear(Color background)
        {
            Graphics graphics = Graphics.FromImage(_bitmap);

            graphics.Clear(background);
            
            graphics.Dispose();

            _valid = false; // force revalidation for preview
        }

        /// <summary>
        /// Paint all drawables
        /// </summary>
        /// <param name="drawables">List of drawable elements</param>
        public void Paint(List<IDrawable> drawables)
        {
            Graphics graphics = Graphics.FromImage(_bitmap);

            // transform coordinate system
            Matrix m = new Matrix(1, 0, 0, -1, 0, 0);
            m.Translate(0, HEIGHT, MatrixOrder.Append);
            graphics.Transform = m;

            // set smoothing mode to antialiasing
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (IDrawable item in drawables)
            {
                item.Paint(graphics);
            }

            m.Dispose();
            graphics.Dispose();

            _valid = false; // force revalidation for preview
        }

        /// <summary>
        /// Save rendered image to file
        /// </summary>
        /// <param name="filename">Path</param>
        public void Save(string filename)
        {
            _bitmap.Save(filename);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Renderer()
        {
            _bitmap.Dispose();
        }
    }
}
