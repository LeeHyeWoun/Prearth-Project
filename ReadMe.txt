+++++++++++++++++++++++++++++++++++
Unity 설정
-version : Unity 2018.2.5f1 Personal(64bit)
-External Tools
SDK : C:/Users/Owner/AppData/Local/Android/Sdk
JDK : C:/Program Files/Java/jdk1.8.0_181
+++++++++++++++++++++++++++++++++++


+++++++++++++++++++++++++++++++++++
Project Log
+++++++++++++++++++++++++++++++++++
/* 2020.03.28
*/
-item1, item2, item3에 Button Component 추가
-I_DragManager : Button의 interactable 값을 통해 드래그 여부 결정

(.......생략...................................)

/* 2020.03.20
*/
-DialogManager : clearPop Fade in 처리


/* 2020.03.19
*<Clear pop 작업>
* <현재 Scene에 따라 다른 대사 불러오게 처리>
*/
-DialogManager : routine_ending()을 통해 clear Pop 처리
-DialogManager : sceneNum으로 대사txt 구분


/* 2020.03.18
*<SoundController의 Play_effect가 Start단계 쯤에서 발생하는 현상>해결
*<03.17작업의 오류 발견 복구 및 대안>
*raycastAll은 순차적으로 안오는게 문제, 정렬을 하자
*/
-slider의 On Value Changed에 Play_effect를 적용하여 슬라이더 value가 초기화되면서 값이 변했다고 판단 소리가 났었던 것!...17:44

-StgManager : 회전 이벤트를 UpDate()에서 OnMouseDrag()로 변경
-ObjController : GetClickedObject()에서 raycast 대신 raycastAll 사용
-정렬
+ using System.Linq;
+ .OrderBy( h=> h.distance ).ToArray();


/* 2020.03.17
*< raycastAll로 인한 오류>
* 기존 '스테이지 오브젝트를 잡고 회전 시키는 방식' 대신
* 화면 전체를 사용한 회전을 사용하고 raycast를 사용해 
*제일 처음 hit하는 오브젝트를 잡게 하자.
*/
-'11_Soil_1'Scene의 (1209)stage 콜라이더 삭제 
-StgManager : 회전 이벤트를 OnMouseDrag()에서 UpDate()로 변경
-ObjController : GetClickedObject()에서 raycastAll 대신 raycast 사용


(.......생략...................................)
