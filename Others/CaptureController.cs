using UnityEngine;
using UnityEngine.UI;

public class CaptureController : MonoBehaviour {

    public RawImage ri_capture;

    //변수
    Camera cmr_main;
    int culling_num;

    //캡쳐 : 스테이지 모델의 Draw Call이 너무 심해서 대사 이벤트가 버벅거리는 것을 해결하기 위해 사용
    protected void Capture() {
        cmr_main = Camera.main;
        culling_num = cmr_main.cullingMask;

        RenderTexture rt = new RenderTexture(cmr_main.pixelWidth, cmr_main.pixelHeight, 24);
        cmr_main.targetTexture = rt;
        cmr_main.Render();
        RenderTexture.active = rt;

        Texture2D screenShot = new Texture2D(cmr_main.pixelWidth, cmr_main.pixelHeight, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
        screenShot.Apply();
        ri_capture.texture = screenShot;

        cmr_main.targetTexture = null;
        cmr_main.cullingMask = 0;
        ri_capture.gameObject.SetActive(true);
    }

    protected void Reset_Culling() {
        cmr_main.cullingMask = culling_num;
    }

}
