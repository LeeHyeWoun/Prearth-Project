public class StgController2 : StgManager {

    public UnityEngine.GameObject obstacle;

#if UNITY_EDITOR
    void Update()
    {
        base.Update();
        if(play)
            if (angle > 240 && obstacle.activeSelf)
                obstacle.SetActive(false);
            else if (angle < 240 && !obstacle.activeSelf)
                obstacle.SetActive(true);

    }

#elif UNITY_ANDROID
    void OnMouseDrag()
    {
        base.OnMouseDrag();
        if(play)
            if (angle > 240 && obstacle.activeSelf)
                obstacle.SetActive(false);
            else if (angle < 240 && !obstacle.activeSelf)
                obstacle.SetActive(true);
    }
#endif

}
