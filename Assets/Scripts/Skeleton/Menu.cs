using UnityEngine;
using DelegateLib;

public class Menu : MonoBehaviour
{
		#region Load
		private const string SCENE_NAME = "Menu";
		public static void Load(VoidDelegate onLoad = null) => Game.LoadScene(SCENE_NAME, onLoad);
		private static void Unload(VoidDelegate onUnload = null) => Game.UnloadScene(SCENE_NAME, onUnload);
		#endregion Load
		private void Awake()
		{
#if UNITY_EDITOR
				if (!Game.IsLoaded)
						Game.LoadSelf(PlayMusic);
				else
						PlayMusic();
#else
				PlayMusic();
#endif
		}
		private void PlayMusic()
		{
				Sound.Menu();
		}
		#region UI
		public void ClickPlay()
		{
				//Unload scene and then load Play
				Unload(delegate () { Play.Load(); });
		}
		public void ClickSettings()
		{
				Settings.Load();
		}
		public void ClickExit()
		{
				Game.Exit();
		}
		#endregion UI
}