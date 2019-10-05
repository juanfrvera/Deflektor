using UnityEngine;
public class Receiver : Beamer
{
		[SerializeField] Play play;
		public override Vector3 Direction => Vector3.zero;

		public override Vector3[] Receive(Beamer predecessor)
		{
				Debug.Log("Win");
				play.LevelWon();
				return new Vector3[] { Position };
		}
}
