using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public float moveSpeed = 4;     //  walking speed
    public float dashDistance = 6;
    public float timeBetweenDashes = 2;         //  min time in seconds between dashes
    public float dashDuration = .5f;        //  dash anim duration

    float lastDashTime = -2;        //  last time player dashed

    //  bool isDashing = false;         //  to check if it is currently dashing

    PlayerController playerController;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
        
    }



    void Update() {
        //  MOVEMENT INPUT
        if (!playerController.isDashing) {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 moveVelocity = moveInput.normalized * moveSpeed;

            if ((Input.GetAxisRaw("Dash") == 1) && (Time.time > lastDashTime + timeBetweenDashes)) {
                
                lastDashTime = Time.time;
                playerController.DashTowards(MousePosition(), dashDistance, dashDuration);
                print("dash");
            } else {

                playerController.Move(moveVelocity);
            }
        }

    }

    Vector2 MousePosition() {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }
}
