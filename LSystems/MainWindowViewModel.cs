using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Input;
using LSystems.Base;
using LSystems.Backend;

namespace LSystems
{
    /// <summary>
    /// ViewModel for MainWindow
    /// </summary>
    class MainWindowViewModel : BaseViewModel, IDataErrorInfo
    {
        /// <summary>
        /// Renderer instance
        /// </summary>
        private Renderer _renderer;

        /// <summary>
        /// Starting position on X axis
        /// </summary>
        public int StartX { get; set; }

        /// <summary>
        /// Starting position on Y axis
        /// </summary>
        public int StartY { get; set; }

        /// <summary>
        /// Starting angle
        /// </summary>
        public float StartAngle { get; set; }

        /// <summary>
        /// Axiom
        /// </summary>
        public string Axiom { get; set; }

        /// <summary>
        /// Rules
        /// </summary>
        public string Rules { get; set; }

        /// <summary>
        /// Number of iterations
        /// </summary>
        public uint Iterations { get; set; }

        /// <summary>
        /// Degree used for turning left/right
        /// </summary>
        public float Delta { get; set; }

        /// <summary>
        /// Step size
        /// </summary>
        public float StepSize { get; set; }

        /// <summary>
        /// Step size randomization
        /// </summary>
        public float StepDelta { get; set; }

        /// <summary>
        /// Step angle randomization
        /// </summary>
        public float AngleDelta { get; set; }

        /// <summary>
        /// Randomization seed
        /// </summary>
        public int Seed { get; set; }

        /// <summary>
        /// Generated image
        /// </summary>
        public Image Image { get { return _renderer.Preview; } }

        /// <summary>
        /// Command handlers
        /// </summary>
        private BaseCommand _generateCommand,
            _saveImageCommand,
            _closeCommand,
            _randomSeedCommand;

        /// <summary>
        /// Generate L-System command binding
        /// </summary>
        public ICommand GenerateCommand { get { return _generateCommand ?? (_generateCommand = new GenerateCommand(this)); } }

        /// <summary>
        /// Save Image command binding
        /// </summary>
        public ICommand SaveImageCommand { get { return _saveImageCommand ?? (_saveImageCommand = new SaveImageCommand(this)); } }

        /// <summary>
        /// Close application command binding
        /// </summary>
        public ICommand CloseCommand { get { return _closeCommand ?? (_closeCommand = new CloseCommand(this)); } }

        /// <summary>
        /// Generate new random seed command binding
        /// </summary>
        public ICommand RandomSeedCommand { get { return _randomSeedCommand ?? (_randomSeedCommand = new RandomSeedCommand(this)); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _renderer = new Renderer();
            _renderer.Clear(Color.White);
            Axiom = "FX";
            Rules = "X=X+YF\nY=FX-Y";
            StartX = 100;
            StartY = 100;
            Iterations = 5;
            Delta = 90.0f;
            StepSize = 10.0f;
        }

        #region Command handlers implementation

        /// <summary>
        /// Close application
        /// </summary>
        public void Close()
        {
            InvokeRequestClose();
        }

        /// <summary>
        /// Generate image (L-System)
        /// </summary>
        public void GenerateImage()
        {
            Backend.Parser parser = new Backend.Parser(Axiom, Rules, Iterations);
            string expandedGrammar = "";

            try
            {
                expandedGrammar = parser.Expand();
            }
            catch (OutOfMemoryException)
            {
                System.Windows.MessageBox.Show("Not enough memory, try decrease number of iterations.");
            }

            Backend.LSystem lsystem = new Backend.LSystem(expandedGrammar,
                new PointF(StartX * ((float)Renderer.WIDTH / Renderer.PREVIEW_WIDTH), StartY * ((float)Renderer.HEIGHT / Renderer.PREVIEW_HEIGHT)),
                (float)(StartAngle / 180 * Math.PI),
                StepSize,
                (float)(Delta / 180 * Math.PI),
                (AngleDelta / 100.0f),
                (StepDelta / 100.0f),
                Seed);

            List<Backend.IDrawable> polylist = lsystem.Generate();

            _renderer.Clear(Color.White);
            _renderer.Paint(polylist);

            // cleanup
            polylist.Clear();

            // update gui
            InvokePropertyChanged("Image");
        }

        /// <summary>
        /// Save generated image to file
        /// </summary>
        public void SaveImage()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "image"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "Image Files(*.png;*.bmp;*.jpg;*.jpeg;*.gif)|*.png;*.bmp;*.jpg;*.jpeg;*.gif|All files (*.*)|*.*"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                _renderer.Save(filename);
            }
        }

        /// <summary>
        /// Randomize seed value
        /// </summary>
        public void RandomizeSeed()
        {
            Random rnd = new Random(Environment.TickCount);
            Seed = rnd.Next();
            InvokePropertyChanged("Seed");
        }

        /// <summary>
        /// Change starting position
        /// </summary>
        /// <param name="point">Position</param>
        public void ChangePosition(System.Windows.Point point)
        {
            StartX = (int) point.X;
            StartY = Renderer.PREVIEW_HEIGHT - (int) point.Y;

            InvokePropertyChanged("StartX");
            InvokePropertyChanged("StartY");
        }

        #endregion // Command handlers implementation

        /// <summary>
        /// Validation of view fields
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Error message or null</returns>
        protected string Validate(string columnName)
        {
            switch (columnName)
            {
                case "StartX":
                    if (StartX < 0 || StartX > Renderer.PREVIEW_WIDTH)
                        return "Starting position on axis X must be between 0 and 480";
                    break;
                case "StartY":
                    if (StartY < 0 || StartY > Renderer.PREVIEW_HEIGHT)
                        return "Starting position on axis Y must be between 0 and 360";
                    break;
                case "StartAngle":
                    if (StartX < 0 || StartX >= 360)
                        return "Starting angle must be between 0 and 359";
                    break;
                case "Iterations":
                    if (Iterations < 1)
                        return "Number of iterations must be greater then zero.";
                    break;
                case "StepSize":
                    if (StepSize < 1)
                        return "Step size must be greater then zero.";
                    break;
                case "StepDelta":
                    if (StepDelta < -100 || StepDelta > 100)
                        return "Step size randomization must be between -100 and 100.";
                    break;
                case "AngleDelta":
                    if (AngleDelta < -100 || AngleDelta > 100)
                        return "Turn angle randomization must be between -100 and 100.";
                    break;
                case "Delta":
                    if (Delta < 0 || Delta >= 360)
                        return "Turn angle must be between 0 and 359";
                    break;
                default:
                    break;
            }
            return null;
        }

        #region IDataErrorInfo implementation

        public string Error { get { return null; } }

        public string this[string columnName]
        {
            get
            {
                return Validate(columnName);
            }
        }

        #endregion // IDataErrorInfo implementation
    }
}
