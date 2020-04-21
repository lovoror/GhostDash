using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;
    Vector2 _velocity;
    Vector2 _dashOffset;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 velocity) {
        _velocity = velocity;
    }

    public void DashTowards(Vector2 point, float dashDistance) {
        Vector2 offsetNorm = (point - rb.position).normalized;
        Vector2 dash = offsetNorm * dashDistance;
        rb.position += dash;
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
    }
}
