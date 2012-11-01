/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using System.Collections.Generic;
using TweenEngine;

namespace TweenEngine
{
	/// <summary>A TweenManager updates all your tweens and timelines at once.</summary>
	/// <remarks>
	/// A TweenManager updates all your tweens and timelines at once.
	/// Its main interest is that it handles the tween/timeline life-cycles for you,
	/// as well as the pooling constraints (if object pooling is enabled).
	/// <p/>
	/// Just give it a bunch of tweens or timelines and call update() periodically,
	/// you don't need to care for anything else! Relax and enjoy your animations.
	/// </remarks>
	/// <seealso cref="Tween">Tween</seealso>
	/// <seealso cref="Timeline">Timeline</seealso>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public class TweenManager
	{
		// -------------------------------------------------------------------------
		// Static API
		// -------------------------------------------------------------------------
		/// <summary>
		/// Disables or enables the "auto remove" mode of any tween manager for a
		/// particular tween or timeline.
		/// </summary>
		/// <remarks>
		/// Disables or enables the "auto remove" mode of any tween manager for a
		/// particular tween or timeline. This mode is activated by default. The
		/// interest of desactivating it is to prevent some tweens or timelines from
		/// being automatically removed from a manager once they are finished.
		/// Therefore, if you update a manager backwards, the tweens or timelines
		/// will be played again, even if they were finished.
		/// </remarks>
		public static void SetAutoRemove(IBaseTween @object, bool value)
		{
			@object.isAutoRemoveEnabled = value;
		}

		/// <summary>
		/// Disables or enables the "auto start" mode of any tween manager for a
		/// particular tween or timeline.
		/// </summary>
		/// <remarks>
		/// Disables or enables the "auto start" mode of any tween manager for a
		/// particular tween or timeline. This mode is activated by default. If it
		/// is not enabled, add a tween or timeline to any manager won't start it
		/// automatically, and you'll need to call .start() manually on your object.
		/// </remarks>
		public static void SetAutoStart(IBaseTween @object, bool value)
		{
			@object.isAutoStartEnabled = value;
		}

		private readonly List<IBaseTween> objects = new List<IBaseTween>(20);

		private bool isPaused = false;

		// -------------------------------------------------------------------------
		// Public API
		// -------------------------------------------------------------------------
		/// <summary>Adds a tween or timeline to the manager and starts or restarts it.</summary>
		/// <remarks>Adds a tween or timeline to the manager and starts or restarts it.</remarks>
		/// <returns>The manager, for instruction chaining.</returns>
		public virtual TweenManager Add(IBaseTween tween)
		{
			if (!objects.Contains(tween))
			{
				objects.Add(tween);
			}
			if (tween.isAutoStartEnabled)
			{
				tween._Start();
			}
			return this;
		}

		/// <summary>
		/// Returns true if the manager contains any valid interpolation associated
		/// to the given target object.
		/// </summary>
		/// <remarks>
		/// Returns true if the manager contains any valid interpolation associated
		/// to the given target object.
		/// </remarks>
		public virtual bool ContainsTarget(object target)
		{
			for (int i = 0, n = objects.Count; i < n; i++)
			{
				IBaseTween obj = objects[i];
				if (obj.ContainsTarget(target))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns true if the manager contains any valid interpolation associated
		/// to the given target object and to the given tween type.
		/// </summary>
		/// <remarks>
		/// Returns true if the manager contains any valid interpolation associated
		/// to the given target object and to the given tween type.
		/// </remarks>
		public virtual bool ContainsTarget(object target, int tweenType)
		{
			for (int i = 0, n = objects.Count; i < n; i++)
			{
				IBaseTween obj = objects[i];
				if (obj.ContainsTarget(target, tweenType))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Kills every managed tweens and timelines.</summary>
		/// <remarks>Kills every managed tweens and timelines.</remarks>
		public virtual void KillAll()
		{
			for (int i = 0, n = objects.Count; i < n; i++)
			{
				IBaseTween obj = objects[i];
				obj.Kill();
			}
		}

		/// <summary>Kills every tweens associated to the given target.</summary>
		/// <remarks>
		/// Kills every tweens associated to the given target. Will also kill every
		/// timelines containing a tween associated to the given target.
		/// </remarks>
		public virtual void KillTarget(object target)
		{
			for (int i = 0, n = objects.Count; i < n; i++)
			{
				IBaseTween obj = objects[i];
				obj.KillTarget(target);
			}
		}

		/// <summary>Kills every tweens associated to the given target and tween type.</summary>
		/// <remarks>
		/// Kills every tweens associated to the given target and tween type. Will
		/// also kill every timelines containing a tween associated to the given
		/// target and tween type.
		/// </remarks>
		public virtual void KillTarget(object target, int tweenType)
		{
			for (int i = 0, n = objects.Count; i < n; i++)
			{
				IBaseTween obj = objects[i];
				obj.KillTarget(target, tweenType);
			}
		}

		/// <summary>Increases the minimum capacity of the manager.</summary>
		/// <remarks>Increases the minimum capacity of the manager. Defaults to 20.</remarks>
		public virtual void EnsureCapacity(int minCapacity)
		{
			//objects.EnsureCapacity(minCapacity);
		}

		/// <summary>Pauses the manager.</summary>
		/// <remarks>Pauses the manager. Further update calls won't have any effect.</remarks>
		public virtual void Pause()
		{
			isPaused = true;
		}

		/// <summary>Resumes the manager, if paused.</summary>
		/// <remarks>Resumes the manager, if paused.</remarks>
		public virtual void Resume()
		{
			isPaused = false;
		}

		/// <summary>
		/// Updates every tweens with a delta time ang handles the tween life-cycles
		/// automatically.
		/// </summary>
		/// <remarks>
		/// Updates every tweens with a delta time ang handles the tween life-cycles
		/// automatically. If a tween is finished, it will be removed from the
		/// manager. The delta time represents the elapsed time between now and the
		/// last update call. Each tween or timeline manages its local time, and adds
		/// this delta to its local time to update itself.
		/// <p/>
		/// Slow motion, fast motion and backward play can be easily achieved by
		/// tweaking this delta time. Multiply it by -1 to play the animation
		/// backward, or by 0.5 to play it twice slower than its normal speed.
		/// </remarks>
		public virtual void Update(float delta)
		{
			for (int i = objects.Count - 1; i >= 0; i--)
			{
				IBaseTween obj = objects[i];
				if (obj.IsFinished() && obj.isAutoRemoveEnabled)
				{
					objects.RemoveAt(i);
					obj.Free();
				}
			}
			if (!isPaused)
			{
				if (delta >= 0)
				{
					for (int i = 0, n = objects.Count; i < n; i++)
					{
						objects[i].Update(delta);
					}
				}
				else
				{
					for (int i = objects.Count - 1; i >= 0; i--)
					{
						objects[i].Update(delta);
					}
				}
			}
		}

		/// <summary>Gets the number of managed objects.</summary>
		/// <remarks>
		/// Gets the number of managed objects. An object may be a tween or a
		/// timeline. Note that a timeline only counts for 1 object, since it
		/// manages its children itself.
		/// <p/>
		/// To get the count of running tweens, see
		/// <see cref="GetRunningTweensCount()">GetRunningTweensCount()</see>
		/// .
		/// </remarks>
		public virtual int Size()
		{
			return objects.Count;
		}

		/// <summary>Gets the number of running tweens.</summary>
		/// <remarks>
		/// Gets the number of running tweens. This number includes the tweens
		/// located inside timelines (and nested timelines).
		/// <p/>
		/// <b>Provided for debug purpose only.</b>
		/// </remarks>
		public virtual int GetRunningTweensCount()
		{
			return GetTweensCount(objects);
		}

		/// <summary>Gets the number of running timelines.</summary>
		/// <remarks>
		/// Gets the number of running timelines. This number includes the timelines
		/// nested inside other timelines.
		/// <p/>
		/// <b>Provided for debug purpose only.</b>
		/// </remarks>
		public virtual int GetRunningTimelinesCount()
		{
			return GetTimelinesCount(objects);
		}

		/// <summary>Gets an immutable list of every managed object.</summary>
		/// <remarks>
		/// Gets an immutable list of every managed object.
		/// <p/>
		/// <b>Provided for debug purpose only.</b>
		/// </remarks>
		//public virtual IList<BaseTween<object>> GetObjects()
		//{
		//	return Sharpen.Collections.UnmodifiableList(objects);
		//}

		// -------------------------------------------------------------------------
		// Helpers
		// -------------------------------------------------------------------------
		private static int GetTweensCount(IList<IBaseTween> objs)
		{
			int cnt = 0;
			for (int i = 0, n = objs.Count; i < n; i++)
			{
				IBaseTween obj = objs[i];
				if (obj is Tween)
				{
					cnt += 1;
				}
				else
				{
					cnt += GetTweensCount(((Timeline)obj).GetChildren());
				}
			}
			return cnt;
		}

		private static int GetTimelinesCount(IList<IBaseTween> objs)
		{
			int cnt = 0;
			for (int i = 0, n = objs.Count; i < n; i++)
			{
				IBaseTween obj = objs[i];
				if (obj is Timeline)
				{
					cnt += 1 + GetTimelinesCount(((Timeline)obj).GetChildren());
				}
			}
			return cnt;
		}
	}
}
