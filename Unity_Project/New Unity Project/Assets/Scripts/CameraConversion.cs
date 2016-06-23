using UnityEngine;
using System.Collections;

public class CameraConversion : MonoBehaviour {

	public GameObject mainCam;
	public GameObject subCam;
	public GameObject conversion_button;

	private bool conversion = true;
	private int i = 0;

	// Use this for initialization
	void Start () {
		mainCam = GameObject.Find ("MainCamera");
		subCam = GameObject.Find ("SubCamera");

		mainCam.GetComponent<Camera> ().enabled = true;
		subCam.GetComponent<Camera> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (conversion) {
			mainCam.GetComponent<Camera> ().enabled = true;
			subCam.GetComponent<Camera> ().enabled = false;
			i++;
			if (i == 500) {
				conversion = false;
				i = 0;
			}
		} else {
			subCam.GetComponent<Camera> ().enabled = true;
			mainCam.GetComponent<Camera> ().enabled = false;
			i++;
			if (i == 500) {
				conversion = true;
				i = 0;
			}
		}*/
	}
}
