using UnityEngine;

public class Drag8Controller : DragController
{

    //커스텀 클래스 인스턴스
    ObjManager8 OM;

    private void Start()
    {
        OM = gameDirector.GetComponent<ObjManager8>();
    }

    protected override void EndCheck()
    {
        string name = gameObject.name;
        if (name.Length > 5 && name.Substring(2, 4) == "Clue")
            OM.Select_Trash(name);

        else if (name.Equals("B_item2"))
            OM.Item_knife();

    }
}
