using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Button_click : MonoBehaviour {
	public GameObject btn;
	public GameObject main_test;
	public GameObject sub_test;
	private int test = 0;

	void Start(){
		main_test = GameObject.Find ("MainCamera");
		sub_test = GameObject.Find ("SubCamera");
		main_test.GetComponent<Camera> ().enabled = true;
		sub_test.GetComponent<Camera> ().enabled = false;
	}
	void Update(){
	}
	public void OnClick(){
		test++;
		//Debug.Log ("Click!! " + test);
		if (test % 2 == 1) {
			main_test.GetComponent<Camera> ().enabled = true;
			sub_test.GetComponent<Camera> ().enabled = false;
		} else {
			sub_test.GetComponent<Camera> ().enabled = true;
			main_test.GetComponent<Camera> ().enabled = false;
		}
	}
}
