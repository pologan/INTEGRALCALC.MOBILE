using System;
using System.Collections.Generic;
using System.Text;

namespace INTEGRALCALCULATOR.MOBILE.Models.Algoritms
{
    public static class IntegrationAlgorithms
    {
        private static ReversePolishNotation _rpn = new ReversePolishNotation();
        private static Random _rand = new Random();
        /// <summary>
        /// Całkowania metodą prostokątów.
        /// </summary>
        /// <param name="xs">Początek przedziału.</param>
        /// <param name="xe">Koniec przedziału.</param>
        /// <param name="steps">Liczba kroków.</param>
        /// <param name="func">Całkowana funkcja.</param>
        /// <returns>Wynik całkowania.</returns>
        public static double Rectanglular(string xs, string xe, int steps, string func)
        {
            double dx, integ, start, end;

            _rpn.Parse(xs);
            start = _rpn.Evaluate();
            _rpn.Parse(xe);
            end = _rpn.Evaluate();

            dx = (end - start) / (double)steps;

            integ = 0;

            _rpn.Parse(func);
            for (int i = 1; i <= steps; i++)
            {
                integ += _rpn.Evaluate(start + i * dx);
            }
            integ *= dx;

            return integ;
        }

        /// <summary>
        /// Całkowania metodą trapezów.
        /// </summary>
        /// <param name="xs">Początek przedziału.</param>
        /// <param name="xe">Koniec przedziału.</param>
        /// <param name="steps">Liczba kroków.</param>
        /// <param name="func">Całkowana funkcja.</param>
        /// <returns>Wynik całkowania.</returns>
        public static double Trapezoidal(string xs, string xe, int steps, string func)
        {
            double dx, integ, start, end;

            _rpn.Parse(xs);
            start = _rpn.Evaluate();
            _rpn.Parse(xe);
            end = _rpn.Evaluate();

            dx = (end - start) / (double)steps;

            integ = 0;

            _rpn.Parse(func);
            for (int i = 1; i < steps; i++)
            {
                integ += _rpn.Evaluate(start + i * dx);
            }
            integ += (_rpn.Evaluate(start) + _rpn.Evaluate(end)) / 2;
            integ *= dx;

            return integ;
        }

        public static double Simpson(string xs, string xe, int steps, string func)
        {
            double dx, integ, s, x, start, end;
            
            _rpn.Parse(xs);
            start = _rpn.Evaluate();
            _rpn.Parse(xe);
            end = _rpn.Evaluate();

            dx = (end - start) / (double)steps;

            integ = 0;
            s = 0;

            _rpn.Parse(func);
            for (int i = 1; i < steps; i++)
            {
                x = start + i * dx;
                s += _rpn.Evaluate(x - dx / 2);
                integ += _rpn.Evaluate(x);
            }
            s += _rpn.Evaluate(end - dx / 2);
            integ = (dx / 6) * (_rpn.Evaluate(start) + _rpn.Evaluate(end) + 2 * integ + 4 * s);

            return integ;
        }

        public static double MonteCarlo(string xs, string xe, int steps, string func)
        {
            double integ, start, end;

            _rpn.Parse(xs);
            start = _rpn.Evaluate();
            _rpn.Parse(xe);
            end = _rpn.Evaluate();


            integ = 0;
            _rpn.Parse(func);
            for (int i = 0; i < steps; i++)
            {
                integ += _rpn.Evaluate(RandomPoint(start, end));
            }

            integ = (integ / steps) * (end - start);

            return integ;
        }

        private static double RandomPoint(double a, double b)
        {

            return a + _rand.NextDouble() * (b - a);
        }
    }
}
