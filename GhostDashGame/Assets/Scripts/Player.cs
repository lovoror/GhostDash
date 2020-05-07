using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public GameObject dashCirclePrefab;
    bool dashCircleActive;

    public float moveSpeed = 4;     //  walking speed
    public float dashDistance = 6;      //  get, private set?
    public float timeBetweenDashes = 2;         //  min time in seconds between dashes
    public float dashDuration = .25f;        //  dash anim duration
    public float maxTimeToDash = 1;     //  max time in second to release the mouse button

    float dashTriggerTime;
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
        //  bool dashConfirmed;     //  unused
        //  inizia il conto del tempo che hai per lasciare il pulsante
        dashTriggerTime = 0;
        // crea il cerchio
        var newDashCircle = Instantiate(dashCirclePrefab, transform.position, Quaternion.identity);
        dashCircleActive = true;
        // aspetta che lasci il tasto
        while((Input.GetAxisRaw("Dash") != 0) && (dashTriggerTime < maxTimeToDash)) {
            dashTriggerTime += Time.deltaTime;
            yield return null;
        }
        //distruggi il cerchio e dasha
        if (dashTriggerTime < maxTimeToDash) {
            playerController.DashTowards(MousePosition(), dashDistance, dashDuration);
        }
        
        Destroy(newDashCircle);

        dashCircleActive = false;

        lastDashTime = Time.time;

    }

    Vector2 MousePosition() {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }
}
