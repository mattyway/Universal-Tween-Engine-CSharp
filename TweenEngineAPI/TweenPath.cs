/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;

namespace TweenEngine
{
	/// <summary>Base class for every paths.</summary>
	/// <remarks>
	/// Base class for every paths. You can create your own paths and directly use
	/// them in the Tween engine by inheriting from this class.
	/// </remarks>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public interface TweenPath
	{
		/// <summary>
		/// Computes the next value of the interpolation, based on its waypoints and
		/// the current progress.
		/// </summary>
		/// <remarks>
		/// Computes the next value of the interpolation, based on its waypoints and
		/// the current progress.
		/// </remarks>
		/// <param name="t">
		/// The progress of the interpolation, between 0 and 1. May be out
		/// of these bounds if the easing equation involves some kind of rebounds.
		/// </param>
		/// <param name="points">The waypoints of the tween, from start to target values.</param>
		/// <param name="pointsCnt">The number of valid points in the array.</param>
		/// <returns>The next value of the interpolation.</returns>
		float Compute(float t, float[] points, int pointsCnt);
	}
}
