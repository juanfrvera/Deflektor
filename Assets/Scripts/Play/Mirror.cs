using UnityEngine;

public class Mirror : MonoBehaviour
{
		[SerializeField] float minDragTime = 1f;//The time you have to waite before start rotating with drag
		[SerializeField] float draggingTime = 0.5f;//The waited time when a drag rotation has occur
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
								Rotate();//Make the first rotation
								rotatedWithDrag = true;
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
				transform.Rotate(0, 0, 22.5f * dir);
		}
}
