using UnityEngine;
using UnityEngine.Events;

namespace TestSnake.Snake
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class ASnakeNode : MonoBehaviour
	{
		public UnityEvent<Collider> OnTriggerEnterNode;
		
		public Rigidbody Rigidbody { get; private set; }
		
		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			OnTriggerEnterNode.Invoke(other);
		}
	}
}