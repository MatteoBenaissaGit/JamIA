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
        public int Money { get; private set; }
        public int Experience { get; private set; }
        public int SeedUnlocked { get; private set; }

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

        private void Start()
        {
            UI.Initialize();
            UnlockSeed(1);
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
            IsHoldingSeed = false;
            
            UI.SetSeedOnMouse(false, SeedType.One);
            _currentPointedBlock?.FeedbackHoldingSeed(false);

            if (block == null || block.CanGetSeed() == false) 
            {
                return;
            }
            
            _currentSeedHeldSlot.SetAmount(-1);
            block.PlantSeed(_currentSeedHeld);
        }

        public void SetMoney(int amountToAdd)
        {
            Money += amountToAdd;
            UI.SetMoney(Money);
        }

        private void UnlockSeed(int amount = 0)
        {
            UI.InventoryBar.CreateInventorySlot((SeedType)SeedUnlocked, amount);
            UI.Shop.UnlockSeed(SeedUnlocked);
            SeedUnlocked++;
        }

        public void AddExperience(int amount)
        {
            Experience += amount;
            SeedUnlockCheck();
            UI.SetExperience(Experience);
        }
        
        private void SeedUnlockCheck()
        {
            if (Seeds.Data.Count <= SeedUnlocked)
            {
                return;
            }
            
            if (Seeds.Data[SeedUnlocked].ExperienceToUnlock <= Experience)
            {
                UnlockSeed();
            }
        }
    }
}