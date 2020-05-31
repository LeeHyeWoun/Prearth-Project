public class Drag8Controller : DragController
{

    //커스텀 클래스 인스턴스
    Game8Controller GC;

    private void Start()
    {
        GC = gameDirector.GetComponent<Game8Controller>();
    }

    protected override bool EndCheck(string name)
    {
        return GC.DE_Aircap();
    }
}
