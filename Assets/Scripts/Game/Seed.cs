using System;
using Data;
using DG.Tweening;
using UnityEngine;

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

        public SeedData Data { get; private set; }
        public SeedState State { get; private set; }

        private void Start()
        {
            Data = GameManager.Instance.Seeds.GetData(Type);

            _seedMesh.SetActive(true);
            _plantMesh.SetActive(false);

            State = SeedState.Seed;

            transform.DOComplete();
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        
        public void Grow()
        {
            _seedMesh.SetActive(false);
            _plantMesh.SetActive(true);

            State = SeedState.Plant;
        }
    }
}