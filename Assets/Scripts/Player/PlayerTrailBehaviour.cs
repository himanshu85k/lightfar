using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTrailBehaviour : MonoBehaviour
{


    void Update()
    {
        if (GameManager.gameState == GameManager.GameStates.playing)
        {
            GetComponent<ParticleSystem>().startSpeed = GameManager.speed;
        }
        else if (GameManager.gameState == GameManager.GameStates.gameover)
        {
            Destroy(this.gameObject);
        }
        if (SceneManager.GetActiveScene().name == "tutorial")
        {
            GetComponent<ParticleSystem>().startSpeed = TutorialManager.speed;
        }
    }
}
