using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Health:" + health);
    }
}
