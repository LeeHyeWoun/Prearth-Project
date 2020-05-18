![image](https://user-images.githubusercontent.com/48902155/80517771-84070480-89c0-11ea-95fd-350a109764d5.png)

>*Unity 설정*  
>version : Unity 2018.2.5f1 Personal(64bit)  
>External Tools  
> * SDK : C:/Users/Owner/AppData/Local/Android/Sdk  
> * JDK : C:/Program Files/Java/jdk1.8.0_181   

### Team.천지개벽  
기획	: 공동  
UI 디자인	: 정진명  
모델링	: 황성미  
개발	: 이혜원, 여연진  

### To Do List  
- google play game service 설정
- 업적 설정

### To Done List
- particle
-- 배경의 유성우, 오브젝트 클릭 시, 육각형 UI용

<br><br><hr />  

04.22~0429  
* 조사일지에 조사 완료 시점 기록 구현  
* Activity Scene과 Additive Scene을 구분한 Scene이동 함수 업데이트  
* SceneController 싱글톤 방식 적용  
* 기존 스크립트를 다양한 씬에서 재사용 가능하도록 수정 및 정리  
* 배경에 유성우 effect(particle), 후광 effect(coroutine) 구현  
* Scene 토양stage2, 수질stage1, 수질 stage2 통합  
* 전처리기를 사용해 안드로이드에 빌드될 구현과 테스트 환경의 구현을 구분  


04.29~05.05  
* 텍스쳐 리소스 최적화( ETC1 형식 + POT처리 안된 텍스쳐 변경 요청)  
* Camera Culling Mask를 활용해서 OverDraw 최적화 (152 --> 14)  
* 개발/기획 상 오류 수정  
* 단서 클릭시 파티클 구현  
* 토양 stage1 구현 완료  
* 시연 영상 제작  

05.05~05.12  
* 세팅창 GUI 수정 반영  
* 히스토리 Scene 통합 및 리소스 / 스크립트 최적화  
* UI 이미지를 Sliced Iamge 처리해서 용량 최적화  

05.12~05.19  
* 리소스 최적화 마무리
* DragController 상속처리 :  
    * 가상함수 적용  
    * 각 스테이지 Controller에 오버라이딩 (Drag2Controller, Drag3Controller,....)
* StgController 상속처리 :  
    * 가상함수 적용  
    * 종류에 따라 구분  
        * StgRotationController : 가장 기본적인 스테이지를 회전시키는 기능을 제공하는 컴포넌트  
        * StgVertexController (StgRotationController 상속): 스테이지의 꼭짓점에 존재하는 오브젝트 활성화를 다루며, 시야 확보를 위해 가장 앞쪽 꼭짓점에 있는 오브젝트들을 숨겨주는 컴포넌트  
        * StgEdgeController (StgRotationController 상속): 시야 확보를 위해 앞에 있는 두개의 벽을 숨김으로서 가장 뒤쪽 모서리만 보이게 해주는 컴포넌트  

05.19~  
* 

