using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class SeedData
    {
        [field:SerializeField] public SeedType Type { get; private set; }
        [field:SerializeField] public Sprite Icon { get; private set; }
        [field:SerializeField] public Texture2D Cursor { get; private set; }
        [field:SerializeField] public Seed Prefab { get; private set; }
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
                Debug.LogError("Seed data not found!");
            }
            return seedData;
        }
    }
}