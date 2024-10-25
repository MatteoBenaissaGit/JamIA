using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class SeedData
    {
        [field: SerializeField] public string Name { get; private set; } = "Seed";
        [field:SerializeField] public SeedType Type { get; private set; }
        [field:SerializeField] public Sprite Icon { get; private set; }
        [field:SerializeField] public Texture2D Cursor { get; private set; }
        [field:SerializeField] public Seed Prefab { get; private set; }
        [field: SerializeField] public int ShopPrice { get; private set; } = 10;
        [field: SerializeField] public int ExperienceToUnlock { get; private set; } = 0;
        [field: SerializeField] public int ExperienceGainedOnPlant { get; private set; } = 1;
        [field: SerializeField] public int ExperienceGainedOnGrowth { get; private set; } = 2;
        [field: SerializeField] public float TimeToBecomePlant { get; private set; } = 5;
        [field: SerializeField] public float TimeToDropMoney { get; private set; } = 5;
        [field: SerializeField] public int MoneyDropped { get; private set; } = 1;
    }
    
    [CreateAssetMenu(fileName = "SeedData", menuName = "ScriptableObjects/Seed", order = 1)]
    public class SeedsData : ScriptableObject
    {
        [field:SerializeField] public List<SeedData> Data { get; private set; }

        public SeedData GetData(SeedType type)
        {
            SeedData seedData = Data.Find(x => x.Type == type);
            if (seedData == null)
            {
                Debug.LogError($"Seed data {type} not found!");
            }
            return seedData;
        }
    }
}