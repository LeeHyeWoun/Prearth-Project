using UnityEngine;

public class DialogController : MonoBehaviour {

    SceneController SC;

	// Use this for initialization
	void Start () {
        SC = GetComponent<SceneController>();

        Dialog(SC.GetActiveScene_num() + "_start");
    }

    public void Dialog(string filename) {
        PlayerPrefs.SetString("DIALOG", filename);
        SC.Load_Scene(9);
    }
}
