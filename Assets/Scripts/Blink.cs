using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour
{

    public float blinkTimePeriod;
    public float minAlpha;
    public float maxAlpha;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float colorVariable = (float)Mathf.Lerp(minAlpha, maxAlpha, Mathf.Abs(Mathf.Sin((float)(2 * 3.14 / blinkTimePeriod) * Time.timeSinceLevelLoad)));
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, colorVariable);
    }
}