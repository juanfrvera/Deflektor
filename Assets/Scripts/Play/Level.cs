using UnityEngine;

[System.Serializable]
public class Level
{
		[SerializeField] string label = "1: Asteroids";
		[SerializeField] string sceneName = "Level";
		public string Label => label;
		public string SceneName => sceneName;
		public void Restart()
		{

		}
}