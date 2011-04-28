using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LSystems.Backend
{
    /// <summary>
    /// Class implementing turtle
    /// </summary>
    class Turtle : ICloneable
    {
        /// <summary>
        /// Current position of turtle
        /// </summary>
        private PointF _position;

        /// <summary>
        /// Getter for current position
        /// </summary>
        public PointF Position
        {
            get { return new PointF(_position.X, _position.Y); }
        }

        /// <summary>
        /// Current angle of turtle
        /// </summary>
        private float _angle;

        /// <summary>
        /// Current angle of turtle
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        /// <summary>
        /// Randomization of angle
        /// </summary>
        private float _angleDelta;

        /// <summary>
        /// Randomization of angle
        /// </summary>
        public float AngleDelta
        {
            get { return _angleDelta; }
            set { _angleDelta = value; }
        }

        /// <summary>
        /// Degree used for turning left/right
        /// </summary>
        private float _delta;

        /// <summary>
        /// Degree used for turning left/right
        /// </summary>
        public float Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        /// <summary>
        /// Length of one step
        /// </summary>
        private float _step;

        /// <summary>
        /// Length of one step
        /// </summary>
        public float Step
        {
            get { return _step; }
            set { _step = value; }
        }

        /// <summary>
        /// Randomization of step length
        /// </summary>
        private float _stepDelta;

        /// <summary>
        /// Randomization of step length
        /// </summary>
        public float StepDelta
        {
            get { return _stepDelta; }
            set { _stepDelta = value; }
        }

        /// <summary>
        /// Randomizer
        /// </summary>
        public Random Randomizer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Initial turtle position</param>
        /// <param name="angle">Initial turtle angle</param>
        /// <param name="step">Length of one step</param>
        /// <param name="delta">Degree used for turning left/right</param>
        public Turtle(PointF position, float angle, float step, float delta, float angleDelta = 0.0f, float stepDelta = 0.0f)
        {
            _position = new PointF(position.X, position.Y);

            _angle = angle;
            _step = step;
            _delta = delta;

            _angleDelta = angleDelta;
            _stepDelta = stepDelta;
        }

        /// <summary>
        /// Compute random step length
        /// </summary>
        /// <returns>step length</returns>
        protected float RandomStep()
        {
            return _step * (float)(1.0 + _stepDelta * (Randomizer.NextDouble() - 1 / 2.0));
        }

        /// <summary>
        /// Compute random angle for turn
        /// </summary>
        /// <returns>random angle</returns>
        protected float RandomAngle()
        {
            return _delta *(float)(1.0 + _angleDelta * (Randomizer.NextDouble() - 1 / 2.0));
        }

        /// <summary>
        /// Do one step forward
        /// </summary>
        public void Forward()
        {
            float randomStep = RandomStep();
            _position.X += randomStep * (float)Math.Cos(_angle);
            _position.Y += randomStep * (float)Math.Sin(_angle);
        }

        /// <summary>
        /// Do one step backward
        /// </summary>
        public void Backward()
        {
            float randomStep = RandomStep();
            _position.X += randomStep * (float)Math.Cos(_angle);
            _position.Y += randomStep * (float)Math.Sin(_angle);
        }

        /// <summary>
        /// Turn left
        /// </summary>
        public void Left()
        {
            _angle += RandomAngle();
        }

        /// <summary>
        /// Turn right
        /// </summary>
        public void Right()
        {
            _angle -= RandomAngle();
        }

        /// <summary>
        /// Turn around
        /// </summary>
        public void TurnAround()
        {
            _angle -= (float) Math.PI;
        }

        #region Implementation of ICloneable
        
        /// <summary>
        /// Clone turtle object
        /// </summary>
        /// <returns>Clone of turtle object</returns>
        public object Clone()
        {
            Turtle clone = new Turtle(_position, _angle, _step, _delta, _angleDelta, _stepDelta);
            clone.Randomizer = Randomizer;
            return clone;
        }

        #endregion // Implementation of ICloneable
    }
}
