using UnityEngine;

namespace Snake3DWorld.GameData.Level
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "Parameters/"+nameof(LevelSettings), order = 0)]
    public class LevelSettings : ScriptableObject, ILevelSettings
    {
        [field: SerializeField]
        public int FoodCount { get; private set; }
    }
}