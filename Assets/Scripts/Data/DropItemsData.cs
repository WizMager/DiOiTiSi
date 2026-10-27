using System.Collections.Generic;
using Data.DataObject;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(DropItemsData), menuName = "Parameters/" + nameof(DropItemsData))]
    public class DropItemsData : ScriptableObject
    {
        [SerializeField] private List<DropItemDo> _dropItems;
        
        public List<DropItemDo> DropItems => _dropItems;
    }
}