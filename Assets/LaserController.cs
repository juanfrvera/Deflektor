using System.Collections.Generic;
using UnityEngine;
public class LaserController : MonoBehaviour
{
		private static LaserController instance;
		private void Awake() => instance = this;

		[SerializeField] Laser laserPrefab;

		public static Laser AddLaser(IList<Vector3> positions)
		{
				Laser clone = Instantiate(instance.laserPrefab);
				clone.Initialize(positions);
				return clone;
		}
}