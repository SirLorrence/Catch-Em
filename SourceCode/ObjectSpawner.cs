// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using System.Collections;
using Managers;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour {
    [SerializeField] private MeshRenderer _levelWidth;

    [SerializeField] private float _spawnRate = 0.5f;

    [Header("SpawnRates")] [Tooltip("How often will this pattern spawn (n%)")] [Range(0, 100)] [SerializeField]
    private int _patternRate;

    [Range(0f, 100f)] [SerializeField] private int _initLargeRate;
    [Range(0f, 100f)] [SerializeField] private int _initToxicRate;

    private int _largeFishRate;
    private int _toxicFishRate;

    private bool _runSpawner = false;
    private bool _canSpawn;

    private bool _inPattern;
    private Vector3 _pointA, _pointB; // to draw line
    private Vector3 _selectedSpawn;

    private Spawners _spawnPositions;
    private float _spawnTimer;

    private ObjectManager _objectManager;


    private void OnEnable() {
        GameManager.Instance().IncreaseDifficulty += IncreaseRates;
        GameManager.Instance().GameStart += ActivateSpawner;
        GameManager.Instance().GameOver += ActivateSpawner;
    }

    private void OnDisable() {
        GameManager.Instance().IncreaseDifficulty -= IncreaseRates;
        GameManager.Instance().GameStart -= ActivateSpawner;
        GameManager.Instance().GameOver -= ActivateSpawner;
    }

    private void Start() {
        _objectManager = GameManager.Instance().ObjectManager;

        SetPoints();
        SetInitRates();
        _selectedSpawn = _spawnPositions.SpawnPointPositions[2];
        _inPattern = false;
        _runSpawner = false;
        _canSpawn = true;
    }

    private void Update() {
        if (_runSpawner) {
            RuntimeTimer();
            SpawnLogic();
        }
    }

    private void ActivateSpawner() => _runSpawner = !_runSpawner;

    private void SetInitRates() {
        _largeFishRate = _initLargeRate;
        _toxicFishRate = _initToxicRate;
        _spawnTimer = _spawnRate;
    }

    private void IncreaseRates() {
        _largeFishRate += 10;
        _toxicFishRate += 5;
        IncreaseEntitySpeed();
    }


    private void RuntimeTimer() {
        if (_spawnTimer > 0)
            _spawnTimer -= Time.deltaTime;
        else
            _canSpawn = true;
    }

    private void SpawnLogic() {
        if (_inPattern || !_canSpawn)
            return;

        SpawnRandom();
        var percentage = (int)Random.Range(0f, 100f);
        if (percentage < _patternRate)
            StartCoroutine(SpawnPattern());
    }

    private void SpawnRandom() {
        SpawnItem(_spawnPositions.GetSpawnPosition());
    }

    private IEnumerator SpawnPattern() {
        _inPattern = true;
        var spawnLength = _spawnPositions.SpawnPointPositions.Length;
        for (var i = -spawnLength + 1; i < spawnLength; i++)
            while (true) {
                if (_canSpawn) {
                    SpawnItem(_spawnPositions.SpawnPointPositions[Mathf.Abs(i)]);
                    break;
                }

                yield return null;
            }

        _inPattern = false;
        yield return null;
    }

    private void IncreaseEntitySpeed() => _objectManager.EntityList.ForEach(x => x.IncreaseSpeed());

    private void SpawnItem(Vector3 pos) {
        _selectedSpawn = pos; // debugging purposes (not really needed)
        pos.y += 1;
        // var item = Instantiate(smallGameObject, pos, transform.rotation);
        var gm = _objectManager.GetFromPool(SelectedPool());
        if (gm != null) {
            gm.transform.position = pos;
            gm.transform.rotation = transform.rotation;
            gm.SetActive(true);
        }

        ResetTimer();
    }

    private PoolOptions SelectedPool() {
        var percentage = (int)Random.Range(0f, 100f);
        PoolOptions poolSelected = PoolOptions.Small;
        if (percentage < _largeFishRate)
            poolSelected = PoolOptions.Large;
        if (percentage < _toxicFishRate)
            poolSelected = PoolOptions.Toxic;
        return poolSelected;
    }

    private void ResetTimer() {
        _canSpawn = false;
        _spawnTimer = _spawnRate;
    }

    private void SetPoints() {
        var position = transform.position;
        var width = _levelWidth.bounds.size.x / 2;
        _pointA = new Vector3(position.x - width, position.y, position.z);
        _pointB = new Vector3(position.x + width, position.y, position.z);
        _spawnPositions = new Spawners(5, _pointA, _pointB);
    }


#if UNITY_EDITOR

    private void OnValidate() { }

    private void OnDrawGizmos() {
        SetPoints();
        Gizmos.DrawSphere(_pointA, 0.25f);
        Handles.Label(_pointA + Vector3.up, "Point A", EditorStyles.label);
        Gizmos.DrawSphere(_pointB, 0.25f);
        Handles.Label(_pointB + Vector3.up, "Point B", EditorStyles.label);
        Gizmos.DrawLine(_pointA, _pointB);


        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_selectedSpawn, 0.2f);

        Gizmos.color = Color.red;
        foreach (var spawnPointPosition in _spawnPositions.SpawnPointPositions) {
            if (spawnPointPosition == _selectedSpawn)
                continue;
            Gizmos.DrawSphere(spawnPointPosition, 0.1f);
            // Handles.Label(spawnPointPosition + Vector3.up, $"{Mathf.Abs(spawnPointPosition.x - _selectedSpawn.x)}",
            //     EditorStyles.label);
        }
    }
#endif
}