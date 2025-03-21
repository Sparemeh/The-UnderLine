using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUps : MonoBehaviour
{
    [Header("Common Settings")]
    public float duration = 5f; // How long the powerup effect lasts

    public abstract void Activate(PlayerController player);


}
