using System;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [field:SerializeField] public InventoryBarController InventoryBar { get; private set; }

        [SerializeField] private Texture2D _baseCursor;

        private void Awake()
        {
            SetBaseCursor();
        }

        private void Start()
        {
            InventoryBar.CreateInventorySlot(SeedType.One, 4);
            InventoryBar.CreateInventorySlot(SeedType.Two, 0);
        }

        private void SetCursor(Texture2D texture, Vector2 hotSpot)
        {
            Cursor.SetCursor(texture, hotSpot, CursorMode.Auto); 
        }
        
        private void SetBaseCursor()
        {
            SetCursor(_baseCursor, new Vector2(0, 0));
        }
        
        public void SetSeedOnMouse(bool doShow, SeedType type)
        {
            if (doShow == false)
            {
                SetBaseCursor();
                return;
            }
            
            Texture2D texture = GameManager.Instance.Seeds.GetData(type).Cursor;
            SetCursor(texture, new Vector2(15, 15));
        }
    }
}