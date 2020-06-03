using UnityEngine;

/**
 * Date     : 2020.05.18
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  스테이지의 모서리 Active 설정을 담당하는 스크립트
 */
public class StgEdgeController : StgRotationController {

    public Camera cmr_main;

    //변수
    int culling;
    
    //안쪽 벽만 보이게 하기
    protected override void Active()
    {
        if (Angle > 45 && Angle < 135)
            culling = (1 << 12) | (1 << 13);

        else if (Angle > 135 && Angle < 225)
            culling = (1 << 11) | (1 << 12);

        else if (Angle > 225 && Angle < 315)
            culling = (1 << 14) | (1 << 11);

        else
            culling = (1 << 13) | (1 << 14);

        cmr_main.cullingMask = culling + 1847;
    }
}
