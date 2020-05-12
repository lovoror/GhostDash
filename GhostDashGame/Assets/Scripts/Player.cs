﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour {

    public bool distanzaFissa = false;

    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    public GameObject dashCircleObj;
    public float moveSpeed = 4;     //  walking speed
    public float dashDistance = 6;      //  get, private set?
    public float timeBetweenDashes = 2;         //  min time in seconds between dashes
    public float dashDuration = .25f;        //  dash anim duration
    public float maxTimeToDash = 1;     //  max time in second to release the mouse button

    float dashTriggerTime;
    float lastDashTime;        //  last time player dashed


    bool dashKeyPressed;
    bool dashKeyReleased;
    bool isDashing;
    bool canDash;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        lastDashTime = -timeBetweenDashes;
    }

    private void Update() {
        dashKeyPressed = (Input.GetAxisRaw("Dash") == 1) ? true : false;
        canDash = (Time.time > lastDashTime + timeBetweenDashes);



        if (dashKeyPressed && !isDashing && canDash ) {
            StartCoroutine(Dash());
            isDashing = true;
        }
    }



    private void FixedUpdate() {
        if (!dashKeyPressed || !canDash) {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 moveVelocity = moveInput.normalized * moveSpeed;
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        }

    }


    IEnumerator Dash() {
        //  inizia il conto del tempo che hai per lasciare il pulsante
        dashTriggerTime = 0;

        //  attiva il cerchio
        dashCircleObj.SetActive(true);
        dashCircleObj.GetComponent<DashCircle>().UpdatePosition();

        // aspetta che il tasto venga rilasciato
        while (dashKeyPressed && dashTriggerTime < maxTimeToDash) {
            dashTriggerTime += Time.deltaTime;
            yield return null;
        }

        // scompare il dashCircle
        dashCircleObj.SetActive(false);

        // se è in tempo, dasha
        if (dashTriggerTime < maxTimeToDash) {

            Vector2 dashDirection = (MousePosition() - rb.position).normalized;
            // se è impostata la distanza fissa, ignora; altrimenti
            if (!distanzaFissa) {
                //  se il mouse è nel cerchio allora aggiorna la posizione
                if ((rb.position - MousePosition()).sqrMagnitude < dashDistance * dashDistance) {
                    dashDistance = Vector2.Distance(rb.position, MousePosition());
                }
            }

            float dashSpeed = dashDistance / dashDuration;

            // disattivo le collisioni
            boxCollider.enabled = false;

            float dashTime = 0;

            while (dashTime < dashDuration) {
                dashTime += Time.deltaTime;
                rb.position += dashDirection * dashSpeed * Time.deltaTime;
                yield return null;
            }

            // riattivo le collisioni
            boxCollider.enabled = true;

            // aggiorno il time dell'ultima dash
            lastDashTime = Time.time;

        } else {    //  se non è in tempo, restituisci il controllo
            //dashKeyPressed = false;
            StartCoroutine(CancelDash());
        }

        isDashing = false;
    }

    IEnumerator CancelDash() {
        while (dashKeyPressed) {
            canDash = false;
            yield return null;
        }
    }


    #region Utility

    Vector2 MousePosition() {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }

    #endregion
}
