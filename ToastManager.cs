using UnityEngine;

/**
 * Date     : 2019.11.21
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  앱종료에 대한 기능을 구현한 스크립트
 *  -> 뒤로가기 두번으로 앱 종료
 *  
 *  Applied Location :
 *  -> SoundManager(빈 오브젝트)
 */

public class ToastManager : MonoBehaviour {

    //변수
    int clickCount = 0;
    const string message_exit = "'뒤로'버튼을 한번 더 누르면 종료됩니다.";

    //안드로이드
    AndroidJavaClass unityPlayer;
    AndroidJavaObject currentActivity;
    AndroidJavaObject context;
    AndroidJavaObject toast;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
        //뒤로 나가기 두번으로 앱 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            clickCount++;

            if(clickCount==1)
                ShowToast(message_exit);

            if (!IsInvoking("CountReset"))
                Invoke("CountReset", 1.0f);
        }
        if (clickCount > 1)
        {
            CancelInvoke("CountReset");
            Application.Quit();
        }
    }

    //토스트 띄우기
    public void ShowToast(string message)
    {
        if(currentActivity!=null)
            currentActivity.Call(
                "runOnUiThread",
                new AndroidJavaRunnable(() =>
                {
                    AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
                    AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", message);

                    toast = Toast.CallStatic<AndroidJavaObject>
                    (
                        "makeText",
                        context,
                        javaString,
                        Toast.GetStatic<int>("LENGTH_SHORT")
                    );

                    toast.Call("show");
                })
             );
        else
            print(message);
    }

    //ClickCount 초기화
    void CountReset()
    {
        clickCount = 0;
        currentActivity.Call(
            "runOnUiThread",
            new AndroidJavaRunnable(() =>
            {
                if (toast != null) toast.Call("cancel");
            }));
    }
}
