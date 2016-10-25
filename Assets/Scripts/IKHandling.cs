using UnityEngine;
using System.Collections;

public class IKHandling : MonoBehaviour {

    private Animator _anim;

    public float IkWeightHand = 1;
    public float IkWeightFoot = 1;
    public float OffsetYFoot = 0 ;
    public float IkWeightLook;
    public float IkWeightBody;
    public float IkWeightHead;
    public float IkWeightEyes;
    public float IkWeightClamp;

    public Transform LookAt;
    public Transform RightHandIK;
    public Transform LeftHandIK;

    private Transform _leftFoot;
    private Transform _rightFoot;
    private Vector3 _rFootPos;
    private Vector3 _lFootPos;
    private Quaternion _rFootRot;
    private Quaternion _lFootRot;
    private float _rFootWeight;
    private float _lFootWeight;

    // Use this for initialization
    void Start () {
        _anim = GetComponent<Animator>();

        _leftFoot   = _anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        _rightFoot  = _anim.GetBoneTransform(HumanBodyBones.RightFoot);
        _lFootPos = _leftFoot.position;
        _rFootPos = _rightFoot.position;
    }

    void Update() {
        RaycastHit hit;
        Vector3 lpos = _leftFoot.TransformPoint(Vector3.zero);
        Vector3 rpos = _rightFoot.TransformPoint(Vector3.zero);

        if (Physics.Raycast(lpos, Vector3.down, out hit, 1)) {
            _lFootPos = hit.point;
            _lFootRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        if (Physics.Raycast(rpos, Vector3.down, out hit, 1)) {
            _rFootPos = hit.point;
            _rFootRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
    }

    void OnAnimatorIK() {
        Vector3 offY = new Vector3(0, OffsetYFoot, 0);

        _anim.SetLookAtWeight(IkWeightLook, IkWeightBody, IkWeightHead, IkWeightEyes, IkWeightClamp);
        _anim.SetLookAtPosition(LookAt.position);

        _lFootWeight = _anim.GetFloat("LeftFoot");
        _rFootWeight = _anim.GetFloat("RightFoot");

        //Left Foot
        _anim.SetIKPosition(        AvatarIKGoal.LeftFoot, _lFootPos + offY);
        _anim.SetIKRotation(        AvatarIKGoal.LeftFoot, _lFootRot);
        _anim.SetIKPositionWeight(  AvatarIKGoal.LeftFoot, _lFootWeight);
        _anim.SetIKRotationWeight(  AvatarIKGoal.LeftFoot, _lFootWeight);

        //Right Foot
        _anim.SetIKPosition(        AvatarIKGoal.RightFoot, _rFootPos + offY);
        _anim.SetIKRotation(        AvatarIKGoal.RightFoot, _rFootRot);
        _anim.SetIKPositionWeight(  AvatarIKGoal.RightFoot, _rFootWeight);
        _anim.SetIKRotationWeight(  AvatarIKGoal.RightFoot, _rFootWeight);


        //Left Hand
        _anim.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIK.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIK.rotation);
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, IkWeightHand);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, IkWeightHand);

        //Right Hand
        _anim.SetIKPosition(AvatarIKGoal.RightHand, RightHandIK.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandIK.rotation);
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, IkWeightHand);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, IkWeightHand);
    }

	
}
