// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using UnityEngine;

public class NetHandling : MonoBehaviour {
    [SerializeField] private Transform netPivot;

    public Transform player;

    [Range(-180, 180)] public float angle;


    private void Update() {
        var followRot = transform.rotation;
        followRot = Quaternion.LookRotation(player.transform.position);
        transform.rotation = followRot;
        transform.eulerAngles = new Vector3(15f, transform.eulerAngles.y, transform.eulerAngles.z);
        netPivot.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}