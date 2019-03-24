using UnityEngine;

#if UNITY_ADS
 using UnityEngine.Advertisements;
#endif

public class SimpleAds : MonoBehaviour
{
    float lastAdShownTime;
    public void Awake()
    {
        lastAdShownTime = 0;
    }
    public void ShowAd()
    {
      #if UNITY_ADS
          if(!Advertisement.isInitialized){
            Advertisement.Initialize("1483039", true);
          }
          if ((lastAdShownTime == 0 || Time.realtimeSinceStartup - lastAdShownTime > 240) && Advertisement.IsReady())
          {

            Debug.Log("Showing Ads");
            Advertisement.Show();
            lastAdShownTime = Time.realtimeSinceStartup;
          }
          else
            Debug.Log("Ads not ready");
      #endif
    }
}