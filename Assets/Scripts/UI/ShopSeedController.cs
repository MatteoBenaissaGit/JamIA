using Data;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopSeedController : MonoBehaviour
    {
        [SerializeField] private SeedType _seedType;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _buyButton;
        [SerializeField] private GameObject _lock;
        [SerializeField] private TMP_Text _lockText;

        private SeedData _data;
        
        public void Initialize()
        {
            _data = GameManager.Instance.Seeds.GetData(_seedType);
            _priceText.text = _data.ShopPrice.ToString();
            _iconImage.sprite = _data.Icon;
            _titleText.text = _data.Name;

            SetLock(false);

            _buyButton.onClick.AddListener(BuyButtonClicked);
        }

        public void SetLock(bool unlock)
        {
            _buyButton.enabled = unlock;
            _lock.SetActive(unlock == false);

            if (unlock == false)
            {
                _lockText.text = $"You need {_data.ExperienceToUnlock} experience to unlock this seed";
            }
        }

        private void BuyButtonClicked()
        {
            if (GameManager.Instance.Money < _data.ShopPrice)
            {
                return;
            }

            GameManager.Instance.UI.InventoryBar.AddSeedToSlot(_seedType, 1);
            GameManager.Instance.SetMoney(-_data.ShopPrice);
        }
    }
}
