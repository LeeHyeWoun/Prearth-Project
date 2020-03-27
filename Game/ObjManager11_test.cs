using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ObjManager11_test : RaycastManager
{
    //할당 받을 객체들
    public GameObject gameDirector, RI_blank,
        trash1_body, trash2_body, trash3_body, trash4_body, //쓰레기 통
        trash1_lid, trash2_lid, trash3_lid, Trash4_lid;      //쓰레기 뚜껑
    public Sprite clue_full;
    public Button clue1, clue2, clue3;

    //변수
    Vector3 defaultposition;
//    GameObject target;
    int[] angle_trash = new int[4] { 0, 0, 0, 0 };
    //bool play = false;
    WaitForEndOfFrame wait = new WaitForEndOfFrame();

    //상수
    const float plane_distance = 12f;

    public void openTrash() {
        target = GetClickedObject();
        if (target != null)
        {
            if (target.Equals(trash1_body))
                StartCoroutine(Motion_Trash(trash1_lid, 0));
            else if (target.Equals(trash2_body))
                StartCoroutine(Motion_Trash(trash2_lid, 1));
            else if (target.Equals(trash3_body))
                StartCoroutine(Motion_Trash(trash3_lid, 2));
            else if (target.Equals(trash4_body))
                StartCoroutine(Motion_Trash(Trash4_lid, 3));
        }
    }

    public void select() {
        if (target != null)
        {
            if (target.Equals(trash1_body) && gameObject.name.Equals("B_Clue_1"))
            {
                gameObject.GetComponent<Image>().sprite = clue_full;
                RI_blank.SetActive(true);
                //적절하다는 효과음 내기
                gameDirector.GetComponent<SoundController>().Play_effect(1);
            }
            else
                //적절하지 않다는 효과음 내기
                gameDirector.GetComponent<SoundController>().Play_effect(2);
        }
        else
            //적절하지 않다는 효과음 내기
            gameDirector.GetComponent<SoundController>().Play_effect(2);
    }

    IEnumerator Motion_Trash(GameObject obj, int num)
    {
        Vector3 vec = Vector3.left;

        //쓰레기통 열기
        if (angle_trash[num] <= 0)
        {
            while (angle_trash[num] < 100)
            {
                if (GetPlay())
                {
                    obj.transform.Rotate(vec, 4);
                    angle_trash[num] += 4;
                }
                yield return null;
            }
        }
    }

}
