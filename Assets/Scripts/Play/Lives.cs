using UnityEngine;

public class Lives : MonoBehaviour
{
		[SerializeField] GameObject firstLive;
		[SerializeField] byte maxLives = 5;


		private GameObject[] liveIcons;

		private void Awake()
		{
				liveIcons = new GameObject[maxLives];
				liveIcons[0] = firstLive;
		}

		public void Set(byte value)
		{
				if (value == 0)
				{
						foreach (var live in liveIcons)
						{
								if(live != null)
										live.SetActive(false);
						}
				}
				else
				{
						firstLive.SetActive(true);
						for (byte i = 1; i < value; i++)//Mostrar las que se quieren
						{
								if (liveIcons[i] == null)
										liveIcons[i] = Instantiate(firstLive, firstLive.transform.parent);
								else
										liveIcons[i].SetActive(true);
						}
						for (byte i = value; i < maxLives; i++)//Ocultar las que sobran
						{
								if (liveIcons[i] != null)
										liveIcons[i].SetActive(false);
						}
				}
		}
}
