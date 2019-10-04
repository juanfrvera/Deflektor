using UnityEngine;

public class ImageOptions : MonoBehaviour
{
		[SerializeField] UnityEngine.UI.Image image;
		[SerializeField] Sprite[] sprites;
		[SerializeField] EventLib.Event onChange;

		private byte index;
		public byte Index
		{
				get => index;
				set
				{
						index = value;
						image.sprite = sprites[value];
				}
		}

		public void Clicked()
		{
				if (Index < sprites.Length - 1)
						Index++;
				else
						Index = 0;

				onChange?.Invoke();
		}
}