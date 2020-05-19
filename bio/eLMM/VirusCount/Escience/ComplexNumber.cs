//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;

namespace MBT.Escience
{
    public struct ComplexNumber
    {
        const double closeEnoughTo0 = 1e-3;
        const double RealRatioThreshold = 1e5;

        public readonly double Real;
        public readonly double Img;

        public double PolarRadius
        {
            get
            {
                return Abs(this);
            }
        }
        public double PolarAngleInRadians
        {
            get
            {
                return Math.Acos(Real / PolarRadius);
            }
        }

        public ComplexNumber(double d)
        {
            Real = d;
            Img = 0;
        }

        public ComplexNumber(double real, double img)
        {
            Real = real;
            Img = img;
        }

        public static ComplexNumber FromPolarCoordinates(double radius, double angleInRadians)
        {
            return new ComplexNumber(radius * Math.Cos(angleInRadians), radius * Math.Sin(angleInRadians));
        }

        public bool IsReal()
        {
            return Math.Abs(Img) < closeEnoughTo0 || Math.Abs(Real / Img) > RealRatioThreshold;
        }

        public bool Approximates(ComplexNumber c, double eps)
        {
            return Math.Abs(Real - c.Real) < eps && Math.Abs(Img - c.Img) < eps;
        }

        /// <summary>
        /// Absolute value is the distance from the origin to the point on the complex plan represented by this ComplexNumber.
        /// </summary>
        public static double Abs(ComplexNumber c)
        {
            return c.Img == 0 ? Math.Abs(c.Real) : Math.Sqrt(Square(c));
        }

        public static double Square(ComplexNumber c)
        {
            return c.Real * c.Real + c.Img * c.Img;
        }


        public static ComplexNumber Reciprocal(ComplexNumber c)
        {
            if (c.IsReal())
                return new ComplexNumber(1.0 / c.Real);
            else
            {
                double square = Square(c);
                return new ComplexNumber(c.Real / square, -c.Img / square);
            }
        }

        public override string ToString()
        {
            if (IsReal())
                return "" + Real;
            //return string.Format("{0:f3}", Real);

            double absImg = Math.Abs(Img);
            return string.Format("({0} {1} {2}i)", Real, Img < 0 ? "-" : "+", absImg == 1 ? "" : "" + absImg);
        }

        public static ComplexNumber operator -(ComplexNumber c)
        {
            return new ComplexNumber(-c.Real, -c.Img);
        }

        public static ComplexNumber operator +(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real + c2.Real, c1.Img + c2.Img);
        }
        public static ComplexNumber operator +(ComplexNumber c, double d)
        {
            return new ComplexNumber(c.Real + d, c.Img);
        }
        public static ComplexNumber operator +(double d, ComplexNumber c)
        {
            return c + d;
        }

        public static ComplexNumber operator -(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real - c2.Real, c1.Img - c2.Img);
        }
        public static ComplexNumber operator -(ComplexNumber c, double d)
        {
            return new ComplexNumber(c.Real - d, c.Img);
        }
        public static ComplexNumber operator -(double d, ComplexNumber c)
        {
            return new ComplexNumber(d - c.Real, c.Img);
        }

        public static ComplexNumber operator *(ComplexNumber c1, ComplexNumber c2)
        {
            return new ComplexNumber(c1.Real * c2.Real - c1.Img * c2.Img, c1.Real * c2.Img + c1.Img * c2.Real);
        }
        public static ComplexNumber operator *(ComplexNumber c, double d)
        {
            return new ComplexNumber(c.Real * d, c.Img * d);
        }
        public static ComplexNumber operator *(double d, ComplexNumber c)
        {
            return c * d;
        }

        public static ComplexNumber operator /(ComplexNumber c1, ComplexNumber c2)
        {
            return c1 * Reciprocal(c2);
        }
        public static ComplexNumber operator /(ComplexNumber c, double d)
        {
            return new ComplexNumber(c.Real / d, c.Img / d);
        }

        public static bool ApproxEqual(ComplexNumber c1, ComplexNumber c2, double eps)
        {
            return (double.IsNaN(c1.Real) && double.IsNaN(c2.Real) || Math.Abs(c1.Real - c2.Real) <= eps) &&
                   (double.IsNaN(c1.Img) && double.IsNaN(c2.Img) || Math.Abs(c1.Img - c2.Img) <= eps);
        }

        public static bool operator ==(ComplexNumber c1, ComplexNumber c2)
        {
            return c1.Real == c2.Real && c1.Img == c2.Img;
        }
        public static bool operator ==(ComplexNumber c1, double d)
        {
            return c1.IsReal() && c1.Real == d;
        }
        public static bool operator ==(double d, ComplexNumber c)
        {
            return c == d;
        }

        public override bool Equals(object obj)
        {
            if (obj is double)
            {
                return (double)obj == this;
            }
            else if (obj is ComplexNumber)
            {
                return (ComplexNumber)obj == this;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Real.GetHashCode() ^ Img.GetHashCode();
        }

        public static bool operator !=(ComplexNumber c1, ComplexNumber c2)
        {
            return !(c1 == c2);
        }
        public static bool operator !=(ComplexNumber c1, double d)
        {
            return !(c1 == d);
        }
        public static bool operator !=(double d, ComplexNumber c)
        {
            return !(c == d);
        }

        public static implicit operator ComplexNumber(double d)
        {
            return new ComplexNumber(d);
        }

        public static explicit operator double(ComplexNumber c)
        {
            if (!c.IsReal())
            {
                throw new InvalidCastException("ERROR: Casting " + c + " to double. This is currently not allowed for type checking reasons.");
            }
            return c.Real;
        }

        // same funtionality as below, but more efficient if we know exponent isn't complex
        public static ComplexNumber Pow(ComplexNumber theBase, double exponent)
        {
            if (theBase.Real >= 0 && theBase.IsReal())
                return Math.Pow(theBase.Real, exponent);

            double baseRadius = theBase.PolarRadius;
            double baseAngle = theBase.PolarAngleInRadians;

            double newRadius = Math.Pow(baseRadius, exponent);
            double newAngle = exponent * baseAngle;

            return ComplexNumber.FromPolarCoordinates(newRadius, newAngle);
        }

        public static ComplexNumber Pow(ComplexNumber theBase, ComplexNumber exponent)
        {
            if (exponent.IsReal())
                return Pow(theBase, exponent.Real);

            double baseRadius = theBase.PolarRadius;
            double baseAngle = theBase.PolarAngleInRadians;

            double newRadius = Math.Pow(baseRadius, exponent.Real) * Math.Exp(-baseAngle * exponent.Img);
            double newAngle = exponent.Img * Math.Log(baseRadius) + exponent.Real * baseAngle;

            return ComplexNumber.FromPolarCoordinates(newRadius, newAngle);
        }

        public static ComplexNumber Exp(ComplexNumber exponent)
        {
            if (exponent.IsReal())
                return Math.Exp(exponent.Real);

            double newRadius = Math.Exp(exponent.Real);
            double newAngle = exponent.Img;

            return ComplexNumber.FromPolarCoordinates(newRadius, newAngle);
        }
    }
}
