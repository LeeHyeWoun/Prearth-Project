using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : RaycastManager
{
    //Effect
    public ParticleSystem[]
        particles_clue_btn,                                 //단서버튼의 이펙트 효과... size = 3
        particles_clue_object;                              //단서 위치의 이펙트 효과... size = 3

    //UI
    public Button[] Btns_clue;                              //clue 박스... size = 3

    public GameObject Go_itemBox;

    //Resource
    public Sprite[] Sprites_clue;                           //size = 5

    //상수
    readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();
    readonly Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);

    //변수
    AdviceController AController;
    bool[] movable_clue = new bool[3] { false, false, false };//단서 클리어 정도

    //접근자
    protected AdviceController AC { get { return AController; } }
    protected bool[] Movable_Clue
    {
        get { return movable_clue; }
        set { movable_clue = value; }
    }

    //가상함수
    protected virtual void SetObjectEvent(string name) { }

    //초기화
    void Start()
    {
        AController = GetComponent<AdviceController>();
    }

    //업데이트
    void Update()
    {
        //누를때 오브젝트를 클릭 했다면
        if (Time.timeScale.Equals(1) && Input.GetMouseButtonDown(0))
        {
            target = GetClickedObject();
            if (target != null)
                SetObjectEvent(target.name);
        }
    }

    //public 함수--------------------------------------------------------------------------------------
    //아이템 박스 열고 닫기
    public void ItemBox()
    {
        SoundManager.Instance.Play_effect(0);
        if (Go_itemBox.activeSelf)
            Go_itemBox.SetActive(false);
        else
            Go_itemBox.SetActive(true);
    }

    //potected 함수들----------------------------------------------------------------------------------
    //단서 발견 시 이벤트...클릭
    protected void Collect(int num, string name)
    {
        Btns_clue[num - 1].GetComponent<Image>().sprite = Sprites_clue[num];
        Movable_Clue[num - 1] = true;
        particles_clue_object[num - 1].Play();

        SoundManager.Instance.Play_effect(1);
        target.SetActive(false);
        AC.Advice(name);

        //모든 단서를 찾았다면
        if (Movable_Clue[0] && Movable_Clue[1] && Movable_Clue[2])
        {
            //초기화
            for (int i = 0; i < 3; i++)
                Movable_Clue[i] = false;

            //다음 명령 내리기
            AC.Dialog_and_Advice("play1");

            //드래그해야할 단서 표시하기
            Effect_clue(particles_clue_btn[0]);

            //잠금 해제
            Enable_drag(true);
        }
    }

    //단서 해결 이벤트
    protected void Clear_Clue(int num)
    {
        Btns_clue[num - 1].GetComponent<Image>().sprite = Sprites_clue[4];

        if (Btns_clue[0].GetComponent<Image>().sprite.Equals(Sprites_clue[4]) &&
            Btns_clue[1].GetComponent<Image>().sprite.Equals(Sprites_clue[4]) &&
            Btns_clue[2].GetComponent<Image>().sprite.Equals(Sprites_clue[4]))
            Invoke("Ending", 1f);
    }

    //게임 마무리 대사
    void Ending()
    {
        GetComponent<AdviceController>().Dialog_and_Advice("clear");
    }

    //단서 드래그 활성화여부 설정 겸 이펙트 효과
    protected void Enable_drag(bool b)
    {
        for (int i = 0; i < 3; i++)
            if (!Movable_Clue[i])
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
