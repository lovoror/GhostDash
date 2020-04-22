using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

    float StartingHealth { get; set; }
    float Health { get; set; }

    void TakeDamage(float damage);
}
