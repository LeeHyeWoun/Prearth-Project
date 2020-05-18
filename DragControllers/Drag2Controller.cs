using UnityEngine;

public class Drag2Controller : DragController {

    //커스텀 클래스 인스턴스
    ObjManager11 OM;

    private void Start()
    {
        OM = gameDirector.GetComponent<ObjManager11>();
    }

    protected override void EndCheck() {
        string name = gameObject.name;
        if (name.Length > 5 && name.Substring(2, 4).Equals("Clue"))
            OM.Select_Trash(name);
        else if (name.Equals("B_Vinyl") || name.Equals("B_Pet"))
        {
            Time.timeScale = 1;
            OM.Drag_DisassembledClue1(name);
        }
        else if (name.Equals("B_icepack_vinyl") || name.Equals("B_icepack_water"))
        {
            Time.timeScale = 1;
            OM.Drag_DisassembledClue2(name);
        }
        else if (name.Equals("B_item2") && GetClickUI().Equals("RI_clue"))
        {
            transform.position = Defaultposition;
            Time.timeScale = 1;
            transform.localScale = transform.localScale / 1.1f;
            OM.Disassembled();
        }

    }
}
