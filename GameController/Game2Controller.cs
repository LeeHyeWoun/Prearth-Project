using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game2Controller : GameController {

    //3D Object
    public GameObject
        blank, blank_RI, B_Pet, B_Vinyl, B_icepack_vinyl, B_icepack_water;

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
    //쓰레기통 선택 이벤트...드래그
    //I_DragManager.cs의 OnEndDrag 위치에서 사용
    public void Select_Trash(string clue_name)
    {

        target = GetClickedObject();
        if (target != null)
        {
            string obj_name = target.name;
            //단서1...페트병
            if (clue_name.Equals("B_Clue_1")
                && obj_name.Equals("trash1_2") && MD.Angle_Dustbin[0] > 0)
            {
                SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
                Btns_clue[0].GetComponent<Image>().sprite = Sprites_clue[0];
                blank_RI.GetComponent<RawImage>().texture = blank_pat;
                blank.SetActive(true);
                Enable_drag(false);  //잠금

                AC.Dialog_and_Advice("play2_1");
            }
            //단서2...닭뼈
            else if (clue_name.Equals("B_Clue_2"))
            {
                if (obj_name.Equals("trash2_2") && MD.Angle_Dustbin[1] > 0)
                {
                    SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
                    Activation[1] = true;
                    Btns_clue[1].interactable = false;
                    Clear_Clue(2);
                    Enable_drag(true);
                    MD.AllClose();
                }
                else if (obj_name.Equals("trash4_2") && MD.Angle_Dustbin[3] > 0)
                {
                    SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
                    AC.Dialog_and_Advice("play2_2");
                }
            }
            //단서3...아이스팩
            else if (clue_name.Equals("B_Clue_3")
                && obj_name.Equals("trash3_2") && MD.Angle_Dustbin[2] > 0)
            {
                SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
                Btns_clue[2].GetComponent<Image>().sprite = Sprites_clue[0];
                blank_RI.GetComponent<RawImage>().texture = blank_icepack;
                blank.SetActive(true);
                Enable_drag(false);  //잠금

                AC.Dialog_and_Advice("play2_3");
            }
            else
                SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
        }
    }

    public void Drag_DisassembledClue1(string name)
    {

        Time.timeScale = 1;
        target = GetClickedObject();
        if (target != null)
        {
            string target_name = target.name;
            //페트병 - 재활용 쓰레기통
            if (name.Equals("B_Pet") && target_name.Equals("trash1_2") && MD.Angle_Dustbin[0] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_Pet.SetActive(false);
                MD.AllClose();

            }
            else
            //비닐 - 비닐 쓰레기통
            if (name.Equals("B_Vinyl") && target_name.Equals("trash3_2") && MD.Angle_Dustbin[2] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_Vinyl.SetActive(false);
                MD.AllClose();
            }
            else
                SoundManager.Instance.Play_effect(2);

            if (!B_Pet.activeSelf && !B_Vinyl.activeSelf)
            {
                Activation[0] = true;
                Enable_drag(true);  //잠금해제
                Clear_Clue(1);
            }
        }
        else
            SoundManager.Instance.Play_effect(2);
    }

    public void Drag_DisassembledClue2(string name)
    {
        Time.timeScale = 1;
        target = GetClickedObject();
        if (target != null)
        {
            string target_name = target.name;
            //비닐 - 비닐 쓰레기통
            if (name.Equals("B_icepack_vinyl") && target_name.Equals("trash3_2") && MD.Angle_Dustbin[2] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_icepack_vinyl.SetActive(false);
                MD.AllClose();

            }
            else
            //아이스팩 내용물 - 일반 쓰레기통
            if (name.Equals("B_icepack_water") && target_name.Equals("trash2_2") && MD.Angle_Dustbin[1] > 0)
            {
                SoundManager.Instance.Play_effect(1);
                B_icepack_water.SetActive(false);
                MD.AllClose();
            }
            else
                SoundManager.Instance.Play_effect(2);

            if (!B_icepack_vinyl.activeSelf && !B_icepack_water.activeSelf)
            {
                Activation[2] = true;
                Enable_drag(true);  //잠금해제
                Clear_Clue(3);
            }
        }
        else
            SoundManager.Instance.Play_effect(2);
    }

    public void KnifeEvent()
    {
        Time.timeScale = 1;
        blank.SetActive(false);
        ItemBox();
        SoundManager.Instance.Play_effect(1);

        //페트병 분리
        if (Btns_clue[0].GetComponent<Image>().sprite.Equals(Sprites_clue[0]))
        {
            B_Vinyl.SetActive(true);
            B_Pet.SetActive(true);
        }
        //아이스팩 분리
        else
        {
            B_icepack_vinyl.SetActive(true);
            B_icepack_water.SetActive(true);
        }
    }
}
