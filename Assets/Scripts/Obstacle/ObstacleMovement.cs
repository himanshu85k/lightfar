using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{

    private static Vector2 vectorSpeed;

    void FixedUpdate()
    {
        vectorSpeed = new Vector2(0, -GameManager.speed);
        transform.Translate(vectorSpeed * Time.deltaTime);
    }
}
