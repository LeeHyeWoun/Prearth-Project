using UnityEngine.UI;

public class Drag3Controller : DragController
{
    //커스텀 클래스 인스턴스
    Game3Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game3Controller>();
    }

    protected override void EndCheck(string name)
    {
        string target = GetClickUI();
        transform.position = Defaultposition;

        if (target.Equals("I_cupA") || target.Equals("I_cupB"))
            switch (name)
            {
                case "B_cup_Paper":
                    GC.Balance(target, 1);
                    break;
                case "B_cup_Plastic":
                    GC.Balance(target, 2);
                    break;
                case "B_cup_Tumbler":
                    GC.Balance(target, 0);
                    break;
                default:
                    SoundManager.Instance.Play_effect(2);
                    break;
            }

        else if ( 
            (target.Equals("I_cup_1") || target.Equals("I_cup_2") || target.Equals("I_cup_3")) &&
            (name.Equals("B_cup_Paper") || name.Equals("B_cup_Plastic") || name.Equals("B_cup_Tumbler")))
            GC.Cup_Position(target, gameObject.GetComponent<Image>().sprite);

        else
            SoundManager.Instance.Play_effect(2);
    }
}
