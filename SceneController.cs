﻿using UnityEngine.SceneManagement;
using UnityEngine;

/**
 * Date     : 2020.02.20
 * Manager  : 이혜원
 * 
 * The function of this script :
 *  씬의 이동을 담당하는 스크립트
 *  
 *  Applied Location :
 *  -> 모든 Scene의 GameDirector(빈 오브젝트)
 */
public class SceneController : MonoBehaviour {

    //싱글톤 인스턴스
    public static SceneController Instance = null;

    //변수
    private int planet_num = 0; //토양 : 0, 수질 : 1, 대기 : 2
    private ToastManager TM;

    private void Awake() {
        TM = GetComponent<ToastManager>();
    }

    //행성값에 대한 설정자
    public void SetPlanetNum(int num) {
        planet_num = num;
    }

    //행성값에 대한 접근자
    public int GetPlanetNum() {
        return planet_num;
    }

    private int Index_padding(ref int num) {   //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!게임 씬 추가 후 삭제 혹은 수정 바람

        if (num > 9)
            num--;
        if (num > 8)
            num--;
        if (num > 6)
            num--;
        if (num > 5)
            num--;
        if (num > 3)
            num--;

        return num;
    }
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    //Scene 이동 (Single mode)
    //주의 사항 : 빌드세팅의 인덱스 설정
    public void Load_Scene(int num) {

        //Scene 존재 여부 체크
        if (num == 3 || num == 5 || num == 6 || num == 8 || num == 9) //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!게임 씬 추가 후 삭제 혹은 수정 바람
        {
            TM.ShowToast("아직 준비되지 않은 Scene입니다.");
            return;
        }

        Index_padding(ref num);
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        if (num > 11- 5/*게임 씬 추가 후  수정 바람*/)
        {
            //일시정지
            Time.timeScale = 0;

            //이미 Additive Scene이 있다면 Unload 하기
            if (SceneManager.sceneCount > 1)
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount-1).name);

            //추가
            SceneManager.LoadSceneAsync(num, LoadSceneMode.Additive);
        }
        else if (num < 11- 5/*게임 씬 추가 후  수정 바람*/)
        {
            //배경음악 설정
            if (num == 0 && GetActiveScene_num() > 1)
                SoundManager.Instance.BGM_reset();
            else if (num > 1 && num < 11)
                SoundManager.Instance.BGM_chage(true);

            //이동
            SceneManager.LoadScene(num, LoadSceneMode.Single);
        }
        else
        {
            //일시정지
            Time.timeScale = 0;

            //추가
            SceneManager.LoadSceneAsync(num, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync(num + 1, LoadSceneMode.Additive);
        }
    }

    //나가기(뒤로가기) 버튼에 적용
    public void Destroy_Scene() {

        //일시정지 해제
        Time.timeScale = 1;

        //Active Scene을 제외한 나머지 Scene Unload
        for (int i = 1; i < SceneManager.sceneCount; ++i)
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
    }

    public string GetActiveScene_name() {
        return SceneManager.GetActiveScene().name;
    }

    public int GetActiveScene_num() {
        return SceneManager.GetActiveScene().buildIndex;
    }

    //MapManager에서 사용
    public void Prevent(int num) {
        TM.ShowToast("'Stage" + num + "'부터 입장해주세요!");
    }
}
