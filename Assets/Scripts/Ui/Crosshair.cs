using UnityEngine;
using UnityEngine.InputSystem;

namespace Ui
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private RectTransform _crosshair;

        private InputAction _crosshairAction;

        private void Start()
        {
            //Cursor.visible = false;

            _crosshairAction = InputSystem.actions.FindAction("Player/Crosshair", true);
        }

        private void LateUpdate()
        {
            _crosshair.position = _crosshairAction.ReadValue<Vector2>();
        }
    }
}
