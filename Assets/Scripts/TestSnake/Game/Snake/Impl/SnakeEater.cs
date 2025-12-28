using TestSnake.Food;

namespace TestSnake.Snake.Impl
{
	public class SnakeEater : IEater
	{
		private readonly ISnake _snake;

		public SnakeEater(ISnake snake)
		{
			_snake = snake;
		}
		
		public void TryEat(AFood food)
		{
			if (food.Eat())
			{
				_snake.Grow();
			}
		}
	}
}