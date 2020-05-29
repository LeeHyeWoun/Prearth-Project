public class Drag6Controller : DragController
{

    //커스텀 클래스 인스턴스
    Game6Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game6Controller>();
    }

    protected override void EndCheck(string name)
    {
        switch (name)
        {
            case "B_item2":
                if (GetClickUI().Equals("RI_clue"))
                {
                    transform.position = Defaultposition;
                    transform.localScale = transform.localScale / 1.1f;
                    GC.KnifeEvent();
                }
                break;
            case "B_item3":
                if (GetClickUI().Equals("RI_clue"))
                {
                    transform.position = Defaultposition;
                    transform.localScale = transform.localScale / 1.1f;
                    GC.DodbogiEvent();
                }
                break;

            /*default:
                if (name.Substring(2, 4).Equals("Clue"))
                    GC.Select_Trash(name);
                break;*/
        }
    }
}
