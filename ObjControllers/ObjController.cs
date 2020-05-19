using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjController : RaycastManager
{
    //Effect
    public ParticleSystem[]
        particles_clue_btn,                                 //단서버튼의 이펙트 효과... size = 3
        particles_clue_object;                              //단서 위치의 이펙트 효과... size = 3

    //UI
    public Button[] Btns_clue;                              //clue 박스... size = 3

    //Resource
    public Texture blank_pat, blank_icepack;
    public Sprite[] Sprites_clue;                           //size = 4

    //상수
    readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();
    readonly Vector3 effectScale = new Vector3(1.2f, 1.2f, 1.2f);

    //변수
    AdviceController AController;
    GameButtonController GBController;

    //접근자
    protected AdviceController AC { get { return AController; } }
    protected GameButtonController GBC { get { return GBController; } }

    //가상함수
    protected virtual void SetObjectEvent(string name) { }
    public virtual void KnifeEvent() { }

    //초기화
    void Start()
    {
        AController = GetComponent<AdviceController>();
        GBController = GetComponent<GameButtonController>();
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

    //단서 드래그 활성화여부 설정 겸 이펙트 효과
    protected void Enable_drag(bool b)
    {
        //for (int i = 0; i < 3; i++)
        //    if (!clear_clue[i])
        //    {
        //        Btns_clue[i].interactable = b;
        //        if (b)
        //            Effect_clue(particles_clue_btn[i]);
        //    }
    }

    //단서 오브젝트 이펙트 효과
    protected void Effect_clue(ParticleSystem ps) {
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
