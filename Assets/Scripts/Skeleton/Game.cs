﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DelegateLib;
public class Game : MonoBehaviour
{
		private const string SCENE_NAME = "Game";

		private static Game instance;//Singleton

#if UNITY_EDITOR//Solo se usa para que probar desde cualquier escena sea más facil (en el juego final no se usa)
		public static bool IsLoaded => instance != null;
#endif
		public static void LoadSelf(VoidDelegate onLoad = null) => LoadScene(SCENE_NAME, onLoad);

		private void Awake()
		{
				instance = this;
				SceneManager.sceneLoaded += SceneLoaded;
				SceneManager.sceneUnloaded += SceneUnloaded;
		}

		#region Scenes
		private class SceneInfo
		{
				public VoidDelegate onLoad;
				public bool setActive;

				public SceneInfo(VoidDelegate onLoad, bool setActive)
				{
						this.onLoad = onLoad;
						this.setActive = setActive;
				}
		}
		private static IDictionary<string, SceneInfo> onSceneLoad;
		private static IDictionary<string, VoidDelegate> onSceneUnload;
		private static IDictionary<string, SceneInfo> OnSceneLoad
		{
				get
				{
						if (onSceneLoad == null)
								onSceneLoad = new Dictionary<string, SceneInfo>();

						return onSceneLoad;
				}
				set => onSceneLoad = value;
		}
		private static IDictionary<string, VoidDelegate> OnSceneUnload
		{
				get
				{
						if (onSceneUnload == null)
								onSceneUnload = new Dictionary<string, VoidDelegate>();

						return onSceneUnload;
				}
				set => onSceneUnload = value;
		}
		public static void LoadScene(string name, VoidDelegate onLoad = null, bool setActive = false)
		{
				if (OnSceneLoad.ContainsKey(name))
				{
						var val = OnSceneLoad[name].onLoad;
						val += onLoad;
						//OnSceneLoad[name] = val;
				}
				else
						OnSceneLoad.Add(name, new SceneInfo(onLoad, setActive));
				SceneManager.LoadScene(name, LoadSceneMode.Additive);
		}
		private void SceneLoaded(Scene scene, LoadSceneMode mode)
		{
				if (OnSceneLoad.ContainsKey(scene.name))//Invocar solo si existe esta key con un valor distinto de null
				{
						SceneInfo info = OnSceneLoad[scene.name];
						if (info.setActive)
								SceneManager.SetActiveScene(scene);

						if (info.onLoad != null)
								info.onLoad.Invoke();

						OnSceneLoad.Remove(scene.name);
				}
		}
		public static void UnloadScene(string name, VoidDelegate onUnload = null)
		{
				if (onUnload != null)
				{
						if (OnSceneUnload.ContainsKey(name))
						{
								var val = OnSceneUnload[name];
								val += onUnload;
								OnSceneUnload[name] = val;
						}
						else
								OnSceneUnload.Add(name, onUnload);
				}
				SceneManager.UnloadSceneAsync(name);
		}
		private void SceneUnloaded(string name)
		{
				if (OnSceneUnload.ContainsKey(name))//Invocar solo si existe esta key con un valor distinto de null
				{
						OnSceneUnload[name].Invoke();
						OnSceneUnload.Remove(name);
				}
		}
		private void SceneUnloaded(Scene scene) => this.SceneUnloaded(scene.name);
		#endregion Scene
		public static void Exit()
		{
				Application.Quit();
		}
}