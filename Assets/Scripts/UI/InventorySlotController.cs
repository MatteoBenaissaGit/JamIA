using System;
using DG.Tweening;
using Game;
using Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _seedIcon;
        [SerializeField] private TMP_Text _amountText;
        
        public SeedType Type { get; private set; }
        public int Amount { get; private set; }
        
        private bool _isMouseOver;
        
        public void Initialize(SeedType type, int amount)
        {
            Type = type;
            
            _seedIcon.sprite = GameManager.Instance.Seeds.GetData(type).Icon;
            SetAmount(amount);
            
            transform.localScale = Vector3.zero;
            transform.DOComplete();
            transform.DOScale(Vector3.one, 0.5f);
            
            InputManager.Instance.InputScheme.Map.ClickLeftButton.started += ctx => OnPointerClick();
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (_isMouseOver ? 1.1f : 1f), 0.1f);
        }

        public void SetAmount(int amountToAdd)
        {
            Amount += amountToAdd;
            
            if (Amount <= 0)
            {
                Amount = 0;
            }
            _seedIcon.DOComplete();
            _seedIcon.DOFade(Amount <= 0 ? 0.5f : 1f, 0.5f);
            
            _amountText.text = Amount.ToString();
            
            if (amountToAdd > 0)
            {
                transform.DOComplete();
                transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Amount <= 0) return;
            
            _isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isMouseOver = false;
        }

        private void OnPointerClick()
        {
            if (_isMouseOver == false || Amount <= 0) return;

            GameManager.Instance.SetHoldingSeed(Type, this);
        }
    }
}