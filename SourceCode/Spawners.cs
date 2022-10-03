// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using UnityEngine;

public class Spawners {
    private const float SPACING = 1.75f;

    private readonly Vector3 _p1, _p2;

    public Spawners(int num, Vector3 lineStart, Vector3 lineEnd) {
        SpawnPointPositions = new Vector3[num];
        _p1 = lineStart;
        _p2 = lineEnd;
        ConstructPoints();
    }

    public Vector3[] SpawnPointPositions { get; }

    public Vector3 GetSpawnPosition() {
        // var t = Mathf.PerlinNoise(Time.time * Random.value, 0);
        var point = Vector3.Lerp(_p1, _p2, Random.value);
        var dist = 1f;
        var closest = new Vector3();
        foreach (var spawnPosition in SpawnPointPositions) {
            var value = spawnPosition.x - point.x;
            if (Mathf.Abs(value) < dist) {
                dist = value;
                closest = spawnPosition;
            }
        }

        if (closest == Vector3.zero)
            closest = SpawnPointPositions[2];

        return closest;
    }

    private void ConstructPoints() {
        for (var i = 0; i < SpawnPointPositions.Length; i++)
            SpawnPointPositions[i] = _p1 + new Vector3(SPACING * (i + 1), 0);
    }
}