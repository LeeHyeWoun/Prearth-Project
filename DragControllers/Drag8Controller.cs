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
        GC.Patch_Aircap();
    }
}
