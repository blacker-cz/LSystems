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
        /// Model
        /// </summary>
        private SettingsModel _model;

        /// <summary>
        /// Starting position on X axis
        /// </summary>
        public int StartX { get { return _model.StartX; } set { _model.StartX = value; } }

        /// <summary>
        /// Starting position on Y axis
        /// </summary>
        public int StartY { get { return _model.StartY; } set { _model.StartY = value; } }

        /// <summary>
        /// Starting angle
        /// </summary>
        public float StartAngle { get { return _model.StartAngle; } set { _model.StartAngle = value; } }

        /// <summary>
        /// Axiom
        /// </summary>
        public string Axiom { get { return _model.Axiom; } set { _model.Axiom = value; } }

        /// <summary>
        /// Rules
        /// </summary>
        public string Rules { get { return _model.Rules; } set { _model.Rules = value; } }

        /// <summary>
        /// Number of iterations
        /// </summary>
        public uint Iterations { get { return _model.Iterations; } set { _model.Iterations = value; } }

        /// <summary>
        /// Degree used for turning left/right
        /// </summary>
        public float Delta { get { return _model.Delta; } set { _model.Delta = value; } }

        /// <summary>
        /// Step size
        /// </summary>
        public float StepSize { get { return _model.StepSize; } set { _model.StepSize = value; } }

        /// <summary>
        /// Step size randomization
        /// </summary>
        public float StepDelta { get { return _model.StepDelta; } set { _model.StepDelta = value; } }

        /// <summary>
        /// Step angle randomization
        /// </summary>
        public float AngleDelta { get { return _model.AngleDelta; } set { _model.AngleDelta = value; } }

        /// <summary>
        /// Randomization seed
        /// </summary>
        public int Seed { get { return _model.Seed; } set { _model.Seed = value; } }

        /// <summary>
        /// Generated image
        /// </summary>
        public Image Image { get { return _renderer.Preview; } }

        /// <summary>
        /// List of known colors
        /// </summary>
        public List<Color> Colors { get { return GetKnownColors(); } }

        /// <summary>
        /// Selected background color
        /// </summary>
        public Color SelectedBackgroundColor { get { return _model.SelectedBackgroundColor; } set { _model.SelectedBackgroundColor = value; } }

        /// <summary>
        /// Line width
        /// </summary>
        public float LineWidth { get { return _model.LineWidth; } set { _model.LineWidth = value; } }

        /// <summary>
        /// Command handlers
        /// </summary>
        private BaseCommand _generateCommand,
            _saveImageCommand,
            _closeCommand,
            _randomSeedCommand,
            _saveDefinitionCommand,
            _loadDefinitionCommand,
            _loadExampleCommand;

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
        /// Save definiton of current l-system command binding
        /// </summary>
        public ICommand SaveDefinitionCommand { get { return _saveDefinitionCommand ?? (_saveDefinitionCommand = new SaveDefinitionCommand(this)); } }

        /// <summary>
        /// Load definiton of l-system command binding
        /// </summary>
        public ICommand LoadDefinitionCommand { get { return _loadDefinitionCommand ?? (_loadDefinitionCommand = new LoadDefinitionCommand(this)); } }

        /// <summary>
        /// Load example of l-system command binding
        /// </summary>
        public ICommand LoadExampleCommand { get { return _loadExampleCommand ?? (_loadExampleCommand = new LoadExampleCommand(this)); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _renderer = new Renderer();
            _model = new SettingsModel();
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
                return;
            }

            Backend.LSystem lsystem = new Backend.LSystem(expandedGrammar,
                new PointF(StartX * ((float)Renderer.WIDTH / Renderer.PREVIEW_WIDTH), StartY * ((float)Renderer.HEIGHT / Renderer.PREVIEW_HEIGHT)),
                (float)(StartAngle / 180 * Math.PI),
                StepSize,
                (float)(Delta / 180 * Math.PI),
                (AngleDelta / 100.0f),
                (StepDelta / 100.0f),
                Seed,
                new Pen(new SolidBrush(Color.Black), LineWidth));

            List<Backend.IDrawable> polylist = lsystem.Generate();

            _renderer.Clear(SelectedBackgroundColor);
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

        /// <summary>
        /// Save current configuration (definition of L-System) to file
        /// </summary>
        public void SaveDefinition()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "l-system"; // Default file name
            dlg.DefaultExt = ".ls"; // Default file extension
            dlg.Filter = "L-System files(*.ls)|*.ls|All files (*.*)|*.*"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                try
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_model.GetType());
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filename))
                    {
                        x.Serialize(writer, _model);
                        writer.Close();
                    }
                }
                catch
                {
                    System.Windows.MessageBox.Show("Couldn't save L-System, please try again later.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Load L-System definition file
        /// </summary>
        public void LoadDefinition()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".ls"; // Default file extension
            dlg.Filter = "L-System files(*.ls)|*.ls|All files (*.*)|*.*"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Load document
                string filename = dlg.FileName;
                try
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_model.GetType());
                    using (System.Xml.XmlReader reader = new System.Xml.XmlTextReader(filename))
                    {
                        if (x.CanDeserialize(reader))
                            _model = x.Deserialize(reader) as SettingsModel;
                        reader.Close();
                    }
                }
                catch
                {
                    System.Windows.MessageBox.Show("Couldn't load L-System, please try again later.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }

            // update all properties
            InvokePropertyChanged(null);
        }

        /// <summary>
        /// Load embedded example
        /// </summary>
        /// <param name="name">Example file name</param>
        public void LoadExample(string name)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(_model.GetType());
                using (System.Xml.XmlReader reader = new System.Xml.XmlTextReader(System.Windows.Application.GetResourceStream(new System.Uri("/examples/" + name, UriKind.Relative)).Stream))
                {
                    if (x.CanDeserialize(reader))
                        _model = x.Deserialize(reader) as SettingsModel;
                    reader.Close();
                }

                // update all properties
                InvokePropertyChanged(null);
            }
            catch
            {
                System.Windows.MessageBox.Show("Couldn't load L-System, please try again later.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        #endregion // Command handlers implementation

        /// <summary>
        /// Get list of known colors
        /// </summary>
        /// <returns></returns>
        private List<Color> GetKnownColors()
        {
            List<Color> colors = new List<Color>();
            string[] color_names = Enum.GetNames(typeof(KnownColor));

            foreach (string color_name in color_names)
            {
                KnownColor known_color = (KnownColor)Enum.Parse(typeof(KnownColor), color_name);

                if ((known_color > KnownColor.Transparent) && (known_color < KnownColor.ButtonFace))
                {
                    colors.Add(Color.FromName(color_name));
                }
            }

            return (colors);
        }

        #region IDataErrorInfo implementation

        public string Error { get { return null; } }

        public string this[string columnName]
        {
            get
            {
                return _model.Validate(columnName);
            }
        }

        #endregion // IDataErrorInfo implementation
    }
}
