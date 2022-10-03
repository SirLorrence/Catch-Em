// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Managers {
    public enum GameState {
        Start,
        Playmode,
        GameOver
    }

    public class GameManager : MonoBehaviour {
        private const int _difficultyInterval = 15; // maybe 30
        private const int _maximumMissesAllowed = 20;

        private int _missCount;
        private int _difficultyLevel = 0;
        private float _entitySpeed;
        private bool _initTimer = false;

        private GameTimer _gameTimer;
        private ObjectManager _objectManager;
        private UIManager _uiManager;
        private AudioManager _audioManager;

        private MoveController _moveController;

        [SerializeField] private GameState gameState;

        public int DifficultyLevel => _difficultyLevel;
        public ObjectManager ObjectManager => _objectManager;
        public AudioManager AudioManager => _audioManager;
        public event Action IncreaseDifficulty;
        public event Action GameStart;
        public event Action Restart;

        public event Action GameOver;


        private static GameManager _instance;

        public static GameManager Instance() {
            // Hella lazy initialization  
            if (_instance == null) {
                _instance = GameObject.FindWithTag("GM").GetComponent<GameManager>();
                if (_instance == null) {
                    GameObject gm = new GameObject("Game Manager");
                    _instance = gm.AddComponent<GameManager>();
                }
            }

            return _instance;
        }

        private void Awake() {
            if (_instance == null) _instance = this;
            if (_instance != this)
                Destroy(this.gameObject);
            DontDestroyOnLoad(this.gameObject);

            GetReferences();
        }

        private void Start() {
            _gameTimer = new GameTimer(_difficultyInterval);
            _gameTimer.TimerEvent += OnIncreaseDifficulty;
        }


        private void OnDisable() {
            _gameTimer.TimerEvent -= OnIncreaseDifficulty;
        }

        private void Update() {
            _uiManager.SetUIScreen(gameState);
            switch (gameState) {
                case GameState.Start:
                    InMenu();
                    StartCoroutine(GetKeyDelay());
                    break;
                case GameState.Playmode:
                    InGame();
                    break;
                case GameState.GameOver:
                    StartCoroutine(GetKeyDelay());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InMenu() {
            _initTimer = false;
        }

        private void InGame() {
            if (!_initTimer) {
                _moveController.EnablePlayer = true;
                GameStart?.Invoke();
                _gameTimer.Start();
                _initTimer = true;
            }

            CheckGameOverState();
            _gameTimer.Tick(Time.deltaTime);
        }

        private IEnumerator GetKeyDelay() {
            yield return new WaitForSeconds(2f);
            CheckForKey(GameState.Playmode);
        }

        private void CheckForKey(GameState nextState) {
            InputSystem.onAnyButtonPress.CallOnce(ctrl => gameState = nextState);
            if (nextState == GameState.Playmode) OnReset();
        }

        private void GetReferences() {
            _objectManager = GameObject.FindWithTag("ObjectManager").GetComponent<ObjectManager>();
            _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
            _audioManager = GameObject.FindWithTag("SoundManager").GetComponent<AudioManager>();

            _moveController = GameObject.FindWithTag("GameController").GetComponent<MoveController>();
        }

        private void CheckGameOverState() {
            if (_missCount >= _maximumMissesAllowed)
                OnGameOver();
        }

        private void OnIncreaseDifficulty() {
            print("Difficulty increase");
            _difficultyLevel += 1;
            IncreaseDifficulty?.Invoke();
        }

        private void OnReset() {
            StopAllCoroutines();
            _uiManager.OnReset();
            _difficultyLevel = 0;
            Restart?.Invoke();
        }

        public void OnModifyScore(int value) {
            if (value < 0) {
                var absValue = Mathf.Abs(value);
                _missCount += absValue;
                _uiManager.IncreaseMissCount(absValue);
                return;
            }

            _uiManager.IncreaseScore(value);
        }

        public void OnGameOver() {
            _moveController.EnablePlayer = false;
            _initTimer = false;
            gameState = GameState.GameOver;
            _objectManager.ResetPools();
            GameOver?.Invoke();
        }
    }
}