using UnityEngine;
using DelegateLib;
using System.Collections;

public class Play : MonoBehaviour, IInputController
{
		#region Load
		private const string SCENE_NAME = "Play";
		public static void Load(VoidDelegate onLoad = null) => Game.LoadScene(SCENE_NAME, onLoad);
		private static void Unload(VoidDelegate onUnload = null)
		{
				onUnload += delegate () { Time.timeScale = 1; };
				Game.UnloadScene(SCENE_NAME, onUnload);
		}
		#endregion Load
		#region Pause
		private bool pause = false;
		//Cuidado que esta pausa activa el menú (ojo con llamarla al ganar o al perder)
		public bool Pause
		{
				get => pause;
				private set
				{
						pause = value;
						Time.timeScale = value == true ? 0 : 1;//Pausar el tiempo si hay pausa
						playMenu.SetActive(value);

						onPause?.Invoke(value);//Si hay suscriptores a la pausa, pasarles el nuevo valor
				}
		}
		private VoidBoolDelegate onPause;
		public void SuscribeToPause(VoidBoolDelegate suscriber) => onPause += suscriber;
		public void UnsuscribeFromPause(VoidBoolDelegate unsuscriber) => onPause -= unsuscriber;
		#endregion Pause
		private static Play instance;//Singleton
		private void Awake()
		{
				instance = this;
#if UNITY_EDITOR
				if (!Game.IsLoaded)
						Game.LoadSelf(GameLoaded);
				else
						GameLoaded();
#else
				Sound.Play();
#endif
		}
		private void GameLoaded()
		{
				Sound.Play();
				LoadLevel();
				Game.InputController = this;
		}
		private void Start()
		{
				levelIndex = 0;
#if !UNITY_EDITOR
				GameLoaded();
#endif
				Pause = false;
		}
		#region UI & Input
		[SerializeField] GameObject playMenu;
		public void ClickResume() => Pause = false;
		public void ClickSettings()
		{
				Settings.Load();
				Settings.ExitListener = delegate () { Game.InputController = this; };
		}
		public void ClickExit() => Unload(delegate () { Menu.Load(); });
		public void ClickRestart() => Restart();

		void IInputController.Escape() => Pause = !Pause;

		void IInputController.Enter() { }
		#endregion UI & Input
		#region Hurt
		[SerializeField] float hurtTime;//Time between hurts
		[SerializeField] float hurtAmount;
		[SerializeField] float maxOverload;
		private float overload = 0f;

		private Coroutine hurtingCoroutine;
		public void BeginHurt()
		{
				if (hurtingCoroutine != null)
						StopCoroutine(hurtingCoroutine);

				hurtingCoroutine = StartCoroutine(Hurting());

				Debug.LogWarning("Started overloading...");
		}
		private IEnumerator Hurting()
		{
				while (overload < maxOverload)
				{

						yield return new WaitForSeconds(hurtTime);
						overload += hurtAmount;
				}
				Overloaded();
		}
		public void EndHurt()
		{
				Debug.Log("Overload fixed");
		}
		private void Overloaded()
		{
				Debug.LogError("Overloaded");
		}
		#endregion Hurt
		[SerializeField] LayerMask beamerLayer;
		public static LayerMask BeamerLayer => instance.beamerLayer;


		[SerializeField] GameObject gameOverScreen;
		public void GameOver()
		{
				gameOverScreen.SetActive(true);
				Sound.GameOver();
				Sound.DecreaseEffects();
				gameOver = true;
		}

		[Header("Levels")]
		[SerializeField] Level[] levels;
		[SerializeField] Level winLevel;

		private byte levelIndex = 0;
		private Level ActualLevel => levels[levelIndex];
		private void LoadLevel(byte level = 0)
		{
				this.levelIndex = level;
				//El actual level va a ser con el nuevo levelIndex
				Game.LoadScene(ActualLevel.SceneName, LevelLoaded);
		}

		private void LevelLoaded() { }
		private bool gameOver = false;
		public void LevelWon()
		{
				if (levelIndex < levels.Length - 1 && !winned)//Si todavía hay más niveles por cargar
						LoadNextLevel();

				//El juego es ganado al matar al boss por lo que no hay evento de ganar aquí
		}
		private void UnloadLevel(VoidDelegate onUnload)
		{
				Game.UnloadScene(ActualLevel.SceneName, onUnload);
		}
		private void LoadNextLevel()
		{
				if (levelIndex < levels.Length - 1)//Si todavía hay más niveles por cargar
				{
						LoadLevel((byte)(levelIndex + 1));
				}
		}
		public void RestartWin()
		{
				Unload(delegate () { Load(); });
		}
		private void Restart()
		{
				gameOverScreen.SetActive(false);

				Sound.IncreaseEffects();
				UnloadLevel(delegate () { Unload(delegate () { Load(); }); });
		}
		private bool winned = false;
		public static void GameWined()
		{
				Sound.Win();
				instance.winned = true;
		}
		private void WinLevelLoaded()
		{

		}
}