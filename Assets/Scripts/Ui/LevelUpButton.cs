using Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class LevelUpButton : MonoBehaviour
    {
        [SerializeField] private GameObject _levelUpButtonObject;
        [SerializeField] private Button _levelUpButton;

        private EntityQuery _levelUpQuery;
        private EntityQuery _inputQuery;
        private int _lvlUps = -1;

        private void Start()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _levelUpQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<LevelUpComponent>());
            _inputQuery = entityManager.CreateEntityQuery(ComponentType.ReadWrite(typeof(InputComponent)));
            
            _levelUpButton.onClick.AddListener(OnLevelUpClick);
        }

        private void OnLevelUpClick()
        {
            var input = _inputQuery.GetSingletonRW<InputComponent>();
            input.ValueRW.LevelUpMenu = true;
            _levelUpButtonObject.SetActive(false);
        }

        private void Update()
        {
            var levelUp = _levelUpQuery.GetSingleton<LevelUpComponent>();
            
            if (levelUp.Value == _lvlUps)
                return;
            
            _lvlUps = levelUp.Value;
            
            _levelUpButtonObject.SetActive(_lvlUps > 0);
        }

        private void OnDestroy()
        {
            _levelUpButton.onClick.RemoveListener(OnLevelUpClick);
        }
    }
}