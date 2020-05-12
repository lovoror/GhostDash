using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCircle : MonoBehaviour {


    Player player;
    float radius;


    void Start() {

        if (FindObjectOfType<Player>() != null) {
            player = FindObjectOfType<Player>();
        }

        radius = player.dashDistance;
        transform.localScale = new Vector3(radius * 2, radius * 2, transform.localScale.z);

        UpdatePosition();
    }


    public void UpdatePosition() {
        if (player != null) {
            transform.position = player.transform.position;
        }
        print("dashCircle: position updated");  
    }

}
