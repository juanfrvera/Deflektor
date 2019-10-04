using UnityEngine;
using DelegateLib;

public class Settings : MonoBehaviour, IInputController
{
		#region Load
		private const string SCENE_NAME = "Settings";
		public static void Load(VoidDelegate onLoad = null) => Game.LoadScene(SCENE_NAME, onLoad);
		private static void Unload(VoidDelegate onUnload = null) => Game.UnloadScene(SCENE_NAME, onUnload);
		#endregion Load

		private static VoidDelegate exitListener;
		public static VoidDelegate ExitListener
		{
				private get => exitListener;
				set => exitListener = value;
		}

		//Al ser estáticos quedan guardados aunque esta instancia sea destruida
		private static bool sfx = true;
		private static bool music = true;

		[SerializeField] ImageOptions sfxOption, musicOption;
		private void Awake()
		{
				sfxOption.Index = sfx ? (byte)0 : (byte)1;
				musicOption.Index = music ? (byte)0 : (byte)1;

				Game.InputController = this;
		}

		#region UI
		public void SFXChanged()
		{
				sfx = !sfx;
				if (sfx) Sound.UnmuteEffects();
				else
						Sound.MuteEffects();
		}
		public void MusicChanged()
		{
				music = !music;
				if (music) Sound.UnmuteMusic();
				else
						Sound.MuteMusic();
		}
		public void ClickExit() => Unload(delegate () { ExitListener?.Invoke(); });

		void IInputController.Escape() => ClickExit();

		void IInputController.Enter() { }
		#endregion UI
}