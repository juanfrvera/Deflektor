using UnityEngine;

[System.Serializable]
public class Level
{
		[SerializeField] string label = "1: Asteroids";
		[SerializeField] string sceneName = "Level";
		[SerializeField] float time = 60f;//Tiempo que duro
		[Header("Effects")]
		[SerializeField] Color startColor, endColor;//Para el efectito
		[SerializeField] bool drivenByTime = true;
		public string Label => label;
		public string SceneName => sceneName;
		//Lujito para cambiar de color el fondo
		public Color ActualColor(float passedTime)
		{
				if (passedTime > time) passedTime = time;

				return Color.Lerp(startColor, endColor, passedTime / time);
		}

		private bool ended = false;
		public virtual bool IsEnded(float passedTime)
		{
				if (drivenByTime)
						return passedTime > time;//Estoy terminado cuando pasó el tiempo
				else
						return ended;
		}
		public void Restart()
		{

		}
}