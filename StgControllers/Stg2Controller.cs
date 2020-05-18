using UnityEngine;

/**
 * Date     : 2020.05.18
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  토양 스테이지1의 모서리와 장애물의 Active 설정을 담당하는 스크립트
 */
public class Stg2Controller : StgEdgeController {

    public GameObject 
        obstacle;       //장애물

    protected override void Active() {
        base.Active();

        if (Angle > 225 && Angle < 315 && obstacle.activeSelf)
            obstacle.SetActive(false);
        else if ((Angle < 225 || Angle > 315) && !obstacle.activeSelf)
            obstacle.SetActive(true);
    }

}
