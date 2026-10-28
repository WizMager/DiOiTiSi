using Components;
using Unity.Entities;
using Unity.Rendering;

namespace Systems
{
    public partial struct InitializeDropItemMaterialsSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (materialsBuffer, materialsIdsBuffer) in SystemAPI.Query<DynamicBuffer<DropItemMaterialsBuffer>, DynamicBuffer<DropItemMaterialsIdBuffer>>())
            {
                var graphicsSystem = state.World.GetExistingSystemManaged<EntitiesGraphicsSystem>();
            
                for (var i = 0; i < materialsBuffer.Length; i++)
                {
                    materialsIdsBuffer.Add(new DropItemMaterialsIdBuffer
                    {
                        Value = graphicsSystem.RegisterMaterial(materialsBuffer[i].Value.Value)
                    });
                }
            }

            state.Enabled = false;
        }
    }
}