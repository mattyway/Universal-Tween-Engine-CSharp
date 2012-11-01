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
	public abstract class Quart : TweenEquation
	{
		private sealed class QuartIn : Quart
		{
			public sealed override float Compute(float t)
			{
				return t * t * t * t;
			}

			public override string ToString()
			{
				return "Quart.IN";
			}
		}

		public static readonly Quart IN = new QuartIn();

		private sealed class QuartOut : Quart
		{
			public sealed override float Compute(float t)
			{
				return -((t -= 1) * t * t * t - 1);
			}

			public override string ToString()
			{
				return "Quart.OUT";
			}
		}

		public static readonly Quart OUT = new QuartOut();

		private sealed class QuartInOut : Quart
		{
			public sealed override float Compute(float t)
			{
				if ((t *= 2) < 1)
				{
					return 0.5f * t * t * t * t;
				}
				return -0.5f * ((t -= 2) * t * t * t - 2);
			}

			public override string ToString()
			{
				return "Quart.INOUT";
			}
		}

		public static readonly Quart INOUT = new QuartInOut();
	}
}
