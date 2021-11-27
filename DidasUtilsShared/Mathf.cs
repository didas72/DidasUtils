using System;

namespace DidasUtils
{
    public static class Mathf
    {
		/// <summary>
		/// Calculates the rounded up quotient of an integer division.
		/// </summary>
		/// <param name="dividend"></param>
		/// <param name="divisor"></param>
		/// <returns></returns>
		public static int DivideRoundUp(int dividend, int divisor) => dividend / divisor + ((dividend % divisor == 0) ? 0 : 1);



		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static byte Clamp(byte value, byte min, byte max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}
		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}
		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static ushort Clamp(ushort value, ushort min, ushort max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}
		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static short Clamp(short value, short min, short max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}
		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static uint Clamp(uint value, uint min, uint max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}
		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static int Clamp(int value, int min, int max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}
		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static long Clamp(long value, long min, long max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}
		/// <summary>
		/// Clamps a value between a minimum and a maximum.
		/// </summary>
		/// <param name="value">The number to be clamped.</param>
		/// <param name="min">Minimum final value.</param>
		/// <param name="max">Maximum final value.</param>
		/// <returns></returns>
		public static ulong Clamp(ulong value, ulong min, ulong max)
		{
			value = value > max ? max : value;

			return value < min ? min : value;
		}



		public static double DegToRad(double angle) => angle * Math.PI / 180;
		public static double RadToDeg(double angle) => angle * 180 / Math.PI;
	}
}
