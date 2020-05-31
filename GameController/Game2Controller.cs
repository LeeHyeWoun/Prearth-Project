using UnityEngine;
using UnityEngine.UI;

public class Game2Controller : Stage1Controller {

    //3D Object
    public GameObject[] Parts;  //size = 4
    public GameObject blank;
    public RawImage blank_RI;

    //Resource
    public Texture blank_pat, blank_icepack;

    //변수
    MotionDustbinController MD;
    MotionFridgeController MF;

    void Start() {
        MD = GetComponent<MotionDustbinController>();
        MF = GetComponent<MotionFridgeController>();
    }

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "Bottle":
                Collect(1, "페트병");
                break;
            case "bone1":
                Collect(2, "닭뼈");
                break;
            case "icepack":
                Collect(3, "아이스팩");
                break;
            case "fridge_left":
                MF.Motion_Left();
                break;
            case "fridge_right":
                MF.Motion_Right();
                break;
            case "trash1_2":
                MD.Open(0);
                break;
            case "trash2_2":
                MD.Open(1);
                break;
            case "trash3_2":
                MD.Open(2);
                break;
            case "trash4_2":
                MD.Open(3);
                break;
        }
    }

    //public 함수-----------------------------------------------------------------------------------
    public void DE_Item2()
    {
        Time.timeScale = 1;
        blank.SetActive(false);
        ItemBox();
        SoundManager.Instance.Play_effect(1);

        //페트병 분리
        if (Btns_clue[0].GetComponent<Image>().sprite.Equals(Sprites_clue[0]))
        {
            Parts[0].SetActive(true);
            Parts[1].SetActive(true);
        }
        //아이스팩 분리
        else
        {
            Parts[2].SetActive(true);
            Parts[3].SetActive(true);
        }
    }

    //쓰레기통 선택 이벤트...드래그
    //I_DragManager.cs의 OnEndDrag 위치에서 사용
    public bool DE_Select_Trash(string clue_name)
    {

        target = GetClickedObject();
        if (target == null)
            return false;

        string obj_name = target.name;

        //단서1...페트병
        if (clue_name.Equals("B_Clue_1"))
        {
            if (obj_name.Equals("trash1_2") == false || MD.Angle_Dustbin[0] <= 0)
                return false;

            SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
            Btns_clue[0].GetComponent<Image>().sprite = Sprites_clue[0];
            blank_RI.texture = blank_pat;
            blank.SetActive(true);
            Enable_drag(false);  //잠금

            AC.Dialog_and_Advice("play2_1");
            return true;
        }
        //단서2...닭뼈
        if (clue_name.Equals("B_Clue_2"))
        {
            if (obj_name.Equals("trash4_2") && MD.Angle_Dustbin[3] > 0)
            {
                AC.Dialog_and_Advice("play2_2");
                return false;
            }

            if (obj_name.Equals("trash2_2") == false || MD.Angle_Dustbin[1] <= 0)
                return false;

            SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
            Activation[1] = true;
            Btns_clue[1].interactable = false;
            Clear_Clue(2);
            Enable_drag(true);
            MD.AllClose();
            return true;
        }
        //단서3...아이스팩
        if (clue_name.Equals("B_Clue_3"))
        {
            if (obj_name.Equals("trash3_2") == false || MD.Angle_Dustbin[2] <= 0)
                return false;

            SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
            Btns_clue[2].GetComponent<Image>().sprite = Sprites_clue[0];
            blank_RI.texture = blank_icepack;
            blank.SetActive(true);
            Enable_drag(false);  //잠금

            AC.Dialog_and_Advice("play2_3");
            return true;
        }

        return false;
    }

    public bool DE_Parts(string name)
    {

        Time.timeScale = 1;
        target = GetClickedObject();
        if (target == null)
            return false;

        //페트병 - 재활용 쓰레기통
        if (name.Equals(Parts[0].name))
        {
            if (target.name.Equals("trash1_2") && MD.Angle_Dustbin[0] > 0)
                return Clear_Part(0);
        }
        //비닐 - 비닐 쓰레기통
        else if (name.Equals(Parts[1].name))
        {
            if (target.name.Equals("trash3_2") && MD.Angle_Dustbin[2] > 0)
                return Clear_Part(1);
        }
        //비닐 - 비닐 쓰레기통
        else if (name.Equals(Parts[2].name))
        {
            if (target.name.Equals("trash3_2") && MD.Angle_Dustbin[2] > 0)
                return Clear_Part(2);
        }
        //아이스팩 내용물 - 일반 쓰레기통
        else if (name.Equals(Parts[3].name))
        {
            if (target.name.Equals("trash2_2") && MD.Angle_Dustbin[1] > 0)
                return Clear_Part(3);
        }

        return false;
    }

    //private 함수-----------------------------------------------------------------------------------
    bool Clear_Part(int num) {
        SoundManager.Instance.Play_effect(1);
        Parts[num].SetActive(false);
        MD.AllClose();

        num = (num < 2) ? 0 : 2;

        if (Parts[0 + num].activeSelf || Parts[1 + num].activeSelf)
            return true;

        Activation[0 + num] = true;
        Enable_drag(true);  //잠금해제
        Clear_Clue(1 + num);
        return true;

    }

}
