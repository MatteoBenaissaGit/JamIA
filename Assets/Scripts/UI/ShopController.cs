using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _shopGroup;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Sprite _shopIcon, _shopCloseIcon;
        [SerializeField] private Button _seedButton, _utilitiesButton;
        [SerializeField] private CanvasGroup _seedGroup, _utilitiesGroup;
        [SerializeField] private List<ShopSeedController> _shopSeedControllers;

        private bool _isShopOpen;

        private void ShowGroup(CanvasGroup group, bool doShow)
        {
            group.alpha = doShow ? 1 : 0;
            group.blocksRaycasts = doShow;
            group.interactable = doShow;
        }
    
        private void Start()
        {
            _shopButton.onClick.AddListener(() =>
            {
                _isShopOpen = _isShopOpen == false;
                _shopButton.image.sprite = _shopButton.image.sprite == _shopIcon ? _shopCloseIcon : _shopIcon;
                ShowGroup(_shopGroup, _isShopOpen);
            });
        
            _seedButton.onClick.AddListener(() =>
            {
                _seedButton.image.color = Color.white;
                _utilitiesButton.image.color = new Color(1f, 1f, 1f, 0.5f);
                ShowGroup(_seedGroup, true);
                ShowGroup(_utilitiesGroup, false);
            });
        
            _utilitiesButton.onClick.AddListener(() =>
            {
                _utilitiesButton.image.color = Color.white;
                _seedButton.image.color = new Color(1f, 1f, 1f, 0.5f);
                ShowGroup(_seedGroup, false);
                ShowGroup(_utilitiesGroup, true);
            });
        
            _seedButton.image.color = Color.white;
            _utilitiesButton.image.color = new Color(1f, 1f, 1f, 0.5f);
            ShowGroup(_seedGroup, true);
            ShowGroup(_utilitiesGroup, false);
        
            ShowGroup(_shopGroup, false);
        
            _shopSeedControllers.ForEach(x => x.Initialize());
        }
        
        public void UnlockSeed(int seedId)
        {
            Debug.Log(_shopSeedControllers[seedId].name);
            _shopSeedControllers[seedId].SetLock(true);
        }
    }
}
