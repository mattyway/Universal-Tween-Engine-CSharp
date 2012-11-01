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
	public abstract class Back : TweenEquation
	{
		private sealed class BackIn : Back
		{
			public sealed override float Compute(float t)
			{
				float s = this.param_s;
				return t * t * ((s + 1) * t - s);
			}

			public override string ToString()
			{
				return "Back.IN";
			}
		}

		public static readonly Back IN = new BackIn();

		private sealed class BackOut : Back
		{
			public sealed override float Compute(float t)
			{
				float s = this.param_s;
				return (t -= 1) * t * ((s + 1) * t + s) + 1;
			}

			public override string ToString()
			{
				return "Back.OUT";
			}
		}

		public static readonly Back OUT = new BackOut();

		private sealed class BackInOut : Back
		{
			public sealed override float Compute(float t)
			{
				float s = this.param_s;
				if ((t *= 2) < 1)
				{
					return 0.5f * (t * t * (((s *= (1.525f)) + 1) * t - s));
				}
				return 0.5f * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2);
			}

			public override string ToString()
			{
				return "Back.INOUT";
			}
		}

		public static readonly Back INOUT = new BackInOut();

		protected internal float param_s = 1.70158f;

		// -------------------------------------------------------------------------
		public virtual Back S(float s)
		{
			param_s = s;
			return this;
		}
	}
}
