using System.Collections;
using UnityEngine;

public class MotionFridgeController : MonoBehaviour {

    //3D object
    public GameObject fridge_L, fridge_R;                                 //냉장고 문

    //변수
    int angle_L, angle_R = 0;

    //public 함수-------------------------------------------------------------------------------
    public void Motion_Left()
    {
        SoundManager.Instance.Play_effect(0);

        if (angle_L <= 0)
            StartCoroutine(Coroutine_Left(true));   //열기
        else if (angle_L >= 120)
            StartCoroutine(Coroutine_Left(false));  //닫기
    }
    public void Motion_Right()
    {
        SoundManager.Instance.Play_effect(0);

        if (angle_R <= 0)
            StartCoroutine(Coroutine_Right(true));  //열기
        else if (angle_R >= 120)
            StartCoroutine(Coroutine_Right(false)); //닫기
    }


    //코루틴---------------------------------------------------------------------------------------
    //왼쪽 문
    IEnumerator Coroutine_Left(bool open) {
        if (open)
        {
            while (angle_L < 120)
            {
                fridge_L.transform.Rotate(Vector3.forward, 4);
                angle_L += 4;
                yield return null;
            }
        }
        else
        {
            while (angle_L > 0)
            {
                fridge_L.transform.Rotate(Vector3.back, 4);
                angle_L -= 4;
                yield return null;
            }
        }
    }
    //오른쪽 문
    IEnumerator Coroutine_Right(bool open)
    {
        if (open)
        {
            while (angle_R < 120)
            {
                fridge_R.transform.Rotate(Vector3.back, 4);
                angle_R += 4;
                yield return null;
            }
        }
        else
        {
            while (angle_R > 0)
            {
                fridge_R.transform.Rotate(Vector3.forward, 4);
                angle_R -= 4;
                yield return null;
            }
        }
    }

}
