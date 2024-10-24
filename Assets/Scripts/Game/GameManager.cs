using System;
using Data;
using Inputs;
using UI;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [field:SerializeField] public BlockManager BlockManager { get; private set; }
        [field:SerializeField] public SeedsData Seeds { get; private set; }
        [field:SerializeField] public UIManager UI { get; private set; }
        
        public bool IsHoldingSeed { get; private set; }

        private SeedType _currentSeedHeld;
        private InventorySlotController _currentSeedHeldSlot;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            ManageSeedHolding();
        }

        private BlockController _currentPointedBlock;
        private void ManageSeedHolding()
        {
            if (IsHoldingSeed == false || InputManager.Instance.HasBlockOnMouse(out var block) == false || block.CanGetSeed() == false)
            {
                _currentPointedBlock?.FeedbackHoldingSeed(false);
                _currentPointedBlock = null;
                return;
            }

            if (block == _currentPointedBlock)
            {
                return;
            }

            _currentPointedBlock?.FeedbackHoldingSeed(false);
            block.FeedbackHoldingSeed(true);
            _currentPointedBlock = block;
        }

        public void SetHoldingSeed(SeedType type, InventorySlotController slot)
        {
            IsHoldingSeed = true;
            _currentSeedHeld = type;
            _currentSeedHeldSlot = slot;
            
            UI.SetSeedOnMouse(true, type);
        }

        public void DropSeedOnBlock(BlockController block)
        {
            UI.SetSeedOnMouse(false, SeedType.One);
            _currentPointedBlock?.FeedbackHoldingSeed(false);

            if (block == null || block.CanGetSeed() == false) 
            {
                IsHoldingSeed = false;
                return;
            }
            
            _currentSeedHeldSlot.SetAmount(-1);
            block.PlantSeed(_currentSeedHeld);
        }
    }
}