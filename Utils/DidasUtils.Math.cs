using System;
using System.Collections.Generic;

namespace DidasUtils.Math
{
    public class Fraction
    {
        public int num { get; set; }
        public int den { get; set; }
        public double Value { get { return num / (double)den; } }


        public static Fraction One { get { return new Fraction(1, 1); } }



        public Fraction(int t, int b)
        {
            num = t; den = b;
        }



        public Fraction Simplify()
        {
            int d = MathUtils.GCD(num, den);

            num /= d;
            den /= d;

            return this;
        }
        public int BitSpace()
        {
            Simplify();

            int n = 2;
            int b = 1;

            while (den > n)
            {
                n *= 2;
                b++;
            }

            n = 2;

            while (num > n)
            {
                n *= 2;
                b++;
            }

            return b;
        }



        public override string ToString()
        {
            if (den == 0)
                return $"Div {num} by 0";

            return $"({num}/{den})";
        }



        public static Fraction operator +(Fraction a, Fraction b)
        {
            Fraction f = new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);
            f.Simplify();

            return f;
        }
        public static Fraction operator -(Fraction a, Fraction b)
        {
            Fraction f = a + new Fraction(-b.num, b.den);

            return f;
        }
        public static Fraction operator *(Fraction a, Fraction b)
        {
            Fraction f = new Fraction(a.num * b.num, a.den * b.den);
            f.Simplify();
            return f;
        }
        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a.num * b.den, a.den * b.num);
        }
    }

    public static class MathUtils
    {
        public static int GCD(int a, int b)
        {
            while (b > 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }
            if (a == 0)
                a = 1;
            return a;
        }
    }

    public static class Randomizer
    {
        public static char GetRandomChar(int seed)
        {
            Random rnd = new Random(seed);
            return (char)rnd.Next(' ', '~');
        }

        public static char GetRandomChar()
        {
            Random rnd = new Random();
            return (char)rnd.Next(' ', '~');
        }
    }
}