using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleChildBehaviour : MonoBehaviour
{

    public bool isPassable = false;
    public ParticleSystem obstacleExplosionParticles;
    public float angularVelocity;

    public void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * angularVelocity);
    }
    public void SetPassable(bool pass)
    {
        isPassable = pass;
    }
    public bool GetPassable()
    {
        return isPassable;
    }

}
