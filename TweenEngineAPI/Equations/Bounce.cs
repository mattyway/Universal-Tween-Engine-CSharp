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
	public abstract class Bounce : TweenEquation
	{
		private sealed class BounceIn : Bounce
		{
			public sealed override float Compute(float t)
			{
				return 1 - Bounce.OUT.Compute(1 - t);
			}

			public override string ToString()
			{
				return "Bounce.IN";
			}
		}

		public static readonly Bounce IN = new BounceIn();

		private sealed class BounceOut : Bounce
		{
			public sealed override float Compute(float t)
			{
				if (t < (1 / 2.75))
				{
					return 7.5625f * t * t;
				}
				else
				{
					if (t < (2 / 2.75))
					{
						return 7.5625f * (t -= (1.5f / 2.75f)) * t + .75f;
					}
					else
					{
						if (t < (2.5 / 2.75))
						{
							return 7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f;
						}
						else
						{
							return 7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f;
						}
					}
				}
			}

			public override string ToString()
			{
				return "Bounce.OUT";
			}
		}

		public static readonly Bounce OUT = new BounceOut();

		private sealed class BounceInOut : Bounce
		{
			public sealed override float Compute(float t)
			{
				if (t < 0.5f)
				{
					return Bounce.IN.Compute(t * 2) * .5f;
				}
				else
				{
					return Bounce.OUT.Compute(t * 2 - 1) * .5f + 0.5f;
				}
			}

			public override string ToString()
			{
				return "Bounce.INOUT";
			}
		}

		public static readonly Bounce INOUT = new BounceInOut();
	}
}
