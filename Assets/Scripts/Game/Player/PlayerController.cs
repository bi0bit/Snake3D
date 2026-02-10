using Snake3DWorld.Game.Snake;
using UnityEngine;

namespace Snake3DWorld.Game.Player
{
	public class PlayerController : MonoBehaviour
	{
		private ISnake _controlSnake;
		private IGameInput _gameInput;

		public void Init(ISnake snake, IGameInput gameInput)
		{
			_controlSnake = snake;
			_gameInput = gameInput;
		}

		private void FixedUpdate()
		{
			_controlSnake.Movement.Move(_gameInput.GetMoveDirection());
		}
		
	}
}