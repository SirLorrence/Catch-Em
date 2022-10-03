// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public enum PoolOptions {
        Small,
        Large,
        Toxic
    }


    public class ObjectManager : MonoBehaviour {
        [Header("Fish Object Pools")] [SerializeField]
        private GameObject _fishSmall;

        [SerializeField] private GameObject _fishLarge;

        [SerializeField] private GameObject _fishToxic;

        // pools
        private GenericObjectPool _smPool;
        private GenericObjectPool _lgPool;
        private GenericObjectPool _txPool;

        private const string _smName = "Small Fish";
        private const string _lgName = "Large Fish";
        private const string _txName = "Toxic Fish";

        // pool sizes
        private const int SM_SIZE = 15;
        private const int LG_SIZE = 15;
        private const int TX_SIZE = 15;


        private List<FishEntity> _entityList;

        public List<FishEntity> EntityList => _entityList;

        private void Awake() {
            _smPool = new GenericObjectPool(_fishSmall, SM_SIZE, _smName);
            _lgPool = new GenericObjectPool(_fishLarge, LG_SIZE, _lgName);
            _txPool = new GenericObjectPool(_fishToxic, TX_SIZE, _txName);
            _entityList = new List<FishEntity>();
        }

        private void Start() {
            _smPool.CreatePool(Create);
            _lgPool.CreatePool(Create);
            _txPool.CreatePool(Create);
        }

        private void Update() { }


        private void Create(GameObject obj, GameObject[] pool, string poolName) {
            var poolTransform = new GameObject(poolName) {
                transform = { parent = transform }
            };

            for (int i = 0; i < pool.Length; i++) {
                var gm = Instantiate(obj, poolTransform.transform);
                gm.SetActive(false);
                pool[i] = gm;
                _entityList.Add(gm.GetComponent<FishEntity>());
            }
        }

        private GameObject[] GetPool(PoolOptions selectPool) {
            return selectPool switch {
                PoolOptions.Small => _smPool.pool,
                PoolOptions.Large => _lgPool.pool,
                PoolOptions.Toxic => _txPool.pool,
                _ => _smPool.pool
            };
        }

        // /// <summary>
        // /// Check if any object from every pool is in use
        // /// </summary>
        // public bool CheckInUse() {
        //     var numberOfPools = Enum.GetNames(typeof(PoolOptions)).Length;
        //     for (int i = 0; i < numberOfPools; i++) {
        //         var pool = GetPool((PoolOptions)i);
        //         foreach (var obj in pool) {
        //             if (obj.activeInHierarchy)
        //                 return true;
        //         }
        //     }
        //     return false;
        // }
        public void ResetPools() {
            var numberOfPools = Enum.GetNames(typeof(PoolOptions)).Length;
            for (int i = 0; i < numberOfPools; i++) {
                var pool = GetPool((PoolOptions)i);
                foreach (var obj in pool) {
                    if (obj.activeInHierarchy)
                        obj.SetActive(false);
                }
            }
        }

        public GameObject GetFromPool(PoolOptions options) {
            var pool = GetPool(options);
            foreach (var o in pool) {
                if (!o.activeInHierarchy)
                    return o;
            }

            return null;
        }
    }
}