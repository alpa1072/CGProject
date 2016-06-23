using UnityEngine;
using System.Collections;

public class CameraConversion : MonoBehaviour {

	public GameObject mainCam;
	public GameObject subCam;
	public GameObject conversion_button;

	private bool conversion = true;
	private int test = 0;

	// Use this for initialization
	void Start () {
		mainCam = GameObject.Find ("MainCamera");
		subCam = GameObject.Find ("SubCamera");

		mainCam.GetComponent<Camera> ().enabled = true;
		subCam.GetComponent<Camera> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void OnClick(){
		test++;
		if (test % 2 == 0) {
			mainCam.GetComponent<Camera> ().enabled = true;
			subCam.GetComponent<Camera> ().enabled = false;
		} else {
			subCam.GetComponent<Camera> ().enabled = true;
			mainCam.GetComponent<Camera> ().enabled = false;
		}
	}
}
