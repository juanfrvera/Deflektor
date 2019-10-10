using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
		[SerializeField] LineRenderer line;

		public void Initialize(IList<Vector3> positions)
		{
				line.positionCount = positions.Count;
				line.SetPositions(((List<Vector3>)positions).ToArray());
		}
}
