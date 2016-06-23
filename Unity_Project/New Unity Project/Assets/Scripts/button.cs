using UnityEngine;
using System.Collections;

public class button : MonoBehaviour {
    public GameObject main;
    public GameObject sub;
    bool bFirstPersonSelection = true;
    int i = 0;
	// Use this for initialization
	void Start () {
        main = GameObject.Find("MainCamera");
        sub = GameObject.Find("subCamera");

        main.GetComponent<Camera>().enabled = true;
        sub.GetComponent<Camera>().enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () {
     
        if (bFirstPersonSelection)
        {
            main.GetComponent<Camera>().enabled = true;
            sub.GetComponent<Camera>().enabled = false;
            i++;
            if (i == 1000)
            {
                bFirstPersonSelection = false;
                i = 0;
            }
        }
        else
        {
            sub.GetComponent<Camera>().enabled = true;
            main.GetComponent<Camera>().enabled = false;
            i++;
            if (i == 1000)
            {
                bFirstPersonSelection = true;
                i = 0;
            }
        }
	}
    void onClick()
    {
        bFirstPersonSelection = false;
    }
}
