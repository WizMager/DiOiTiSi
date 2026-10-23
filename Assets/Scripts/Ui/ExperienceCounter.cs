using Components;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class ExperienceCounter : MonoBehaviour
    {
        [SerializeField] private Slider _expSlider;
        [SerializeField] private TMP_Text _expText;
        
        private EntityManager _entityManager;
        private EntityQuery _playersExperienceQuery;
        private float _previousValue;

        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _playersExperienceQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<ExperienceComponent>());
        }

        private void Update()
        {
            var expSingleton = _playersExperienceQuery.GetSingleton<ExperienceComponent>();
            if (Mathf.Approximately(_previousValue, expSingleton.Current))
                return;
            
            _previousValue = expSingleton.Current;
            _expSlider.value = expSingleton.Current / expSingleton.LevelUp;
            _expText.text = expSingleton.Current + " / " + expSingleton.LevelUp;
        }
    }
}