using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject ball;
    public int speed;
    private int my_number;
    private int random_time = 0;
    private int random_x;
    private int random_z;
    private int random_speed;
    private int frequency_z = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (player.name == "SoccerPlayer" + (i + 1))
            { // 자신의 number를 찾는 부분
                my_number = i;
                break;
            }
        }
        if (my_number >= 5)
        { // 5~9가 컴퓨터 팀이므로 컴퓨터라면
            if ((int)Time.time != random_time)
            { // 1초마다 랜덤으로 업데이트한다.
                Vector3 vec_temp;
                int cnt = 0;
                do
                { // 랜덤하게 움직이기 위해 설정하는 부분
                    random_x = Random.Range(-100, 100);
                    random_z = Random.Range(-100, 100);
                    random_speed = Random.Range(35, 50);
                    vec_temp = new Vector3(random_x, 0.0f, random_z).normalized;
                    cnt++;
                    if (cnt > 100) break;
                } while ((my_number != CameraController.curr_player_number) &&
                    (ball.transform.position - (player.transform.position + vec_temp * 70.0f)).magnitude > 70.0f);
                random_time = (int)Time.time;
                frequency_z *= -1; // 지그재그로 움직이기 위해서 1초마다 z방향을 바꾸는 부분
            }
            Vector3 vec = new Vector3(random_x, 0.0f, random_z); // 플레이어가 움직일 방향을 저장하는 변수
            float ball_x = ball.transform.position.x;
            float ball_z = ball.transform.position.z;

            if (CameraController.curr_player_number < 5 || CameraController.curr_player_number == -1)
            { // 공이 상대팀에게 있거나 공을 아무도 잡고있지 않으면 공을향해서 움직임
                if (CameraController.nearest_player_number_TeamB == my_number)
                { // 자신이 자신의 팀 중에서 공과 제일 가깝다면
                    vec = ball.transform.position - player.transform.position; // 공의 방향으로 움직인다.
                    vec.y = 0.0f;
                }
                player.GetComponent<Rigidbody>().velocity = vec.normalized * random_speed;
            }
            else
            { // 공이 자기팀에게 있으면
                if (my_number == CameraController.curr_player_number)
                { // 내가 공을 잡고 있다면
                    if (ball.transform.position.x < 100.0f/* || CameraController.distance_xz_min_TeamA < 30.0f*/)
                    { // 조건이 맞으면 슛하는 지점
                        // 골대와 적당히 가까워지면 슛을 한다.
                        // 위의 조건문에서 주석부분은 상대팀이 30만큼 가까워져도 슛을 하도록 하는 부분이다.
                        // 이 부분을 넣으면 너무 어려워져서 주석처리 하였다.
                        // 난이도를 높이려면 이 부분을 슛 뿐만 아니라 패스를 하도록 하던지 방향을 인식해서 피하도록 할 수도 있을 것이다.
                        Vector3 goal = new Vector3(-30.0f, 0.0f, 150.0f); // 골대의 위치
                        Vector3 shoot_direction = (goal - ball.transform.position).normalized; // 자신이 골대로 슛을 할 방향
                        shoot_direction.y = 0.25f; // CameraController와 마찬가지로 어느정도 띄우는 부분
                        shoot_direction = shoot_direction.normalized; // 방향이므로 normalize
                        ball.GetComponent<Rigidbody>().velocity = shoot_direction * (400); // 400의 세기로 슛
                        CameraController.curr_player_number = -1; // 슛을 했으므로 curr를 -1로 해준다.
                    }
                    else
                    { // 그 외 일반적인 상황이라면 지그재그로 전진한다.
                        vec.x = -1.0f; // 상대팀 골대방향
                        vec.y = 0.0f;
                        vec.z = frequency_z; // 1초마다 +-1 바뀜
                    }
                }
                player.GetComponent<Rigidbody>().velocity = vec.normalized * speed; // 위에서 정한 방향으로 이동
            }

        }
    }
}