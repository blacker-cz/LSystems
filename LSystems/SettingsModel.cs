using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LSystems.Backend;
using System.Xml.Serialization;

namespace LSystems
{
    [Serializable]
    public class SettingsModel
    {

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
        /// Selected background color
        /// </summary>
        [XmlIgnore]
        public Color SelectedBackgroundColor { get; set; }

        /// <summary>
        /// Hack for xml serialization of selected background color
        /// </summary>
        [XmlElement("SelectedBackgroundColor")]
        public string SelectedBackgroundColorHtml
        {
            get { return ColorTranslator.ToHtml(SelectedBackgroundColor); }
            set { SelectedBackgroundColor = ColorTranslator.FromHtml(value); }
        }
        /// <summary>
        /// Line width
        /// </summary>
        public float LineWidth { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsModel()
        {
            Axiom = "FX";
            Rules = "X=X+YF\nY=FX-Y";
            StartX = 100;
            StartY = 100;
            Iterations = 5;
            Delta = 90.0f;
            StepSize = 10.0f;
            LineWidth = 1.0f;

            SelectedBackgroundColor = Color.White;
        }

        /// <summary>
        /// Validation of fields
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>Error message or null</returns>
        public string Validate(string columnName)
        {
            switch (columnName)
            {
/*                case "StartX":
                    if (StartX < 0 || StartX > Renderer.PREVIEW_WIDTH)
                        return "Starting position on axis X must be between 0 and 480";
                    break;
                case "StartY":
                    if (StartY < 0 || StartY > Renderer.PREVIEW_HEIGHT)
                        return "Starting position on axis Y must be between 0 and 360";
                    break;*/
                case "StartAngle":
                    if (StartAngle < 0 || StartAngle >= 360)
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
    }
}
