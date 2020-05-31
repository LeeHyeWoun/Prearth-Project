using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Controller : GameController
{

    public ParticleSystem[] particles_clue_btn; //단서버튼의 이펙트 효과... size = 3

    //상수
    readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();
    readonly Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);

    //변수
    bool[] bools_activation = new bool[3] { false, false, false };
    bool[] bools_disappear_clue = new bool[3] { true, true, true };

    //접근자
    protected bool[] Activation
    {
        get { return bools_activation; }
        set { bools_activation = value; }
    }
    protected bool[] Disappear_Clue
    {
        get { return bools_disappear_clue; }
        set { bools_disappear_clue = value; }
    }

    //potected 함수들----------------------------------------------------------------------------------
    //단서 발견 시 이벤트...클릭
    protected void Collect(int num, string name)
    {
        Btns_clue[num - 1].GetComponent<Image>().sprite = Sprites_clue[num];
        Activation[num - 1] = true;
        particles_clue_object[num - 1].Play();

        SoundManager.Instance.Play_effect(1);
        if (Disappear_Clue[num - 1])
            target.SetActive(false);
        AC.Advice(name);

        //모든 단서를 찾았다면
        if (Activation[0] && Activation[1] && Activation[2])
        {
            //초기화
            for (int i = 0; i < 3; i++)
                Activation[i] = false;

            //다음 명령 내리기
            AC.Dialog_and_Advice("play1");

            //드래그해야할 단서 표시하기
            Effect_clue(particles_clue_btn[0]);

            //잠금 해제
            Enable_drag(true);
        }
    }

    //단서 드래그 활성화여부 설정 겸 이펙트 효과
    protected void Enable_drag(bool b)
    {
        for (int i = 0; i < 3; i++)
            if (!Activation[i])
            {
                Btns_clue[i].interactable = b;
                if (b)
                    Effect_clue(particles_clue_btn[i]);
            }
    }

    //단서 오브젝트 이펙트 효과
    protected void Effect_clue(ParticleSystem ps)
    {
        StartCoroutine(coroutine_effect_btn(ps));
    }

    //코루틴---------------------------------------------------------------------------------------------
    IEnumerator coroutine_effect_btn(ParticleSystem ps)
    {
        yield return wait;
        ps.transform.localScale = effectScale * cmr.orthographicSize / 5f;
        ps.Play();
    }

}
