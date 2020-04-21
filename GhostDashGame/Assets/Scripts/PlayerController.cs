using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;
    Vector2 _velocity;
    Vector2 _dashOffset;
    public bool isDashing { get; private set; }


    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 velocity) {
        _velocity = velocity;
    }

    public void DashTowards(Vector2 point, float dashDistance, float duration) {
        Vector2 dashDirection = (point - rb.position).normalized;

        Vector2 dash = dashDirection * dashDistance;
        //   rb.position += dash;

        float dashSpeed = dashDistance / duration;
        StartCoroutine(Dash( dashDirection, dashSpeed, duration ));

    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + _velocity * Time.fixedDeltaTime);
    }

    IEnumerator Dash(Vector2 direction, float speed, float duration) {
        isDashing = true;
        GetComponent<CircleCollider2D>().enabled = false;       //   disable collisions
        float time = 0;
        while (time < duration) {
            time += Time.deltaTime;
            rb.position += direction * speed * Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        GetComponent<CircleCollider2D>().enabled = true;
    }

}
