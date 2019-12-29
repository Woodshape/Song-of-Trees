using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int TakeDamage(Entity attacker, Entity target);

}
