public class Drag9Controller : DragController {
    //커스텀 클래스 인스턴스
    Game9Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game9Controller>();
    }

    protected override bool EndCheck(string name)
    {
        return GC.DE_Item1();
    }

}
