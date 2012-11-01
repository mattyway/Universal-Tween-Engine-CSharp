/*
 * This code is derived from Universal Tween Engine (http://code.google.com/p/java-universal-tween-engine/)
 * 
 * @author Aurelien Ribon | http://www.aurelienribon.com/
 */

using TweenEngine;

namespace TweenEngine
{
	/// <summary>TweenCallbacks are used to trigger actions at some specific times.</summary>
	/// <remarks>
	/// TweenCallbacks are used to trigger actions at some specific times. They are
	/// used in both Tweens and Timelines. The moment when the callback is
	/// triggered depends on its registered triggers:
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
	/// <seealso cref="Tween">Tween</seealso>
	/// <seealso cref="Timeline">Timeline</seealso>
	/// <author>Aurelien Ribon | http://www.aurelienribon.com/</author>
	public abstract class TweenCallback
	{
		public const int BEGIN = unchecked((int)(0x01));

		public const int START = unchecked((int)(0x02));

		public const int END = unchecked((int)(0x04));

		public const int COMPLETE = unchecked((int)(0x08));

		public const int BACK_BEGIN = unchecked((int)(0x10));

		public const int BACK_START = unchecked((int)(0x20));

		public const int BACK_END = unchecked((int)(0x40));

		public const int BACK_COMPLETE = unchecked((int)(0x80));

		public const int ANY_FORWARD = unchecked((int)(0x0F));

		public const int ANY_BACKWARD = unchecked((int)(0xF0));

		public const int ANY = unchecked((int)(0xFF));

		public abstract void OnEvent<T>(int type, IBaseTween source);
	}
}
