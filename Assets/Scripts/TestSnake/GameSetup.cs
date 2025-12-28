using TestSnake.Game;
using TestSnake.Game.Data;
using TestSnake.Map;
using TestSnake.Player;
using UnityEngine;

namespace TestSnake
{
	public class GameSetup : MonoBehaviour
	{
		[SerializeField]
		private GameProperties _gameData;
		
		private PlayerController _playerController;

		private IGameInput _gameInput;

		// private Snake _snake;

		private IMap _map;

		private FoodGenerator _foodGenerator;

		private void Awake()
		{
			Application.targetFrameRate = 60;
			QualitySettings.vSyncCount = 0;
			
			InitComponents();

			InitPlayer();
			
			InitFoodGenerator();
		}

		private void InitComponents()
		{
			_playerController = GetComponentInChildren<PlayerController>();
			// _snake = GetComponentInChildren<ISnake>();
			_gameInput = GetComponentInChildren<IGameInput>();
			_map = GetComponentInChildren<IMap>();
		}

		private void InitPlayer()
		{
			// _playerController.Init(_snake, _gameInput);
		}

		private void InitFoodGenerator()
		{
			_foodGenerator = new FoodGenerator(_map.Mesh, _gameData);
			_foodGenerator.InitFoods();
		}

	}
}