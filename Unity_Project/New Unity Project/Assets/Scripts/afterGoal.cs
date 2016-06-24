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
    bool isIn = false; // 아래에서 start시간이 골이 들어갔을 때 딱 1번만 되도록 하기 위한 변수이다. 

    //공이 A팀의 골대에 들어갔는지, B팀의 골대에 들어갔는지를 파악하기 위한 변수이다.
    bool isa = false;
    bool isb = false;
    static bool isafirst = false;
    static bool isbfirst = false;

    //처음에만 0으로 초기화 되고 그 이후로는 바뀐 값이 반영되어야 하므로 static으로 선언하였다. 
    static float Ascore = 0; // A팀 점수
    static float Bscore = 0; // B팀 점수
    static float timer = 0; // 시간

    float start;
    float end;

    void Start()
    {
        test = GameObject.Find("Text_test").GetComponent<Text>(); //골이 들어갔을 때 몇 초가 지난지 확인하기 위한 text

    }
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime; //지나간 시간을 계속 계산한다. 
        b = ball.transform.position;

        //골인을 하면 
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
                if (isb && !isbfirst)
                {
                    //A가 골인이면 
                    Ascore = Ascore + 0.5f;
                    isbfirst = true;
                }
                else if (isb && isbfirst) Ascore = Ascore + 1f;
                
                else if (isa && !isafirst)
                {
                    //B가 골인이면
                    Bscore = Bscore + 0.5f;
                    isafirst = true;
                }
                else if (isa && isafirst) Bscore = Bscore + 1f;
                isa = false;
                isb = false;
                CameraController.curr_player_number = -1;
                
                //처음 장면 불러오기. (reset)
                SceneManager.LoadScene("home");
            }
            
        }
	}

    //screen에 점수와 타이머 표시하는 함수
    void OnGUI()
    {
        int h = Screen.height;
        int w = Screen.width;

        //점수를 표시하는 부분입니다.
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 0.02f); // 보여줄 화면의 위치 계산
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 5 / 100;
        style.normal.textColor = Color.red;
        string text = string.Format("A:{0:0} / B:{1:0}", Ascore, Bscore);
        GUI.Label(rect, text, style);

        //시간을 표시하는 부분입니다. 
        GUIStyle style2 = new GUIStyle();
        Rect rect2 = new Rect(15f, 0, w, h * 0.02f); // 보여줄 화면의 위치 계산
        style2.alignment = TextAnchor.UpperCenter;
        style2.fontSize = h * 5 / 100;
        style2.normal.textColor = Color.black;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string text2 = string.Format("TIME : {0:0}:{1:00}", minutes, seconds); // 분과 초로 시간을 화면에 보여준다.
        GUI.Label(rect2, text2, style2);
    } 
}