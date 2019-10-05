using UnityEngine;

public class Mirror : Beamer, IBeamable
{
		[SerializeField] float minDragTime = 1f;//The time you have to waite before start rotating with drag
		[SerializeField] float draggingTime = 0.5f;//The waited time when a drag rotation has occur



		private Vector3 Normal => transform.up;
		public override Vector3 Direction => Vector3.Reflect(prevBeamer.Direction, Normal);

		#region Rotation
		private float startX;
		private float timeDragged = 0f;//This variable is reused for the first wait and for the dragging waits
		private bool rotatedWithDrag = false;
		public void OnMouseDown()
		{
				startX = Input.mousePosition.x;
				timeDragged = 0f;
		}
		public void OnMouseDrag()
		{
				timeDragged += Time.deltaTime;
				if (rotatedWithDrag)
				{
						if (timeDragged >= draggingTime)
						{
								Rotate();
								timeDragged = 0f;//You have to wait again for the next drag
						}
				}//rotatedWithDrag == false
				else
				{
						if (timeDragged >= minDragTime)
						{
								rotatedWithDrag = true;
								Rotate();//Make the first rotation
								timeDragged = 0f;//Restart so the next rotation occurs in draggintTime
						}
				}
		}
		public void OnMouseUp()
		{
				//If there wasn't a rotation based on drag
				if (!rotatedWithDrag)
						Rotate();
				else
						rotatedWithDrag = false;//Prepare for the next click
		}
		#endregion Rotation
		public override Vector3[] Receive(Beamer predecessor)
		{
				this.prevBeamer = predecessor;
				predecessor.NewInChain(this);
				return AddMeToChain(RecalculateBeam());
		}

		private Vector3[] RecalculateBeam()
		{
				Vector3[] reflections = LookForReflections(Position, Direction);
				return reflections;
		}

		private void Rotate()
		{
				float endX = Input.mousePosition.x;
				float dir = Mathf.Sign(endX - startX);
				transform.Rotate(0, 0, (90f / 8f) * dir);

				//Recalculate reflections
				InformChange();
		}
		private void InformChange()
		{
				//If i'm connected to the chain
				if (prevBeamer != null)
				{
						if (nextBeamer != null)
						{
								nextBeamer.OutOfChain();
								nextBeamer = null;
						}
						ChainChanged(RecalculateBeam());
				}
		}
}
