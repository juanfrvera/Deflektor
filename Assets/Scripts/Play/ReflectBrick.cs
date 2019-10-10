using NodeSpace;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBrick : Beamer
{
		public override Node Conect(Vector3 hitPoint, Vector3 direction, Vector3 normal)
		{
				print(name + " " + normal);
				var nodes = LookForReflections(hitPoint, Reflect(direction, normal));
				AddNodes(nodes);
				return new Node(this, hitPoint);
		}
		private Vector3 Reflect(Vector3 direction, Vector3 normal)
		{
				return Vector3.Reflect(direction, normal);
		}
}
