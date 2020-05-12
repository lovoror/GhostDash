using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;
    Vector2 _velocity;
    Vector2 _dashOffset;

    Vector2 a, b;


    BoxCollider2D boxCollider;

    public bool isDashing { get; private set; }


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Move(Vector2 velocity) {
        _velocity = velocity;
    }

    public void Stop() {
        _velocity = Vector2.zero;
    }

    public void DashTowards(Vector2 point, float dashDistance, float duration) {
        Vector2 dashDirection = (point - rb.position).normalized;

        //  Vector2 dash = dashDirection * dashDistance;
        if ((rb.position - point).sqrMagnitude < dashDistance*dashDistance) {       //  il mouse è nel cerchio
            dashDistance = Vector2.Distance(rb.position, point);
        }



        float dashSpeed = dashDistance / duration;
        StartCoroutine(Dash( dashDirection, dashSpeed, duration ));

    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
    }

    IEnumerator Dash(Vector2 direction, float speed, float duration) {
        isDashing = true;
        a = rb.position;
        boxCollider.enabled = false;       //   disable collisions
        float time = 0;
        while (time < duration) {
            time += Time.deltaTime;
            rb.position += direction * speed * Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        boxCollider.enabled = true;
        b = rb.position;

    }

}
