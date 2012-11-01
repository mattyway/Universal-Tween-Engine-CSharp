/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using System;
using TweenEngine;
using TweenEngine.Equations;

namespace TweenEngine.Equations
{
	/// <summary>
	/// Easing equation based on Robert Penner's work:
	/// http://robertpenner.com/easing/
	/// </summary>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public abstract class Expo : TweenEquation
	{
		private sealed class ExpoIn : Expo
		{
			public sealed override float Compute(float t)
			{
				return (t == 0) ? 0 : (float)Math.Pow(2, 10 * (t - 1));
			}

			public override string ToString()
			{
				return "Expo.IN";
			}
		}

		public static readonly Expo IN = new ExpoIn();

		private sealed class ExpoOut : Expo
		{
			public sealed override float Compute(float t)
			{
				return (t == 1) ? 1 : -(float)Math.Pow(2, -10 * t) + 1;
			}

			public override string ToString()
			{
				return "Expo.OUT";
			}
		}

		public static readonly Expo OUT = new ExpoOut();

		private sealed class ExpoInOut : Expo
		{
			public sealed override float Compute(float t)
			{
				if (t == 0)
				{
					return 0;
				}
				if (t == 1)
				{
					return 1;
				}
				if ((t *= 2) < 1)
				{
					return 0.5f * (float)Math.Pow(2, 10 * (t - 1));
				}
				return 0.5f * (-(float)Math.Pow(2, -10 * --t) + 2);
			}

			public override string ToString()
			{
				return "Expo.INOUT";
			}
		}

		public static readonly Expo INOUT = new ExpoInOut();
	}
}
