/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;
using TweenEngine.Equations;

namespace TweenEngine
{
	/// <summary>Collection of built-in easing equations</summary>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public abstract class TweenEquations
	{
		public static readonly Linear easeNone = Linear.INOUT;

		public static readonly Quad easeInQuad = Quad.IN;

		public static readonly Quad easeOutQuad = Quad.OUT;

		public static readonly Quad easeInOutQuad = Quad.INOUT;

		public static readonly Cubic easeInCubic = Cubic.IN;

		public static readonly Cubic easeOutCubic = Cubic.OUT;

		public static readonly Cubic easeInOutCubic = Cubic.INOUT;

		public static readonly Quart easeInQuart = Quart.IN;

		public static readonly Quart easeOutQuart = Quart.OUT;

		public static readonly Quart easeInOutQuart = Quart.INOUT;

		public static readonly Quint easeInQuint = Quint.IN;

		public static readonly Quint easeOutQuint = Quint.OUT;

		public static readonly Quint easeInOutQuint = Quint.INOUT;

		public static readonly Circ easeInCirc = Circ.IN;

		public static readonly Circ easeOutCirc = Circ.OUT;

		public static readonly Circ easeInOutCirc = Circ.INOUT;

		public static readonly Sine easeInSine = Sine.IN;

		public static readonly Sine easeOutSine = Sine.OUT;

		public static readonly Sine easeInOutSine = Sine.INOUT;

		public static readonly Expo easeInExpo = Expo.IN;

		public static readonly Expo easeOutExpo = Expo.OUT;

		public static readonly Expo easeInOutExpo = Expo.INOUT;

		public static readonly Back easeInBack = Back.IN;

		public static readonly Back easeOutBack = Back.OUT;

		public static readonly Back easeInOutBack = Back.INOUT;

		public static readonly Bounce easeInBounce = Bounce.IN;

		public static readonly Bounce easeOutBounce = Bounce.OUT;

		public static readonly Bounce easeInOutBounce = Bounce.INOUT;

		public static readonly Elastic easeInElastic = Elastic.IN;

		public static readonly Elastic easeOutElastic = Elastic.OUT;

		public static readonly Elastic easeInOutElastic = Elastic.INOUT;
	}
}
