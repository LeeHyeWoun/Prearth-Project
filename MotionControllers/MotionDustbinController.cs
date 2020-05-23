using System.Collections;
using UnityEngine;

public class MotionDustbinController : MonoBehaviour {

    public GameObject[] obj_lid;                              //쓰레기 뚜껑... size = 4

    //변수
    IEnumerator[] coroutine = new IEnumerator[4];
    int[] angle = new int[4] { 0, 0, 0, 0 };          //쓰레기통 뚜껑 문 열림 정도

    //접근자 
    public int[] Angle_Dustbin { get { return angle; } }


    //private 함수---------------------------------------------------------------------------------
    //쓰레기통 뚜껑 여닫기 이벤트...클릭
    public void Open(int num)
    {
        SoundManager.Instance.Play_effect(0);

        if (angle[num] <= 0)          //열기
        {
            coroutine[num] = Open_Trash(num);
            StartCoroutine(coroutine[num]);
        }
        else if (angle[num] >= 100)   //닫기
            StartCoroutine(Close_Trash(num));
    }

    public void AllClose() {
        for (int i = 0; i < 4; i++)
            StartCoroutine(Close_Trash(i));
    }

    //코루틴---------------------------------------------------------------------------------------
    //쓰레기통 열기
    IEnumerator Open_Trash(int num)
    {
        Vector3 vec = Vector3.left;
        while (angle[num] < 100)
        {
            for (int i = 0; i < 4; i++)
            {
                //(num+1)번 쓰레기통은 뚜껑을 열고
                if (i == num)
                {
                    obj_lid[i].transform.Rotate(vec, 4);
                    angle[i] += 4;
                }
                //나머지 뚜껑은 닫기
                else if (angle[i] > 0)
                {
                    StopCoroutine(coroutine[i]);
                    obj_lid[i].transform.Rotate(vec, -4);
                    angle[i] -= 4;
                }
            }
            yield return null;
        }
    }
    //쓰레기통 닫기
    IEnumerator Close_Trash(int num)
    {
        Vector3 vec = Vector3.left;
        while (angle[num] > 0)
        {
            obj_lid[num].transform.Rotate(vec, -4);
            angle[num] -= 4;
            yield return null;
        }
    }

}
