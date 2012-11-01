/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using System;
using System.Collections.Generic;
using TweenEngine;

namespace TweenEngine
{
	/// <summary>
	/// A Timeline can be used to create complex animations made of sequences and
	/// parallel sets of Tweens.
	/// </summary>
	/// <remarks>
	/// A Timeline can be used to create complex animations made of sequences and
	/// parallel sets of Tweens.
	/// <p/>
	/// The following example will create an animation sequence composed of 5 parts:
	/// <p/>
	/// 1. First, opacity and scale are set to 0 (with Tween.set() calls).<br/>
	/// 2. Then, opacity and scale are animated in parallel.<br/>
	/// 3. Then, the animation is paused for 1s.<br/>
	/// 4. Then, position is animated to x=100.<br/>
	/// 5. Then, rotation is animated to 360Â°.
	/// <p/>
	/// This animation will be repeated 5 times, with a 500ms delay between each
	/// iteration:
	/// <br/><br/>
	/// <pre>
	/// <code>
	/// Timeline.createSequence()
	/// .push(Tween.set(myObject, OPACITY).target(0))
	/// .push(Tween.set(myObject, SCALE).target(0, 0))
	/// .beginParallel()
	/// .push(Tween.to(myObject, OPACITY, 0.5f).target(1).ease(Quad.INOUT))
	/// .push(Tween.to(myObject, SCALE, 0.5f).target(1, 1).ease(Quad.INOUT))
	/// .end()
	/// .pushPause(1.0f)
	/// .push(Tween.to(myObject, POSITION_X, 0.5f).target(100).ease(Quad.INOUT))
	/// .push(Tween.to(myObject, ROTATION, 0.5f).target(360).ease(Quad.INOUT))
	/// .repeat(5, 0.5f)
	/// .start(myManager);
	/// </code>
	/// </pre>
	/// </remarks>
	/// <seealso cref="Tween">Tween</seealso>
	/// <seealso cref="TweenManager">TweenManager</seealso>
	/// <seealso cref="TweenCallback">TweenCallback</seealso>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public sealed class Timeline : BaseTween<Timeline>
	{
		// -------------------------------------------------------------------------
		// Static -- pool
		// -------------------------------------------------------------------------

		private sealed class _Callback_51 : Pool<Timeline>.Callback<Timeline>
		{
			public void OnPool(Timeline obj)
			{
				obj.Reset();
			}

			public void OnUnPool(Timeline obj)
			{
				obj.Reset();
			}
		}

		private static readonly Pool<Timeline>.Callback<Timeline> poolCallback = new _Callback_51();

		private sealed class _Pool_56 : Pool<Timeline>
		{
			public _Pool_56(int baseArg1, Pool<Timeline>.Callback<Timeline> baseArg2)
				: base
				(baseArg1, baseArg2)
			{
			}

			protected internal override Timeline Create()
			{
				return new Timeline();
			}
		}

		internal static readonly Pool<Timeline> pool = new _Pool_56(10, poolCallback);

		/// <summary>Used for debug purpose.</summary>
		/// <remarks>
		/// Used for debug purpose. Gets the current number of empty timelines that
		/// are waiting in the Timeline pool.
		/// </remarks>
		public static int GetPoolSize()
		{
			return pool.Size();
		}

		/// <summary>Increases the minimum capacity of the pool.</summary>
		/// <remarks>Increases the minimum capacity of the pool. Capacity defaults to 10.</remarks>
		public static void EnsurePoolCapacity(int minCapacity)
		{
			pool.EnsureCapacity(minCapacity);
		}

		// -------------------------------------------------------------------------
		// Static -- factories
		// -------------------------------------------------------------------------

		/// <summary>Creates a new timeline with a 'sequence' behavior.</summary>
		/// <remarks>
		/// Creates a new timeline with a 'sequence' behavior. Its children will
		/// be delayed so that they are triggered one after the other.
		/// </remarks>
		public static Timeline CreateSequence()
		{
			Timeline tl = pool.Get();
			tl.Setup(Timeline.Modes.SEQUENCE);
			return tl;
		}

		/// <summary>Creates a new timeline with a 'parallel' behavior.</summary>
		/// <remarks>
		/// Creates a new timeline with a 'parallel' behavior. Its children will be
		/// triggered all at once.
		/// </remarks>
		public static Timeline CreateParallel()
		{
			Timeline tl = pool.Get();
			tl.Setup(Timeline.Modes.PARALLEL);
			return tl;
		}

		// -------------------------------------------------------------------------
		// Attributes
		// -------------------------------------------------------------------------

		private enum Modes
		{
			SEQUENCE,
			PARALLEL
		}

		private IList<IBaseTween> children = new List<IBaseTween>(10);

		private Timeline current;

		private Timeline parent;

		private Timeline.Modes mode;

		private bool isBuilt;

		// -------------------------------------------------------------------------
		// Setup
		// -------------------------------------------------------------------------

		public Timeline()
		{
			Reset();
		}

		protected internal override void Reset()
		{
			base.Reset();
			children.Clear();
			current = parent = null;
			isBuilt = false;
		}

		private void Setup(Timeline.Modes mode)
		{
			this.mode = mode;
			this.current = this;
		}

		// -------------------------------------------------------------------------
		// Public API
		// -------------------------------------------------------------------------
		/// <summary>Adds a Tween to the current timeline.</summary>
		/// <remarks>Adds a Tween to the current timeline.</remarks>
		/// <returns>The current timeline, for chaining instructions.</returns>
		public Timeline Push(Tween tween)
		{
			if (isBuilt)
			{
				throw new InvalidOperationException("You can't push anything to a timeline once it is started");
			}
			current.children.Add(tween);
			return this;
		}

		/// <summary>Nests a Timeline in the current one.</summary>
		/// <remarks>Nests a Timeline in the current one.</remarks>
		/// <returns>The current timeline, for chaining instructions.</returns>
		public Timeline Push(Timeline timeline)
		{
			if (isBuilt)
			{
				throw new InvalidOperationException("You can't push anything to a timeline once it is started");
			}
			if (timeline.current != timeline)
			{
				throw new InvalidOperationException("You forgot to call a few 'end()' statements in your pushed timeline");
			}
			timeline.parent = current;
			current.children.Add((IBaseTween)timeline);
			return this;
		}

		/// <summary>Adds a pause to the timeline.</summary>
		/// <remarks>
		/// Adds a pause to the timeline. The pause may be negative if you want to
		/// overlap the preceding and following children.
		/// </remarks>
		/// <param name="time">A positive or negative duration.</param>
		/// <returns>The current timeline, for chaining instructions.</returns>
		public Timeline PushPause(float time)
		{
			if (isBuilt)
			{
				throw new InvalidOperationException("You can't push anything to a timeline once it is started");
			}
			current.children.Add(Tween.Mark().Delay(time));
			return this;
		}

		/// <summary>Starts a nested timeline with a 'sequence' behavior.</summary>
		/// <remarks>
		/// Starts a nested timeline with a 'sequence' behavior. Don't forget to
		/// call
		/// <see>end()</see>
		/// to close this nested timeline.
		/// </remarks>
		/// <returns>The current timeline, for chaining instructions.</returns>
		public Timeline BeginSequence()
		{
			if (isBuilt)
			{
				throw new InvalidOperationException("You can't push anything to a timeline once it is started");
			}
			Timeline tl = pool.Get();
			tl.parent = current;
			tl.mode = Timeline.Modes.SEQUENCE;
			current.children.Add(tl);
			current = tl;
			return this;
		}

		/// <summary>Starts a nested timeline with a 'parallel' behavior.</summary>
		/// <remarks>
		/// Starts a nested timeline with a 'parallel' behavior. Don't forget to
		/// call
		/// <see>end()</see>
		/// to close this nested timeline.
		/// </remarks>
		/// <returns>The current timeline, for chaining instructions.</returns>
		public Timeline BeginParallel()
		{
			if (isBuilt)
			{
				throw new InvalidOperationException("You can't push anything to a timeline once it is started");
			}
			Timeline tl = pool.Get();
			tl.parent = current;
			tl.mode = Timeline.Modes.PARALLEL;
			current.children.Add(tl);
			current = tl;
			return this;
		}

		/// <summary>Closes the last nested timeline.</summary>
		/// <remarks>Closes the last nested timeline.</remarks>
		/// <returns>The current timeline, for chaining instructions.</returns>
		public Timeline End()
		{
			if (isBuilt)
			{
				throw new InvalidOperationException("You can't push anything to a timeline once it is started");
			}
			if (current == this)
			{
				throw new InvalidOperationException("Nothing to end...");
			}
			current = current.parent;
			return this;
		}

		/// <summary>Gets a list of the timeline children.</summary>
		/// <remarks>
		/// Gets a list of the timeline children. If the timeline is started, the
		/// list will be immutable.
		/// </remarks>
		public IList<IBaseTween> GetChildren()
		{
			if (isBuilt)
			{
				//return Sharpen.Collections.UnmodifiableList(current.children);

				// @TODO: Return a readonly collection
				return current.children;
			}
			else
			{
				return current.children;
			}
		}

		// -------------------------------------------------------------------------
		// Overrides
		// -------------------------------------------------------------------------
		public override Timeline Build()
		{
			if (isBuilt)
			{
				return this;
			}
			duration = 0;
			for (int i = 0; i < children.Count; i++)
			{
				IBaseTween obj = children[i];
				if (obj.GetRepeatCount() < 0)
				{
					throw new InvalidOperationException("You can't push an object with infinite repetitions in a timeline");
				}
				obj._Build();
				switch (mode)
				{
					case Timeline.Modes.SEQUENCE:
					{
						float tDelay = duration;
						duration += obj.GetFullDuration();
						obj.delay += tDelay;
						break;
					}

					case Timeline.Modes.PARALLEL:
					{
						duration = Math.Max(duration, obj.GetFullDuration());
						break;
					}
				}
			}
			isBuilt = true;
			return this;
		}

		public override Timeline Start()
		{
			base.Start();
			for (int i = 0; i < children.Count; i++)
			{
				IBaseTween obj = children[i];
				obj._Start();
			}
			return this;
		}

		public override void Free()
		{
			for (int i = children.Count - 1; i >= 0; i--)
			{
				IBaseTween obj = children[i];
				children.RemoveAt(i);
				obj.Free();
			}
			pool.Free(this);
		}

		protected internal override void UpdateOverride(int step, int lastStep, bool isIterationStep, float delta)
		{
			if (!isIterationStep && step > lastStep)
			{
				//assert delta >= 0
				float dt = IsReverse(lastStep) ? -delta - 1 : delta + 1;
				for (int i = 0, n = children.Count; i < n; i++)
				{
					children[i].Update(dt);
				}
				return;
			}
			if (!isIterationStep && step < lastStep)
			{
				//assert delta <= 0
				float dt = IsReverse(lastStep) ? -delta - 1 : delta + 1;
				for (int i = children.Count - 1; i >= 0; i--)
				{
					children[i].Update(dt);
				}
				return;
			}

			//assert isIterationStep

			if (step > lastStep)
			{
				if (IsReverse(step))
				{
					ForceEndValues();
					for (int i = 0, n = children.Count; i < n; i++)
					{
						children[i].Update(delta);
					}
				}
				else
				{
					ForceStartValues();
					for (int i = 0, n = children.Count; i < n; i++)
					{
						children[i].Update(delta);
					}
				}
			}
			else if (step < lastStep)
			{
				if (IsReverse(step))
				{
					ForceStartValues();
					for (int i = children.Count - 1; i >= 0; i--)
					{
						children[i].Update(delta);
					}
				}
				else
				{
					ForceEndValues();
					for (int i = children.Count - 1; i >= 0; i--)
					{
						children[i].Update(delta);
					}
				}
			}
			else
			{
				float dt = IsReverse(step) ? -delta : delta;
				if (delta >= 0)
				{
					for (int i = 0, n = children.Count; i < n; i++)
					{
						children[i].Update(dt);
					}
				}
				else
				{
					for (int i = children.Count - 1; i >= 0; i--)
					{
						children[i].Update(dt);
					}
				}
			}
		}

		// -------------------------------------------------------------------------
		// BaseTween impl.
		// -------------------------------------------------------------------------
		protected internal override void ForceStartValues()
		{
			for (int i = children.Count - 1; i >= 0; i--)
			{
				IBaseTween obj = children[i];
				obj.ForceToStart();
			}
		}

		protected internal override void ForceEndValues()
		{
			for (int i = 0, n = children.Count; i < n; i++)
			{
				IBaseTween obj = children[i];
				obj.ForceToEnd(duration);
			}
		}

		protected internal override bool ContainsTarget(object target)
		{
			for (int i = 0, n = children.Count; i < n; i++)
			{
				IBaseTween obj = children[i];
				if (obj.ContainsTarget(target))
				{
					return true;
				}
			}
			return false;
		}

		protected internal override bool ContainsTarget(object target, int tweenType)
		{
			for (int i = 0, n = children.Count; i < n; i++)
			{
				IBaseTween obj = children[i];
				if (obj.ContainsTarget(target, tweenType))
				{
					return true;
				}
			}
			return false;
		}

		protected override Timeline ThisAsT()
		{
			return this;
		}
	}
}
