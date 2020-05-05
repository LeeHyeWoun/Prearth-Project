using UnityEngine;
using System.Linq;

/**
 * Date     : 2020.03.23
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  ObjManager에 상속시키기 위한 부모 클래스
 *  
 *  Applied Location :
 *  -> ObjManager에 상속
 */

public class RaycastManager : MonoBehaviour {

    protected GameObject target;                                      //레이케스트 충돌 오브젝트

    private Camera cmr;

    void Awake() {
        cmr = Camera.main;
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

    //클릭한 UI 이름 반환
    protected string GetClickUI() {
        Vector2 touchPosition = cmr.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(touchPosition, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        return hit ? hit.collider.name : "";
    }
}
