/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;
using System.Collections.Generic;

namespace TweenEngine
{
	/// <summary>A light pool of objects that can be resused to avoid allocation.</summary>
	/// <remarks>
	/// A light pool of objects that can be resused to avoid allocation.
	/// Based on Nathan Sweet pool implementation
	/// </remarks>
	internal abstract class Pool<T>
	{
		private List<T> objects;

		private Callback<T> callback;

		protected internal abstract T Create();

		public Pool(int initCapacity, Callback<T> callback)
		{
			this.objects = new List<T>(initCapacity);
			this.callback = callback;
		}

		public virtual T Get()
		{
			T obj;
			if(objects.Count == 0)
			{
				obj = Create();
			}
			else
			{
				obj = objects[objects.Count - 1];
				objects.RemoveAt(objects.Count - 1);
			}

			if (callback != null)
			{
				callback.OnUnPool(obj);
			}
			return obj;
		}

		public virtual void Free(T obj)
		{
			if (!objects.Contains(obj))
			{
				if (callback != null)
				{
					callback.OnPool(obj);
				}
				objects.Add(obj);
			}
		}

		public virtual void Clear()
		{
			objects.Clear();
		}

		public virtual int Size()
		{
			return objects.Count;
		}

		public virtual void EnsureCapacity(int minCapacity)
		{
			// @TODO: Duplicate what Java does to ensure capacity in a collection.
			//objects.EnsureCapacity(minCapacity);
		}

		public interface Callback<T>
		{
			void OnPool(T obj);

			void OnUnPool(T obj);
		}
	}
}
