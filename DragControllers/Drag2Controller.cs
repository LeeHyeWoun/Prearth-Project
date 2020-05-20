public class Drag2Controller : DragController {

    //커스텀 클래스 인스턴스
    Game2Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game2Controller>();
    }

    protected override void EndCheck(string name) {
        switch (name) {
            case "B_Vinyl":
                GC.Drag_DisassembledClue1(name);
                break;

            case "B_Pet":
                GC.Drag_DisassembledClue1(name);
                break;

            case "B_icepack_vinyl":
                GC.Drag_DisassembledClue2(name);
                break;

            case "B_icepack_water":
                GC.Drag_DisassembledClue2(name);
                break;

            case "B_item2":
                if (GetClickUI().Equals("RI_clue"))
                {
                    transform.position = Defaultposition;
                    transform.localScale = transform.localScale / 1.1f;
                    GC.KnifeEvent();
                }
                break;

            default:
                if (name.Substring(2, 4).Equals("Clue"))
                    GC.Select_Trash(name);
                break;
        }
    }
}
