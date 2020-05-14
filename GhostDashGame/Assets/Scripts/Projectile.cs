using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public float projectileSpeed = 10;

    public float lifetime = 5;

    void Start() {
        Invoke("Deactivate", lifetime);
    }


    void Update() {
        
    }

    void SetSpeed(float newSpeed) {
        projectileSpeed = newSpeed;
    }

    void Deactivate() {
        gameObject.SetActive(false);
    }

}
