using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerHitBehaviour : MonoBehaviour
{

    public GameObject playerExplosionParticles;
    public GameObject obstacleBurst;
    public AudioClip powerUpAudioClip;

    int smashCount = 0;
    PowerUpManager powerUpManager;
    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "tutorial")
        {//if tutorial is ongoing
            powerUpManager = GameObject.Find("PowerUpManager").GetComponent<PowerUpManager>();
        }
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        // if(SceneManager.GetActiveScene().name == "play"){
        if (coll.gameObject.tag == "Obstacle")
        {
            ObstacleChildBehaviour obs = coll.gameObject.GetComponent<ObstacleChildBehaviour>() as ObstacleChildBehaviour;
            if (!obs.GetPassable())//obstacle is not passable
            {
                if (SceneManager.GetActiveScene().name == "tutorial")
                {// if tutorial is ongoing
                    GameObject p = Instantiate(playerExplosionParticles, this.transform.position, this.transform.rotation);
                    Destroy(p, 3);

                    TutorialManager tm = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
                    smashCount = 0;
                    tm.RestartObstaclesTutorial();
                    return;
                }
                if (PowerUpManager.powerUpActive == false)
                { //kill the player
                    GameObject p = Instantiate(playerExplosionParticles, this.transform.position, this.transform.rotation);
                    Destroy(p, 3);
                    GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
                    g.EndGame();
                }
                else
                { //player still has life, because power up is active thus disable the powerup 
                    Destroy(Instantiate(obstacleBurst, obs.transform.position, obs.transform.rotation), 1.5f);
                    obs.gameObject.SetActive(false);
                    powerUpManager.DisableAdvantages();
                    GameManager.IncreaseScore(1);
                }
            }
            else
            { //obstacle is passable
                if (SceneManager.GetActiveScene().name == "tutorial")
                { //if tutorial is ongoing
                    smashCount++;
                    if (smashCount >= 5)
                    {
                        TutorialManager tm = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
                        tm.EndTutorial();
                    }
                }
                Destroy(Instantiate(obstacleBurst, obs.transform.position, obs.transform.rotation), 1.5f);
                obs.gameObject.SetActive(false);
                GameManager.IncreaseScore(1);
            }
        }
        else if (coll.gameObject.tag == "PowerUp")
        { //player has collided with a power up
            GetComponent<AudioSource>().PlayOneShot(powerUpAudioClip);
            coll.gameObject.SetActive(false);
            powerUpManager.ActivatePowerUp();
            Destroy(coll.gameObject);
        }
    }
}
