using Gameplay.Components;
using Netcode.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

namespace Netcode.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct GoInGameServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerPrefabComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var playerPrefab = SystemAPI.GetSingleton<PlayerPrefabComponent>().Value;

            foreach (var (rpc, rpcEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestRpc>().WithEntityAccess())
            {
                var connection = rpc.ValueRO.SourceConnection;
                var networkId = state.EntityManager.GetComponentData<NetworkId>(connection);
                
                ecb.AddComponent<NetworkStreamInGame>(connection);

                var playerEntity = state.EntityManager.Instantiate(playerPrefab);
                ecb.SetComponent(playerEntity, new GhostOwner
                {
                    NetworkId = networkId.Value
                });
                
                ecb.AppendToBuffer(connection, new LinkedEntityGroup
                {
                    Value = playerEntity
                });
                
                ecb.DestroyEntity(rpcEntity);
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}