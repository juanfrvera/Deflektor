using NodeSpace;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : Beamer, IBeamable
{
		#region Rotation
		[SerializeField] float minDragTime = 1f;//The time you have to waite before start rotating with drag
		[SerializeField] float draggingTime = 0.5f;//The waited time when a drag rotation has occur
		[SerializeField] Transform graphic;
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
		private void Rotate()
		{
				float endX = Input.mousePosition.x;
				float dir = Mathf.Sign(endX - startX);
				graphic.Rotate(0, 0, (90f / 8f) * dir);

				//Recalculate reflections
				UpdateBeam();
		}
		#endregion Rotation
		#region Reflection
		private Vector3 Normal => graphic.up;
		private Vector3 enterDirection;//Direction of the enter ray
		private Laser laser;
		public override Node Conect(Vector3 hitPoint, Vector3 direction, Vector3 normal)
		{
				enterDirection = direction;
				RecalculateBeam(direction);
				return new Node(this, Position);
		}
		private void RecalculateBeam(Vector3 direction) {
				var nodes = LookForReflections(Position, Reflect(direction));
				DrawLaser(nodes);
				AddNodes(nodes);
		}
		private Vector3 Reflect(Vector3 direction) => Vector3.Reflect(direction, Normal);
		private void UpdateBeam()
		{
				Disconected();
				RecalculateBeam(enterDirection);
		}
		private void DrawLaser(IList<Node> nodes)
		{
				var positions = AddMeToChain(nodes).Positions();
				if (laser == null)
						laser = LaserController.AddLaser(positions);
				else
						laser.Initialize(positions);
		}
		#endregion Reflection
}
