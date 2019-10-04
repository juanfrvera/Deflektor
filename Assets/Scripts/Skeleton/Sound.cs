using UnityEngine;

public class Sound : MonoBehaviour
{
		private static Sound instance;//Singleton

		private void Awake() => instance = this;
		#region Music
		[Header("Music")]
		[SerializeField] AudioSource musicSource;
		[SerializeField] AudioClip menu;
		[SerializeField] AudioClip play;
		[SerializeField] AudioClip win;
		[SerializeField] AudioClip gameOver;
		[SerializeField] AudioClip boss;
		public static void Menu() => Music(instance.menu, false);
		public static void Play() => Music(instance.play, true);
		public static void Win() => Music(instance.win, false);
		public static void GameOver() => Music(instance.gameOver, false);
		private static void Music(AudioClip clip, bool loop)
		{
				instance.musicSource.clip = clip;
				instance.musicSource.Play();
				instance.musicSource.loop = loop;
		}
		public static void MuteMusic()
		{
				instance.musicSource.mute = true;
		}
		public static void UnmuteMusic()
		{
				instance.musicSource.mute = false;
		}
		#endregion Music
		#region SFX
		[Header("SFX")]
		[SerializeField] AudioSource sfxSource;
		[SerializeField] AudioClip explosion;
		[SerializeField] AudioClip playerExplosion;
		[SerializeField] AudioClip laser1;

		private static void SFX(ref AudioClip clip, Vector3 position)
		{
				instance.sfxSource.clip = clip;
				instance.sfxSource.transform.position = position;
				instance.sfxSource.Play();
		}
		public static void Explosion(Vector3 position)
		{
				SFX(ref instance.explosion, position);
		}
		public static void PlayerExplosion(Vector3 position)
		{
				SFX(ref instance.playerExplosion, position);
		}
		public static void Laser(Vector3 position)
		{
				SFX(ref instance.laser1, position);
		}
		public static void DecreaseEffects()
		{
				instance.sfxSource.volume = 0.1f;
		}
		public static void IncreaseEffects()
		{
				instance.sfxSource.volume = 1f;
		}
		public static void MuteEffects()
		{
				instance.sfxSource.mute = true;
		}
		public static void UnmuteEffects()
		{
				instance.sfxSource.mute = false;
		}
		#endregion Effects
}