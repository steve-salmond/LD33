using UnityEngine;
using System.Collections;

public class HealingWeapon : Weapon 
{

    protected override bool IsValidTarget(Transform target)
    {
        if (!base.IsValidTarget(target))
            return false;

        // Only heal damaged objects.
        var destructible = target.GetComponent<Destructible>();
        return destructible && destructible.IsDamaged;

    }
}
