public class Drag6Controller : DragController
{

    //커스텀 클래스 인스턴스
    Game6Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game6Controller>();
    }

    protected override bool EndCheck(string name)
    {
        if (!GetClickUI().Equals("RI_clue"))
            return false;

        switch (name)
        {
            case "B_item2":
                transform.position = Defaultposition;
                transform.localScale = transform.localScale / 1.1f;
                return GC.DE_Item2();

            case "B_item3":
                transform.position = Defaultposition;
                transform.localScale = transform.localScale / 1.1f;
                return GC.DE_Item3();

            default:
                return false;
        }
    }
}
