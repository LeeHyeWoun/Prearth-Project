using UnityEngine;

public class DialogController : MonoBehaviour {

    SceneController SC = SceneController.Instance;

	// Use this for initialization
	void Start () {
        Dialog(SC.GetActiveScene_num() + "_start");
    }

    public void Dialog(string filename) {
        PlayerPrefs.SetString("DIALOG", filename);
        SC.Load_Scene(16);
    }
}
