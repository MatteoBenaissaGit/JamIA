using Game;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class BlockCreator : EditorWindow
    {
        private BlockManager _blockManager;
        private BlockController _blockControllerPrefab;

        private Vector2Int _size;

        [MenuItem("Tools/Block Creator")]
        public static void ShowWindow()
        {
            GetWindow<BlockCreator>("Block Creator");
        }

        private void OnGUI()
        {
            _blockControllerPrefab = (BlockController)EditorGUILayout.ObjectField("Block Controller Prefab", _blockControllerPrefab, typeof(BlockController), false);
            _blockManager = (BlockManager)EditorGUILayout.ObjectField("Block Manager", _blockManager, typeof(BlockManager), true);
            _size = EditorGUILayout.Vector2IntField("Size", _size);

            if (GUILayout.Button("Create Blocks"))
            {
                CreateBlocks();
            }
        }

        private void CreateBlocks()
        {
            if (_blockManager == null || _blockControllerPrefab == null) return;

            _blockManager.ClearBlocks();
            
            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    BlockController block = Instantiate(_blockControllerPrefab, _blockManager.transform);
                    block.transform.position = new Vector3(x, 0, y);
                    _blockManager.AddBlock(block);
                }
            }

            EditorUtility.SetDirty(_blockManager);
            AssetDatabase.SaveAssetIfDirty(_blockManager);
        }
    }
}