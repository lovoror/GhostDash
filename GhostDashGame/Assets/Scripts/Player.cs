using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public float moveSpeed = 4;

    PlayerController playerController;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
    }


    void Update() {
        //  MOVEMENT INPUT
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 moveVelocity = moveInput.normalized * moveSpeed;

        playerController.Move(moveVelocity);
    }
}
