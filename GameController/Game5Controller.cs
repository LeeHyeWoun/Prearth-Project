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
public class Game5Controller : Stage1Controller {

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
    public bool DE_Clue(string clue_name)
    {

        target = GetClickedObject();
        if (target == null)
            return false;

        if (clue_name.Equals("B_Clue_1"))
            return Match_Clue(1);

        if (clue_name.Equals("B_Clue_2"))
            return Match_Clue(2);

        if (clue_name.Equals("B_Clue_3"))
            return Match_Clue(3);

        return false;
    }
    //private 함수-----------------------------------------------------------------------------------
    bool Match_Clue(int num) {

        if (target.name.Equals("bin" + num) == false)
            return false;

        SoundManager.Instance.Play_effect(1);
        AC.Dialog_and_Advice("play2_" + num);
        Activation[num-1] = true;
        Clear_Clue(num);
        return true;
    }
}
