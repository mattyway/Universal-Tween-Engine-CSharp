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
	public class MutableInteger : TweenAccessor<MutableInteger>
	{
		private int value;

		public MutableInteger(int value)
		{
			this.value = value;
		}

		public virtual void SetValue(int value)
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

		public override int GetValues(MutableInteger target, int tweenType, float[] returnValues)
		{
			returnValues[0] = target.value;
			return 1;
		}

		public override void SetValues(MutableInteger target, int tweenType, float[] newValues)
		{
			target.value = (int)newValues[0];
		}
	}
}
