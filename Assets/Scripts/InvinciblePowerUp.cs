using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvinciblePowerUp : PowerUps
{
    // Calls player.ApplyInvincibility() and destroys itself
    public override void Activate(PlayerController player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.ApplyInvincibility(duration);
        }
        Destroy(gameObject);
    }
}
