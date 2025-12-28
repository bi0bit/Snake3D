
using Reflex.Core;
using Reflex.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace TestSnake.Loader
{
    public class StartUpLoader : MonoBehaviour
    {
        [SerializeField]
        private AssetReference _loaderScene;
        
        private void Awake()
        {
            var container = gameObject.scene.GetSceneContainer();
            
            void OverrideParent(Scene scene, ContainerBuilder builder)
            {
                builder.SetParent(container);
            }
            
            SceneScope.OnSceneContainerBuilding += OverrideParent;
            
            Addressables.LoadSceneAsync(_loaderScene, LoadSceneMode.Additive).Completed += operation =>
            {
                SceneScope.OnSceneContainerBuilding -= OverrideParent;
            };
        }
        
    }
}