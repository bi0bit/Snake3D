using UnityEngine;

namespace TestSnake.Map.Data
{
    [CreateAssetMenu(fileName = nameof(MapData), menuName = "Parameters/"+nameof(MapData), order = 0)]
    public class MapData : ScriptableObject
    {
        [field: SerializeField, Tooltip("Target snake size for end level")] public float StartSnakeSize { get; private set; }
        [field: SerializeField, Tooltip("Start snake size in segments")] public float TargetSnakeSize { get; private set; }
        
        [field: SerializeField, Tooltip("Snake for this map")] public Snake.Impl.Snake Snake { get; private set; }
    }
}