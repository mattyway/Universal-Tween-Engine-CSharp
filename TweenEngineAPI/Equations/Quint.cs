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
	public abstract class Quint : TweenEquation
	{
		private sealed class QuintIn : Quint
		{
			public sealed override float Compute(float t)
			{
				return t * t * t * t * t;
			}

			public override string ToString()
			{
				return "Quint.IN";
			}
		}

		public static readonly Quint IN = new QuintIn();

		private sealed class QuintOut : Quint
		{
			public sealed override float Compute(float t)
			{
				return (t -= 1) * t * t * t * t + 1;
			}

			public override string ToString()
			{
				return "Quint.OUT";
			}
		}

		public static readonly Quint OUT = new QuintOut();

		private sealed class QuintInOut : Quint
		{
			public sealed override float Compute(float t)
			{
				if ((t *= 2) < 1)
				{
					return 0.5f * t * t * t * t * t;
				}
				return 0.5f * ((t -= 2) * t * t * t * t + 2);
			}

			public override string ToString()
			{
				return "Quint.INOUT";
			}
		}

		public static readonly Quint INOUT = new QuintInOut();
	}
}
