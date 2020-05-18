using UnityEngine;

/**
 * Date     : 2020.05.18
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  스테이지의 모서리 Active 설정을 담당하는 스크립트
 */
public class StgEdgeController : StgRotationController {

    public GameObject
        objects1, objects2, objects3, objects4; //벽

    //안쪽 벽만 보이게 하기
    protected override void Active()
    {
        if (Angle > 45 && Angle < 135)
            WallActive(objects1, objects4, objects3, objects2);

        else if (Angle > 135 && Angle < 225)
            WallActive(objects4, objects3, objects2, objects1);

        else if (Angle > 225 && Angle < 315)
            WallActive(objects3, objects2, objects1, objects4);

        else if (Angle > 315 || Angle < 45)
            WallActive(objects2, objects1, objects4, objects3);
    }

    void WallActive(GameObject f1, GameObject f2, GameObject b1, GameObject b2)
    {
        if (!b1.activeSelf || !b2.activeSelf)
        {
            f1.SetActive(false);
            f2.SetActive(false);
            b1.SetActive(true);
            b2.SetActive(true);
        }
    }

}
