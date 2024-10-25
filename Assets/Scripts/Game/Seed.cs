using System;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public enum SeedType
    {
        One = 0,
        Two = 1,
        Three = 2,
        Four = 3
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
        [SerializeField] private float _windForce = 15f;

        public SeedData Data { get; private set; }
        public SeedState State { get; private set; }
        
        private float _timeToBecomePlant;
        private BlockController _block;
        private float _plantMoneyDropImagePositionY;
        private Vector3 _plantMoneyDropImageScale;
        private float _windMultiplier = 1f;
        private float _time;

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
            _plantMoneyDropImageScale = _plantMoneyDropImage.rectTransform.localScale;
            _plantMoneyDropImage.color = new Color(1f, 1f, 1f, 0f);
            
            GameManager.Instance.AddExperience(Data.ExperienceGainedOnPlant);
        }

        private void Update()
        {
            ManageGrowth();
            ManageMoneyDrop();

            WindEffect();
        }

        private void WindEffect()
        {
            if (State != SeedState.Plant) return;

            _time += Time.deltaTime;
            float wind = Mathf.Sin(_time * _windMultiplier) * _windForce;
            transform.localRotation = Quaternion.Euler(new Vector3(0, wind, 0));
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

                _plantMoneyDropImage.rectTransform.DOComplete();
                _plantMoneyDropImage.rectTransform.localScale = Vector3.zero;
                _plantMoneyDropImage.rectTransform.DOScale(_plantMoneyDropImageScale, 0.2f);
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
            GameManager.Instance.AddExperience(Data.ExperienceGainedOnPlant);

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