using UnityEngine;

namespace Snake3DWorld.Common.Input.Impl
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