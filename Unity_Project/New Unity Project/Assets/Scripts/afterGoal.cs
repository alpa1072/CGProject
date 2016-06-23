using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class afterGoal : MonoBehaviour {
    public GameObject ball;
    public Text test;
    public Text score;
    
    private Vector3 b;
    private int j=0;

    bool isGoal = false;
    bool isIn = false;
    bool isa = false;
    bool isb = false;

    static float Ascore = 0;
    static float Bscore = 8;
    float start;
    float end;
    //float deltaTime = 0.0f;
    void Start()
    {
        test = GameObject.Find("Text_test").GetComponent<Text>();

    }
	// Update is called once per frame
	void Update () {

        b = ball.transform.position;

        if (((b.y < 50f && b.y > 0f) && (b.z > 100f && b.z < 200f) && (b.x > -29.5f && b.x < 0.7f)) && !isIn)    
        {
            isGoal = true;
            isIn = true;
            start = Time.time;
            isa = true;
       
        }
        else if (((b.y < 50f && b.y > 0f) && (b.z > 100f && b.z < 200f) && (b.x < 530f && b.x > 495f)) && !isIn)
        {
            isGoal = true;
            isIn = true;
            start = Time.time;
            isb = true;
        }
        if (isGoal)
        {
            end = Time.time - start;
            test.text = end.ToString();
            if (end > 5.0f)
            {
                isGoal = false;
                if (isa) Ascore = Ascore + 0.5f;
                else if (isb) Bscore = Bscore + 0.5f;
                isa = false;
                isb = false;
                SceneManager.LoadScene("home");
            }
            
        }
	}
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, Screen.height - Screen.height * 0.05f, Screen.width, Screen.height * 0.02f);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = Screen.height * 5 / 100;
        style.normal.textColor = Color.red;
        string text = string.Format("A:{0:0} / B:{1:0}", Ascore, Bscore);
        GUI.Label(rect, text, style);
    }

}