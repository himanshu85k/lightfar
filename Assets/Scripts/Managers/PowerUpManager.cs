using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    public GameObject powerUp;
    public static bool powerUpActive;
    public Color boostColor;
    public float boostSpeedIncrease;
    public GameManager gameManager;
    GameObject p;
    static int indexOfSelectedPowerUp;
    static float speedBeforePowerUp;
    ParticleSystem.MinMaxGradient playerColor;
    ParticleSystem.MinMaxGradient playerTrailGradient;
    Color trailColorBeforePowerUp;

    public void SpawnPowerUp(Vector2 spawnPosition)
    {
        p = Instantiate(powerUp, spawnPosition, powerUp.transform.rotation);
        p.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -GameManager.speed);
        Destroy(p, 15f);
    }
    public IEnumerator EnableAdvantages()
    {
        switch (indexOfSelectedPowerUp)
        {
            case 0:
                yield return new WaitForSeconds(.4f);
                powerUpActive = true;
                //GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gameManager.EnableBoostTrail();
                speedBeforePowerUp = GameManager.speed;
                GameManager.speed += boostSpeedIncrease;
                ObstacleManager.modifySpeedOfAllObstacles();

                yield return new WaitForSeconds(15);
                DisableAdvantages();
                break;
        }
    }
    public void DisableAdvantages()
    {
        switch (indexOfSelectedPowerUp)
        {
            case 0:
                powerUpActive = false;
                gameManager.DisableBoostTrail();
                GameManager.speed = speedBeforePowerUp;
                ObstacleManager.modifySpeedOfAllObstacles();
                StopCoroutine("EnableAdvantages");
                break;
        }
    }
    public void ActivatePowerUp()
    {
        StartCoroutine("EnableAdvantages");
    }

}
