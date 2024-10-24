using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockManager : MonoBehaviour
    {
        public List<BlockController> Blocks { get; private set; }

        private void Awake()
        {
            Blocks ??= new List<BlockController>();
            foreach (Transform child in transform)
            {
                Blocks.Add(child.GetComponent<BlockController>());
            }
        }

        public void AddBlock(BlockController block)
        {
            Blocks ??= new List<BlockController>();
            Blocks.Add(block);
        }

        public void ClearBlocks()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            
            Blocks?.Clear();
        }
    }
}
