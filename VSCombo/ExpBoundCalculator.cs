using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VSCombo
{
    public sealed class ExpBoundCalculator
    {
        private static int PRECALC_COUNT { get; } = 100;
        private static double[] precalc_value;
        static ExpBoundCalculator()
        {
            precalc_value = new double[PRECALC_COUNT];
            double value = 1;
            for (int i = 0; i < PRECALC_COUNT; i++)
            {
                precalc_value[i] = 1 - value;
                value /= Math.E;
            }
        }

        public double BaseValue { get; }
        public double LimitedValue { get; }
        private double valueDiff;

        public ExpBoundCalculator(double baseValue, double limitedValue)
        {
            BaseValue = baseValue;
            LimitedValue = limitedValue;
            valueDiff = limitedValue - baseValue;
        }

        public double Calculate(int x)
        {
            if (x < PRECALC_COUNT)
                return BaseValue + valueDiff * precalc_value[x];
            else
                return BaseValue + valueDiff * (1 - Math.Exp(-x));
        }
    }

    public sealed class ColorCalculator
    {
        private byte fromR, fromB, fromG;
        private byte toR, toB, toG;
        private int diffR, diffB, diffG;
        private double stepR, stepB, stepG;

        public Color From { get; }
        public Color To { get; }
        public int Distance { get; }
        public int Base { get; }
        public ColorCalculator(Color from, Color to, int distance, int @base)
        {
            if (distance <= 0)
                throw new ArgumentOutOfRangeException(nameof(distance));

            From = from;
            To = to;
            Base = @base;
            Distance = distance;

            fromR = from.R;
            fromB = from.B;
            fromG = from.G;
            toR = to.R;
            toB = to.B;
            toG = to.G;
            diffR = toR - fromR;
            diffG = toG - fromG;
            diffB = toB - fromB;
            stepR = diffR * 1.0 / distance;
            stepB = diffB * 1.0 / distance;
            stepG = diffG * 1.0 / distance;
        }

        public Color Calculate(int x)
        {
            if (x < 0)
                x = 0;
            else if (x > Distance)
                x = Distance;

            if (x == 0)
                return From;
            if (x == Distance)
                return To;
            byte r = (byte)(fromR + stepR * x);
            byte g = (byte)(fromG + stepG * x);
            byte b = (byte)(fromB + stepB * x);
            return Color.FromRgb(r, g, b);
        }
    }

}
