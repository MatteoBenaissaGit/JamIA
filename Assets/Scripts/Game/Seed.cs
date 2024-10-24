using System;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public enum SeedType
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }

    public enum SeedState
    {
        Seed = 0,
        Plant = 1
    }

    public class Seed : MonoBehaviour
    {
        [field:SerializeField] public SeedType Type { get; private set; }

        [SerializeField] private GameObject _seedMesh;
        [SerializeField] private GameObject _plantMesh;
        
        [SerializeField] private GameObject _plantGrowthFill;
        [SerializeField] private Image _plantGrowthFillImage;
        [SerializeField] private Image _plantMoneyDropImage;

        public SeedData Data { get; private set; }
        public SeedState State { get; private set; }
        
        private float _timeToBecomePlant;
        private BlockController _block;
        private float _plantMoneyDropImagePositionY;

        public void Initialize(BlockController block)
        {
            _block = block;
            
            Data = GameManager.Instance.Seeds.GetData(Type);

            _seedMesh.SetActive(true);
            _plantMesh.SetActive(false);

            State = SeedState.Seed;

            transform.DOComplete();
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            
            _timeToBecomePlant = Data.TimeToBecomePlant;

            _plantMoneyDropImagePositionY = _plantMoneyDropImage.rectTransform.localPosition.y;
            _plantMoneyDropImage.color = new Color(1f, 1f, 1f, 0f);
        }

        private void Update()
        {
            ManageGrowth();
            ManageMoneyDrop();
        }

        private float _currentMoneyDropTime;
        private void ManageMoneyDrop()
        {
            if (State != SeedState.Plant) return;
            
            _currentMoneyDropTime -= Time.deltaTime;
            
            if (_currentMoneyDropTime <= 0)
            {
                _currentMoneyDropTime = Data.TimeToDropMoney;
                GameManager.Instance.SetMoney(Data.MoneyDropped);

                _plantMoneyDropImage.DOComplete();
                _plantMoneyDropImage.color = Color.white;
                _plantMoneyDropImage.DOFade(0, 1).SetEase(Ease.InCubic);
                
                _plantMoneyDropImage.rectTransform.localPosition = new Vector3(0, _plantMoneyDropImagePositionY, 0);
                _plantMoneyDropImage.rectTransform.DOLocalMoveY(_plantMoneyDropImagePositionY + 2, 1).SetEase(Ease.InCubic);
            }
        }

        private void ManageGrowth()
        {
            if (State != SeedState.Seed) return;
            
            _timeToBecomePlant -= Time.deltaTime;
            
            _plantGrowthFillImage.fillAmount = 1 - _timeToBecomePlant / Data.TimeToBecomePlant;
            
            if (_timeToBecomePlant <= 0)
            {
                _plantGrowthFill.SetActive(false);
                Grow();
            }
        }

        public void Grow()
        {
            _block.SeedPlantVFX.Play();
            
            _seedMesh.SetActive(false);
            _plantMesh.SetActive(true);

            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            
            State = SeedState.Plant;
            
            _currentMoneyDropTime = Data.TimeToDropMoney;
        }
    }
}