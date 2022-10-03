// //////////////////////////////
// Authors: Laurence
// GitHub: @SirLorrence
// //////////////////////////////

using UnityEngine;

public class IKController : MonoBehaviour {
    private Animator _animator;
    [SerializeField] private Transform _rightHandObj;
    [SerializeField] private Transform _leftHandObj;
    [SerializeField] private Transform _lookPos;

    [Space] [SerializeField] [Range(0f, 1f)]
    private float rightWeight = .25f, leftWeight = 1, bodyWeight = .1f;


    private void Awake() {
        _animator = GetComponent<Animator>();
    }


    private void OnAnimatorIK(int layerIndex) {
        // Neck and Torso movement
        _animator.SetLookAtWeight(1, bodyWeight);
        _animator.SetLookAtPosition(_lookPos.position);

        //Hand Placement
        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightWeight);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightWeight);
        _animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandObj.position);
        _animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandObj.rotation);

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftWeight);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftWeight);
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandObj.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandObj.rotation);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos() {
        // var tLocation = _lookPos.position + _body.position;
        // Gizmos.DrawSphere(tLocation,.25f);
    }
#endif
}