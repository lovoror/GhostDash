using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {

    
    public float StartingHealth { get; set; }

    public float Health { get; set; }

    public void TakeDamage(float damage) {
        throw new System.NotImplementedException();
    }



}
