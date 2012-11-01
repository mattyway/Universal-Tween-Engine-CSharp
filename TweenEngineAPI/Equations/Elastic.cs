/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using System;
using TweenEngine;
using TweenEngine.Equations;

namespace TweenEngine.Equations
{
	/// <summary>
	/// Easing equation based on Robert Penner's work:
	/// http://robertpenner.com/easing/
	/// </summary>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public abstract class Elastic : TweenEquation
	{
		private const float PI = 3.14159265f;

		private sealed class ElasticIn : Elastic
		{
			public sealed override float Compute(float t)
			{
				float a = this.param_a;
				float p = this.param_p;
				if (t == 0)
				{
					return 0;
				}
				if (t == 1)
				{
					return 1;
				}
				if (!this.setP)
				{
					p = .3f;
				}
				float s;
				if (!this.setA || a < 1)
				{
					a = 1;
					s = p / 4;
				}
				else
				{
					s = p / (2 * Elastic.PI) * (float)Math.Asin(1 / a);
				}
				return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t - s) * (2 * Elastic
					.PI) / p));
			}

			public override string ToString()
			{
				return "Elastic.IN";
			}
		}

		public static readonly Elastic IN = new ElasticIn();

		private sealed class ElasticOut : Elastic
		{
			public sealed override float Compute(float t)
			{
				float a = this.param_a;
				float p = this.param_p;
				if (t == 0)
				{
					return 0;
				}
				if (t == 1)
				{
					return 1;
				}
				if (!this.setP)
				{
					p = .3f;
				}
				float s;
				if (!this.setA || a < 1)
				{
					a = 1;
					s = p / 4;
				}
				else
				{
					s = p / (2 * Elastic.PI) * (float)Math.Asin(1 / a);
				}
				return a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t - s) * (2 * Elastic.PI
					) / p) + 1;
			}

			public override string ToString()
			{
				return "Elastic.OUT";
			}
		}

		public static readonly Elastic OUT = new ElasticOut();

		private sealed class ElasticInOut : Elastic
		{
			public sealed override float Compute(float t)
			{
				float a = this.param_a;
				float p = this.param_p;
				if (t == 0)
				{
					return 0;
				}
				if ((t *= 2) == 2)
				{
					return 1;
				}
				if (!this.setP)
				{
					p = .3f * 1.5f;
				}
				float s;
				if (!this.setA || a < 1)
				{
					a = 1;
					s = p / 4;
				}
				else
				{
					s = p / (2 * Elastic.PI) * (float)Math.Asin(1 / a);
				}
				if (t < 1)
				{
					return -.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t - s) * 
						(2 * Elastic.PI) / p));
				}
				return a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin((t - s) * (2 * Elastic
					.PI) / p) * .5f + 1;
			}

			public override string ToString()
			{
				return "Elastic.INOUT";
			}
		}

		public static readonly Elastic INOUT = new ElasticInOut();

		protected internal float param_a;

		protected internal float param_p;

		protected internal bool setA = false;

		protected internal bool setP = false;

		// -------------------------------------------------------------------------
		public virtual Elastic A(float a)
		{
			param_a = a;
			this.setA = true;
			return this;
		}

		public virtual Elastic P(float p)
		{
			param_p = p;
			this.setP = true;
			return this;
		}
	}
}
