using UnityEngine.UI;

public class Drag3Controller : DragController
{
    //커스텀 클래스 인스턴스
    Game3Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game3Controller>();
    }

    protected override bool EndCheck(string name)
    {
        string target = GetClickUI();
        transform.position = Defaultposition;

        if (target.Equals("I_cupA") || target.Equals("I_cupB"))
            switch (name)
            {
                case "B_cup_Tumbler":
                    GC.DE_Balance(target, 0);
                    return true;
                case "B_cup_Paper":
                    GC.DE_Balance(target, 1);
                    return true;
                case "B_cup_Plastic":
                    GC.DE_Balance(target, 2);
                    return true;
                default:
                    return false;
            }

        else if (target.Substring(0, 6).Equals("I_cup_") && name.Substring(0, 6).Equals("B_cup_")) {
            GC.DE_Result(target, gameObject.GetComponent<Image>().sprite);
            return true;
        }
        else
            return false;
    }
}
