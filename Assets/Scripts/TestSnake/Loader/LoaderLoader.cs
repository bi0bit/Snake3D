using Reflex.Attributes;
using Reflex.Core;
using Reflex.Extensions;
using TestSnake.Loader.Service;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace TestSnake.Loader
{
    public class LoaderLoader : MonoBehaviour
    {
        private Container _container;
        
        private ILoadGameService _loadGameService;
        
        [Inject]
        private void Inject(ILoadGameService loadGameService)
        {
            _loadGameService = loadGameService;
        }
        
        private void Awake()
        {
            _container = gameObject.scene.GetSceneContainer();
            
            SceneScope.OnSceneContainerBuilding += OverrideParent;
            
            _loadGameService.LoadMainMenu();
        }
        
        private void OverrideParent(Scene scene, ContainerBuilder builder)
        {
            builder.SetParent(_container);
        }
        
        private void OnDestroy()
        {
            SceneScope.OnSceneContainerBuilding -= OverrideParent;
        }
    }
}