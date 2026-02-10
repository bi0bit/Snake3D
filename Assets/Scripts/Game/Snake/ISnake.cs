using Snake3DWorld.Game.Food;
using Snake3DWorld.Game.Movement;
using Snake3DWorld.Game.Snake.Data;
using UnityEngine.Events;

namespace Snake3DWorld.Game.Snake
{
	public interface ISnake
	{
		public SnakeParameters Data { get; }
		
		public ASnakeNode[] Body { get; }
		
		public IMovement Movement { get; }
		
		public IEater Eater { get; }
		
		public int SizeTail { get; } 
		
		public event UnityAction OnGrow;

		public void Grow();
	}
}