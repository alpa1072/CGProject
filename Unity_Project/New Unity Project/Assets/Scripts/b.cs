using UnityEngine;
using System.Collections;

public class b : MonoBehaviour {
    public GameObject main = GameObject.Find("MainCamera");
    public GameObject sub = GameObject.Find("subCamera");
    bool i = true;

    public void change()
    {
        if (i == true)
        {
            main.GetComponent<Camera>().enabled = true;
            sub.GetComponent<Camera>().enabled = false;
            i = false;
        }
        else
        {
            sub.GetComponent<Camera>().enabled = true;
            main.GetComponent<Camera>().enabled = false;
            i = true;
        }
    }
}
