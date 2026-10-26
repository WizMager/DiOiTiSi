using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class LevelUpMenuElement : MonoBehaviour
    {
        public event Action<int> OnElementClicked;
        
        [SerializeField] private Button _elementButton;
        [SerializeField] private Image _elementImage;
        [SerializeField] private TMP_Text _elementInfotext;
        [SerializeField] private int _index;

        private void Start()
        {
            _elementButton.onClick.AddListener(OnElementClick);
        }

        private void OnElementClick()
        {
            OnElementClicked?.Invoke(_index);
        }

        private void OnDestroy()
        {
            _elementButton.onClick.RemoveListener(OnElementClick);
        }
    }
}