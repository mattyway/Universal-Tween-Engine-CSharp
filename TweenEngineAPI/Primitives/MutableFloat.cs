/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;

namespace TweenEngine.Primitives
{
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	[System.Serializable]
	public class MutableFloat : TweenAccessor<MutableFloat>
	{
		private float value;

		public MutableFloat(float value)
		{
			this.value = value;
		}

		public virtual void SetValue(float value)
		{
			this.value = value;
		}

		public int IntValue()
		{
			return (int)value;
		}

		public long LongValue()
		{
			return (long)value;
		}

		public float FloatValue()
		{
			return (float)value;
		}

		public double DoubleValue()
		{
			return (double)value;
		}

		public override int GetValues(MutableFloat target, int tweenType, float[] returnValues)
		{
			returnValues[0] = target.value;
			return 1;
		}

		public override void SetValues(MutableFloat target, int tweenType, float[] newValues)
		{
			target.value = newValues[0];
		}
	}
}
