using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTintController : MonoBehaviour
{

    public float colorChangeRate;
    public Color[] tintColors = {
                                    new Color32(61,0,101,255),//dark purple
									new Color32(0,95,111,255),//dark cyan
									new Color32(3,0,101,255),//dark blue
									new Color32(44,131,59,255),//dark green
								 };
    static Color targetColor;
    Camera camera;
    void Awake()
    {
        camera = this.gameObject.GetComponent<Camera>();
        targetColor = tintColors[GameManager.level];
        camera.backgroundColor = targetColor;
        //currentColor = camera.backgroundColor;
    }

    void Update()
    {
        if (camera.backgroundColor != targetColor)
        {
            camera.backgroundColor = Color.Lerp(camera.backgroundColor, targetColor, Time.deltaTime * colorChangeRate);
        }

    }
    public void refreshColor()
    {
        targetColor = tintColors[GameManager.level];
    }
    public void removeColor()
    {
        targetColor = Color.black;
    }
}
