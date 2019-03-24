using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -GameManager.speed);
    }
}
