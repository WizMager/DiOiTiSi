using System;
using Enums;

namespace Data.DataObject
{
    [Serializable]
    public struct DropItemDo
    {
        public EDropItem type;
        public float weight;
    }
}