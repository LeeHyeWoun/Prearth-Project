using UnityEngine;

public class StgController2 : StgManager {

    public GameObject obstacle;

#if UNITY_EDITOR
    new void Update()
    {
        base.Update();
        if(Time.timeScale.Equals(1))
            if (angle > 225 && angle <315 && obstacle.activeSelf)
                obstacle.SetActive(false);
            else if ((angle < 225 || angle > 315) && !obstacle.activeSelf)
                obstacle.SetActive(true);

    }

#elif UNITY_ANDROID
    new void OnMouseDrag()
    {
        base.OnMouseDrag();
        if(Time.timeScale.Equals(1))
            if (angle > 225 && angle <315 && obstacle.activeSelf)
                obstacle.SetActive(false);
            else if ((angle < 225 || angle > 315) && !obstacle.activeSelf)
                obstacle.SetActive(true);
    }
#endif

}
