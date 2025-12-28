using System;
using UnityEngine;

namespace TestSnake.Player.Impl
{
	public class TouchJoystickGameInput : MonoBehaviour, IGameInput
	{
		public Vector2 GetMoveDirection()
		{
			// return _joystick.Direction;
			return Vector2.zero;
		}
	}
}