using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //UI
    public Camera cmr;
    public GameObject Go_itemBox;
    public Button[] Btns_clue;                              //clue 박스... size = 3

    //Resource
    public Sprite[] Sprites_clue;                           //size = 5 / 2

    //Effect
    public ParticleSystem[] particles_clue_object;                              //단서 위치의 이펙트 효과... size = 3

    //변수
    protected AdviceController AC;
    protected GameObject target;                                      //레이케스트 충돌 오브젝트

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
    //단서 해결 이벤트
    protected void Clear_Clue(int num)
    {
        int last = Sprites_clue.Length - 1;
        Btns_clue[num - 1].GetComponent<Image>().sprite = Sprites_clue[last];

        if (Btns_clue[0].GetComponent<Image>().sprite.Equals(Sprites_clue[last]) &&
            Btns_clue[1].GetComponent<Image>().sprite.Equals(Sprites_clue[last]) &&
            Btns_clue[2].GetComponent<Image>().sprite.Equals(Sprites_clue[last]))
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
        hits = Physics.RaycastAll(ray.origin, ray.direction * 10, MaxDistance, cmr.cullingMask).OrderBy(h => h.distance).ToArray();

        //스테이지 회전을 위해 스테이지가 가장 먼저 콜라이더에 닿으므로 2개 이상이 잡힐 때 작동해야함 
        if (hits.Length > 1)
        {
            //2번째 충돌 오브젝트로 저장
            target = hits[1].collider.gameObject;
        }

        return target;

    }
}
