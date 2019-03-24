using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public GameObject playerPrefab;
    public Canvas helpCanvas;
    public Text helpCanvasText;


    GameObject player;
    public static float speed;
    PlayerMovement playerMovement;

    void Start()
    {
        player = Instantiate(playerPrefab);
        playerMovement = player.GetComponent<PlayerMovement>();
        speed = 8;
        GameObject boostTrail = GameObject.Find("boostTrail");
        boostTrail.SetActive(false);
        //start intro coroutine
        StartCoroutine("Intro");


    }
    IEnumerator Intro()
    {
        helpCanvasText.text = "Captain! There has been an interruption to \nMission: LightFar.";
        yield return new WaitForSeconds(4f);
        helpCanvasText.text += "\n\n We have encountered a strange galaxy.";
        yield return new WaitForSeconds(4f);
        helpCanvasText.text += "\n\n You should take control over the ship right now.";
        yield return new WaitForSeconds(4f);
        StartCoroutine("SwipeTutorial");
    }
    IEnumerator SwipeTutorial()
    {
        helpCanvasText.text = "Swipe to steer the ship";
        while (true)
        {
            if (playerMovement.didSwipeOccur() == true)
            {
                break;
            }
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(2f);
        helpCanvasText.text = "";
        yield return new WaitForSeconds(2f);
        StartCoroutine("ObstaclesTutorial");
    }
    IEnumerator ObstaclesTutorial()
    {
        yield return null;
        helpCanvasText.text = "Smash through the Odd One Out";
        yield return new WaitForSeconds(1f);
        ObstacleManager obsM = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
        obsM.StartObstacleManager();
    }
    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2f);
        helpCanvasText.text = "Try Again.";
        yield return new WaitForSeconds(3f);
        player = Instantiate(playerPrefab);
        GameObject boostTrail = GameObject.Find("boostTrail");
        boostTrail.SetActive(false);
        StartCoroutine("ObstaclesTutorial");
    }
    public void RestartObstaclesTutorial()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i], 4f);
        }
        Destroy(player);
        StopCoroutine("ObstaclesTutorial");
        StartCoroutine("RespawnPlayer");
    }
    public void EndTutorial()
    {
        StartCoroutine("Conclusion");
    }
    IEnumerator Conclusion()
    {
        yield return new WaitForSeconds(2f);
        helpCanvasText.text = "You are Good to GO!";
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("play");
        PlayerPrefs.SetInt("firstTimePlay", 1);
    }
}
