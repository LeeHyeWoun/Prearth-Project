public class Drag5Controller : DragController {

    //커스텀 클래스 인스턴스
    ObjectManager5 OM;

    private void Start()
    {
        OM = gameDirector.GetComponent<ObjectManager5>();
    }

    protected override void EndCheck(string name)
    {
        if (name.Substring(2, 4) == "Clue")
            OM.Select_Trash(name);
    }
}
