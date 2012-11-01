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
	public abstract class Cubic : TweenEquation
	{
		private sealed class CubicIn : Cubic
		{
			public sealed override float Compute(float t)
			{
				return t * t * t;
			}

			public override string ToString()
			{
				return "Cubic.IN";
			}
		}

		public static readonly Cubic IN = new CubicIn();

		private sealed class CubicOut : Cubic
		{
			public sealed override float Compute(float t)
			{
				return (t -= 1) * t * t + 1;
			}

			public override string ToString()
			{
				return "Cubic.OUT";
			}
		}

		public static readonly Cubic OUT = new CubicOut();

		private sealed class CubicInOut : Cubic
		{
			public sealed override float Compute(float t)
			{
				if ((t *= 2) < 1)
				{
					return 0.5f * t * t * t;
				}
				return 0.5f * ((t -= 2) * t * t + 2);
			}

			public override string ToString()
			{
				return "Cubic.INOUT";
			}
		}

		public static readonly Cubic INOUT = new CubicInOut();
	}
}
