/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 *
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using System;
using TweenEngine;

namespace TweenEngine
{
	public abstract class IBaseTween
	{
		internal IBaseTween() { }

		// General
		private int step;
		private int repeatCnt;
		private bool isIterationStep;
		private bool isYoyo;

		// Timings
		protected internal float delay;
		protected internal float duration;
		private float repeatDelay;
		private float currentTime;
		private float deltaTime;
		private bool isStarted; // true when the object is started
		private bool isInitialized; // true after the delay
		private bool isFinished; // true when all repetitions are done
		private bool isKilled; // true if kill() was called
		private bool isPaused; // true if pause() was called

		// Misc
		private TweenCallback callback;
		private int callbackTriggers;
		private object userData;

		// Package access
		internal bool isAutoRemoveEnabled;
		internal bool isAutoStartEnabled;

		// -------------------------------------------------------------------------

		protected internal virtual void Reset()
		{
			step = -2;
			repeatCnt = 0;
			isIterationStep = isYoyo = false;
			delay = duration = repeatDelay = currentTime = deltaTime = 0;
			isStarted = isInitialized = isFinished = isKilled = isPaused = false;
			callback = null;
			callbackTriggers = TweenCallback.COMPLETE;
			userData = null;
			isAutoRemoveEnabled = isAutoStartEnabled = true;
		}

		/// These "Internal*" methods are part of a workaround for C# not supporting wildcards in generics.
		/// Because there is no C# equivalent to Java's "BaseTween<?>", we can't make a collection of BaseTween objects.
		/// Too work around this limitation, the bulk of the original BaseTween implementation has been moved to IBaseTween but
		/// some methods remain in BaseTween because they use template types. These methods still needed access to private
		/// members of IBaseTween. The Internal* methods were added instead of making the members more visible.

		protected void InternalStart()
		{
			_Build();
			currentTime = 0;
			isStarted = true;
		}

		protected void InternalRepeat(int count, float delay)
		{
			if (isStarted)
			{
				throw new InvalidOperationException("You can't change the repetitions of a tween or timeline once it is started");
			}
			repeatCnt = count;
			repeatDelay = delay >= 0 ? delay : 0;
			isYoyo = false;
		}

		protected void InternalRepeatYoyo(int count, float delay)
		{
			if (isStarted)
			{
				throw new InvalidOperationException("You can't change the repetitions of a tween or timeline once it is started");
			}
			repeatCnt = count;
			repeatDelay = delay >= 0 ? delay : 0;
			isYoyo = true;
		}

		protected void InternalSetCallback(TweenCallback callback)
		{
			this.callback = callback;
		}

		protected void InternalSetCallbackTriggers(int flags)
		{
			callbackTriggers = flags;
		}

		protected void InternalSetUserData(object data)
		{
			userData = data;
		}

		// -------------------------------------------------------------------------
		// Public API
		// -------------------------------------------------------------------------

		/// <summary>Kills the tween or timeline.</summary>
		/// <remarks>
		/// Kills the tween or timeline. If you are using a TweenManager, this object
		/// will be removed automatically.
		/// </remarks>
		public virtual void Kill()
		{
			isKilled = true;
		}

		/// <summary>Stops and resets the tween or timeline.</summary>
		/// <remarks>
		/// Stops and resets the tween or timeline, and sends it to its pool, for
		/// later reuse. Note that if you use a
		/// <see cref="TweenManager">TweenManager</see>, 
		/// this method is automatically called once the animation is finished.
		/// </remarks>
		public virtual void Free()
		{
		}

		/// <summary>Pauses the tween or timeline.</summary>
		/// <remarks>Pauses the tween or timeline. Further update calls won't have any effect.</remarks>
		public virtual void Pause()
		{
			isPaused = true;
		}

		/// <summary>Resumes the tween or timeline.</summary>
		/// <remarks>Resumes the tween or timeline. Has no effect is it was no already paused.</remarks>
		public virtual void Resume()
		{
			isPaused = false;
		}

		// -------------------------------------------------------------------------
		// Getters
		// -------------------------------------------------------------------------
		/// <summary>Gets the delay of the tween or timeline.</summary>
		/// <remarks>
		/// Gets the delay of the tween or timeline. Nothing will happen before
		/// this delay.
		/// </remarks>
		public virtual float GetDelay()
		{
			return delay;
		}

		/// <summary>Gets the duration of a single iteration.</summary>
		public virtual float GetDuration()
		{
			return duration;
		}

		/// <summary>Gets the number of iterations that will be played.</summary>
		public virtual int GetRepeatCount()
		{
			return repeatCnt;
		}

		/// <summary>Gets the delay occuring between two iterations.</summary>
		public virtual float GetRepeatDelay()
		{
			return repeatDelay;
		}

		/// <summary>Returns the complete duration, including initial delay and repetitions.</summary>
		/// <remarks>
		/// Returns the complete duration, including initial delay and repetitions.
		/// The formula is as follows:
		/// <pre>
		/// fullDuration = delay + duration + (repeatDelay + duration) * repeatCnt
		/// </pre>
		/// </remarks>
		public virtual float GetFullDuration()
		{
			if (repeatCnt < 0)
			{
				return -1;
			}
			return delay + duration + (repeatDelay + duration) * repeatCnt;
		}

		/// <summary>Gets the attached data, or null if none.</summary>
		public virtual object GetUserData()
		{
			return userData;
		}

		/// <summary>Gets the id of the current step.</summary>
		/// <remarks>
		/// Gets the id of the current step. Values are as follows:<br/>
		/// <ul>
		/// <li>even numbers mean that an iteration is playing,<br/>
		/// <li>odd numbers mean that we are between two iterations,<br/>
		/// <li>-2 means that the initial delay has not ended,<br/>
		/// <li>-1 means that we are before the first iteration,<br/>
		/// <li>repeatCount*2 + 1 means that we are after the last iteration
		/// </remarks>
		public virtual int GetStep()
		{
			return step;
		}

		/// <summary>Gets the local time.</summary>
		public virtual float GetCurrentTime()
		{
			return currentTime;
		}

		/// <summary>Returns true if the tween or timeline has been started.</summary>
		public virtual bool IsStarted()
		{
			return isStarted;
		}

		/// <summary>Returns true if the tween or timeline has been initialized.</summary>
		/// <remarks>
		/// Returns true if the tween or timeline has been initialized. Starting
		/// values for tweens are stored at initialization time. This initialization
		/// takes place right after the initial delay, if any.
		/// </remarks>
		public virtual bool IsInitialized()
		{
			return isInitialized;
		}

		/// <summary>Returns true if the tween is finished (i.e.</summary>
		/// <remarks>
		/// Returns true if the tween is finished (i.e. if the tween has reached
		/// its end or has been killed). If you don't use a TweenManager, you may
		/// want to call
		/// <see>free()</see>
		/// to reuse the object later.
		/// </remarks>
		public virtual bool IsFinished()
		{
			return isFinished || isKilled;
		}

		/// <summary>Returns true if the iterations are played as yoyo.</summary>
		/// <remarks>
		/// Returns true if the iterations are played as yoyo. Yoyo means that
		/// every two iterations, the animation will be played backwards.
		/// </remarks>
		public virtual bool IsYoyo()
		{
			return isYoyo;
		}

		/// <summary>Returns true if the tween or timeline is currently paused.</summary>
		public virtual bool IsPaused()
		{
			return isPaused;
		}

		// -------------------------------------------------------------------------
		// Abstract API
		// -------------------------------------------------------------------------
		protected internal abstract void ForceStartValues();

		protected internal abstract void ForceEndValues();

		protected internal abstract bool ContainsTarget(object target);

		protected internal abstract bool ContainsTarget(object target, int tweenType);

		/// These _Build, _Start methods are part of a workaround for C# not supporting wildcards in generics.
		/// Because there is no C# equivalent to Java's "BaseTween<?>", we can't make a collection of BaseTween objects.
		/// Too work around this limitation, the bulk of the original BaseTween implementation has been moved to IBaseTween,
		/// leaving only the methods that used the template type in BaseTween. 
		/// These methods act as a proxy to the methods still left in BaseTween.
		internal abstract void _Build();
		internal abstract void _Start();
		internal abstract void _Start(TweenManager manager);
		internal abstract void _Delay(float delay);
		internal abstract void _Repeat(int count, float delay);
		internal abstract void _RepeatYoyo(int count, float delay);
		internal abstract void _SetCallback(TweenCallback callback);
		internal abstract void _SetCallbackTriggers(int flags);
		internal abstract void _SetUserData(object data);

		// -------------------------------------------------------------------------
		// Protected API
		// -------------------------------------------------------------------------
		protected internal virtual void InitializeOverride()
		{
		}

		protected internal virtual void UpdateOverride(int step, int lastStep, bool isIterationStep, float delta)
		{
		}

		protected internal virtual void ForceToStart()
		{
			currentTime = -delay;
			step = -1;
			isIterationStep = false;
			if (IsReverse(0))
			{
				ForceEndValues();
			}
			else
			{
				ForceStartValues();
			}
		}

		protected internal virtual void ForceToEnd(float time)
		{
			currentTime = time - GetFullDuration();
			step = repeatCnt * 2 + 1;
			isIterationStep = false;
			if (IsReverse(repeatCnt * 2))
			{
				ForceStartValues();
			}
			else
			{
				ForceEndValues();
			}
		}

		protected internal virtual void CallCallback(int type)
		{
			if (callback != null && (callbackTriggers & type) > 0)
			{
				// @TODO: Fix callbacks. Make them more C# ish.
				//callback.OnEvent(type, this);
			}
		}

		protected internal virtual bool IsReverse(int step)
		{
			return isYoyo && Math.Abs(step % 4) == 2;
		}

		protected internal virtual bool IsValid(int step)
		{
			return (step >= 0 && step <= repeatCnt * 2) || repeatCnt < 0;
		}

		protected internal virtual void KillTarget(object target)
		{
			if (ContainsTarget(target))
			{
				Kill();
			}
		}

		protected internal virtual void KillTarget(object target, int tweenType)
		{
			if (ContainsTarget(target, tweenType))
			{
				Kill();
			}
		}

		// -------------------------------------------------------------------------
		// Update engine
		// -------------------------------------------------------------------------
		/// <summary>Updates the tween or timeline state.</summary>
		/// <remarks>
		/// Updates the tween or timeline state. <b>You may want to use a
		/// TweenManager to update objects for you.</b>
		/// Slow motion, fast motion and backward play can be easily achieved by
		/// tweaking this delta time. Multiply it by -1 to play the animation
		/// backward, or by 0.5 to play it twice slower than its normal speed.
		/// </remarks>
		/// <param name="delta">A delta time between now and the last call.</param>
		public virtual void Update(float delta)
		{
			if (!isStarted || isPaused || isKilled)
			{
				return;
			}
			deltaTime = delta;
			if (!isInitialized)
			{
				Initialize();
			}
			if (isInitialized)
			{
				TestRelaunch();
				UpdateStep();
				TestCompletion();
			}
			currentTime += deltaTime;
			deltaTime = 0;
		}

		private void Initialize()
		{
			if (currentTime + deltaTime >= delay)
			{
				InitializeOverride();
				isInitialized = true;
				isIterationStep = true;
				step = 0;
				deltaTime -= delay - currentTime;
				currentTime = 0;
				CallCallback(TweenCallback.BEGIN);
				CallCallback(TweenCallback.START);
			}
		}

		private void TestRelaunch()
		{
			if (!isIterationStep && repeatCnt >= 0 && step < 0 && currentTime + deltaTime >= 0)
			{
				isIterationStep = true;
				step = 0;
				float delta = 0 - currentTime;
				deltaTime -= delta;
				currentTime = 0;
				CallCallback(TweenCallback.BEGIN);
				CallCallback(TweenCallback.START);
				UpdateOverride(step, step - 1, isIterationStep, delta);
			}
			else if (!isIterationStep && repeatCnt >= 0 && step > repeatCnt * 2 && currentTime + deltaTime < 0)
			{
				isIterationStep = true;
				step = repeatCnt * 2;
				float delta = 0 - currentTime;
				deltaTime -= delta;
				currentTime = duration;
				CallCallback(TweenCallback.BACK_BEGIN);
				CallCallback(TweenCallback.BACK_START);
				UpdateOverride(step, step + 1, isIterationStep, delta);
			}
		}

		private void UpdateStep()
		{
			while (IsValid(step))
			{
				if (!isIterationStep && currentTime + deltaTime <= 0)
				{
					isIterationStep = true;
					step -= 1;
					float delta = 0 - currentTime;
					deltaTime -= delta;
					currentTime = duration;
					if (IsReverse(step))
					{
						ForceStartValues();
					}
					else
					{
						ForceEndValues();
					}
					CallCallback(TweenCallback.BACK_START);
					UpdateOverride(step, step + 1, isIterationStep, delta);
				}
				else
				{
					if (!isIterationStep && currentTime + deltaTime >= repeatDelay)
					{
						isIterationStep = true;
						step += 1;
						float delta = repeatDelay - currentTime;
						deltaTime -= delta;
						currentTime = 0;
						if (IsReverse(step))
						{
							ForceEndValues();
						}
						else
						{
							ForceStartValues();
						}
						CallCallback(TweenCallback.START);
						UpdateOverride(step, step - 1, isIterationStep, delta);
					}
					else
					{
						if (isIterationStep && currentTime + deltaTime < 0)
						{
							isIterationStep = false;
							step -= 1;
							float delta = 0 - currentTime;
							deltaTime -= delta;
							currentTime = 0;
							UpdateOverride(step, step + 1, isIterationStep, delta);
							CallCallback(TweenCallback.BACK_END);
							if (step < 0 && repeatCnt >= 0)
							{
								CallCallback(TweenCallback.BACK_COMPLETE);
							}
							else
							{
								currentTime = repeatDelay;
							}
						}
						else
						{
							if (isIterationStep && currentTime + deltaTime > duration)
							{
								isIterationStep = false;
								step += 1;
								float delta = duration - currentTime;
								deltaTime -= delta;
								currentTime = duration;
								UpdateOverride(step, step - 1, isIterationStep, delta);
								CallCallback(TweenCallback.END);
								if (step > repeatCnt * 2 && repeatCnt >= 0)
								{
									CallCallback(TweenCallback.COMPLETE);
								}
								currentTime = 0;
							}
							else
							{
								if (isIterationStep)
								{
									float delta = deltaTime;
									deltaTime -= delta;
									currentTime += delta;
									UpdateOverride(step, step, isIterationStep, delta);
									break;
								}
								else
								{
									float delta = deltaTime;
									deltaTime -= delta;
									currentTime += delta;
									break;
								}
							}
						}
					}
				}
			}
		}

		private void TestCompletion()
		{
			isFinished = repeatCnt >= 0 && (step > repeatCnt * 2 || step < 0);
		}
	}

	/// <summary>BaseTween is the base class of Tween and Timeline.</summary>
	/// <remarks>
	/// BaseTween is the base class of Tween and Timeline. It defines the
	/// iteration engine used to play animations for any number of times, and in
	/// any direction, at any speed.
	/// <p/>
	/// It is responsible for calling the different callbacks at the right moments,
	/// and for making sure that every callbacks are triggered, even if the update
	/// engine gets a big delta time at once.
	/// </remarks>
	/// <seealso cref="Tween">Tween</seealso>
	/// <seealso cref="Timeline">Timeline</seealso>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public abstract class BaseTween<T> : IBaseTween
		where T : class
	{
		/// <summary>
		/// This method allows the BaseTween class to safely cast to the template type.
		/// </summary>
		/// <remarks>
		/// The implementation of this method should just return "this".
		/// </remarks>
		/// <returns>T</returns>
		protected abstract T ThisAsT();

		// Implement proxy methods.
		internal sealed override void _Build() { Build(); }
		internal sealed override void _Start() { Start(); }
		internal sealed override void _Start(TweenManager manager) { Start(manager); }
		internal sealed override void _Delay(float delay) { Delay(delay); }
		internal sealed override void _Repeat(int count, float delay) { Repeat(count, delay); }
		internal sealed override void _RepeatYoyo(int count, float delay) { RepeatYoyo(count, delay); }
		internal sealed override void _SetCallback(TweenCallback callback) { SetCallback(callback); }
		internal sealed override void _SetCallbackTriggers(int flags) { SetCallbackTriggers(flags); }
		internal sealed override void _SetUserData(object data) { SetUserData(data); }

		// -------------------------------------------------------------------------
		// Public API
		// -------------------------------------------------------------------------
		/// <summary>Builds and validates the object.</summary>
		/// <remarks>
		/// Builds and validates the object. Only needed if you want to finalize a
		/// tween or timeline without starting it, since a call to ".start()" also
		/// calls this method.
		/// </remarks>
		/// <returns>The current object, for chaining instructions.</returns>
		public virtual T Build()
		{
			return ThisAsT();
		}

		/// <summary>Starts or restarts the object unmanaged.</summary>
		/// <remarks>
		/// Starts or restarts the object unmanaged. You will need to take care of
		/// its life-cycle. If you want the tween to be managed for you, use a
		/// <see cref="TweenManager">TweenManager</see>
		/// .
		/// </remarks>
		/// <returns>The current object, for chaining instructions.</returns>
		public virtual T Start()
		{
			InternalStart();
			return ThisAsT();
		}

		/// <summary>Convenience method to add an object to a manager.</summary>
		/// <remarks>
		/// Convenience method to add an object to a manager. Its life-cycle will be
		/// handled for you. Relax and enjoy the animation.
		/// </remarks>
		/// <returns>The current object, for chaining instructions.</returns>
		public virtual T Start(TweenManager manager)
		{
			manager.Add(this);
			return ThisAsT();
		}

		/// <summary>Adds a delay to the tween or timeline.</summary>
		/// <remarks>Adds a delay to the tween or timeline.</remarks>
		/// <param name="delay">A duration.</param>
		/// <returns>The current object, for chaining instructions.</returns>
		public virtual T Delay(float delay)
		{
			this.delay += delay;
			return ThisAsT();
		}

		/// <summary>Repeats the tween or timeline for a given number of times.</summary>
		/// <remarks>Repeats the tween or timeline for a given number of times.</remarks>
		/// <param name="count">
		/// The number of repetitions. For infinite repetition,
		/// use Tween.INFINITY, or a negative number.
		/// </param>
		/// <param name="delay">A delay between each iteration.</param>
		/// <returns>The current tween or timeline, for chaining instructions.</returns>
		public virtual T Repeat(int count, float delay)
		{
			InternalRepeat(count, delay);
			return ThisAsT();
		}

		/// <summary>Repeats the tween or timeline for a given number of times.</summary>
		/// <remarks>
		/// Repeats the tween or timeline for a given number of times.
		/// Every two iterations, it will be played backwards.
		/// </remarks>
		/// <param name="count">
		/// The number of repetitions. For infinite repetition,
		/// use Tween.INFINITY, or '-1'.
		/// </param>
		/// <param name="delay">A delay before each repetition.</param>
		/// <returns>The current tween or timeline, for chaining instructions.</returns>
		public virtual T RepeatYoyo(int count, float delay)
		{
			InternalRepeatYoyo(count, delay);
			return ThisAsT();
		}

		/// <summary>Sets the callback.</summary>
		/// <remarks>
		/// Sets the callback. By default, it will be fired at the completion of the
		/// tween or timeline (event COMPLETE). If you want to change this behavior
		/// and add more triggers, use the
		/// <see>setCallbackTriggers()</see>
		/// method.
		/// </remarks>
		/// <seealso cref="TweenCallback">TweenCallback</seealso>
		public virtual T SetCallback(TweenCallback callback)
		{
			InternalSetCallback(callback);
			return ThisAsT();
		}

		/// <summary>Changes the triggers of the callback.</summary>
		/// <remarks>
		/// Changes the triggers of the callback. The available triggers, listed as
		/// members of the
		/// <see cref="TweenCallback">TweenCallback</see>
		/// interface, are:
		/// <p/>
		/// <b>BEGIN</b>: right after the delay (if any)<br/>
		/// <b>START</b>: at each iteration beginning<br/>
		/// <b>END</b>: at each iteration ending, before the repeat delay<br/>
		/// <b>COMPLETE</b>: at last END event<br/>
		/// <b>BACK_BEGIN</b>: at the beginning of the first backward iteration<br/>
		/// <b>BACK_START</b>: at each backward iteration beginning, after the repeat delay<br/>
		/// <b>BACK_END</b>: at each backward iteration ending<br/>
		/// <b>BACK_COMPLETE</b>: at last BACK_END event
		/// <p/>
		/// <pre>
		/// <code>
		/// forward :	  BEGIN								   COMPLETE
		/// forward :	  START	END	  START	END	  START	END
		/// |--------------[XXXXXXXXXX]------[XXXXXXXXXX]------[XXXXXXXXXX]
		/// backward:	  bEND  bSTART	  bEND  bSTART	  bEND  bSTART
		/// backward:	  bCOMPLETE								 bBEGIN
		/// </code>
		/// </pre>
		/// </remarks>
		/// <param name="flags">one or more triggers, separated by the '|' operator.</param>
		/// <seealso cref="TweenCallback">TweenCallback</seealso>
		public virtual T SetCallbackTriggers(int flags)
		{
			InternalSetCallbackTriggers(flags);
			return ThisAsT();
		}

		/// <summary>Attaches an object to this tween or timeline.</summary>
		/// <remarks>
		/// Attaches an object to this tween or timeline. It can be useful in order
		/// to retrieve some data from a TweenCallback.
		/// </remarks>
		/// <param name="data">Any kind of object.</param>
		/// <returns>The current tween or timeline, for chaining instructions.</returns>
		public virtual T SetUserData(object data)
		{
			InternalSetUserData(data);
			return ThisAsT();
		}
	}
}
