using UnityEngine;
using DelegateLib;
using System.Collections;

public class Play : MonoBehaviour
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
		}
		[SerializeField] TMPro.TextMeshProUGUI helpText;
		[SerializeField] string moveHelp, shotHelp, swapHelp, weaponHelp;
		private void Start()
		{
				levelIndex = 0;
#if !UNITY_EDITOR
				GameLoaded();
#endif
				Pause = false;
				if (!helped)
						RestartHelp();
		}
		private static bool helped = false;
		public static bool Moved = false, Shot = false, Swap = false, Weapon = false;
		private void RestartHelp()
		{
				helped = false;
				Moved = false; Shot = false; Swap = false; Weapon = false;
				StartCoroutine("HelpCoroutine");//Solo una
		}
		IEnumerator HelpCoroutine()
		{
				helpText.gameObject.SetActive(true);
				helpText.text = moveHelp;
				do
				{
						yield return new WaitForSeconds(1f);
				} while (!Moved);
				helpText.text = shotHelp;
				do
				{
						yield return new WaitForSeconds(1f);
				} while (!Shot);
				helpText.text = swapHelp;
				do
				{
						yield return new WaitForSeconds(1f);
				} while (!Swap);
				helpText.text = weaponHelp;
				do
				{
						yield return new WaitForSeconds(1f);
				} while (!Weapon);
				helpText.gameObject.SetActive(false);

				helped = true;
		}
		private void Update()
		{
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
						EscapePressed();
		}
		#region UI
		[SerializeField] GameObject playMenu;
		private void EscapePressed() => Pause = !Pause;
		public void ClickResume() => Pause = false;
		public void ClickTutorial() => RestartHelp();
		public void ClickSettings() => Settings.Load();
		public void ClickExit() => Unload(delegate () { Menu.Load(); });
		public void ClickRestart() => Restart();
		#endregion

		[SerializeField] GameObject gameOverScreen;
		[SerializeField] GameObject winScreen;
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

#if UNITY_EDITOR
		[SerializeField] bool disableLevels;
#endif
		private void LoadLevel(byte level = 0)
		{
#if UNITY_EDITOR
				if (disableLevels)
						return;

				this.levelIndex = level;
#else
				this.levelIndex = level;
#endif
				//El actual level va a ser con el nuevo levelIndex
				Game.LoadScene(ActualLevel.SceneName, LevelLoaded);
		}

		private void LevelLoaded()
		{
				var progress = (float)(levelIndex + 1) / levels.Length;
				passedTime = 0f;
				CheckLevelProgression();
		}

		private bool gameOver = false;
		float passedTime = 0;
		private void CheckLevelProgression()
		{
				//Al perder se deja de chequear el progreso hasta reiniciar
				if (!gameOver && !winned)
				{
						passedTime++;
						if (!ActualLevel.IsEnded(passedTime))//Si el nivel no terminó aún
								this.CallOnSeconds(1, CheckLevelProgression, false);
						else
								LevelEnded();
				}
		}
		private void LevelEnded()
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
				instance.winScreen.SetActive(true);
				instance.winned = true;
		}
		private void WinLevelLoaded()
		{

		}
}