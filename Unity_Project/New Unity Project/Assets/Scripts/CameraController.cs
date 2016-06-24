using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    Vector3 hit_vec;
    Vector3 ball_vector;

    private float[] distance_xz = new float[10]; // 공과 플레이어 사이의 거리 (평면 상에서만의 거리)
    private float[] distance_p2p_xz = new float[10]; // 플레이어와 플레이어 사이의 거리 (평면거리)
    private Vector3[] players_direction = new Vector3[10]; // 플레이어들의 현재 달리는 방향을 저장할 배열
    private Vector3[] player2players_direction = new Vector3[10]; // 현재 플레이어에서 다른 플레이어를 향하는 방향
    private int first;
    public static float distance_xz_min_TeamA = 99999.9f; // A Team player 중에서 공과 가장 가까운  사람의 거리를 저장하기 위한 변수
    // public static은 다른 스크립트 파일에서 이 값에 접근할 수 있도록 하기 위해서 붙임
    /*  
        - player 구분 -
        공과 가장 가까운 사람 : nearest, 
        현재 공을 가지고 있는 사람 : curr, 
        현재 조종할 수 있도록 선택되어 있는 사람 : selected
    */
    private int nearest_player_number = 1;
    public static int nearest_player_number_TeamA = 5; // 공과 가장 가까운 A팀 플레이어
    public static int nearest_player_number_TeamB = 0; // 공과 가장 가까운 B팀 플레이어
    public static int curr_player_number = -1;
    public static int selected_player_number = 0;
    private static bool touch_player = false; // 터치한 것이 플레이어인지 판단하는 변수

    public GameObject ball; // 축구공 GameObject
    public GameObject[] players = new GameObject[10]; // 플레이어 10명을 담고있는 GameObject배열

    public float speed; // 플레이어들의 달리기 속력
    private Vector3 offset; // 카메라와 공 사이의 거리 조절
    // 2 finger click 을 하면 슛이나 패스를 하는데 이를 결정하기 위한 변수들이다.
    private float start = -1, twoClickInterval = -1;
    private float end;

    // 슛 모션이 0.240초 인데 이 모션을 실행한 후에 슛이나 패스를 시작한다. 이 시간을 측정하기 위한 변수들이다.
    private static float sum_time = 0;
    private static bool shoot_start = false;

    void Start()
    {


        // curr 붙은 게 공을 잡은 플레이어, selected 붙은게 손으로 선택한 플레이어, nearest = 지금 공과 가장 가까운 플레이어

        /*camera position setting*/
        offset = transform.position - ball.transform.position; //카메라와 공 사이의 거리


        /*get ball (공 잡는 것)*/
        for (int i = 0; i < 10; i++)
        {
            distance_xz[i] = 100.0f; //0번 플레이어와 공의 거리 초기화
            distance_p2p_xz[i] = 100.0f; //0번
            players_direction[i] = new Vector2((i < 5) ? 1.0f : -1.0f, 0.0f); //처음의 플레이어의 보고 있는 방향 초기화
            // players_direction -> team1(0~4) -> +x / team2(5~9) -> -x
            players[i].GetComponent<Rigidbody>().freezeRotation = true; // 플레이어끼리 부딪혔을때 플레이어가 회전하지 않도록 freeze.
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
            distance_xz[i] = Mathf.Pow(dis_x * dis_x + dis_z * dis_z, 0.5f); // 평면상에서 공과 모든 선수들의 거리를 구한다.

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
                player2players_direction[i] = player2players_direction[i].normalized; // 현재 플레이어로부터 플레이어[i]로의 direction
            }
        }

        //nearest player 구하기 (for all, for teamA, for teamB)
        float distance_xz_min = 99999.9f;
        distance_xz_min_TeamA = 99999.9f;
        float distance_xz_min_TeamB = 99999.9f;
        for (int i = 0; i < 10; i++)
        {
            if (distance_xz[i] < distance_xz_min)
            {
                distance_xz_min = distance_xz[i];
                nearest_player_number = i;
            }
            if (distance_xz[i] < distance_xz_min_TeamA && i < 5)
            {
                distance_xz_min_TeamA = distance_xz[i];
                nearest_player_number_TeamA = i;
            }
            if (distance_xz[i] < distance_xz_min_TeamB && i >= 5)
            {
                distance_xz_min_TeamB = distance_xz[i];
                nearest_player_number_TeamB = i;
            }
        }



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
        //16이면 공을 잡도록 하는데 잡고나면 거리를 20으로 늘리기 때문에 버벅거리는 것처럼 보이게 된다. 이를 해결하기 위해 이 부분을 추가하였다.
        else if (curr_player_number != -1)
        { // 초기화를 -1로 해놨다. 누군가 공을 잡은 상태라면 이 부분으로 들어간다.
            ball_vector.x = players[curr_player_number].transform.position.x + players_direction[curr_player_number].x * 20.0f;
            ball_vector.y = ball.transform.position.y;
            ball_vector.z = players[curr_player_number].transform.position.z + players_direction[curr_player_number].z * 20.0f;
            ball.transform.position = ball_vector;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////
        /* Movement */
        if (cnt == 1 || cnt == 0) // 한손가락만 터치하고 있거나 터치하고 있지 않는 경우
        {
            if (start != -1) // 터치가 2개 이상이었다가 1개 이하로 떨어진 경우이다. 이 경우에는 두손가락터치 시간 계산을 하여 슛과 패스를 결정한다.
            {
                end = Time.time;
                twoClickInterval = end - start; // 두손가락 터치를 시작한 시간과 두손가락에서  한손가락 이하로 바뀐 시간의 차이를 저장해서 판단한다.
                start = -1;
                shoot_start = true; // 슛이나 패스를 시작한다는 뜻
            }
        }
        if (cnt == 1)
        { //화면을 터치한 개수가 1개이면 선수선택을 하거나 이동을 수행
            Touch touch = Input.GetTouch(0);
            Vector2 pos = touch.position; // touch한 부분의 좌표를 받는다. 

            Ray ray = Camera.main.ScreenPointToRay(pos); // 카메라에서 화면으로 쏘는 ray
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 10000f))
            { // ray에서 10000까지 쏘는 과정에 collider에 부딪히는 부분의 정보를 hitinfo로 내보낸다.

                touch_player = false; // 초기화
                for (int i = 0; i < 10; i++)
                {
                    if (hitInfo.collider.gameObject.Equals(players[i]) && players[i].tag == "teamA")
                    { //부딪힌 것이 i번 플레이어이면 
                        touch_player = true; // 플레이어를 터치했다고 표시한 후
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
                    //xz평면에서 보는 방향을 바꾸는 것이므로 y축으로만 회전하면 된다.
                    players[selected_player_number].transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
                    players[selected_player_number].GetComponent<Rigidbody>().freezeRotation = true; // 플레이어가 계속해서 도는 것을 방지
                    players[selected_player_number].GetComponent<Rigidbody>().velocity = vec.normalized * speed; //속도 조절
                }
            }
        }
        else if (cnt > 1 && start == -1 && curr_player_number != -1 && players[curr_player_number].tag == "teamA")
        { // 터치가 2개 이상이 되고 && 2 finger touch 의 시간측정을 시작하지 않았고 && 누군가가 공을 잡고 있고 && 공을 잡은 사람이 나의 팀일 때
            start = Time.time; // 2 finger touch 시간 측정을 시작한다.
        }
        ///////////////////////////////////
        int pass_i = -1; // 초기화
        if (shoot_start == true) // 슛이나 패스를 하는 것으로 결정되었다면
        {
            players[curr_player_number].GetComponent<Animator>().CrossFade("shoot", 0.0f); // 슛 모션을 시작한다.
            sum_time += Time.deltaTime;
            // Time.deltaTime은 프레임과 프레임 사이의 시간이다. 이를 모두 더해서 지나간 시간이 얼마인지 계산한다.
            // 이 시간이 0.24초가 넘어가면 실제 슛이나 패스를 시작하고 run 모션으로 바꿀 것이다.
        }

        if (sum_time > 0.24f) // shoot 의 모션의 길이가 0.24초이다. 이 모션을 한번만 하고 슛이나 패스를 실행하기 위해 sum_time을 사용.
        { // 슛 모션을 다 했으면 자신은 run 모션으로 돌아가고 실제 공을 슛이나 패스로 이동시킨다.
            // 슛 모션을 실행하는 0.24초 동안 터치를 통해 보는 방향을 바꾸면 그 방향으로 슛이나 패스를 하도록 방향조절이 가능하다.
            players[curr_player_number].GetComponent<Animator>().CrossFade("run", 0.0f); // run 모션을 시작한다.
            if (twoClickInterval < 1 && twoClickInterval >= 0)
            { // 2 finger click이 1초 미만이면 pass 를 함.
                float min = 99999.9f; // 초기화
                for (int i = 0; i < 10; i++)
                {
                    if (Vector3.Dot(player2players_direction[i], players_direction[curr_player_number]) > Mathf.Pow(0.5f, 0.5f)
                       && curr_player_number != i
                       && min > distance_p2p_xz[i]
                       && players[curr_player_number].tag == players[i].tag)
                    { // 사이 각도가 +- 45도 이고 && 패스해줄 대상이 다른 사람이고 && 제일 가깝고 && 공을 잡고있는 자신과 같은 팀이라면
                        min = distance_p2p_xz[i];
                        pass_i = i; // 패스해줄 대상으로 정한다.
                    }
                }
                if (pass_i == -1) // 패스해줄 대상이 없다면(+-45도 사이에 우리팀이 없다면) 현재 보고있는 방향으로 스루패스한다.
                {
                    ball.GetComponent<Rigidbody>().velocity = players_direction[curr_player_number] * speed * 2; // 현재 달리던 방향으로 (기본 플레이어 속력 * 2)만큼의 속력으로 공을 패스한다.
                    curr_player_number = -1; // 공이 자신을 떠났으므로 현재 공을 잡고있는 사람이 없어 -1로 바꾸어 준다.
                    twoClickInterval = -1; // 인터벌 사용이 끝났으므로 초기화해준다.
                }
                else // 아니면(패스해줄 대상이 있다면) 그 대상에게 패스한다.
                {
                    ball.GetComponent<Rigidbody>().velocity = player2players_direction[pass_i] * 350; // 자신으로부터 패스할 대상이 있는 방향으로 350의 속력으로(기본플레이어 속력 : 70) 패스한다.
                    curr_player_number = -1; // 내가 패스한 이후 패스받을 대상이 공을 받기 전까지 공을 잡고있는 사람이 없으므로 -1로 바꿔준다.
                    twoClickInterval = -1; // 인터벌 사용이 끝났으므로 초기화해준다.
                    pass_i = -1; // 패스할 대상도 초기화 해준다.
                }
            }
            else if (twoClickInterval >= 1)
            { // 2 finger click이 1초 이상이면 shoot 을 함.
                Vector3 shoot_direction = players_direction[curr_player_number]; // 현재 달리던 방향으로 슛을 진행한다.
                shoot_direction.y = 0.25f; // 위에서 direction 이 normalize 되어있으므로 각도는 arctan(0.25/1.0)이 된다.
                shoot_direction = shoot_direction.normalized; // 이 벡터 normalize
                ball.GetComponent<Rigidbody>().velocity = shoot_direction * (200 + 100 * Mathf.Min(twoClickInterval, 2.0f)); // 누르고 있는 시간에 비례해서(최대 2초) 슛을 강하게 한다.
                curr_player_number = -1; // 공이 자신을 떠났으므로 현재 공을 잡고있는 사람이 없어 -1로 바꾸어 준다.
                twoClickInterval = -1; // 인터벌 사용이 끝났으므로 초기화해준다.
            }
            shoot_start = false; // 슛을 완료했으므로 초기화 해준다.
            sum_time = 0;
        }
    }
}