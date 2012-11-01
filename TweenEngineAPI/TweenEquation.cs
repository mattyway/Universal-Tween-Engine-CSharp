/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;

namespace TweenEngine
{
	/// <summary>Base class for every easing equation.</summary>
	/// <remarks>
	/// Base class for every easing equation. You can create your own equations
	/// and directly use them in the Tween engine by inheriting from this class.
	/// </remarks>
	/// <seealso cref="Tween">Tween</seealso>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public abstract class TweenEquation
	{
		/// <summary>Computes the next value of the interpolation.</summary>
		/// <remarks>Computes the next value of the interpolation.</remarks>
		/// <param name="t">The current time, between 0 and 1.</param>
		/// <returns>The current value.</returns>
		public abstract float Compute(float t);

		/// <summary>
		/// Returns true if the given string is the name of this equation (the name
		/// is returned in the toString() method, don't forget to override it).
		/// </summary>
		/// <remarks>
		/// Returns true if the given string is the name of this equation (the name
		/// is returned in the toString() method, don't forget to override it).
		/// This method is usually used to save/load a tween to/from a text file.
		/// </remarks>
		public virtual bool IsValueOf(string str)
		{
			return str.Equals(ToString());
		}
	}
}
