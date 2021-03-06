﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LSystems.Backend
{
    public class LSystem
    {
        private string _expandedGrammar;

        private Stack<State> _stateStack;

        private PointF _startPoint;

        private float _startAngle;

        private float _step;

        private float _delta;

        private float _angleDelta;

        private float _stepDelta;

        private Random _random;

        private Pen _pen;

        /// <summary>
        /// Constructor (overloaded)
        /// </summary>
        /// <param name="expandedGrammar">Expanded grammar string</param>
        public LSystem(string expandedGrammar)
        {
            _expandedGrammar = expandedGrammar;
            _startPoint = new PointF(0.0f, 0.0f);
            _step = 10.0f;
            _delta = ((float)Math.PI / 2);
            _random = new Random();

            _stateStack = new Stack<State>();
        }

        /// <summary>
        /// Constructor (overloaded)
        /// </summary>
        /// <param name="expandedGrammar">Expanded grammar string</param>
        /// <param name="startPoint">Starting point</param>
        public LSystem(string expandedGrammar, PointF startPoint, float startAngle = 0.0f, float step = 10.0f, float delta = ((float) Math.PI / 2),
            float angleDelta = 0.0f, float stepDelta = 0.0f, int seed = 0, Pen pen = null)
        {
            _expandedGrammar = expandedGrammar;
            _startPoint = new PointF(startPoint.X, startPoint.Y);
            _startAngle = startAngle;
            _step = step;
            _delta = delta;
            _angleDelta = angleDelta;
            _stepDelta = stepDelta;
            _random = new Random(seed);
            _pen = pen ?? new Pen(Color.Black);

            _stateStack = new Stack<State>();
        }

        /// <summary>
        /// Constructor (overloaded)
        /// </summary>
        /// <param name="parser">Grammar parser instance</param>
        /// <param name="startPoint">Starting point</param>
        public LSystem(Parser parser, PointF startPoint, float startAngle = 0.0f, float step = 10.0f, float delta = ((float) Math.PI / 2),
            float angleDelta = 0.0f, float stepDelta = 0.0f, int seed = 0, Pen pen = null)
        {
            _expandedGrammar = parser.Expand();
            _startPoint = new PointF(startPoint.X, startPoint.Y);
            _startAngle = startAngle;
            _step = step;
            _delta = delta;
            _angleDelta = angleDelta;
            _stepDelta = stepDelta;
            _random = new Random(seed);
            _pen = pen ?? new Pen(Color.Black);

            _stateStack = new Stack<State>();
        }

        /// <summary>
        /// Generate L-System polylines
        /// </summary>
        /// <returns>List of generated polylines</returns>
        public List<IDrawable> Generate()
        {
            List<IDrawable> polylines = new List<IDrawable>();
            Turtle turtle = new Turtle(_startPoint, _startAngle, _step, _delta, _angleDelta, _stepDelta);
            turtle.Randomizer = _random;
            Polyline polyline = new Polyline();
            polyline.Pen = (Pen) _pen.Clone();
            polyline.Add(turtle.Position);

            for (int i = 0; i < _expandedGrammar.Length; i++)
            {
                switch (_expandedGrammar[i])
                {
                    case 'A':       // draw forward
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                        turtle.Forward();
                        polyline.Add(turtle.Position);
                        break;
                    case 'K':       // draw backward
                    case 'L':
                    case 'M':
                    case 'N':
                        turtle.Backward();
                        polyline.Add(turtle.Position);
                        break;
                    case 'a':       // move forward without drawing
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                        if(polyline.Points.Count > 1) // save polyline only if there is an actual line
                            polylines.Add(polyline);
                        polyline = new Polyline();  // start new polyline
                        polyline.Pen = (Pen) _pen.Clone();
                        turtle.Forward();
                        polyline.Add(turtle.Position);
                        break;
                    case '+':       // turn left
                        turtle.Left();
                        break;
                    case '-':       // turn right
                        turtle.Right();
                        break;
                    case '|':       // turn around
                        turtle.TurnAround();
                        break;
                    case '[':       // save state to stack
                        _stateStack.Push(new State((Turtle)turtle.Clone()));
                        break;
                    case ']':       // load state from stack
                        State state = _stateStack.Pop();
                        turtle = state.Turtle;
                        if (polyline.Points.Count > 1)
                            polylines.Add(polyline);
                        polyline = new Polyline();
                        polyline.Pen = (Pen) _pen.Clone();
                        polyline.Add(turtle.Position);
                        break;
                    default:
                        break;
                }
            }

            // add last polyline to polylines list (only when it's not empty)
            if(polyline.Points.Count > 1)
                polylines.Add(polyline);
            
            return polylines;
        }

    }
}
