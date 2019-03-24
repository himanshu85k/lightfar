using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Canvas homeCanvas, optionsCanvas, playCanvas, pausedCanvas, gameOverCanvas;
    private static Canvas home, options, playing, paused, gameOver;
    public Sprite settingsExpand;
    public Sprite settingsCompact;
    public Toggle themeMusicToggle;
    public Toggle sfxToggle;
    public Animator optionsCanvasAnim;
    public AudioSource themeMusic;
    public Text HighScoreTextHome;
    public Text ScoreTextPlay;
    public Text ScoreTextGameOver, HighScoreTextGameOver;

    public AudioMixerSnapshot musicSFXSnapshot, musicSnapshot, sfxSnapshot;
    static Animator homeMenuAnim;
    public void Awake()
    {
        home = homeCanvas;
        options = optionsCanvas;
        paused = pausedCanvas;
        gameOver = gameOverCanvas;

        homeCanvas.enabled = true;
        optionsCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        pausedCanvas.enabled = false;

        homeMenuAnim = GameObject.Find("homeCanvas").GetComponent<Animator>();

        setPreferences();
    }

    public void setPreferences()
    {
        if (PlayerPrefs.GetInt("firstTimePlay", 0) == 0)
        {
            SceneManager.LoadScene("tutorial");
        }
        HighScoreTextHome.text = "Best : " + PlayerPrefs.GetInt("highScore", 0);

        int tm = PlayerPrefs.GetInt("themeMusicToggle", 1);
        if (tm == 1) themeMusicToggle.isOn = true;
        else themeMusicToggle.isOn = false;
        if (themeMusicToggle.isOn)
        {
            themeMusic.Play();
            themeMusic.loop = true;
        }

        int sfx = PlayerPrefs.GetInt("sfxToggle", 1);
        if (sfx == 1) sfxToggle.isOn = true;
        else sfxToggle.isOn = false;

        setSnapshots();
    }
    public void Update()
    {
        if (homeCanvas.enabled == true && Input.GetKeyUp("escape"))
        {
            SimpleAds ad = new SimpleAds();
            ad.ShowAd();
            Application.Quit();
        };
    }
    public void setSnapshots()
    {
        if (themeMusicToggle.isOn && sfxToggle.isOn)
        {
            musicSFXSnapshot.TransitionTo(1);
        }
        else if (themeMusicToggle.isOn && !sfxToggle.isOn)
        {
            musicSnapshot.TransitionTo(1);
        }
        else sfxSnapshot.TransitionTo(1);
    }
    public void SetScores()
    {
        HighScoreTextHome.text = "Best : " + PlayerPrefs.GetInt("highScore", 0);
        ScoreTextGameOver.text = "" + GameManager.GetScore();
        HighScoreTextGameOver.text = "Best : " + PlayerPrefs.GetInt("highScore", 0);
    }
    public void UpdateScore(int score)
    {
        ScoreTextPlay.text = "" + score;
    }
    public void playButtonClick()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.StartGame();
    }
    public void pauseClick()
    {
        GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
        g.PauseGame();
    }
    public void ResumeClick()
    {
        GameManager.ResumeGame();
    }
    public void RetryClick()
    {
        GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
        g.RestartGame();
    }
    public void HomeClick()
    {
        GameManager g = GameObject.Find("GameManager").GetComponent<GameManager>();
        g.PlayHomeClickAnimations();

        SimpleAds ad = new SimpleAds();
        ad.ShowAd();
    }
    public void settingsClick()
    {
        Image i = GameObject.Find("settings").GetComponent<Image>();
        if (i.sprite == settingsCompact)
        {
            optionsCanvasAnim.SetTrigger("playOptions");
            i.sprite = settingsExpand;
        }
        else
        {
            optionsCanvasAnim.SetTrigger("playReverseOptions");
            i.sprite = settingsCompact;
        }
    }
    public void toggleThemeMusic()
    {
        if (themeMusicToggle.isOn)
        {
            //audioMix.SetFloat("musicVolume", 0);
            themeMusic.Play();
            themeMusic.loop = true;
            PlayerPrefs.SetInt("themeMusicToggle", 1);
        }
        else
        {
            themeMusic.Stop();
            PlayerPrefs.SetInt("themeMusicToggle", 0);
        }
        setSnapshots();
    }
    public void toggleSfx()
    {
        if (sfxToggle.isOn) PlayerPrefs.SetInt("sfxToggle", 1);
        else PlayerPrefs.SetInt("sfxToggle", 0);
        setSnapshots();
    }

    string subject = "Mission: LightFar";
    string body;
    string link = "http://play.google.com/store/apps/details?id=com.MoonTrail.LightFar";
    public void rateClick()
    {
        Application.OpenURL(link);
    }
    public void challengeClick()
    {
#if UNITY_ANDROID
		
		//Refernece of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		
		//Refernece of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		
		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		
		//add data to be passed to the other activity i.e., the data to be sent
		body="I just scored "+GameManager.score+" points on "+subject+" Beat That!! \n\nGet this game now from "+link;
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		
		//start the activity by sending the intent data
		currentActivity.Call ("startActivity", intentObject);
#endif
    }

    public void shareClick()
    {
#if UNITY_ANDROID
		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");

		//Refernece of AndroidJavaObject class for intent
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		
		//call setAction method of the Intent object created
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

		//set the type of sharing that is happening
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");

		//add data to be passed to the other activity i.e., the data to be sent
		body="Hey! let's space travel? try out this cool game: "+link;
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
		
		//get the current activity
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		
		//start the activity by sending the intent data
		currentActivity.Call ("startActivity", intentObject);
#endif
    }
}
