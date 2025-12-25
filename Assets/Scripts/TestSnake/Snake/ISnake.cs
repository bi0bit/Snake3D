using System.Collections.Generic;
using TestSnake.Food;
using TestSnake.Snake.Data;
using UnityEngine.Events;

namespace TestSnake.Snake
{
	public interface ISnake
	{
		public SnakeParameters Data { get; }
		
		public LinkedList<ASnakeNode> Body { get; }
		
		public IMovement Movement { get; }
		
		public IEater Eater { get; }
		
		public int SizeTail { get; } 
		
		public event UnityAction OnGrow;

		public void Grow();
	}
}