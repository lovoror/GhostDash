using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    //Animator animator;
    //public float attackTypeTreshold = .5f;


    public Transform attackPoint;
    public float attackRange = .5f;
    public LayerMask enemyLayers;


    void Awake() {
        //animator = GetComponent<Animator>();
    }


    void Update() {

        if (Input.GetButtonDown("Attack1")) {
            Attack();
        }


    }

    private void Attack() {

        // animator.SetTrigger("Attack");

        // detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // damage the first?
        if (hitEnemies.Length > 0) {
            print("Hit " + hitEnemies[0].name);
        }
        // foreach to damage all


    }

    private void OnDrawGizmosSelected() {
        if (attackPoint != null) {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
