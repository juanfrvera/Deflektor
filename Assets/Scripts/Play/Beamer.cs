using System.Collections.Generic;
using UnityEngine;

public abstract class Beamer : MonoBehaviour, IBeamable
{
		[SerializeField] float maxDistance = 100f;

		protected Beamer prevBeamer, nextBeamer;

		//Properties
		public Vector3 Position => transform.position;
		public abstract Vector3 Direction { get; }

		//Methods
		public abstract Vector3[] Receive(Beamer predecessor);
		public virtual void Emit() { }

		protected virtual Vector3[] LookForReflections(Vector3 origin, Vector3 direction)
		{
				RaycastHit2D hit = Physics2D.Raycast(origin, direction);

				if (hit.transform != null)
				{
						var beamer = hit.transform.GetComponent<Beamer>();
						if (beamer != null)
						{
								if (beamer != this)
								{
										if (beamer != prevBeamer)
												return beamer.Receive(this);
										else
										{
												Debug.LogWarning("I was trying to reflect my predecessor");
												prevBeamer.Hurt();
												return new Vector3[] { beamer.Position };
										}
								}
								else
										Debug.LogError("Somehow the raycast hited on myself");
						}
				}
				//Return a point outside screen
				return new Vector3[] { origin + direction * maxDistance };
		}

		protected Vector3[] AddMeToChain(Vector3[] reflections)
		{
				if (reflections != null)
				{
						Vector3[] finalReflections = new Vector3[reflections.Length + 1];
						finalReflections[0] = Position;//Add me firts
						reflections.CopyTo(finalReflections, 1);//Copy the others

						return finalReflections;
				}
				else
						return new Vector3[] { Position };
		}

		public virtual void ChainChanged(Vector3[] points)
		{
				if (prevBeamer != null)
				{
						prevBeamer.ChainChanged(AddMeToChain(points));
				}
		}
		public virtual void Hurt()
		{
				prevBeamer?.Hurt();
		}
		public virtual void NewInChain(Beamer beamer)
		{
				nextBeamer = beamer;
		}
		public virtual void OutOfChain()
		{
				prevBeamer = null;
				if(nextBeamer != null)
				{
						nextBeamer.OutOfChain();
						nextBeamer = null;
				}
		}
}