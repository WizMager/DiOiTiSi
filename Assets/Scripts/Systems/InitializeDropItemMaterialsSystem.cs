using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace Systems
{
    public partial struct InitializeDropItemMaterialsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DropItemMaterialsBuffer>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var materialsBuffer = SystemAPI.GetSingletonBuffer<DropItemMaterialsBuffer>();
            var graphicsSystem = state.World.GetExistingSystemManaged<EntitiesGraphicsSystem>();
            var materialsIdsBuffer =  SystemAPI.GetSingletonBuffer<DropItemMaterialsIdBuffer>();

            if (materialsIdsBuffer.Length > 0)
            {
                state.Enabled = false;
                
                return;
            }
            
            for (var i = 0; i < materialsBuffer.Length; i++)
            {
                materialsIdsBuffer.Add(new DropItemMaterialsIdBuffer
                {
                    Value = graphicsSystem.RegisterMaterial(materialsBuffer[i].Value.Value)
                });
            }

            state.Enabled = false;
        }
    }
}