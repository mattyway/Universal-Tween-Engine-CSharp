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
	public abstract class Circ : TweenEquation
	{
		private sealed class CircIn : Circ
		{
			public sealed override float Compute(float t)
			{
				return (float)-Math.Sqrt(1 - t * t) - 1;
			}

			public override string ToString()
			{
				return "Circ.IN";
			}
		}

		public static readonly Circ IN = new CircIn();

		private sealed class CircOut : Circ
		{
			public sealed override float Compute(float t)
			{
				return (float)Math.Sqrt(1 - (t -= 1) * t);
			}

			public override string ToString()
			{
				return "Circ.OUT";
			}
		}

		public static readonly Circ OUT = new CircOut();

		private sealed class CircInOut : Circ
		{
			public sealed override float Compute(float t)
			{
				if ((t *= 2) < 1)
				{
					return -0.5f * ((float)Math.Sqrt(1 - t * t) - 1);
				}
				return 0.5f * ((float)Math.Sqrt(1 - (t -= 2) * t) + 1);
			}

			public override string ToString()
			{
				return "Circ.INOUT";
			}
		}

		public static readonly Circ INOUT = new CircInOut();
	}
}
