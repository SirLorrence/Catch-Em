// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using System;
using Managers;
using UnityEngine;

public class FishEntity : MonoBehaviour {
    private const float _initSpeed = 5f;
    private const float _speedIncrease = 1.2f;
    private float _currentSpeed;
    private float _futureSpeed;
    private int _points;
    private Animator _animator;
    public event Action GameOver;
    public event Action<int> ModifyScore;

    private enum FishType {
        Small,
        Large,
        Toxic
    }

    [SerializeField] private FishType fishType;

    private void Awake() {
        _animator = GetComponent<Animator>();
        SetUpEntity();
        SetInitSpeed();
    }

    private void Start() {
        AssignEvents();
    }

    private void Update() {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _futureSpeed, Time.deltaTime);
        transform.Translate(Vector3.forward * (Time.deltaTime * _currentSpeed));
        if (transform.position.z < -5f) {
            ModifyScore?.Invoke(-_points);
            if (fishType != FishType.Toxic) GameManager.Instance().AudioManager.PlayAudio(AudioID.Miss);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            OnCollected();
        }

        Debug.Log(other.name);
    }

    private void AssignEvents() {
        // GameManager.Instance().IncreaseDifficulty += IncreaseSpeed;
        GameManager.Instance().Restart += OnRest;
        ModifyScore += GameManager.Instance().OnModifyScore;
        GameOver += GameManager.Instance().OnGameOver;
    }

    private void SetUpEntity() {
        _points = fishType switch {
            FishType.Small => 1,
            FishType.Large => 5,
            _ => 0 // should be impossible to get....hopefully
        };
    }

    private void OnCollected() {
        if (fishType == FishType.Toxic) {
            GameOver?.Invoke();
            GameManager.Instance().AudioManager.PlayAudio(AudioID.Toxic);
        }
        else {
            ModifyScore?.Invoke(_points);
            GameManager.Instance().AudioManager.PlayAudio(AudioID.Collected);
        }

        gameObject.SetActive(false);
    }

    private void SetInitSpeed() {
        _currentSpeed = _initSpeed;
        _futureSpeed = _currentSpeed;
        _animator.speed = 1;
    }


    private void OnRest() => SetInitSpeed();

    public void IncreaseSpeed() {
        var dLvl = GameManager.Instance().DifficultyLevel;
        _futureSpeed = _initSpeed * Mathf.Pow(_speedIncrease, dLvl);
        _animator.speed += 0.25f;
    }
}