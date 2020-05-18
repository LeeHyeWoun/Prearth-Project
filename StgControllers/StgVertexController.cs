using UnityEngine;

/**
 * Date     : 2020.05.18
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  스테이지 꼭짓점에 있는 오브젝트의 Active 설정을 담당하는 스크립트
 */
public class StgVertexController : StgRotationController
{
    public GameObject
        objects1, objects2, objects3, objects4; //장애물들

    //안쪽 벽만 보이게 하기
    protected override void Active()
    {
        if (Angle > 45 && Angle < 135 && objects2.activeSelf)
            VectexObjsActive(objects4, objects1, objects3);

        else if (Angle > 135 && Angle < 225 && objects3.activeSelf)
            VectexObjsActive(objects3, objects4, objects2);

        else if (Angle > 225 && Angle < 315 && objects2.activeSelf)
            VectexObjsActive(objects2, objects3, objects1);

        else if (Angle > 315 || Angle < 45 && objects1.activeSelf)
            VectexObjsActive(objects1, objects2, objects4);
    }

    void VectexObjsActive(GameObject center, GameObject left, GameObject right)
    {
        center.SetActive(false);
        left.SetActive(true);
        right.SetActive(true);
    }
}
