/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;
using TweenEngine.Equations;

namespace TweenEngine.Equations
{
	/// <summary>
	/// Easing equation based on Robert Penner's work:
	/// http://robertpenner.com/easing/
	/// </summary>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public abstract class Quad : TweenEquation
	{
		private sealed class QuadIn : Quad
		{
			public sealed override float Compute(float t)
			{
				return t * t;
			}

			public override string ToString()
			{
				return "Quad.IN";
			}
		}

		public static readonly Quad IN = new QuadIn();

		private sealed class QuadOut : Quad
		{
			public sealed override float Compute(float t)
			{
				return -t * (t - 2);
			}

			public override string ToString()
			{
				return "Quad.OUT";
			}
		}

		public static readonly Quad OUT = new QuadOut();

		private sealed class QuadInOut : Quad
		{
			public sealed override float Compute(float t)
			{
				if ((t *= 2) < 1)
				{
					return 0.5f * t * t;
				}
				return -0.5f * ((--t) * (t - 2) - 1);
			}

			public override string ToString()
			{
				return "Quad.INOUT";
			}
		}

		public static readonly Quad INOUT = new QuadInOut();
	}
}
