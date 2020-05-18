using System;
using System.Runtime.InteropServices;

namespace MyMath
{
    /// <summary>
    /// An interface used to access the MathFunctions class.
    /// </summary>
    [Guid("EA82646C-2531-42ff-AABF-55028FE0B0B5"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMathFunctions
    {
        Int32 DoAdd(Int32 Value1, Int32 Value2);
        Int32 DoSubtract(Int32 Value1, Int32 Value2);
        Int32 DoMultiply(Int32 Value1, Int32 Value2);
        Int32 DoDivide(Int32 Value1, Int32 Value2);
    }

    /// <summary>
    /// A simple class that shows the four basic math functions.
    /// </summary>
    [Guid("0C4340A2-C362-4287-9A03-8CDD3D1F80F6"),
    ClassInterface(ClassInterfaceType.None)]
    public class MathFunctions : IMathFunctions
    {
        /// <summary>
        /// Add two Int32 values together.
        /// </summary>
        /// <param name="Value1">The first number.</param>
        /// <param name="Value2">The second number.</param>
        /// <returns>The two numbers added.</returns>
        public Int32 DoAdd(Int32 Value1, Int32 Value2)
        {
            return Value1 + Value2;
        }

        /// <summary>
        /// Subtract one Int32 value from another.
        /// </summary>
        /// <param name="Value1">Initial value</param>
        /// <param name="Value2">Value to subtract</param>
        /// <returns>Value2 subtracted from Value1</returns>
        public Int32 DoSubtract(Int32 Value1, Int32 Value2)
        {
            return Value1 - Value2;
        }

        /// <summary>
        /// Multiply two Int32 values.
        /// </summary>
        /// <param name="Value1">The first number.</param>
        /// <param name="Value">The second number.</param>
        /// <returns>The two values multiplied.</returns>
        public Int32 DoMultiply(Int32 Value1, Int32 Value2)
        {
            return Value1 * Value2;
        }

        /// <summary>
        /// Divide one Int32 value by another.
        /// </summary>
        /// <param name="Value1">Initial value.</param>
        /// <param name="Value2">Divisor</param>
        /// <returns>Value1 divided by Value2</returns>
        public Int32 DoDivide(Int32 Value1, Int32 Value2)
        {
            return Value1 / Value2;
        }
    }
}
