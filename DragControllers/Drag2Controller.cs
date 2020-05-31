public class Drag2Controller : DragController {

    //커스텀 클래스 인스턴스
    Game2Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game2Controller>();
    }

    protected override bool EndCheck(string name) {
        switch (name) {
            case "B_Vinyl":
                return GC.DE_Parts(name);

            case "B_Pet":
                return GC.DE_Parts(name);

            case "B_icepack_vinyl":
                return GC.DE_Parts(name);

            case "B_icepack_water":
                return GC.DE_Parts(name);

            case "B_item2":
                if (GetClickUI().Equals("RI_clue"))
                {
                    transform.position = Defaultposition;
                    transform.localScale = transform.localScale / 1.1f;
                    GC.DE_Item2();
                    return true;
                }
                else
                    return false;

            default:
                if (name.Substring(2, 4).Equals("Clue"))
                    return GC.DE_Select_Trash(name);
                else
                    return false;
        }
    }
}
