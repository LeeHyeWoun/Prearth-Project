using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //UI
    public Camera cmr;
    public GameObject Go_itemBox;
    public Button[] Btns_clue;                              //clue 박스... size = 3

    //Effect
    public ParticleSystem[]
        particles_clue_btn,                                 //단서버튼의 이펙트 효과... size = 3
        particles_clue_object;                              //단서 위치의 이펙트 효과... size = 3

    //Resource
    public Sprite[] Sprites_clue;                           //size = 5

    //상수
    readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();
    readonly Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);

    //변수
    protected AdviceController AC;
    protected GameObject target;                                      //레이케스트 충돌 오브젝트
    bool[] activation = new bool[3] { false, false, false };
    bool[] disappear_clue = new bool[3] { true, true, true };

    //접근자
    protected bool[] Activation
    {
        get { return activation; }
        set { activation = value; }
    }
    protected bool[] Disappear_Clue {
        get { return disappear_clue; }
        set { disappear_clue = value; }
    }

    //가상함수
    protected virtual void SetObjectEvent(string name) { }

    //초기화
    void Awake()
    {
        AC = GetComponent<AdviceController>();
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
        Activation[num - 1] = true;
        particles_clue_object[num - 1].Play();

        SoundManager.Instance.Play_effect(1);
        if(Disappear_Clue[num - 1])
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
        AC.Dialog_and_Advice("clear");
    }

    //클릭한 오브젝트를 반환
    protected GameObject GetClickedObject()
    {

        RaycastHit[] hits;
        GameObject target = null;
        float MaxDistance = 30f;

        //마우스 포인트 근처 좌표를 만든다.
        Ray ray = cmr.ScreenPointToRay(Input.mousePosition);

        //RaycastNonAlloc로 최적화하기
        // Raycast : 터치한 위치의 일직선 상(z축)으로 있는 콜라이더 인식 + 거리 순으로 배열에 입력
        hits = Physics.RaycastAll(ray.origin, ray.direction * 10, MaxDistance).OrderBy(h => h.distance).ToArray();

        //스테이지 회전을 위해 스테이지가 가장 먼저 콜라이더에 닿으므로 2개 이상이 잡힐 때 작동해야함 
        if (hits.Length > 1)
        {
            //2번째 충돌 오브젝트로 저장
            target = hits[1].collider.gameObject;
        }

        return target;

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
