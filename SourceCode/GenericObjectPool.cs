// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using UnityEngine;

public class GenericObjectPool {
    private static int POOL_SIZE;
    public GameObject[] pool;
    private GameObject obj;
    private string Name;

    public delegate void CreateObject(GameObject gameObject, GameObject[] pool, string pName);

    public GenericObjectPool(GameObject poolObject, int size, string poolName) {
        POOL_SIZE = size;
        pool = new GameObject[POOL_SIZE];
        obj = poolObject;
        Name = poolName;
    }

    public void CreatePool(CreateObject create) => create(obj, pool, Name);
}