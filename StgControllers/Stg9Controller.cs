using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stg9Controller : StgVertexController
{

    public GameObject
        ground1, ground2, ground3, ground4; //벽

    //안쪽 벽만 보이게 하기
    protected override void Active()
    {
        base.Active();

        if (Angle > 45 && Angle < 135)
            WallActive(ground1, ground4, ground3, ground2);

        else if (Angle > 135 && Angle < 225)
            WallActive(ground4, ground3, ground2, ground1);

        else if (Angle > 225 && Angle < 315)
            WallActive(ground3, ground2, ground1, ground4);

        else if (Angle > 315 || Angle < 45)
            WallActive(ground2, ground1, ground4, ground3);
    }

    void WallActive(GameObject f1, GameObject f2, GameObject b1, GameObject b2)
    {
        if (!f1.activeSelf || !f2.activeSelf)
        {
            f1.SetActive(true);
            f2.SetActive(true);
            b1.SetActive(false);
            b2.SetActive(false);
        }
    }
}
