/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;
using TweenEngine.Equations;

namespace TweenEngine
{
	/// <summary>Collection of miscellaneous utilities.</summary>
	/// <remarks>Collection of miscellaneous utilities.</remarks>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public class TweenUtils
	{
		private static TweenEquation[] easings;

		/// <summary>Takes an easing name and gives you the corresponding TweenEquation.</summary>
		/// <remarks>
		/// Takes an easing name and gives you the corresponding TweenEquation.
		/// You probably won't need this, but tools will love that.
		/// </remarks>
		/// <param name="easingName">The name of an easing, like "Quad.INOUT".</param>
		/// <returns>The parsed equation, or null if there is no match.</returns>
		public static TweenEquation ParseEasing(string easingName)
		{
			if (easings == null)
			{
				easings = new TweenEquation[] { Linear.INOUT, Quad.IN, Quad.OUT, Quad.INOUT, Cubic
					.IN, Cubic.OUT, Cubic.INOUT, Quart.IN, Quart.OUT, Quart.INOUT, Quint.IN, Quint.OUT
					, Quint.INOUT, Circ.IN, Circ.OUT, Circ.INOUT, Sine.IN, Sine.OUT, Sine.INOUT, Expo
					.IN, Expo.OUT, Expo.INOUT, Back.IN, Back.OUT, Back.INOUT, Bounce.IN, Bounce.OUT, 
					Bounce.INOUT, Elastic.IN, Elastic.OUT, Elastic.INOUT };
			}
			for (int i = 0; i < easings.Length; i++)
			{
				if (easingName.Equals(easings[i].ToString()))
				{
					return easings[i];
				}
			}
			return null;
		}
	}
}
