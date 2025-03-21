using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPowerUp : PowerUps
{
    [SerializeField]
    float shrinkMultiplier;

    // Calls player.ApplyShrink() and destroys itself
    public override void Activate(PlayerController player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ApplyShrink(duration, shrinkMultiplier);
        }
        Destroy(gameObject);
    }
}
