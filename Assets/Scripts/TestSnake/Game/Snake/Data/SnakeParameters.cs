using UnityEngine;

namespace TestSnake.Snake.Data
{
	[CreateAssetMenu(fileName = nameof(SnakeParameters), menuName = "Parameters/"+nameof(SnakeParameters), order = 0)]
	public class SnakeParameters : ScriptableObject
	{
		
		[field: SerializeField] public float MoveSpeed { get; private set; }
		
		[field: SerializeField] public float BodySpeed { get; private set; }
		
		[field: SerializeField] public float TurnSpeed { get; private set; }
		
		[field: SerializeField] public float BodySpace { get; private set; }
	}
}