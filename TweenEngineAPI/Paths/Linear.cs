/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using System;
using TweenEngine;
using TweenEngine.Paths;

namespace TweenEngine.Paths
{
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public class Linear : TweenPath
	{
		public virtual float Compute(float t, float[] points, int pointsCnt)
		{
			int segment = (int)Math.Floor((pointsCnt - 1) * t);
			segment = Math.Max(segment, 0);
			segment = Math.Min(segment, pointsCnt - 2);
			t = t * (pointsCnt - 1) - segment;
			return points[segment] + t * (points[segment + 1] - points[segment]);
		}
	}
}
