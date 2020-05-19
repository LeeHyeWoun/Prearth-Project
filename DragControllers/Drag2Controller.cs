public class Drag2Controller : DragController {

    //커스텀 클래스 인스턴스
    ObjManager11 OM;

    private void Start()
    {
        OM = gameDirector.GetComponent<ObjManager11>();
    }

    protected override void EndCheck(string name) {
        switch (name) {
            case "B_Vinyl":
                OM.Drag_DisassembledClue1(name);
                break;

            case "B_Pet":
                OM.Drag_DisassembledClue1(name);
                break;

            case "B_icepack_vinyl":
                OM.Drag_DisassembledClue2(name);
                break;

            case "B_icepack_water":
                OM.Drag_DisassembledClue2(name);
                break;

            case "B_item2":
                if (GetClickUI().Equals("RI_clue"))
                {
                    transform.position = Defaultposition;
                    transform.localScale = transform.localScale / 1.1f;
                    OM.Disassembled();
                }
                break;

            default:
                if (name.Substring(2, 4).Equals("Clue"))
                    OM.Select_Trash(name);
                break;
        }
    }
}
