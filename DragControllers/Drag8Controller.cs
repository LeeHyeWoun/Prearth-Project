public class Drag8Controller : DragController
{

    //커스텀 클래스 인스턴스
    Game8Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game8Controller>();
    }

    protected override void EndCheck(string name)
    {
        if (name.Substring(2, 4) == "Clue")
            GC.Select_Trash(name);

        else if (name.Equals("B_item2"))
            GC.Item_knife();

    }
}
