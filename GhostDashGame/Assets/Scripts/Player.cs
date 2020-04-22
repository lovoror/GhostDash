using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    public GameObject dashCirclePrefab;
    bool dashCircleActive;

    public float moveSpeed = 4;     //  walking speed
    public float dashDistance = 6;      //  get, private set?
    public float timeBetweenDashes = 2;         //  min time in seconds between dashes
    public float dashDuration = .25f;        //  dash anim duration

    float lastDashTime = -2;        //  last time player dashed

    //  bool isDashing = false;         //  to check if it is currently dashing

    PlayerController playerController;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
        
    }



    void Update() {
        //  MOVEMENT INPUT
        if (!playerController.isDashing && !dashCircleActive) {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 moveVelocity = moveInput.normalized * moveSpeed;

            if ((Input.GetAxisRaw("Dash") == 1) && (Time.time > lastDashTime + timeBetweenDashes)) {
                StartCoroutine(Dash());
                playerController.Stop();
            } else {
                playerController.Move(moveVelocity);
            }
        }

    }

    IEnumerator Dash() {
        lastDashTime = Time.time;
        // crea il cerchio
        var newDashCircle = Instantiate(dashCirclePrefab, transform.position, Quaternion.identity);
        dashCircleActive = true;
        // aspetta che lasci il tasto
        while(Input.GetAxisRaw("Dash") != 0) {
            yield return null;
        }
        //distruggi il cerchio e dasha
        Destroy(newDashCircle);
        playerController.DashTowards(MousePosition(), dashDistance, dashDuration);
        dashCircleActive = false;
        
    }

    Vector2 MousePosition() {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }
}
