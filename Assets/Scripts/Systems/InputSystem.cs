using Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems
{
    public partial class InputSystem : SystemBase
    {
        private InputAction _move;
        private InputAction _crosshair;
        private InputAction _attack;
        private InputAction _levelUpMenu;

        protected override void OnCreate()
        {
            var actions = UnityEngine.InputSystem.InputSystem.actions;
            actions.Enable();

            _move = actions.FindAction("Player/Move", true);
            _crosshair = actions.FindAction("Player/Crosshair", true);
            _attack = actions.FindAction("Player/Attack", true);
            _levelUpMenu = actions.FindAction("Player/LevelUpMenu", true);
        }

        protected override void OnUpdate()
        {
            foreach (var input in SystemAPI.Query<RefRW<InputComponent>>().WithAll<PlayerTag>())
            {
                input.ValueRW.MoveDirection = _move.ReadValue<Vector2>();
                input.ValueRW.CrosshairPosition = _crosshair.ReadValue<Vector2>();
                input.ValueRW.Attack |= _attack.WasPressedThisFrame();
                input.ValueRW.LevelUpMenu |= _levelUpMenu.WasPressedThisFrame();
            }
        }
    }
}
