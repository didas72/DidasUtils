﻿using System;

namespace DidasUtils
{
	/// <summary>
	/// Class that contains several math-related methods.
	/// </summary>
	public static class Mathf
    {
		/// <summary>
		/// Calculates the rounded up quotient of an integer division.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static int DivideRoundUp(int a, int b) => (a - 1) / b + 1;
		//OLD: public static int DivideRoundUp(int dividend, int divisor) => dividend / divisor + ((dividend % divisor == 0) ? 0 : 1);



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



		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static double DegToRad(double angle) => angle * Math.PI / 180;
		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static double RadToDeg(double angle) => angle * 180 / Math.PI;
	}
}
