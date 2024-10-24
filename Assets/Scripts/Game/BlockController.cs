using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField] private bool _isIrrigatedAtStart;
        [Space]
        [SerializeField] private Material _baseMaterial;
        [SerializeField] private Material _irrigatedMaterial;
        [SerializeField] private Material _waterMaterial;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private ParticleSystem _seedPlantVFX;
        
        public bool IsIrrigated { get; private set; }
        public Seed Seed { get; private set; }

        private float _baseY;
        
        private void Awake()
        {
            SetIrrigated(_isIrrigatedAtStart);
            _baseY = transform.position.y;
        }

        public bool CanGetSeed()
        {
            return IsIrrigated && Seed == null; //TODO also check if block is water / has engine on it
        }
        
        public void SetIrrigated(bool irrigated)
        {
            IsIrrigated = irrigated;
            _renderer.material = irrigated ? _irrigatedMaterial : _baseMaterial;
        }

        public void SetWater()
        {
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
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_isIrrigatedAtStart)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position + Vector3.up * 0.5f, 0.2f);
            }
        }

#endif
    }
}
