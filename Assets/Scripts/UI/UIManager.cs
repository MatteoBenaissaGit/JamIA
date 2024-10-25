using System;
using DG.Tweening;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [field:SerializeField] public InventoryBarController InventoryBar { get; private set; }
        [field:SerializeField] public ShopController Shop { get; private set; }

        [SerializeField] private Texture2D _baseCursor;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private Image _moneyIcon;
        [SerializeField] private TMP_Text _experienceText;
        [SerializeField] private Image _experienceIcon;

        private void Awake()
        {
            SetBaseCursor();
        }

        public void Initialize()
        {
            SetMoney(GameManager.Instance.Money);
            SetExperience(GameManager.Instance.Experience);
        }

        public void SetMoney(int amount)
        {
            _moneyText.text = amount.ToString();
            if (amount == 0) return;
            _moneyIcon.transform.DOComplete();
            _moneyIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        }
        
        public void SetExperience(int amount)
        {
            _experienceText.text = amount.ToString();
            if (amount == 0) return;
            _experienceIcon.transform.DOComplete();
            _experienceIcon.transform.DORotate(new Vector3(0,0,360), 1.5f, RotateMode.FastBeyond360).SetEase(Ease.OutBack);
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