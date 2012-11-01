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
	public abstract class Linear : TweenEquation
	{
		private sealed class LinearInOut : Linear
		{
			public override float Compute(float t)
			{
				return t;
			}

			public override string ToString()
			{
				return "Linear.INOUT";
			}
		}

		public static readonly Linear INOUT = new LinearInOut();
	}
}
