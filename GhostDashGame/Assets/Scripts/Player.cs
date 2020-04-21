using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public float moveSpeed = 4;
    public float dashDistance = 6;
    public float timeBetweenDashes = 2;

    float lastDashTime = -2;

    PlayerController playerController;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
    }



    void Update() {
        //  MOVEMENT INPUT
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 moveVelocity = moveInput.normalized * moveSpeed;

        if ((Input.GetAxisRaw("Dash") == 1) && (Time.time > lastDashTime + timeBetweenDashes)) {
            lastDashTime = Time.time;
            playerController.DashTowards(MousePosition(), dashDistance);
            print("dash");
        } else {

            playerController.Move(moveVelocity);
        }

    }

    Vector2 MousePosition() {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }
}
