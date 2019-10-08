using UnityEngine;
using DelegateLib;

public class Menu : MonoBehaviour, IInputController
{
		#region Load
		private const string SCENE_NAME = "Menu";
		public static void Load(VoidDelegate onLoad = null) => Game.LoadScene(SCENE_NAME, onLoad);
		private static void Unload(VoidDelegate onUnload = null) => Game.UnloadScene(SCENE_NAME, onUnload);
		#endregion Load
		private void Awake()
		{
				if (!Game.IsLoaded)
						Game.LoadSelf(Loaded);
				else
						Loaded();
		}
		private void Loaded()
		{
				Sound.Menu();
				Game.InputController = this;
		}
		#region UI & Input
		public void ClickPlay()
		{
				//Unload scene and then load Play
				Unload(delegate () { Play.Load(); });
		}
		public void ClickSettings()
		{
				Settings.Load();
				Settings.ExitListener = delegate () { Game.InputController = this; };
		}

		public void ClickExit() => Game.Exit();

		void IInputController.Escape() => ClickExit();

		void IInputController.Enter() { }
		#endregion UI & Input
}