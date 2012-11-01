/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using System;
using System.Collections.Generic;
using TweenEngine;
using TweenEngine.Equations;

namespace TweenEngine
{
	/// <summary>Core class of the Tween Engine.</summary>
	/// <remarks>
	/// Core class of the Tween Engine. A Tween is basically an interpolation
	/// between two values of an object attribute. However, the main interest of a
	/// Tween is that you can apply an easing formula on this interpolation, in
	/// order to smooth the transitions or to achieve cool effects like springs or
	/// bounces.
	/// <p/>
	/// The Universal Tween Engine is called "universal" because it is able to apply
	/// interpolations on every attribute from every possible object. Therefore,
	/// every object in your application can be animated with cool effects: it does
	/// not matter if your application is a game, a desktop interface or even a
	/// console program! If it makes sense to animate something, then it can be
	/// animated through this engine.
	/// <p/>
	/// This class contains many static factory methods to create and instantiate
	/// new interpolations easily. The common way to create a Tween is by using one
	/// of these factories:
	/// <p/>
	/// - Tween.to(...)<br/>
	/// - Tween.from(...)<br/>
	/// - Tween.set(...)<br/>
	/// - Tween.call(...)
	/// <p/>
	/// <h2>Example - firing a Tween</h2>
	/// The following example will move the target horizontal position from its
	/// current value to x=200 and y=300, during 500ms, but only after a delay of
	/// 1000ms. The animation will also be repeated 2 times (the starting position
	/// is registered at the end of the delay, so the animation will automatically
	/// restart from this registered position).
	/// <p/>
	/// <pre>
	/// <code>
	/// Tween.to(myObject, POSITION_XY, 0.5f)
	/// .target(200, 300)
	/// .ease(Quad.INOUT)
	/// .delay(1.0f)
	/// .repeat(2, 0.2f)
	/// .start(myManager);
	/// </code>
	/// </pre>
	/// Tween life-cycles can be automatically managed for you, thanks to the
	/// <see cref="TweenManager">TweenManager</see>
	/// class. If you choose to manage your tween when you start
	/// it, then you don't need to care about it anymore. <b>Tweens are
	/// <i>fire-and-forget</i>: don't think about them anymore once you started
	/// them (if they are managed of course).</b>
	/// <p/>
	/// You need to periodicaly update the tween engine, in order to compute the new
	/// values. If your tweens are managed, only update the manager; else you need
	/// to call
	/// <see cref="BaseTween{T}.Update(float)">BaseTween&lt;T&gt;.Update(float)</see>
	/// on your tweens periodically.
	/// <p/>
	/// <h2>Example - setting up the engine</h2>
	/// The engine cannot directly change your objects attributes, since it doesn't
	/// know them. Therefore, you need to tell him how to get and set the different
	/// attributes of your objects: <b>you need to implement the
	/// <see cref="TweenAccessor{T}">TweenAccessor&lt;T&gt;</see>
	/// interface for each object class you will animate</b>. Once
	/// done, don't forget to register these implementations, using the static method
	/// <see>registerAccessor()</see>
	/// , when you start your application.
	/// </remarks>
	/// <seealso cref="TweenAccessor{T}">TweenAccessor&lt;T&gt;</seealso>
	/// <seealso cref="TweenManager">TweenManager</seealso>
	/// <seealso cref="TweenEquation">TweenEquation</seealso>
	/// <seealso cref="Timeline">Timeline</seealso>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public sealed class Tween : BaseTween<Tween>
	{
		/// <summary>
		/// Used as parameter in
		/// <see cref="BaseTween{T}.Repeat(int, float)">BaseTween&lt;T&gt;.Repeat(int, float)
		/// 	</see>
		/// and
		/// <see cref="BaseTween{T}.RepeatYoyo(int, float)">BaseTween&lt;T&gt;.RepeatYoyo(int, float)
		/// 	</see>
		/// methods.
		/// </summary>
		public const int INFINITY = -1;

		private static int combinedAttrsLimit = 3;

		private static int waypointsLimit = 0;

		// -------------------------------------------------------------------------
		// Static -- misc
		// -------------------------------------------------------------------------
		/// <summary>Changes the limit for combined attributes.</summary>
		/// <remarks>
		/// Changes the limit for combined attributes. Defaults to 3 to reduce
		/// memory footprint.
		/// </remarks>
		public static void SetCombinedAttributesLimit(int limit)
		{
			TweenEngine.Tween.combinedAttrsLimit = limit;
		}

		/// <summary>Changes the limit of allowed waypoints for each tween.</summary>
		/// <remarks>
		/// Changes the limit of allowed waypoints for each tween. Defaults to 0 to
		/// reduce memory footprint.
		/// </remarks>
		public static void SetWaypointsLimit(int limit)
		{
			TweenEngine.Tween.waypointsLimit = limit;
		}

		/// <summary>Gets the version number of the library.</summary>
		/// <remarks>Gets the version number of the library.</remarks>
		public static string GetVersion()
		{
			return "6.3.3";
		}

		// -------------------------------------------------------------------------
		// Static -- pool
		// -------------------------------------------------------------------------

		private sealed class _PoolCallback : Pool<TweenEngine.Tween>.Callback<TweenEngine.Tween>
		{
			public void OnPool(TweenEngine.Tween obj)
			{
				obj.Reset();
			}

			public void OnUnPool(TweenEngine.Tween obj)
			{
				obj.Reset();
			}
		}

		private static readonly Pool<TweenEngine.Tween>.Callback<TweenEngine.Tween> poolCallback = new _PoolCallback();

		private sealed class _Pool : Pool<TweenEngine.Tween>
		{
			public _Pool(int initCapacity, Pool<TweenEngine.Tween>.Callback<TweenEngine.Tween> callback)
				: base(initCapacity, callback)
			{
			}

			protected internal override TweenEngine.Tween Create()
			{
				return new TweenEngine.Tween();
			}
		}

		private static readonly Pool<TweenEngine.Tween> pool = new _Pool(20, poolCallback);

		/// <summary>Used for debug purpose.</summary>
		/// <remarks>
		/// Used for debug purpose. Gets the current number of objects that are
		/// waiting in the Tween pool.
		/// </remarks>
		public static int GetPoolSize()
		{
			return pool.Size();
		}

		/// <summary>Increases the minimum capacity of the pool.</summary>
		/// <remarks>Increases the minimum capacity of the pool. Capacity defaults to 20.</remarks>
		public static void EnsurePoolCapacity(int minCapacity)
		{
			pool.EnsureCapacity(minCapacity);
		}

		// -------------------------------------------------------------------------
		// Static -- tween accessors
		// -------------------------------------------------------------------------

		private static readonly IDictionary<Type, ITweenAccessor> registeredAccessors = new Dictionary<Type, ITweenAccessor>();

		/// <summary>Registers an accessor with the class of an object.</summary>
		/// <remarks>
		/// Registers an accessor with the class of an object. This accessor will be
		/// used by tweens applied to every objects implementing the registered
		/// class, or inheriting from it.
		/// </remarks>
		/// <param name="someClass">An object class.</param>
		/// <param name="defaultAccessor">
		/// The accessor that will be used to tween any
		/// object of class "someClass".
		/// </param>
		public static void RegisterAccessor(Type someClass, ITweenAccessor defaultAccessor)
		{
			registeredAccessors[someClass] = defaultAccessor;
		}

		/// <summary>Gets the registered TweenAccessor associated with the given object class.
		/// 	</summary>
		/// <remarks>Gets the registered TweenAccessor associated with the given object class.
		/// 	</remarks>
		/// <param name="someClass">An object class.</param>
		public static ITweenAccessor GetRegisteredAccessor(Type someClass)
		{
			return registeredAccessors[someClass];
		}

		// -------------------------------------------------------------------------
		// Static -- factories
		// -------------------------------------------------------------------------
		/// <summary>Factory creating a new standard interpolation.</summary>
		/// <remarks>
		/// Factory creating a new standard interpolation. This is the most common
		/// type of interpolation. The starting values are retrieved automatically
		/// after the delay (if any).
		/// <br/><br/>
		/// <b>You need to set the target values of the interpolation by using one
		/// of the target() methods</b>. The interpolation will run from the
		/// starting values to these target values.
		/// <br/><br/>
		/// The common use of Tweens is "fire-and-forget": you do not need to care
		/// for tweens once you added them to a TweenManager, they will be updated
		/// automatically, and cleaned once finished. Common call:
		/// <br/><br/>
		/// <pre>
		/// <code>
		/// Tween.to(myObject, POSITION, 1.0f)
		/// .target(50, 70)
		/// .ease(Quad.INOUT)
		/// .start(myManager);
		/// </code>
		/// </pre>
		/// Several options such as delay, repetitions and callbacks can be added to
		/// the tween.
		/// </remarks>
		/// <param name="target">The target object of the interpolation.</param>
		/// <param name="tweenType">The desired type of interpolation.</param>
		/// <param name="duration">The duration of the interpolation, in milliseconds.</param>
		/// <returns>The generated Tween.</returns>
		public static TweenEngine.Tween To(object target, int tweenType, float duration)
		{
			TweenEngine.Tween tween = pool.Get();
			tween.Setup(target, tweenType, duration);
			tween.Ease(Quad.INOUT);
			tween.Path(TweenPaths.catmullRom);
			return tween;
		}

		/// <summary>Factory creating a new reversed interpolation.</summary>
		/// <remarks>
		/// Factory creating a new reversed interpolation. The ending values are
		/// retrieved automatically after the delay (if any).
		/// <br/><br/>
		/// <b>You need to set the starting values of the interpolation by using one
		/// of the target() methods</b>. The interpolation will run from the
		/// starting values to these target values.
		/// <br/><br/>
		/// The common use of Tweens is "fire-and-forget": you do not need to care
		/// for tweens once you added them to a TweenManager, they will be updated
		/// automatically, and cleaned once finished. Common call:
		/// <br/><br/>
		/// <pre>
		/// <code>
		/// Tween.from(myObject, POSITION, 1.0f)
		/// .target(0, 0)
		/// .ease(Quad.INOUT)
		/// .start(myManager);
		/// </code>
		/// </pre>
		/// Several options such as delay, repetitions and callbacks can be added to
		/// the tween.
		/// </remarks>
		/// <param name="target">The target object of the interpolation.</param>
		/// <param name="tweenType">The desired type of interpolation.</param>
		/// <param name="duration">The duration of the interpolation, in milliseconds.</param>
		/// <returns>The generated Tween.</returns>
		public static TweenEngine.Tween From(object target, int tweenType, float duration)
		{
			TweenEngine.Tween tween = pool.Get();
			tween.Setup(target, tweenType, duration);
			tween.Ease(Quad.INOUT);
			tween.Path(TweenPaths.catmullRom);
			tween.isFrom = true;
			return tween;
		}

		/// <summary>
		/// Factory creating a new instantaneous interpolation (thus this is not
		/// really an interpolation).
		/// </summary>
		/// <remarks>
		/// Factory creating a new instantaneous interpolation (thus this is not
		/// really an interpolation).
		/// <br/><br/>
		/// <b>You need to set the target values of the interpolation by using one
		/// of the target() methods</b>. The interpolation will set the target
		/// attribute to these values after the delay (if any).
		/// <br/><br/>
		/// The common use of Tweens is "fire-and-forget": you do not need to care
		/// for tweens once you added them to a TweenManager, they will be updated
		/// automatically, and cleaned once finished. Common call:
		/// <br/><br/>
		/// <pre>
		/// <code>
		/// Tween.set(myObject, POSITION)
		/// .target(50, 70)
		/// .delay(1.0f)
		/// .start(myManager);
		/// </code>
		/// </pre>
		/// Several options such as delay, repetitions and callbacks can be added to
		/// the tween.
		/// </remarks>
		/// <param name="target">The target object of the interpolation.</param>
		/// <param name="tweenType">The desired type of interpolation.</param>
		/// <returns>The generated Tween.</returns>
		public static TweenEngine.Tween Set(object target, int tweenType)
		{
			TweenEngine.Tween tween = pool.Get();
			tween.Setup(target, tweenType, 0);
			tween.Ease(Quad.INOUT);
			return tween;
		}

		/// <summary>Factory creating a new timer.</summary>
		/// <remarks>
		/// Factory creating a new timer. The given callback will be triggered on
		/// each iteration start, after the delay.
		/// <br/><br/>
		/// The common use of Tweens is "fire-and-forget": you do not need to care
		/// for tweens once you added them to a TweenManager, they will be updated
		/// automatically, and cleaned once finished. Common call:
		/// <br/><br/>
		/// <pre>
		/// <code>
		/// Tween.call(myCallback)
		/// .delay(1.0f)
		/// .repeat(10, 1000)
		/// .start(myManager);
		/// </code>
		/// </pre>
		/// </remarks>
		/// <param name="callback">
		/// The callback that will be triggered on each iteration
		/// start.
		/// </param>
		/// <returns>The generated Tween.</returns>
		/// <seealso cref="TweenCallback">TweenCallback</seealso>
		public static TweenEngine.Tween Call(TweenCallback callback)
		{
			TweenEngine.Tween tween = pool.Get();
			tween.Setup(null, -1, 0);
			tween.SetCallback(callback);
			tween.SetCallbackTriggers(TweenCallback.START);
			return tween;
		}

		/// <summary>Convenience method to create an empty tween.</summary>
		/// <remarks>
		/// Convenience method to create an empty tween. Such object is only useful
		/// when placed inside animation sequences (see
		/// <see cref="Timeline">Timeline</see>
		/// ), in which
		/// it may act as a beacon, so you can set a callback on it in order to
		/// trigger some action at the right moment.
		/// </remarks>
		/// <returns>The generated Tween.</returns>
		/// <seealso cref="Timeline">Timeline</seealso>
		public static TweenEngine.Tween Mark()
		{
			TweenEngine.Tween tween = pool.Get();
			tween.Setup(null, -1, 0);
			return tween;
		}

		private object target;

		private Type targetClass;

		private ITweenAccessor accessor;

		private int type;

		private TweenEquation equation;

		private TweenPath path;

		private bool isFrom;

		private bool isRelative;

		private int combinedAttrsCnt;

		private int waypointsCnt;

		private readonly float[] startValues = new float[combinedAttrsLimit];

		private readonly float[] targetValues = new float[combinedAttrsLimit];

		private readonly float[] waypoints = new float[waypointsLimit * combinedAttrsLimit];

		private float[] accessorBuffer = new float[combinedAttrsLimit];

		private float[] pathBuffer = new float[(2 + waypointsLimit) * combinedAttrsLimit];

		public Tween()
		{
			// -------------------------------------------------------------------------
			// Attributes
			// -------------------------------------------------------------------------
			// Main
			// General
			// Values
			// Buffers
			// -------------------------------------------------------------------------
			// Setup
			// -------------------------------------------------------------------------
			Reset();
		}

		protected internal override void Reset()
		{
			base.Reset();
			target = null;
			targetClass = null;
			accessor = null;
			type = -1;
			equation = null;
			path = null;
			isFrom = isRelative = false;
			combinedAttrsCnt = waypointsCnt = 0;
			if (accessorBuffer.Length != combinedAttrsLimit)
			{
				accessorBuffer = new float[combinedAttrsLimit];
			}
			if (pathBuffer.Length != (2 + waypointsLimit) * combinedAttrsLimit)
			{
				pathBuffer = new float[(2 + waypointsLimit) * combinedAttrsLimit];
			}
		}

		private void Setup(object target, int tweenType, float duration)
		{
			if (duration < 0)
			{
				throw new ArgumentException("Duration can't be negative");
			}
			this.target = target;
			this.targetClass = target != null ? FindTargetClass() : null;
			this.type = tweenType;
			this.duration = duration;
		}

		private Type FindTargetClass()
		{
			if (registeredAccessors.ContainsKey(target.GetType()))
			{
				return target.GetType();
			}
			if (target is ITweenAccessor)
			{
				return target.GetType();
			}
			Type parentClass = target.GetType().BaseType;
			while (parentClass != null && !registeredAccessors.ContainsKey(parentClass))
			{
				parentClass = parentClass.BaseType;
			}
			return parentClass;
		}

		// -------------------------------------------------------------------------
		// Public API
		// -------------------------------------------------------------------------
		/// <summary>Sets the easing equation of the tween.</summary>
		/// <remarks>
		/// Sets the easing equation of the tween. Existing equations are located in
		/// <i>aurelienribon.tweenengine.equations</i> package, but you can of course
		/// implement your owns, see
		/// <see cref="TweenEquation">TweenEquation</see>
		/// . You can also use the
		/// <see cref="TweenEquations">TweenEquations</see>
		/// static instances to quickly access all the
		/// equations. Default equation is Quad.INOUT.
		/// <p/>
		/// <b>Proposed equations are:</b><br/>
		/// - Linear.INOUT,<br/>
		/// - Quad.IN | OUT | INOUT,<br/>
		/// - Cubic.IN | OUT | INOUT,<br/>
		/// - Quart.IN | OUT | INOUT,<br/>
		/// - Quint.IN | OUT | INOUT,<br/>
		/// - Circ.IN | OUT | INOUT,<br/>
		/// - Sine.IN | OUT | INOUT,<br/>
		/// - Expo.IN | OUT | INOUT,<br/>
		/// - Back.IN | OUT | INOUT,<br/>
		/// - Bounce.IN | OUT | INOUT,<br/>
		/// - Elastic.IN | OUT | INOUT
		/// </remarks>
		/// <returns>The current tween, for chaining instructions.</returns>
		/// <seealso cref="TweenEquation">TweenEquation</seealso>
		/// <seealso cref="TweenEquations">TweenEquations</seealso>
		public TweenEngine.Tween Ease(TweenEquation easeEquation)
		{
			this.equation = easeEquation;
			return this;
		}

		/// <summary>
		/// Forces the tween to use the TweenAccessor registered with the given
		/// target class.
		/// </summary>
		/// <remarks>
		/// Forces the tween to use the TweenAccessor registered with the given
		/// target class. Useful if you want to use a specific accessor associated
		/// to an interface, for instance.
		/// </remarks>
		/// <param name="targetClass">A class registered with an accessor.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Cast<_T0>(Type targetClass)
		{
			if (IsStarted())
			{
				throw new InvalidOperationException("You can't cast the target of a tween once it is started");
			}
			this.targetClass = targetClass;
			return this;
		}

		/// <summary>Sets the target value of the interpolation.</summary>
		/// <remarks>
		/// Sets the target value of the interpolation. The interpolation will run
		/// from the <b>value at start time (after the delay, if any)</b> to this
		/// target value.
		/// <p/>
		/// To sum-up:<br/>
		/// - start value: value at start time, after delay<br/>
		/// - end value: param
		/// </remarks>
		/// <param name="targetValue">The target value of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Target(float targetValue)
		{
			targetValues[0] = targetValue;
			return this;
		}

		/// <summary>Sets the target values of the interpolation.</summary>
		/// <remarks>
		/// Sets the target values of the interpolation. The interpolation will run
		/// from the <b>values at start time (after the delay, if any)</b> to these
		/// target values.
		/// <p/>
		/// To sum-up:<br/>
		/// - start values: values at start time, after delay<br/>
		/// - end values: params
		/// </remarks>
		/// <param name="targetValue1">The 1st target value of the interpolation.</param>
		/// <param name="targetValue2">The 2nd target value of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Target(float targetValue1, float targetValue2)
		{
			targetValues[0] = targetValue1;
			targetValues[1] = targetValue2;
			return this;
		}

		/// <summary>Sets the target values of the interpolation.</summary>
		/// <remarks>
		/// Sets the target values of the interpolation. The interpolation will run
		/// from the <b>values at start time (after the delay, if any)</b> to these
		/// target values.
		/// <p/>
		/// To sum-up:<br/>
		/// - start values: values at start time, after delay<br/>
		/// - end values: params
		/// </remarks>
		/// <param name="targetValue1">The 1st target value of the interpolation.</param>
		/// <param name="targetValue2">The 2nd target value of the interpolation.</param>
		/// <param name="targetValue3">The 3rd target value of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Target(float targetValue1, float targetValue2, float targetValue3
			)
		{
			targetValues[0] = targetValue1;
			targetValues[1] = targetValue2;
			targetValues[2] = targetValue3;
			return this;
		}

		/// <summary>Sets the target values of the interpolation.</summary>
		/// <remarks>
		/// Sets the target values of the interpolation. The interpolation will run
		/// from the <b>values at start time (after the delay, if any)</b> to these
		/// target values.
		/// <p/>
		/// To sum-up:<br/>
		/// - start values: values at start time, after delay<br/>
		/// - end values: params
		/// </remarks>
		/// <param name="targetValues">The target values of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Target(params float[] targetValues)
		{
			if (targetValues.Length > combinedAttrsLimit)
			{
				ThrowCombinedAttrsLimitReached();
			}
			System.Array.Copy(targetValues, 0, this.targetValues, 0, targetValues.Length);
			return this;
		}

		/// <summary>
		/// Sets the target value of the interpolation, relatively to the <b>value
		/// at start time (after the delay, if any)</b>.
		/// </summary>
		/// <remarks>
		/// Sets the target value of the interpolation, relatively to the <b>value
		/// at start time (after the delay, if any)</b>.
		/// <p/>
		/// To sum-up:<br/>
		/// - start value: value at start time, after delay<br/>
		/// - end value: param + value at start time, after delay
		/// </remarks>
		/// <param name="targetValue">The relative target value of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween TargetRelative(float targetValue)
		{
			isRelative = true;
			targetValues[0] = IsInitialized() ? targetValue + startValues[0] : targetValue;
			return this;
		}

		/// <summary>
		/// Sets the target values of the interpolation, relatively to the <b>values
		/// at start time (after the delay, if any)</b>.
		/// </summary>
		/// <remarks>
		/// Sets the target values of the interpolation, relatively to the <b>values
		/// at start time (after the delay, if any)</b>.
		/// <p/>
		/// To sum-up:<br/>
		/// - start values: values at start time, after delay<br/>
		/// - end values: params + values at start time, after delay
		/// </remarks>
		/// <param name="targetValue1">The 1st relative target value of the interpolation.</param>
		/// <param name="targetValue2">The 2nd relative target value of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween TargetRelative(float targetValue1, float targetValue2)
		{
			isRelative = true;
			targetValues[0] = IsInitialized() ? targetValue1 + startValues[0] : targetValue1;
			targetValues[1] = IsInitialized() ? targetValue2 + startValues[1] : targetValue2;
			return this;
		}

		/// <summary>
		/// Sets the target values of the interpolation, relatively to the <b>values
		/// at start time (after the delay, if any)</b>.
		/// </summary>
		/// <remarks>
		/// Sets the target values of the interpolation, relatively to the <b>values
		/// at start time (after the delay, if any)</b>.
		/// <p/>
		/// To sum-up:<br/>
		/// - start values: values at start time, after delay<br/>
		/// - end values: params + values at start time, after delay
		/// </remarks>
		/// <param name="targetValue1">The 1st relative target value of the interpolation.</param>
		/// <param name="targetValue2">The 2nd relative target value of the interpolation.</param>
		/// <param name="targetValue3">The 3rd relative target value of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween TargetRelative(float targetValue1, float targetValue2, float
			 targetValue3)
		{
			isRelative = true;
			targetValues[0] = IsInitialized() ? targetValue1 + startValues[0] : targetValue1;
			targetValues[1] = IsInitialized() ? targetValue2 + startValues[1] : targetValue2;
			targetValues[2] = IsInitialized() ? targetValue3 + startValues[2] : targetValue3;
			return this;
		}

		/// <summary>
		/// Sets the target values of the interpolation, relatively to the <b>values
		/// at start time (after the delay, if any)</b>.
		/// </summary>
		/// <remarks>
		/// Sets the target values of the interpolation, relatively to the <b>values
		/// at start time (after the delay, if any)</b>.
		/// <p/>
		/// To sum-up:<br/>
		/// - start values: values at start time, after delay<br/>
		/// - end values: params + values at start time, after delay
		/// </remarks>
		/// <param name="targetValues">The relative target values of the interpolation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween TargetRelative(params float[] targetValues)
		{
			if (targetValues.Length > combinedAttrsLimit)
			{
				ThrowCombinedAttrsLimitReached();
			}
			for (int i = 0; i < targetValues.Length; i++)
			{
				this.targetValues[i] = IsInitialized() ? targetValues[i] + startValues[i] : targetValues
					[i];
			}
			isRelative = true;
			return this;
		}

		/// <summary>Adds a waypoint to the path.</summary>
		/// <remarks>
		/// Adds a waypoint to the path. The default path runs from the start values
		/// to the end values linearly. If you add waypoints, the default path will
		/// use a smooth catmull-rom spline to navigate between the waypoints, but
		/// you can change this behavior by using the
		/// <see cref="Path(TweenPath)">Path(TweenPath)</see>
		/// method.
		/// </remarks>
		/// <param name="targetValue">The target of this waypoint.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Waypoint(float targetValue)
		{
			if (waypointsCnt == waypointsLimit)
			{
				ThrowWaypointsLimitReached();
			}
			waypoints[waypointsCnt] = targetValue;
			waypointsCnt += 1;
			return this;
		}

		/// <summary>Adds a waypoint to the path.</summary>
		/// <remarks>
		/// Adds a waypoint to the path. The default path runs from the start values
		/// to the end values linearly. If you add waypoints, the default path will
		/// use a smooth catmull-rom spline to navigate between the waypoints, but
		/// you can change this behavior by using the
		/// <see cref="Path(TweenPath)">Path(TweenPath)</see>
		/// method.
		/// <p/>
		/// Note that if you want waypoints relative to the start values, use one of
		/// the .targetRelative() methods to define your target.
		/// </remarks>
		/// <param name="targetValue1">The 1st target of this waypoint.</param>
		/// <param name="targetValue2">The 2nd target of this waypoint.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Waypoint(float targetValue1, float targetValue2)
		{
			if (waypointsCnt == waypointsLimit)
			{
				ThrowWaypointsLimitReached();
			}
			waypoints[waypointsCnt * 2] = targetValue1;
			waypoints[waypointsCnt * 2 + 1] = targetValue2;
			waypointsCnt += 1;
			return this;
		}

		/// <summary>Adds a waypoint to the path.</summary>
		/// <remarks>
		/// Adds a waypoint to the path. The default path runs from the start values
		/// to the end values linearly. If you add waypoints, the default path will
		/// use a smooth catmull-rom spline to navigate between the waypoints, but
		/// you can change this behavior by using the
		/// <see cref="Path(TweenPath)">Path(TweenPath)</see>
		/// method.
		/// <p/>
		/// Note that if you want waypoints relative to the start values, use one of
		/// the .targetRelative() methods to define your target.
		/// </remarks>
		/// <param name="targetValue1">The 1st target of this waypoint.</param>
		/// <param name="targetValue2">The 2nd target of this waypoint.</param>
		/// <param name="targetValue3">The 3rd target of this waypoint.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Waypoint(float targetValue1, float targetValue2, float targetValue3
			)
		{
			if (waypointsCnt == waypointsLimit)
			{
				ThrowWaypointsLimitReached();
			}
			waypoints[waypointsCnt * 3] = targetValue1;
			waypoints[waypointsCnt * 3 + 1] = targetValue2;
			waypoints[waypointsCnt * 3 + 2] = targetValue3;
			waypointsCnt += 1;
			return this;
		}

		/// <summary>Adds a waypoint to the path.</summary>
		/// <remarks>
		/// Adds a waypoint to the path. The default path runs from the start values
		/// to the end values linearly. If you add waypoints, the default path will
		/// use a smooth catmull-rom spline to navigate between the waypoints, but
		/// you can change this behavior by using the
		/// <see cref="Path(TweenPath)">Path(TweenPath)</see>
		/// method.
		/// <p/>
		/// Note that if you want waypoints relative to the start values, use one of
		/// the .targetRelative() methods to define your target.
		/// </remarks>
		/// <param name="targetValues">The targets of this waypoint.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		public TweenEngine.Tween Waypoint(params float[] targetValues)
		{
			if (waypointsCnt == waypointsLimit)
			{
				ThrowWaypointsLimitReached();
			}
			System.Array.Copy(targetValues, 0, waypoints, waypointsCnt * targetValues.Length, 
				targetValues.Length);
			waypointsCnt += 1;
			return this;
		}

		/// <summary>
		/// Sets the algorithm that will be used to navigate through the waypoints,
		/// from the start values to the end values.
		/// </summary>
		/// <remarks>
		/// Sets the algorithm that will be used to navigate through the waypoints,
		/// from the start values to the end values. Default is a catmull-rom spline,
		/// but you can find other paths in the
		/// <see cref="TweenPaths">TweenPaths</see>
		/// class.
		/// </remarks>
		/// <param name="path">A TweenPath implementation.</param>
		/// <returns>The current tween, for chaining instructions.</returns>
		/// <seealso cref="TweenPath">TweenPath</seealso>
		/// <seealso cref="TweenPaths">TweenPaths</seealso>
		public TweenEngine.Tween Path(TweenPath path)
		{
			this.path = path;
			return this;
		}

		// -------------------------------------------------------------------------
		// Getters
		// -------------------------------------------------------------------------
		/// <summary>Gets the target object.</summary>
		/// <remarks>Gets the target object.</remarks>
		public object GetTarget()
		{
			return target;
		}

		/// <summary>Gets the type of the tween.</summary>
		/// <remarks>Gets the type of the tween.</remarks>
		public int GetType()
		{
			return type;
		}

		/// <summary>Gets the easing equation.</summary>
		/// <remarks>Gets the easing equation.</remarks>
		public TweenEquation GetEasing()
		{
			return equation;
		}

		/// <summary>Gets the target values.</summary>
		/// <remarks>
		/// Gets the target values. The returned buffer is as long as the maximum
		/// allowed combined values. Therefore, you're surely not interested in all
		/// its content. Use
		/// <see cref="#getCombinedTweenCount()">#getCombinedTweenCount()</see>
		/// to get the number of
		/// interesting slots.
		/// </remarks>
		public float[] GetTargetValues()
		{
			return targetValues;
		}

		/// <summary>Gets the number of combined animations.</summary>
		/// <remarks>Gets the number of combined animations.</remarks>
		public int GetCombinedAttributesCount()
		{
			return combinedAttrsCnt;
		}

		/// <summary>Gets the TweenAccessor used with the target.</summary>
		/// <remarks>Gets the TweenAccessor used with the target.</remarks>
		public ITweenAccessor GetAccessor()
		{
			return accessor;
		}

		/// <summary>Gets the class that was used to find the associated TweenAccessor.</summary>
		/// <remarks>Gets the class that was used to find the associated TweenAccessor.</remarks>
		public Type GetTargetClass()
		{
			return targetClass;
		}

		// -------------------------------------------------------------------------
		// Overrides
		// -------------------------------------------------------------------------
		public override Tween Build()
		{
			if (target == null)
			{
				return this;
			}
			accessor = (ITweenAccessor)registeredAccessors[targetClass];
			if (accessor == null && target is TweenAccessor<object>)
			{
				accessor = (TweenAccessor<object>)target;
			}
			if (accessor != null)
			{
				combinedAttrsCnt = accessor._GetValues(target, type, accessorBuffer);
			}
			else
			{
				throw new InvalidOperationException("No TweenAccessor was found for the target");
			}
			if (combinedAttrsCnt > combinedAttrsLimit)
			{
				ThrowCombinedAttrsLimitReached();
			}
			return this;
		}

		public override void Free()
		{
			pool.Free(this);
		}

		protected internal override void InitializeOverride()
		{
			if (target == null)
			{
				return;
			}
			accessor._GetValues(target, type, startValues);
			for (int i = 0; i < combinedAttrsCnt; i++)
			{
				targetValues[i] += isRelative ? startValues[i] : 0;
				for (int ii = 0; ii < waypointsCnt; ii++)
				{
					waypoints[ii * combinedAttrsCnt + i] += isRelative ? startValues[i] : 0;
				}
				if (isFrom)
				{
					float tmp = startValues[i];
					startValues[i] = targetValues[i];
					targetValues[i] = tmp;
				}
			}
		}

		protected internal override void UpdateOverride(int step, int lastStep, bool isIterationStep
			, float delta)
		{
			if (target == null || equation == null)
			{
				return;
			}
			// Case iteration end has been reached
			if (!isIterationStep && step > lastStep)
			{
				accessor._SetValues(target, type, IsReverse(lastStep) ? startValues : targetValues);
				return;
			}
			if (!isIterationStep && step < lastStep)
			{
				accessor._SetValues(target, type, IsReverse(lastStep) ? targetValues : startValues);
				return;
			}
			// Validation
			//assert getCurrentTime() >= 0;
			//assert getCurrentTime() <= duration;

			// Case duration equals zero

			if (duration < 0.00000000001f && delta > -0.00000000001f)
			{
				accessor._SetValues(target, type, IsReverse(step) ? targetValues : startValues);
				return;
			}
			if (duration < 0.00000000001f && delta < 0.00000000001f)
			{
				accessor._SetValues(target, type, IsReverse(step) ? startValues : targetValues);
				return;
			}
			// Normal behavior
			float time = IsReverse(step) ? duration - GetCurrentTime() : GetCurrentTime();
			float t = equation.Compute(time / duration);
			if (waypointsCnt == 0 || path == null)
			{
				for (int i = 0; i < combinedAttrsCnt; i++)
				{
					accessorBuffer[i] = startValues[i] + t * (targetValues[i] - startValues[i]);
				}
			}
			else
			{
				for (int i = 0; i < combinedAttrsCnt; i++)
				{
					pathBuffer[0] = startValues[i];
					pathBuffer[1 + waypointsCnt] = targetValues[i];
					for (int ii = 0; ii < waypointsCnt; ii++)
					{
						pathBuffer[ii + 1] = waypoints[ii * combinedAttrsCnt + i];
					}
					accessorBuffer[i] = path.Compute(t, pathBuffer, waypointsCnt + 2);
				}
			}
			accessor._SetValues(target, type, accessorBuffer);
		}

		// -------------------------------------------------------------------------
		// BaseTween impl.
		// -------------------------------------------------------------------------
		protected internal override void ForceStartValues()
		{
			if (target == null)
			{
				return;
			}
			accessor._SetValues(target, type, startValues);
		}

		protected internal override void ForceEndValues()
		{
			if (target == null)
			{
				return;
			}
			accessor._SetValues(target, type, targetValues);
		}

		protected internal override bool ContainsTarget(object target)
		{
			return this.target == target;
		}

		protected internal override bool ContainsTarget(object target, int tweenType)
		{
			return this.target == target && this.type == tweenType;
		}

		protected override Tween ThisAsT()
		{
			return this;
		}

		// -------------------------------------------------------------------------
		// Helpers
		// -------------------------------------------------------------------------
		private void ThrowCombinedAttrsLimitReached()
		{
			string msg = "You cannot combine more than " + combinedAttrsLimit + " " + "attributes in a tween. You can raise this limit with "
				 + "Tween.setCombinedAttributesLimit(), which should be called once " + "in application initialization code.";
			throw new InvalidOperationException(msg);
		}

		private void ThrowWaypointsLimitReached()
		{
			string msg = "You cannot add more than " + waypointsLimit + " " + "waypoints to a tween. You can raise this limit with "
				 + "Tween.setWaypointsLimit(), which should be called once in " + "application initialization code.";
			throw new InvalidOperationException(msg);
		}
	}
}
