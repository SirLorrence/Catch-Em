// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using Managers;
using UnityEngine;

public class MoveController : MonoBehaviour {
    private PlayerActions _playerActions;
    [SerializeField] private float speed;
    [SerializeField] private MeshRenderer _leveMesh;

    private Vector3 _startPos;

    private float _movementBonds;
    private Bounds _playableArea;

    private bool _enablePlayer;

    public bool EnablePlayer {
        get => _enablePlayer;
        set => _enablePlayer = value;
    }


    [SerializeField] private bool debug;

    private void Awake() {
        _playerActions = new PlayerActions();
        _movementBonds = _leveMesh.bounds.size.x;
        _enablePlayer = false;
#if UNITY_EDITOR
        if (debug) _enablePlayer = true;
#endif
    }

    private void Start() {
        Vector3 vb = new Vector3(_movementBonds, 2f, 2f);
        _playableArea = new Bounds(transform.position, vb);
        _startPos = transform.position;
    }

    private void Update() {
        Move();
    }


    private void Move() {
        if (_enablePlayer) {
            Vector2 moveVector = _playerActions.GamePlay.Movement.ReadValue<Vector2>();
            moveVector.y = 0; // just to be sure
            Vector3 movePos = (Vector3)moveVector * (speed * Time.deltaTime);
            var futurePos = transform.position + movePos;
            if (_playableArea.Contains(futurePos)) {
                transform.Translate(movePos);
            }
        }
    }

    private void OnRestart() {
        transform.position = _startPos;
    }


    private void OnEnable() {
        _playerActions.GamePlay.Enable();
        GameManager.Instance().Restart += OnRestart;
    }

    private void OnDisable() {
        _playerActions.GamePlay.Disable();
        GameManager.Instance().Restart -= OnRestart;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        _movementBonds = _leveMesh.bounds.size.x;
    }

    private void OnDrawGizmos() {
        var bounds = new Vector3(_movementBonds, 2f, 2f);
        Gizmos.DrawWireCube(Vector3.forward * 3f, bounds);
    }
#endif
}