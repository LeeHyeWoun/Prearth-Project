using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragController : RaycastManager,
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    //할당 받을 객체들
    public GameObject gameDirector, stg;

    //변수
    Vector3 defaultposition;
    bool isInteractable = false;

    //상수
    readonly WaitForEndOfFrame wait = new WaitForEndOfFrame();

    //접근자
    protected Vector3 Defaultposition {
        get { return defaultposition; }
        set { defaultposition = value; }
    }

    //가상함수
    protected virtual void EndCheck(string name) { }


    public void OnPointerDown(PointerEventData eventData)
    {
        isInteractable = GetComponent<Button>().interactable;
        if (isInteractable)
        {
            Time.timeScale = 0;
            SoundManager.Instance.Play_effect(0);
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
        if (isInteractable)
        {
            Defaultposition = transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isInteractable)
        {
            var screenPoint = new Vector3(eventData.position.x, eventData.position.y, 10f);
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isInteractable)
        {
            //놓는 위치에서 움찔 효과를 주기 위해 살짝 커지게 하기
            transform.localScale = transform.localScale * 1.1f;
            StartCoroutine(ItemReturn());

            EndCheck(gameObject.name);
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
        transform.position = Defaultposition;
        Time.timeScale = 1;
        transform.localScale = transform.localScale / 1.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInteractable)
        {
            transform.localScale = transform.localScale / 1.2f;
        }
    }
}
