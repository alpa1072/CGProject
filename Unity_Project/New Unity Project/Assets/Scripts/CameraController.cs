using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    Vector3 hit_vec;
    Vector3 ball_vector;

    public static Text text_temp;
    static int text_initialize = 0;

    private float[] distance_xz = new float[10]; // 공과 플레이어 사이의 거리 (평면 상에서만의 거리)
    private float[] distance_p2p_xz = new float[10];
    private Vector3[] players_direction = new Vector3[10];
    private Vector3[] player2players_direction = new Vector3[10];
    private int first;
    private static int nearest_player_number = 1;
    private static int curr_player_number = -1;
    private static int selected_player_number = 0;
    private static bool touch_player = false;

    public GameObject ball;
    public GameObject[] players = new GameObject[10];
    public GameObject button_conversion;

    public float speed;
    private Vector3 offset;

    private static float start = -1, twoClickInterval = -1;
    private float end;
    private static float sum_time = 0;
    private static bool shoot_start = false;

    // Use this for initialization

    void Start()
    {
        if (text_initialize == 0)
        {
            text_temp = GameObject.Find("TEXT_TEMP").GetComponent<Text>();
            text_initialize = 1;
        }

        // curr 붙은 게 공을 잡은 플레이어, selected 붙은게 손으로 선택한 플레이어, nearest = 지금 공과 가장 가까운 플레이어

        /*camer position setting*/
        offset = transform.position - ball.transform.position; //카메라와 공 사이의 거리


        /*get ball (공 잡는 것)*/
        for (int i = 0; i < 10; i++)
        {
            distance_xz[i] = 100.0f; //0번 플레이어와 공의 거리 초기화
            distance_p2p_xz[i] = 100.0f; //0번
            players_direction[i] = new Vector2((i < 5) ? 1.0f : -1.0f, 0.0f); //처음의 플레이어의 보고 있는 방향 초기화
            players[i].GetComponent<Rigidbody>().freezeRotation = true;
            // players_direction -> team1(0~4) -> +x / team2(5~9) -> -x
        }


        /*player movement management*/
        hit_vec = players[selected_player_number].transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /* Camera position setting */
        transform.position = ball.transform.position + offset; //카메라 포지션

        int cnt = Input.touchCount; //안드로이드 화면에서 터치를 한 개수이다. 

        /* Get ball */
        float dis_x = 0, dis_z = 0;

        for (int i = 0; i < 10; i++)
        { //반복문을 돌면서 모든 선수들과 공과의 거리 계산한다.
            dis_x = ball.transform.position.x - players[i].transform.position.x;
            dis_z = ball.transform.position.z - players[i].transform.position.z;
            distance_xz[i] = Mathf.Pow(dis_x * dis_x + dis_z * dis_z, 0.5f); // 평면상에서 공과 선수의 거리를 구한다. 

            if (players[i].GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.1f)
            {//플레이어가 제일 최근에 움직이고 있던 방향을 구한다.
                players_direction[i] = players[i].GetComponent<Rigidbody>().velocity;
                players_direction[i].y = 0.0f;
                players_direction[i] = players_direction[i].normalized;
            }
            if (i != curr_player_number && curr_player_number != -1)
            {
                dis_x = players[i].transform.position.x - players[curr_player_number].transform.position.x;
                dis_z = players[i].transform.position.z - players[curr_player_number].transform.position.z;
                distance_p2p_xz[i] = Mathf.Pow(dis_x * dis_x + dis_z * dis_z, 0.5f); // 평면상에서 선수(curr)와 선수(i)의 거리를 구한다.
                player2players_direction[i].x = dis_x;
                player2players_direction[i].y = 0.0f;
                player2players_direction[i].z = dis_z;
                player2players_direction[i] = player2players_direction[i].normalized; // curr to i direction
            }
        }

        //nearest player 구하기
        float distance_xz_min = 99999.9f;
        for (int i = 0; i < 10; i++)
        {
            if (distance_xz[i] < distance_xz_min)
            {
                distance_xz_min = distance_xz[i];
                nearest_player_number = i;
            }
        }
        text_temp.text = "n:" + nearest_player_number + "," + (int)distance_xz[nearest_player_number] +
            "/c:" + curr_player_number + ",";
        if (curr_player_number != -1)
            text_temp.text = text_temp.text + (int)distance_xz[curr_player_number];
        text_temp.text = text_temp.text + "/s" + selected_player_number + "," + (int)distance_xz[selected_player_number];
        text_temp.text = text_temp.text + "\npos:" + ball.transform.position;

        //일단 공을 잡은 플레이어는 공과의 거리가 20으로 설정되어 있다.
        //nearest player와 공과의 거리가 16.0f보다 작아지고 neareset player와 현재 공을 잡고 있던 플레이어가 다르면
        //공을 뺏도록 한다. 그 과정을 구현한 부분이다.
        if (distance_xz[nearest_player_number] < 16.0f && ball.transform.position.y <= 45.0f && curr_player_number != nearest_player_number)
        {
            if (curr_player_number == -1 && players[nearest_player_number].tag == "teamA")
            {
                selected_player_number = nearest_player_number;
            }
            curr_player_number = nearest_player_number; // 뺏는다.(=  가장 가까운 플레이어가 공 잡도록 한다.)
            
            ball_vector.x = players[curr_player_number].transform.position.x + players_direction[curr_player_number].x * 20.0f;
            ball_vector.y = ball.transform.position.y;
            ball_vector.z = players[curr_player_number].transform.position.z + players_direction[curr_player_number].z * 20.0f;
            ball.transform.position = ball_vector;
        }
        else if (curr_player_number != -1)
        { // 초기화를 -1로 해놨다. 누군가 한번이라도 공을 잡았으면 이 부분으로 들어간다.
            ball_vector.x = players[curr_player_number].transform.position.x + players_direction[curr_player_number].x * 20.0f;
            ball_vector.y = ball.transform.position.y;
            ball_vector.z = players[curr_player_number].transform.position.z + players_direction[curr_player_number].z * 20.0f;
            ball.transform.position = ball_vector;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////
        /* Movement */
        if (cnt == 1 || cnt == 0)
        {
            if (start != -1)
            {
                end = Time.time;
                twoClickInterval = end - start;
                start = -1;
                shoot_start = true;
            }
        }
        if (cnt == 1)
        { //화면을 터치한 개수가 1개이면 (우리는 선수를 이동시키므로 1번의 터치만 있으면 된다.)
            Touch touch = Input.GetTouch(0);
            Vector2 pos = touch.position; // touch한 부분의 좌표를 받는다. 

            Ray ray = Camera.main.ScreenPointToRay(pos); // 카메라에서 화면으로 쏘는 ray
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 10000f))
            { // ray에서 10000까지 쏘는 과정에 collider에 부딪히는 부분의 정보를 hitinfo로 내보낸다.

                touch_player = false;
                for (int i = 0; i < 10; i++)
                {
                    if (hitInfo.collider.gameObject.Equals(players[i]) && players[i].tag == "teamA")
                    { //부딪힌 것이 i번 플레이어이면 
                        touch_player = true;
                        selected_player_number = i; // i번 플레이어가 선택된다.
                        break;
                    }
                }
                if (!touch_player)
                { // 플레이어를 선택하지 않을 때이다. 선택되어 있는 선수를 움직일 때 사용된다.
                    hit_vec = hitInfo.point;
                    hit_vec.y = 0.0f;
                    Vector3 vec = hitInfo.point - players[selected_player_number].transform.position; // 선수로부터 터치한 방향의 벡터를 구한다.
                    float x = vec.normalized.x;
                    float z = vec.normalized.z;
                    float test = 1.0f;
                    if (z < 0.0f)
                        test = -1.0f;

                    //플레이어가 터치한 곳으로 방향을 바로바로 바꾸도록 한 부분이다.
                    //y축으로 적절한 각도만큼 회전(acos, radian, degree이용), -90.0f는 처음의 플레이어 방향 때문에 넣음
                    players[selected_player_number].transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
                    players[selected_player_number].GetComponent<Rigidbody>().freezeRotation = true; // 플레이어가 계속해서 도는 것을 방지
                    players[selected_player_number].GetComponent<Rigidbody>().velocity = vec.normalized * speed; //속도 조절
                }
            }
        }
        else if (cnt > 1 && start == -1 && curr_player_number != -1 && players[curr_player_number].tag == "teamA")
        {
            start = Time.time;
        }
        ///////////////////////////////////
        int pass_i = -1;
        if (shoot_start == true)
        {
            players[curr_player_number].GetComponent<Animator>().CrossFade("shoot", 0.0f);
            sum_time += Time.deltaTime;
        }

        if (sum_time > 0.24f)
        {
            players[curr_player_number].GetComponent<Animator>().CrossFade("run", 0.0f);
            if (twoClickInterval < 1 && twoClickInterval >= 0)
            { // pass
                float min = 99999.9f;
                for (int i = 0; i < 10; i++)
                {
                    if (Vector3.Dot(player2players_direction[i], players_direction[curr_player_number]) > Mathf.Pow(0.5f, 0.5f)
                       && curr_player_number != i
                       && min > distance_p2p_xz[i]
                        && players[curr_player_number].tag == players[i].tag)
                    { // 사이 각도가 +- 45도 이고 && 다른 사람이고 && 더 가깝다면
                        min = distance_p2p_xz[i];
                        pass_i = i;
                    }
                }
                if (pass_i == -1)
                {
                    ball.GetComponent<Rigidbody>().velocity = players_direction[curr_player_number] * speed * 2;
                    curr_player_number = -1;
                    twoClickInterval = -1;
                }
                else
                {
                    ball.GetComponent<Rigidbody>().velocity = player2players_direction[pass_i] * 350;
                    curr_player_number = -1;
                    twoClickInterval = -1;
                    pass_i = -1;
                }
            }
            else if (twoClickInterval >= 1)
            { // shoot
                Vector3 shoot_direction = players_direction[curr_player_number];
                shoot_direction.y = 0.25f;
                shoot_direction = shoot_direction.normalized;
                ball.GetComponent<Rigidbody>().velocity = shoot_direction * (200 + 100 * Mathf.Min(twoClickInterval, 2.0f));
                curr_player_number = -1;
                twoClickInterval = -1;
            }
            shoot_start = false;
            sum_time = 0;
        }
    }
}