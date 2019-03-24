using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject obstacle;
    public int numberOfObstacles;//number of Obstacles to be recycled
    public float gapBetweenObstacles;
    public float maxExtraGapBetweebObstacles;
    public float angularVelocityChildObs;
    public int levelChangeOnCountOf_pb;
    int levelChangeOnCountOf;
    int powerUpSpawnOnCountOf;

    public Sprite spriteImage;
    public int spriteRowCount = 3;
    public int spriteColumnCount = 7;
    private int obstaclesCount;
    int passableChildIndex;
    ObstacleChildBehaviour obstacleChild;
    Rigidbody2D childRigidBody;

    private bool levelShift;
    private static int w = 200;//width in pixels of each path unit sprite
    private static int h = 200;

    BackgroundTintController backgroundTintController;
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "tutorial")
        {
            numberOfObstacles = 5;
        }
        obstacles = new GameObject[numberOfObstacles];

        ObstacleStyleSetter.sprites = new Sprite[spriteRowCount, spriteColumnCount];

        for (int i = 0; i < spriteRowCount; i++)
        {
            for (int j = 0; j < spriteColumnCount; j++)
            {
                Sprite spr = Sprite.Create(spriteImage.texture, new Rect(j * w, i * h, w, h), new Vector2(.5f, .5f));//0,0 corresponds to bottom left
                ObstacleStyleSetter.sprites[i, j] = spr;
            }
        }
        levelShift = false;
        backgroundTintController = new BackgroundTintController();

    }

    public void StartObstacleManager()
    {

        for (int i = 0; i < numberOfObstacles; i++)
        {
            obstacles[i] = Instantiate(obstacle, new Vector2(0, 50 + i * gapBetweenObstacles), this.transform.rotation);

            ObstacleStyleSetter obs = obstacles[i].GetComponent<ObstacleStyleSetter>() as ObstacleStyleSetter;
            passableChildIndex = Random.Range(0, 3);

            obs.SetSprites(passableChildIndex);
            resetPassableChild(i, passableChildIndex);
            resetVelocities(i);
        }
        if (SceneManager.GetActiveScene().name != "tutorial")
        {
            StartCoroutine("ObstaclesUpdater");
        }
        levelShift = false;
        obstaclesCount = numberOfObstacles;
        ObstacleStyleSetter.style = ObstacleStyleSetter.Style.diffShape;


        levelChangeOnCountOf = levelChangeOnCountOf_pb + Random.Range(0, 10);
        powerUpSpawnOnCountOf = levelChangeOnCountOf / 2;
    }


    IEnumerator ObstaclesUpdater()
    {
        while (true)
        {
            if (GameManager.gameState == GameManager.GameStates.playing && levelShift == false)
            {//on some ongoing level	
                for (int i = 0; i < numberOfObstacles; i++)
                {
                    if (obstacles[i].transform.position.y <= -6)
                    {
                        obstaclesCount++;
                        obstacles[i].SetActive(true);
                        passableChildIndex = Random.Range(0, 3);
                        resetPosition(i);
                        resetSprites(i, passableChildIndex);
                        resetPassableChild(i, passableChildIndex);
                        resetVelocities(i);

                        if (powerUpSpawnOnCountOf == obstaclesCount)
                        {//spawn powerUp
                            PowerUpManager p = GameObject.Find("PowerUpManager").GetComponent<PowerUpManager>();
                            p.SpawnPowerUp(new Vector2(0,
                                        calculateMaxY() + gapBetweenObstacles / 2 +
                                            Random.Range(0, maxExtraGapBetweebObstacles) / 2));
                        }

                        if (levelChangeOnCountOf == obstaclesCount)
                        {//start level shifting							
                            levelShift = true;
                            break;
                        }
                    }
                }
            }
            if (GameManager.gameState == GameManager.GameStates.playing && levelShift == true)
            {//when level is changing
             //calculate total number of obstacles which are below the screen:
                int obsBelowScreenCount = 0;
                for (int i = 0; i < numberOfObstacles; i++)
                {
                    if (obstacles[i].transform.position.y <= -6)
                        obsBelowScreenCount++;
                }
                if (obsBelowScreenCount == numberOfObstacles)//all the obstacles have passed
                {
                    GameManager.level = (GameManager.level + 1) % spriteRowCount;//goto next level
                    float speedBeforeWait = GameManager.speed;

                    backgroundTintController.removeColor();
                    yield return new WaitForSeconds(2.5f);
                    backgroundTintController.refreshColor();
                    yield return new WaitForSeconds(2.5f);

                    GameManager.speed = speedBeforeWait;
                    for (int j = 0; j < numberOfObstacles; j++)
                    {//reset all obstacles
                        obstacles[j].transform.position = new Vector2(0, 20 + j * gapBetweenObstacles);
                        passableChildIndex = Random.Range(0, 3);
                        obstacles[j].SetActive(true);
                        resetSprites(j, passableChildIndex);
                        resetPassableChild(j, passableChildIndex);
                        resetVelocities(j);
                    }
                    obstaclesCount += numberOfObstacles;
                    levelShift = false;

                    levelChangeOnCountOf += levelChangeOnCountOf_pb + Random.Range(15, 20) * (GameManager.level + 1);
                    powerUpSpawnOnCountOf = levelChangeOnCountOf / 2;
                }
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    public void StopObstacleManager()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            Destroy(obstacles[i], 3f);
        }
        StopCoroutine("ObstaclesUpdater");
    }

    void resetPassableChild(int i, int passableChildIndex)
    {

        for (int j = 0; j < 3; j++)
        {
            obstacleChild = obstacles[i].transform.GetChild(j).gameObject.GetComponent<ObstacleChildBehaviour>()
                                 as ObstacleChildBehaviour;
            if (j == passableChildIndex) obstacleChild.SetPassable(true);
            else obstacleChild.SetPassable(false);

            obstacleChild.gameObject.SetActive(true);
        }
    }
    private void resetPosition(int i)
    {
        obstacles[i].transform.position = new Vector2(0,
        calculateMaxY() + gapBetweenObstacles + Random.Range(0, maxExtraGapBetweebObstacles));
    }

    private float calculateMaxY()
    {
        float maxY = 0;
        for (int j = 0; j < numberOfObstacles; j++)
        {
            if (obstacles[j].transform.position.y > maxY)
            {
                maxY = obstacles[j].transform.position.y;
            }
        }
        return maxY;
    }

    private void resetSprites(int i, int passableChildIndex)
    {
        ObstacleStyleSetter obs = obstacles[i].GetComponent<ObstacleStyleSetter>() as ObstacleStyleSetter;
        obs.SetSprites(passableChildIndex);
    }
    private void resetVelocities(int i)
    {

        float angularVelocity = Random.Range(angularVelocityChildObs - 20, angularVelocityChildObs);
        angularVelocity *= Random.Range(-1, 2) >= 0 ? 1 : -1;

        for (int k = 0; k < 3; k++)
        {
            //obstacleChild.transform.rotation = Quaternion.Euler(0,0,Random.Range(0,360));
            obstacleChild = obstacles[i].transform.GetChild(k).gameObject.GetComponent<ObstacleChildBehaviour>()
                                as ObstacleChildBehaviour;
            obstacleChild.angularVelocity = angularVelocity;
        }
        if (SceneManager.GetActiveScene().name == "play")
            obstacles[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, -GameManager.speed);
        else
            obstacles[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, -TutorialManager.speed);
    }
    public static void modifySpeedOfAllObstacles()
    {
        ObstacleManager o = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
        for (int i = 0; i < o.numberOfObstacles; i++)
        {
            o.resetVelocities(i);
        }
    }
}
