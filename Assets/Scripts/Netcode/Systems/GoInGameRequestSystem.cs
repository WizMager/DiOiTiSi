using Netcode.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Netcode.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial struct GoInGameRequestSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkId>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (_, entity) in SystemAPI.Query<RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess())
            {
                ecb.AddComponent<NetworkStreamInGame>(entity);

                var rpcEntity = ecb.CreateEntity();
                ecb.AddComponent<GoInGameRequestRpc>(rpcEntity);
                ecb.AddComponent(rpcEntity, new SendRpcCommandRequest
                {
                    TargetConnection = entity
                });
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}