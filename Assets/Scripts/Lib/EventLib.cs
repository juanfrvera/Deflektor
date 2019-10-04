using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using DelegateLib;

public static class EventLib
{
		[System.Serializable] public class Event : UnityEvent { }
		[System.Serializable] public class FloatEvent : UnityEvent<float> { }
		[System.Serializable] public class Collider2DEvent : UnityEvent<Collider2D> { }
		[System.Serializable] public class ColliderEvent : UnityEvent<Collider> { }
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> { }
		[System.Serializable] public class ByteEvent : UnityEvent<byte> { }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="mono"></param>
		/// <param name="seconds"></param>
		/// <param name="called"></param>
		/// <param name="realtime">Si es verdadero, las pausas no afectan al tiempo esperado</param>
		/// <returns></returns>
		public static Coroutine CallOnSeconds(this MonoBehaviour mono, float seconds, VoidDelegate called, bool realtime)
		{
				return mono.StartCoroutine(CallOnSecondsCoroutine(seconds, called, realtime));
		}

		public static Coroutine CallOnFixedFrames(this MonoBehaviour mono, uint frames, VoidDelegate called)
		{
				return mono.StartCoroutine(CallOnFixedFramesCoroutine(frames, called));
		}
		private static IEnumerator CallOnFixedFramesCoroutine(uint frames, VoidDelegate called)
		{
				for (uint i = 0; i < frames; i++)
				{
						yield return new WaitForFixedUpdate();
				}
				called.Invoke();
		}

		private static IEnumerator CallOnSecondsCoroutine(float seconds, VoidDelegate called, bool realtime)
		{
				if (realtime)
						yield return new WaitForSecondsRealtime(seconds);//Not affected by Time.timeScale
				else
						yield return new WaitForSeconds(seconds);//Affected by Time.timeScale

				called.Invoke();
		}
}
