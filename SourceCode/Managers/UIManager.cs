// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using TMPro;
using UnityEngine;

namespace Managers {
    public class UIManager : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI missText;


        [SerializeField] private GameObject uiStart;
        [SerializeField] private GameObject uiGame;
        [SerializeField] private GameObject uiEnd;
        private int _score;
        private int _missCount;

        #region Unity Calls

        private void Start() {
            OnReset();
        }

        public void Update() {
            scoreText.text = $"Score:{_score.ToString()}";
            missText.text = $"Misses:{_missCount.ToString()}";
        }

        public void SetUIScreen(GameState state) {
            uiStart.SetActive(false);
            uiGame.SetActive(false);
            uiEnd.SetActive(false);
            switch (state) {
                case GameState.Start:
                    uiStart.SetActive(true);
                    break;
                case GameState.Playmode:
                    uiGame.SetActive(true);
                    break;
                case GameState.GameOver:
                    uiEnd.SetActive(true);
                    break;
            }
        }

        #endregion


        public void IncreaseScore(int value) => _score += value;
        public void IncreaseMissCount(int value) => _missCount += value;

        public void OnReset() {
            _score = 0;
            _missCount = 0;
        }


#if UNITY_EDITOR
        private void OnValidate() {
            scoreText.text = "Score";
            missText.text = "Miss";
        }
#endif
    }
}