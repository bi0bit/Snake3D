using UnityEngine;

namespace TestSnake.Player
{
	public interface IGameInput
	{
		public Vector2 GetMoveDirection();
	}
}