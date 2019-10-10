using NodeSpace;
using UnityEngine;
public class Receiver : Beamer
{
		[SerializeField] Play play;
		public override Node Conect(Vector3 hitPoint, Vector3 direction, Vector3 normal)
		{
				Debug.Log("Win");
				play.LevelWon();
				return new Node(Position);
		}
}
