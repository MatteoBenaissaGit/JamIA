using System.Collections.Generic;
using Game;
using UnityEngine;

namespace UI
{
    public class InventoryBarController : MonoBehaviour
    {
        [SerializeField] private InventorySlotController _inventorySlotPrefab;
        [SerializeField] private Transform _inventorySlotParent;

        private List<InventorySlotController> _slots = new();

        public void CreateInventorySlot(SeedType seedType, int amount)
        {
            var slot = Instantiate(_inventorySlotPrefab, _inventorySlotParent);
            _slots.Add(slot);
            slot.Initialize(seedType, amount);
        }
    }
}