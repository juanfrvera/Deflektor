using NodeSpace;
using System.Collections.Generic;
using UnityEngine;

public abstract class Beamer : MonoBehaviour, IBeamable
{
		[SerializeField] float maxDistance = 100f;
		private IList<Node> nextNodes;
		//Properties
		public Vector3 Position => transform.position;
		//Methods
		public abstract Node Conect(Vector3 hitPoint, Vector3 direction, Vector3 normal);
		public virtual void Emit() { }
		protected virtual IList<Node> LookForReflections(Vector3 origin, Vector3 direction)
		{
				RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, Play.BeamerLayer);

				if (hit.transform != null)
				{
						var beamer = hit.transform.GetComponent<Beamer>();
						if (beamer != null)
						{
								if (beamer != this)
								{
										if (!beamer.AmIYourChild(this))
												return new List<Node> { beamer.Conect(hit.point, direction, hit.normal) };
										else
										{
												Debug.LogWarning("I was trying to reflect my predecessor");
												return null;
										}
								}
								else
										Debug.LogError("Somehow the raycast hited on myself");
						}
				}
				//Return a point outside screen
				return new List<Node> { new Node(origin + direction * maxDistance) };
		}

		protected IList<Node> AddMeToChain(IList<Node> chain, Vector3 position)
		{
				Node node = new Node(this, position);
				if (chain != null)
				{
						chain.Insert(0, node);//Insert at the start
						return chain;
				}
				else
						return new List<Node> { node };
		}
		protected IList<Node> AddMeToChain(IList<Node> chain) => AddMeToChain(chain, Position);

		protected virtual void AddNodes(IList<Node> nodes)
		{
				if (nextNodes == null)
						nextNodes = new List<Node>(nodes.Count);

				foreach (var node in nodes)
				{
						if (node.Beamer != null && node.Beamer == this)
								print("I was trying to add myself");
						else
								nextNodes.Add(node);
				}
		}

		/// <summary>
		/// Returns true if the beamer is one of my next nodes
		/// </summary>
		/// <param name="child"></param>
		/// <returns></returns>
		public bool AmIYourChild(Beamer child)
		{
				if (nextNodes == null)
						return false;
				else
						return ((List<Node>)nextNodes).Find(n => n.Beamer == child) != null;
		}
		public virtual void Disconected()
		{
				foreach (var node in nextNodes)
				{
						if (node.Beamer != null)
								node.Beamer.Disconected();
				}

				nextNodes.Clear();
				print(nextNodes.Count);
		}
}