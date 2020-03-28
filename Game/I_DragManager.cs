using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Date     : 2019.12.08
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  아이템에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> item1,item2,item3, clue1, clue2, clue3
 */
public class I_DragManager : RaycastManager, 
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    //할당 받을 객체들
    public GameObject gameDirector,stg;

    //변수
    Vector3 defaultposition;
    readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();
    bool isInteractable = false;

    //상수
    const float plane_distance=12f;

    //커스텀 클래스 인스턴스
    ObjManager11 OM;
    ZoomManager ZM;
    StgManager SM;

    private void Start()
    {
        OM = gameDirector.GetComponent<ObjManager11>();
        ZM = gameDirector.GetComponent<ZoomManager>();
        SM = stg.GetComponent<StgManager>();
    }

    private void SetPlays(bool b) {
        OM.SetPlay(b);
        ZM.SetPlay(b);
        SM.setPlay(b);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isInteractable = GetComponent<Button>().interactable;
        if (GetPlay()&& isInteractable)
        {
            SetPlays(false);
            transform.localScale = transform.localScale * 1.2f;
        }
    }

    /*
     * CmrController의 Zoom으로 인해 orthographicSize가 변경되면
     * 'Screen Soace - Camera'인 Canvas의 크기도 비례해서 작아지므로
     * 아이템의 위치도 이동하게 된다. 따라서 매번 드래그 시작 전 자신의 위치를 지정할 필요가 있음.
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetPlay()&& isInteractable)
        {
            defaultposition = transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GetPlay()&& isInteractable)
        {
            var screenPoint = new Vector3(eventData.position.x, eventData.position.y, plane_distance);
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetPlay()&& isInteractable)
        {
            string name = gameObject.name;
            if (name.Substring(2, 4) == "Clue")
                OM.Select_Trash(name);

            //놓는 위치에서 움찔 효과를 주기 위해 살짝 커지게 하기
            transform.localScale = transform.localScale * 1.1f;
            StartCoroutine(ItemReturn());
        }
    }

    /*
     * 손을 떼는 순간의 Raycast로 오브젝트의 기본 이벤트(open등)이 발생하지 않도록 
     * 터치 종료 후 update에 양보 후 이벤트 발생 허가를 내려야한다.
     */
    IEnumerator ItemReturn()
    {
        yield return wait;

        //아이템을 원래상태로 복귀
        transform.position = defaultposition;
        SetPlays(true);
        transform.localScale = transform.localScale / 1.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GetPlay()&& isInteractable)
        {
            transform.localScale = transform.localScale / 1.2f;
        }
    }
}
