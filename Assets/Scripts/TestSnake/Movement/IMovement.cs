using UnityEngine;

namespace TestSnake.Snake
{
	public interface IMovement
	{
		public void Move(Vector2 inputDirection);
	}
}