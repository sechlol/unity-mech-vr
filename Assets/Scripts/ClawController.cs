using UnityEngine;
using System.Collections;
using System;

public class ClawController : MonoBehaviour {

    public Transform MissileSpawn;
    public ParticleSystem SparkleParticle;
    public ParticleSystem ChargeParticlePrefab;
    public GameObject MissilePrefab;
    public float ShootCooldown = 1.0f;

    private SteamVR_Controller.Device Device;
    private SteamVR_TrackedObject TrackedObj;
    private Animator _anim;
    private bool _rotate = false;
    private float _rotSpeed = 0;
    private float _shootCd = 0;
    public float MaxRotSpeed = 10f;

    public Valve.VR.EVRButtonId ShootBtn = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public Valve.VR.EVRButtonId ClampBtn = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    public Valve.VR.EVRButtonId SpinBtn = Valve.VR.EVRButtonId.k_EButton_Grip;

    void Awake() {
        _anim = GetComponentInChildren<Animator>();
    }


    public void SetController(SteamVR_TrackedObject obj) {
        TrackedObj = obj;
        StartCoroutine(Pulse());
    }

    // Update is called once per frame
    void Update() {

        if (TrackedObj != null)
            Device = SteamVR_Controller.Input((int)TrackedObj.index);

        if (Device == null || !Device.valid)
            return;

        if (Device.GetPressDown(ShootBtn) && _shootCd == 0)
            Shoot();
        else
            _shootCd = Mathf.Clamp(_shootCd - Time.deltaTime, 0, _shootCd);

        if (Device.GetPressDown(ClampBtn))
            _anim.SetBool("Clamp", true);
        else if (Device.GetPressUp(ClampBtn))
            _anim.SetBool("Clamp", false);

        if (Device.GetPressDown(SpinBtn))
            _rotate = true;
        else if (Device.GetPressUp(SpinBtn))
            _rotate = false;

        if (_rotate)
            _rotSpeed = Mathf.Lerp(_rotSpeed, MaxRotSpeed, Time.deltaTime * 3.0f);
        else
            _rotSpeed = Mathf.Lerp(_rotSpeed, 0, Time.deltaTime * 4.0f);

        if (_rotSpeed <= 1f)
            _rotSpeed = 0;

        _anim.transform.Rotate(Vector3.forward, _rotSpeed);
    }

    private void Shoot() {
        Instantiate(MissilePrefab, MissileSpawn);
        _shootCd = ShootCooldown;
    }

    private IEnumerator Pulse() {
        ushort pulseDuration = 0;
        float pulseInterval = 0;

        while (true) {
            if (_rotSpeed > 1f && Device != null && Device.valid) {
                pulseDuration = (ushort)(Mathf.Lerp(200, 3999, _rotSpeed / MaxRotSpeed));
                pulseInterval = Mathf.Lerp(100, 1, _rotSpeed / MaxRotSpeed) * 0.001f;

                Device.TriggerHapticPulse(pulseDuration);
                yield return new WaitForSeconds(pulseInterval);
            }
            else
                yield return null;
        }
    }

    public void DoSpark() {
        SparkleParticle.Stop();
        SparkleParticle.Play(true);
        StartCoroutine(LongVibration(100, 0.7f));
    }

    IEnumerator LongVibration(float length, float strength) {
        for (float i = 0; i < length; i += Time.deltaTime) {
            Device.TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
        }
    }

    IEnumerator LongVibration(int vibrationCount, float vibrationLength, float gapLength, float strength) {
        strength = Mathf.Clamp01(strength);
        for (int i = 0; i < vibrationCount; i++) {
            if (i != 0) yield return new WaitForSeconds(gapLength);
            yield return StartCoroutine(LongVibration(vibrationLength, strength));
        }
    }



}
