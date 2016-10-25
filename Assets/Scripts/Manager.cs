using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

    public Transform Mech;
    public Transform MechHandle;
    public Transform RightIK;
    public Transform LeftIK;
    public Transform LookAt;
    

    public Transform Visor;
    public Transform LeftStick;
    public Transform RightStick;

    private Vector3 delta;

    // Use this for initialization
    void Awake () {
        delta = Mech.position - MechHandle.position;
	}

    void Start() {
        float visorYRot = Visor.rotation.eulerAngles.y;
        Vector3 MechRot = Mech.rotation.eulerAngles;
        MechRot.y = visorYRot;

        //align the mech with the visor's initial rotation 
        Mech.rotation = Quaternion.Euler(MechRot);

    }
	
	// Update is called once per frame
	void Update () {
        Mech.position = Visor.position + delta;
        LookAt.position = Visor.forward + MechHandle.position;

       // Debug.DrawLine(MechHandle.position, LookAt.position, Color.red);
      //  Debug.DrawRay(Visor.position, Visor.forward * 2, Color.green);


    }
}
