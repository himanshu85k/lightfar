using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{

    public GameObject backgroundImage1;
    public GameObject backgroundImage2;
    public int backgroundImageHeight;
    public int yResetPoint;
    private float vectorSpeed;
    public bool isMainMenuScene;

    void Update()
    {
        vectorSpeed = -GameManager.speed;
        vectorSpeed = vectorSpeed == 0 ? -8 : vectorSpeed;
        backgroundImage1.transform.position += new Vector3(0, vectorSpeed * Time.deltaTime, 0);
        backgroundImage2.transform.position += new Vector3(0, vectorSpeed * Time.deltaTime, 0);

        if (backgroundImage1.transform.position.y <= yResetPoint)
        {
            backgroundImage1.transform.position = new Vector3(0, backgroundImage2.transform.position.y + backgroundImageHeight,
                                                            backgroundImage1.transform.position.z);
        }
        if (backgroundImage2.transform.position.y <= yResetPoint)
        {
            backgroundImage2.transform.position = new Vector3(0, backgroundImage1.transform.position.y + backgroundImageHeight,
                                                            backgroundImage2.transform.position.z);
        }
    }
}
