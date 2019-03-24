using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public float speedGainPerMinute;
    public static int score;
    public static int level;
    public AudioClip mainMenuAudioClip, gameAudioClip;
    private GameObject player;
    public enum GameStates { playing, gameover, paused };
    public float startSpeed;
    public static float speed;
    public float speed_pb;
    public static GameStates gameState;
    static Animator CameraAnim;
    static Animator homeMenuAnim;
    static Animator gameOverAnim;
    static Animator pausedAnim;
    static float speedBeforePause;
    CanvasManager canvasManager;
    GameObject boostTrail;

    void Awake()
    {
        gameState = GameStates.gameover;
        level = 0;
        try
        {
            CameraAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
            homeMenuAnim = GameObject.Find("homeCanvas").GetComponent<Animator>();
            pausedAnim = GameObject.Find("pausedCanvas").GetComponent<Animator>();
            gameOverAnim = GameObject.Find("gameOverCanvas").GetComponent<Animator>();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("canvas not found : " + e.Message);
        }
        // PlayerHitBehaviour.numberOfLives=1;
        canvasManager = GameObject.Find("Canvases").GetComponent<CanvasManager>();
    }
    void Update()
    {
        switch (gameState)
        {
            case GameStates.playing:
                IncreaseSpeed();
                if (Input.GetKeyUp("escape"))
                    PauseGame();

                break;
            case GameStates.paused:
                if (Input.GetKeyUp("escape"))
                {
                    ResumeGame();
                }
                break;
            case GameStates.gameover:
                speed = Mathf.Lerp(speed, 0, Time.deltaTime * 2);
                break;
        }


    }
    public void IncreaseSpeed()
    {
        speed_pb = speed;
        if (speed <= 10)
        {
            speed += (speedGainPerMinute / 15) * Time.deltaTime;
        }
        else if (speed <= 13)
            speed += (speedGainPerMinute / 40) * Time.deltaTime;
        else if (speed <= 17)
            speed += (speedGainPerMinute / 60) * Time.deltaTime;
        else
            speed += (speedGainPerMinute / 80) * Time.deltaTime;
    }
    public static void IncreaseScore(int increase)
    {
        if (SceneManager.GetActiveScene().name == "play")
        {
            score += increase;
            CanvasManager c = GameObject.Find("Canvases").GetComponent<CanvasManager>();
            c.UpdateScore(score);
        }
    }
    public void StartGame()
    {
        if (gameState != GameStates.playing)
        {
            Camera.main.GetComponent<AudioSource>().clip = gameAudioClip;
            Camera.main.GetComponent<AudioSource>().Play();
            speed = startSpeed;
            score = 0;
            level = 0;
            BackgroundTintController backgroundTintController = new BackgroundTintController();
            backgroundTintController.refreshColor();
            CameraAnim.SetTrigger("play");
            homeMenuAnim.SetTrigger("play");
            player = Instantiate(playerPrefab);
            ObstacleManager o = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
            o.StartObstacleManager();
            gameState = GameStates.playing;

            boostTrail = GameObject.Find("boostTrail");
            DisableBoostTrail();
        }
    }
    public static void ResumeGame()
    {
        if (gameState == GameStates.paused)
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            Time.timeScale = 1;
            pausedAnim.SetTrigger("playReverse");
            gameState = GameStates.playing;
        }
    }
    public void PauseGame()
    {
        if (gameState == GameStates.playing)
        {
            pausedAnim.SetTrigger("play");
            StartCoroutine("Pause");
        }
    }
    IEnumerator Pause()
    {
        yield return new WaitForSeconds(.25f);
        Time.timeScale = 0;
        gameState = GameStates.paused;
    }
    public void RestartGame()
    {
        if (gameState == GameStates.gameover)
        {
            Camera.main.GetComponent<AudioSource>().clip = gameAudioClip;
            //Camera.main.GetComponent<AudioSource>().Stop();
            Camera.main.GetComponent<AudioSource>().Play();
            speed = startSpeed;
            score = 0;
            level = 0;
            BackgroundTintController backgroundTintController = new BackgroundTintController();
            backgroundTintController.refreshColor();

            canvasManager.UpdateScore(score);
            CameraAnim.SetTrigger("play");
            gameOverAnim.SetTrigger("playReverse");

            player = Instantiate(playerPrefab);
            boostTrail = GameObject.Find("boostTrail");
            boostTrail.SetActive(false);

            GameObject[] powerUp = GameObject.FindGameObjectsWithTag("PowerUp");
            for (int i = 0; i < powerUp.Length; i++)
            {
                if (powerUp[i] != null)
                {
                    Destroy(powerUp[i]);
                }
            }

            ObstacleManager o = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
            o.StopObstacleManager();
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            for (int i = 0; i < obstacles.Length; i++)
            {
                Destroy(obstacles[i]);
            }
            o.StartObstacleManager();
            gameState = GameStates.playing;
        }
    }
    public void EndGame()
    {
        if (gameState == GameStates.playing)
        {
            Camera.main.GetComponent<AudioSource>().clip = mainMenuAudioClip;
            Camera.main.GetComponent<AudioSource>().Play();

            if (score > PlayerPrefs.GetInt("highScore", 0))
            {
                PlayerPrefs.SetInt("highScore", score);
            }
            gameOverAnim.SetTrigger("play");
            CameraAnim.SetTrigger("playReverse");

            GameObject trail = player.GetComponentInChildren<ParticleSystem>().gameObject;
            Destroy(trail);
            Destroy(player);

            ObstacleManager o = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
            o.StopObstacleManager();

            GameObject[] powerUp = GameObject.FindGameObjectsWithTag("PowerUp");
            for (int i = 0; i < powerUp.Length; i++)
            {
                if (powerUp[i] != null)
                {
                    Destroy(powerUp[i], 2);
                }
            }

            canvasManager.SetScores();
            gameState = GameStates.gameover;
        }
    }

    public void PlayHomeClickAnimations()
    {
        gameOverAnim.SetTrigger("playReverse");
        homeMenuAnim.SetTrigger("playReverse");
    }
    public static int GetScore()
    {
        return score;
    }
    public void EnableBoostTrail()
    {
        if (boostTrail != null)
        {
            Debug.Log("boostrail not found");
            boostTrail.SetActive(true);
        }
    }
    public void DisableBoostTrail()
    {
        boostTrail.SetActive(false);
    }

}
