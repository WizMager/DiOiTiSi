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
            RequireForUpdate<CameraComponent>();

            var actions = UnityEngine.InputSystem.InputSystem.actions;
            actions.Enable();

            _move = actions.FindAction("Player/Move", true);
            _crosshair = actions.FindAction("Player/Crosshair", true);
            _attack = actions.FindAction("Player/Attack", true);
            _levelUpMenu = actions.FindAction("Player/LevelUpMenu", true);
        }

        protected override void OnUpdate()
        {
            var camera = SystemAPI.GetSingleton<CameraComponent>().Value.Value;
            var crosshairPosition = _crosshair.ReadValue<Vector2>();
            var aimRay = camera.ScreenPointToRay(crosshairPosition);

            foreach (var input in SystemAPI.Query<RefRW<InputComponent>>().WithAll<PlayerTag>())
            {
                input.ValueRW.MoveDirection = _move.ReadValue<Vector2>();
                input.ValueRW.CrosshairPosition = crosshairPosition;
                input.ValueRW.AimRayStart = aimRay.origin;
                input.ValueRW.AimRayEnd = aimRay.origin + aimRay.direction * 100f;
                input.ValueRW.Attack |= _attack.WasPressedThisFrame();
                input.ValueRW.LevelUpMenu |= _levelUpMenu.WasPressedThisFrame();
            }
        }
    }
}
