using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BlockManager : MonoBehaviour
    {
        public List<BlockController> Blocks { get; private set; }

        public void AddBlock(BlockController block)
        {
            Blocks ??= new List<BlockController>();
            Blocks.Add(block);
        }

        public void ClearBlocks()
        {
            if (Blocks == null) return;
            
            foreach (BlockController block in Blocks)
            {
                if (block == null) continue;
                DestroyImmediate(block.gameObject);
            }
            Blocks.Clear();
        }
    }
}
