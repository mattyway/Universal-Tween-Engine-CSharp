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
	public abstract class Sine : TweenEquation
	{
		private const float PI = 3.14159265f;

		private sealed class SineIn : Sine
		{
			public sealed override float Compute(float t)
			{
				return (float)-Math.Cos(t * (Sine.PI / 2)) + 1;
			}

			public override string ToString()
			{
				return "Sine.IN";
			}
		}

		public static readonly Sine IN = new SineIn();

		private sealed class SineOut : Sine
		{
			public sealed override float Compute(float t)
			{
				return (float)Math.Sin(t * (Sine.PI / 2));
			}

			public override string ToString()
			{
				return "Sine.OUT";
			}
		}

		public static readonly Sine OUT = new SineOut();

		private sealed class SineInOut : Sine
		{
			public sealed override float Compute(float t)
			{
				return -0.5f * ((float)Math.Cos(Sine.PI * t) - 1);
			}

			public override string ToString()
			{
				return "Sine.INOUT";
			}
		}

		public static readonly Sine INOUT = new SineInOut();
	}
}
