using NodeSpace;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : Beamer
{
		[SerializeField] Play play;
		[SerializeField] float life = 5f;

		public Vector3 Direction => transform.up;
		private void Start() => Emit();

		public override void Emit()
		{
				IList<Node> nodes = LookForReflections(Position, Direction);
				ChainChanged(AddMeToChain(nodes));
		}
		public override Node Conect(Vector3 hitPoint, Vector3 direction, Vector3 normal)
		{
				print("Emitter touched!");
				return new Node(Position);
		}
		private bool hurting = false;
		private bool changedFromHurt = false;
		public void ChainChanged(IList<Node> nodes)
		{
				LaserController.AddLaser(nodes.Positions());

				if (changedFromHurt && hurting)
						NotHurt();
				else
						changedFromHurt = true;
		}
		private void NotHurt()
		{
				hurting = false;
				play.EndHurt();
		}
		public void Hurt()
		{
				hurting = true;
				changedFromHurt = false;
				play.BeginHurt();
		}
}