// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using System;
using System.Collections;
using UnityEngine;

namespace Managers {
    public enum AudioID {
        Collected,
        Toxic,
        Miss
    }

    public class AudioManager : MonoBehaviour {
        private const string POOL_NAME = "Audio Sources";
        private const Int16 SFX_SIZE = 10;
        private GenericObjectPool _sourceAudioPool;

        [SerializeField] private AudioClip _collectedClip;
        [SerializeField] private AudioClip _toxicClip;
        [SerializeField] private AudioClip _missedClip;

        private void Awake() {
            GameObject audioObj = new GameObject("SFX");
            audioObj.AddComponent<AudioSource>();
            _sourceAudioPool = new GenericObjectPool(audioObj, SFX_SIZE, POOL_NAME);
        }

        private void Start() {
            _sourceAudioPool.CreatePool(Create);
        }

        private void Create(GameObject obj, GameObject[] pool, string poolName) {
            var poolTransform = new GameObject(poolName) {
                transform = { parent = transform }
            };

            for (int i = 0; i < pool.Length; i++) {
                var gm = Instantiate(obj, poolTransform.transform);
                gm.SetActive(false);
                pool[i] = gm;
            }
        }

        public void PlayAudio(AudioID id) {
            AudioClip audioClip = id switch {
                AudioID.Collected => _collectedClip,
                AudioID.Toxic => _toxicClip,
                AudioID.Miss => _missedClip,
            };
            StartCoroutine(RunAudio(audioClip));
        }

        IEnumerator RunAudio(AudioClip clip) {
            var sourceGameObject = GetFromPool();
            var source = sourceGameObject.GetComponent<AudioSource>();
            source.clip = clip;
            sourceGameObject.SetActive(true);
            source.Play();
            yield return new WaitWhile(() => source.isPlaying);
            sourceGameObject.SetActive(false);
        }

        private GameObject GetFromPool() {
            foreach (var source in _sourceAudioPool.pool) {
                if (!source.activeInHierarchy)
                    return source;
            }

            return null;
        }
    }
}