using UnityEngine;
using System.Collections;

public class Manager2 : MonoBehaviour {

    public Transform Mech;
    public Transform MechHandle;
    public Transform RightIK;
    public Transform LeftIK;
    public Transform LookAt;
    public float DistanceIKMultiplier = 0;
    public bool RotateBodyWithVisor = true;

    public Transform Visor;
    public Transform LeftStick;
    public Transform RightStick;

    public ClawController RightClaw;
    public ClawController LeftClaw;
    private Vector3 delta;

    // Use this for initialization
    void Awake () {
        delta = Mech.position - MechHandle.position;
	}

    void Start() {
        AlignBodyWithVisor();

        //get controllers and assign to claws
        RightClaw.SetController(RightStick.GetComponent<SteamVR_TrackedObject>());
        LeftClaw.SetController(LeftStick.GetComponent<SteamVR_TrackedObject>());
    }
	
	// Update is called once per frame
	void Update () {
        Mech.position = Visor.position + delta;
        LookAt.position = Visor.forward + MechHandle.position;

        RightIK.position = RightStick.position + RightStick.forward * DistanceIKMultiplier;
        RightIK.rotation = RightStick.rotation;

        LeftIK.position = LeftStick.position + LeftStick.forward * DistanceIKMultiplier;
        LeftIK.rotation = LeftStick.rotation;

        if (RotateBodyWithVisor)
            AlignBodyWithVisor();

        // Debug.DrawLine(MechHandle.position, LookAt.position, Color.red);
        //  Debug.DrawRay(Visor.position, Visor.forward * 2, Color.green);
    }

    private void AlignBodyWithVisor(){
        float visorYRot = Visor.rotation.eulerAngles.y;
        Vector3 MechRot = Mech.rotation.eulerAngles;
        MechRot.y = visorYRot;

        //align the mech with the visor's initial rotation 
        Mech.rotation = Quaternion.Euler(MechRot);
    }
}
