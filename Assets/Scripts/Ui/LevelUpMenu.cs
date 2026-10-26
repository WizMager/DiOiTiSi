using Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Ui
{
    public class LevelUpMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private LevelUpMenuElement[] _levelUpMenuElements;

        private EntityManager _entityManager;
        private EntityQuery _gamePlayQuery;
        private bool _isPause;

        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _gamePlayQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<LevelUpComponent>());

            foreach (var levelUpMenuElement in _levelUpMenuElements)
            {
                levelUpMenuElement.OnElementClicked += OnElementClicked;
            }
        }

        private void OnElementClicked(int index)
        {
            Debug.Log($"Chosen  {index}");
            
            if (!_gamePlayQuery.TryGetSingletonEntity<LevelUpComponent>(out var continueGameEntity))
                return;
            
            _entityManager.SetComponentEnabled<ContinueGameTag>(continueGameEntity, true);
        }

        private void Update()
        {
            if (!_gamePlayQuery.TryGetSingletonEntity<LevelUpComponent>(out var isPauseEntity))
                return;
            
            if (_isPause == _entityManager.IsComponentEnabled<IsPauseTag>(isPauseEntity))
                return;
            
            _isPause = _entityManager.IsComponentEnabled<IsPauseTag>(isPauseEntity);
            
            _root.SetActive(_isPause);
        }

        private void OnDestroy()
        {
            foreach (var levelUpMenuElement in _levelUpMenuElements)
            {
                levelUpMenuElement.OnElementClicked -= OnElementClicked;
            }
        }
    }
}