public class Drag5Controller : DragController {

    //커스텀 클래스 인스턴스
    Game5Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game5Controller>();
    }

    protected override bool EndCheck(string name)
    {
        if (name.Substring(2, 4) == "Clue")
            return GC.DE_Clue(name);

        else
            return false;

    }
}
