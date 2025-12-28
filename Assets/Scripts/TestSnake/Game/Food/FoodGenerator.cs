using TestSnake.Food;
using TestSnake.Game.Data;
using UnityEngine;
using UnityEngine.Pool;
using Utils.Extenstions.MeshExtensions;

namespace TestSnake.Game.Food
{
	public class FoodGenerator
	{
		private GameProperties _gameData;

		private ObjectPool<AFood> _foodPool;

		private Mesh _mapMesh;

		private float[] _cachedSizes;
		
		private float[] _cachedCumulativeSizes;
		
		private float _totalCached;

		public FoodGenerator(Mesh mapMesh, GameProperties gameData)
		{
			_gameData = gameData;
			_foodPool = new ObjectPool<AFood>(CreateFood, 
				TakeFood, 
				ReturnFood,
				DestroyFood,
				defaultCapacity: gameData.StartFoodCount);

			_mapMesh = mapMesh;
			_totalCached = mapMesh.CalculateTotals(ref _cachedSizes, ref _cachedCumulativeSizes);
		}

		public void InitFoods()
		{
			for (var i = 0; i < _gameData.StartFoodCount; ++i)
			{
				GetFoodInRandomPoint();
			}
		}

		private void GetFoodInRandomPoint()
		{
			var food = _foodPool.Get();

			food.transform.position = 
				_mapMesh.GetRandomPointOnMesh(_cachedSizes, _cachedCumulativeSizes, _totalCached);
		}
		
		private void OnAte(AFood ateFood)
		{
			_foodPool.Release(ateFood);
			GetFoodInRandomPoint();
		}

		#region ObjectPoolMethod

		private AFood CreateFood()
		{
			var newFood = Object.Instantiate(_gameData.FoodPrefab, Vector3.zero, Quaternion.identity);
			newFood.OnAte += OnAte;
			return newFood;
		}

		private void TakeFood(AFood food)
		{
			food.gameObject.SetActive(true);
			food.Reset();
		}

		private void ReturnFood(AFood food)
		{
			food.gameObject.SetActive(false);
		}

		private void DestroyFood(AFood food)
		{
			food.OnAte -= OnAte;
			Object.Destroy(food.gameObject);
		}
		

		#endregion
	}
}