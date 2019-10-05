using UnityEngine;

public class Emitter : Beamer
{
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
				print("hurt");
				return null;
		}
		public override void ChainChanged(Vector3[] points)
		{
				laser.positionCount = points.Length+1;
				laser.SetPositions(AddMeToChain(points));
		}
		public override void Hurt()
		{
				print("hurt");
		}
}