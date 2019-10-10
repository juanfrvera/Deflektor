using System.Collections.Generic;
using UnityEngine;

namespace NodeSpace
{
		public class Node
		{
				private Vector3 position;
				private Beamer beamer;
				//Properties
				public Vector3 Position
				{
						get => position;
						private set => position = value;
				}
				public Beamer Beamer
				{
						get => beamer;
						private set => beamer = value;
				}
				//Constructors
				public Node(Vector3 position)
				{
						Position = position;
				}
				public Node(Beamer beamer, Vector3 position)
				{
						Position = position;
						Beamer = beamer;
				}
		}
		public static class Utility
		{
				public static IList<Vector3> Positions(this IList<Node> nodes)
				{
						IList<Vector3> positions = new List<Vector3>(nodes.Count);
						foreach (var node in nodes)
						{
								positions.Add(node.Position);
						}
						return positions;
				}
		}
}