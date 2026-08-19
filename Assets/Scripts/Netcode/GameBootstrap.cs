using Unity.NetCode;
using UnityEngine.Scripting;

namespace Netcode
{
    [Preserve]
    public class GameBootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName)
        {
            CreateLocalWorld(defaultWorldName);
            return true;
        }
    }
}