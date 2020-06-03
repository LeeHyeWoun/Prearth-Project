using UnityEngine;

public class Stg5Controller : StgEdgeController
{
    public Camera cmr_mirror;

    //변수
    Vector3 angle_of_incidence;

    void Start() {
        angle_of_incidence = cmr_main.transform.rotation * Vector3.back;
        angle_of_incidence += Vector3.down*0.5f;
        cmr_mirror.transform.LookAt(cmr_mirror.transform.position + angle_of_incidence, cmr_main.transform.rotation * Vector3.down);
    }

    protected override void Active()
    {
        base.Active();
        cmr_mirror.transform.LookAt(cmr_mirror.transform.position + angle_of_incidence, cmr_main.transform.rotation * Vector3.down);
    }
}
