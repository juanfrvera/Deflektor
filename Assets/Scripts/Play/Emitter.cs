using UnityEngine;

public class Emitter : Beamer
{
		[SerializeField] Play play;
		[SerializeField] Vector3 emitDir;
		[SerializeField] LineRenderer laser;
		[SerializeField] float life = 5f;

		public override Vector3 Direction => emitDir;
		private void Start()
		{
				Emit();
		}

		public override void Emit()
		{
				Vector3[] reflections = LookForReflections(Position, Direction);
				ChainChanged(AddMeToChain(reflections));
		}
		public override Vector3[] Receive(Beamer predecessor)
		{
				print("hurt from receive");
				return null;
		}
		private bool hurting = false;
		private bool changedFromHurt = false;
		public override void ChainChanged(Vector3[] points)
		{
				print("chain changed, hurting: "+hurting);
				laser.positionCount = points.Length+1;
				laser.SetPositions(AddMeToChain(points));

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
		public override void Hurt()
		{
				hurting = true;
				changedFromHurt = false;
				play.BeginHurt();
		}
}