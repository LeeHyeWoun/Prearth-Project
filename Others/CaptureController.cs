using UnityEngine;
using UnityEngine.UI;

public class CaptureController : MonoBehaviour {

    public RawImage ri_capture;

    //변수
    Camera cmr_main;
    RenderTexture renderTexture;
    int culling_num;

    protected void Capture_Set()
    {
        //Active Scene의 Camera 설정
        cmr_main = Camera.main;
        culling_num = cmr_main.cullingMask;

        //전체 화면 사이즈 설정
        float width = ri_capture.gameObject.GetComponent<RectTransform>().rect.width;
        float height = ri_capture.gameObject.GetComponent<RectTransform>().rect.height;
        renderTexture = new RenderTexture((int)width, (int)height, 24, RenderTextureFormat.ARGB64);

    }

    protected void Capture() {
        //캡쳐 : 스테이지 모델의 Draw Call이 너무 심해서 대사 이벤트가 버벅거리는 것을 해결하기 위해 사용
        cmr_main.targetTexture = renderTexture;
        cmr_main.Render();
        ri_capture.texture = renderTexture;
        while(cmr_main.targetTexture != null)
            cmr_main.targetTexture = null;
        cmr_main.cullingMask = 0;
        ri_capture.gameObject.SetActive(true);
    }

    protected void Reset_Culling() {
        cmr_main.cullingMask = culling_num;
    }

}
