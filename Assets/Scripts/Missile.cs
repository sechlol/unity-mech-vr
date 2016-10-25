using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {

    public float Speed = 10;
    public float AutodestroyTime = 5;
    public float ExplosionForce = 50;
    public float ExplosionRadius = 10;
    public LayerMask Mask;
    public GameObject ExplosionPrefab;

    private Coroutine _autoDestroy;
    private Rigidbody _body;

    void Start() {
        _body = GetComponent<Rigidbody>();
        _autoDestroy = StartCoroutine(AutoDestroy(AutodestroyTime));
        _body.velocity = transform.forward * Speed;
    }

    void FixedUpdate(){
        _body.AddForce(transform.forward * Speed);
    }

    void OnCollisionEnter(Collision coll) {
        Explode(coll.contacts[0].point);
    }

    private IEnumerator AutoDestroy(float sec) {
        yield return new WaitForSeconds(sec);
        Explode(transform.position);
    }

    private void Explode(Vector3 point) {
        if (_autoDestroy != null)
            StopCoroutine(_autoDestroy);

        Collider[] stuff = Physics.OverlapSphere(point, ExplosionRadius, Mask);
        foreach(var c in stuff) {
            Rigidbody body = c.GetComponent<Rigidbody>();
            if(body != null) 
                body.AddExplosionForce(ExplosionForce, point, ExplosionRadius);
        }

        GameObject expl = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(expl, 5f);
        Destroy(gameObject);
    }
}
