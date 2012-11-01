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
	public class CatmullRom : TweenPath
	{
		public virtual float Compute(float t, float[] points, int pointsCnt)
		{
			int segment = (int)Math.Floor((pointsCnt - 1) * t);
			segment = Math.Max(segment, 0);
			segment = Math.Min(segment, pointsCnt - 2);
			t = t * (pointsCnt - 1) - segment;
			if (segment == 0)
			{
				return CatmullRomSpline(points[0], points[0], points[1], points[2], t);
			}
			if (segment == pointsCnt - 2)
			{
				return CatmullRomSpline(points[pointsCnt - 3], points[pointsCnt - 2], points[pointsCnt
					 - 1], points[pointsCnt - 1], t);
			}
			return CatmullRomSpline(points[segment - 1], points[segment], points[segment + 1]
				, points[segment + 2], t);
		}

		private float CatmullRomSpline(float a, float b, float c, float d, float t)
		{
			float t1 = (c - a) * 0.5f;
			float t2 = (d - b) * 0.5f;
			float h1 = +2 * t * t * t - 3 * t * t + 1;
			float h2 = -2 * t * t * t + 3 * t * t;
			float h3 = t * t * t - 2 * t * t + t;
			float h4 = t * t * t - t * t;
			return b * h1 + c * h2 + t1 * h3 + t2 * h4;
		}
	}
}
