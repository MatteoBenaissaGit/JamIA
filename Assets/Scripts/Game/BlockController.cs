using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField] private bool _isIrrigatedAtStart;
        [SerializeField] private bool _isWaterAtStart;
        [Space]
        [SerializeField] private Material _baseMaterial;
        [SerializeField] private Material _irrigatedMaterial;
        [SerializeField] private Material _waterMaterial;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private ParticleSystem _seedPlantVFX;
        
        public ParticleSystem SeedPlantVFX => _seedPlantVFX;
        
        public bool IsIrrigated { get; private set; }
        public bool IsWater { get; private set; }
        public Seed Seed { get; private set; }

        private float _baseY;
        
        private void Awake()
        {
            SetIrrigated(_isIrrigatedAtStart);
            
            _baseY = transform.position.y;

            if (_isWaterAtStart)
            {
                SetWater();
            }
        }

        public bool CanGetSeed()
        {
            return IsIrrigated && Seed == null && IsWater == false; //TODO also check if block has engine on it
        }
        
        public void SetIrrigated(bool irrigated)
        {
            IsIrrigated = irrigated;
            _renderer.material = irrigated ? _irrigatedMaterial : _baseMaterial;
        }

        public void SetWater()
        {
            IsWater = true;
            _renderer.material = _waterMaterial;
        }

        private bool _holdingFeedback;
        public void FeedbackHoldingSeed(bool doPoint)
        {
            if (_holdingFeedback == doPoint)
            {
                return;
            }
            _holdingFeedback = doPoint;
            
            float y = doPoint ? _baseY + 0.2f : _baseY;
            
            transform.DOComplete();
            transform.DOMoveY(y, 0.25f).SetEase(Ease.InOutSine);
        }
        
        public void PlantSeed(SeedType currentSeedHeld)
        {
            _seedPlantVFX.Play();
            
            var seedPrefab = GameManager.Instance.Seeds.GetData(currentSeedHeld).Prefab.GetComponent<Seed>();
            Seed = Instantiate(seedPrefab, transform);
            Seed.transform.position = transform.position + Vector3.up * 0.5f;
            Seed.Initialize(this);
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_isIrrigatedAtStart)
            {
                Gizmos.color = new Color(1f, 0.55f, 0.22f);
                Gizmos.DrawSphere(transform.position + Vector3.up * 0.5f, 0.2f);
            }
            if (_isWaterAtStart)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position + Vector3.up * 0.5f, 0.2f);
            }
        }

#endif
    }
}
