/**
 * Date     : 2020.03.27
 * Manager  : 여연진
 * 
 * Date     : 2020.05.14
 * Modify   : 이혜원
 * 
 * The function of this script :
 *  Scene'05_Water_1의 오브젝트에 관한 다양한 이벤트를 다루는 스크립트
 *  
 *  Applied Location :
 *  -> GameDirector
 */
public class Game5Controller : GameController {

    //Overriding-----------------------------------------------------------------------------------
    protected override void SetObjectEvent(string name)
    {
        switch (name)
        {
            case "pan":
                Collect(1, "기름");
                break;
            case "drug":
                Collect(2, "물약");
                break;
            case "lens":
                Collect(3, "렌즈");
                break;
        }
    }

    //public 함수-----------------------------------------------------------------------------------
    //기름 선택 이벤트...드래그
    //DragController14.cs의 OnEndDrag 위치에서 사용
    public void Select_Trash(string clue_name)
    {

        target = GetClickedObject();
        if (target != null)
        {
            int num = -1;
            SetTrash_Number(target.name, ref num);

            if (Match_Clue_to_Trash(clue_name, num))
                SoundManager.Instance.Play_effect(1);  //적절하다는 효과음 내기
            else
                SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
        }
    }

    //private 함수---------------------------------------------------------------------------------
    void SetTrash_Number(string target_name, ref int num)
    {
        switch (target_name)
        {
            case "oil_can":
                num = 0;
                break;
            case "drug_can":
                num = 1;
                break;
            case "lens_trashcan":
                num = 2;
                break;

            default:
                SoundManager.Instance.Play_effect(2);  //적절하지 않다는 효과음 내기
                break;
        }
    }

    //적절한 단서와 쓰레기통이 매치되었는지 확인
    bool Match_Clue_to_Trash(string clue_name, int trash_num)
    {
        bool success = false;
        switch (clue_name)
        {

            case "B_Clue_1":    //기름
                //폐기름통
                if (trash_num == 0)
                {
                    success = true;
                    Activation[0] = true;
                    Clear_Clue(1);
                    AC.Dialog_and_Advice("play2_1");
                }
                break;

            case "B_Clue_2":    //물약
                //폐약품함
                if (trash_num == 1)
                {
                    success = true;
                    Activation[1] = true;
                    Clear_Clue(2);
                    AC.Dialog_and_Advice("play2_2");
                }

                break;

            case "B_Clue_3":    //렌즈
                //비닐 쓰레기통
                if (trash_num == 2)
                {
                    success = true;
                    Activation[2] = true;
                    Clear_Clue(3);
                    AC.Dialog_and_Advice("play2_3");
                }
                break;

        }
        return success;
    }
}
