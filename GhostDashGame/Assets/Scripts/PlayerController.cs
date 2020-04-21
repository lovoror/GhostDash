using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;
    Vector2 _velocity;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector3 velocity) {
        _velocity = velocity;
    }


    void FixedUpdate() {
        rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
    }
}
