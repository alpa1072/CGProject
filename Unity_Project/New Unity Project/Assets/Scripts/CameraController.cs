using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Vector3 hit_vec;

	private float[] distance_xz = new float[2]; // 공과 플레이어 사이의 거리 (평면 상에서만의 거리)
	private Vector3[] players_direction = new Vector3[2];
	private int first;
	private int nearest_player_number = 1;
	private int curr_player_number = 1;
	private int selected_player_number = 1;

	public GameObject ball;
	private GameObject curr_player; // 현재 선택한 플레이어
	private GameObject selected_player;
	public GameObject[] players = new GameObject[2];
	//public GameObject player1;
	//public GameObject player2;

	public float speed;
	private Vector3 offset;
	private Rigidbody rb;

	private float start = -1;
	private float end, interval = -1;

	// Use this for initialization

	void Start () {
        // curr 붙은 게 공을 잡은 플레이어, selected 붙은게 손으로 선택한 플레이어, nearest = 지금 공과 가장 가까운 플레이어

        /*camer position setting*/
		offset = transform.position - ball.transform.position; //카메라와 공 사이의 거리


		/*get ball (공 잡는 것)*/
		distance_xz[0] = 100.0f; //0번 플레이어와 공의 거리 초기화
        distance_xz[1] = 100.0f; //1번 플레이어와 공의 거리 초기화
		players_direction[0] = new Vector2(1.0f, 0.0f); //처음의 플레이어의 보고 있는 방향 초기화
        players_direction[1] = new Vector2(1.0f, 0.0f); //처음의 플레이어의 보고 있는 방향 초기화
		// players_direction -> team1 -> +x / team2 -> -x
		curr_player_number = -1; //현재 공 잡은 사람이 없으므로 -1로 초기화
		

        /*player movement management*/
		selected_player_number = 0; //처음에 선택된 플레이어는 0번으로 초기화
        selected_player = players[selected_player_number];  //처음에 선택된 플레이어는 0번으로 초기화

		rb = selected_player.GetComponent<Rigidbody>(); //선택된 (selected) 플레이어의 동작에 관한 변수
		hit_vec = rb.transform.position; // 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/* Camera position setting */
		transform.position = ball.transform.position + offset; //카메라 포지션

		int cnt = Input.touchCount; //안드로이드 화면에서 터치를 한 개수이다. 
		/* Get ball */
		float dis_x, dis_z;

        
		for (int i = 0; i < 2; i++) { //반복문을 돌면서 모든 선수들과 공과의 거리 계산한다.
			dis_x = ball.transform.position.x - players[i].transform.position.x;
			dis_z = ball.transform.position.z - players[i].transform.position.z;
			distance_xz[i] = Mathf.Pow(dis_x * dis_x + dis_z * dis_z, 0.5f); // 평면상에서 공과 선수의 거리를 구한다. 

			if (players[i].GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.1f) {//플레이어가 제일 최근에 움직이고 있던 방향을 구한다.
				players_direction[i] = players[i].GetComponent<Rigidbody>().velocity;
				players_direction[i].y = 0.0f;
				players_direction[i] = players_direction[i].normalized;
			}
		}

        //nearest player 구하기
		if (distance_xz [0] < distance_xz [1]) {
			nearest_player_number = 0;
		} else {
			nearest_player_number = 1;
		}

        //일단 공을 잡은 플레이어는 공과의 거리가 20으로 설정되어 있다.
        //nearest player와 공과의 거리가 16.0f보다 작아지고 neareset player와 현재 공을 잡고 있던 플레이어가 다르면
        //공을 뺏도록 한다. 그 과정을 구현한 부분이다.
		if (distance_xz[nearest_player_number] < 16.0f && curr_player_number != nearest_player_number) {
            curr_player_number = nearest_player_number; // 뺏는다.(=  가장 가까운 플레이어가 공 잡도록 한다.) 
			Vector3 temp = players[curr_player_number].GetComponent<Rigidbody>().transform.position;
			temp.y = ball.transform.position.y;
			temp.x = temp.x + players_direction[curr_player_number].x * 20.0f;
			temp.z = temp.z + players_direction[curr_player_number].z * 20.0f;
			ball.transform.position = temp;

		} else if (curr_player_number != -1) { // 초기화를 -1로 해놨다. 누군가 한번이라도 공을 잡았으면 이 부분으로 들어간다.
			Vector3 temp = players[curr_player_number].GetComponent<Rigidbody>().transform.position;
			temp.y = ball.transform.position.y;
			temp.x = temp.x + players_direction[curr_player_number].x * 20.0f;
			temp.z = temp.z + players_direction[curr_player_number].z * 20.0f;
			ball.transform.position = temp;
		}

		////////////////////////////////////////////////////////////////////////////////////////////
		/* Movement */

		if (cnt == 1) { //화면을 터치한 개수가 1개이면 (우리는 선수를 이동시키므로 1번의 터치만 있으면 된다.)
			if (start != -1) {
				end = Time.time;
				interval = end - start;
				start = -1;
			}
			Touch touch = Input.GetTouch (0);
			Vector2 pos = touch.position; // touch한 부분의 좌표를 받는다. 

			Ray ray = Camera.main.ScreenPointToRay (pos); // 카메라에서 화면으로 쏘는 ray
			RaycastHit hitInfo;

			if (Physics.Raycast (ray, out hitInfo, 10000f)) { // ray에서 10000까지 쏘는 과정에 collider에 부딪히는 부분의 정보를 hitinfo로 내보낸다.
				if (hitInfo.collider.gameObject.Equals (players [0])) { //부딪힌 것이 0번 플레이어이면 
					selected_player = players [0];
					selected_player_number = 0; // 0번 플레이어가 선택된다. 
					rb = selected_player.GetComponent<Rigidbody> ();
				} else if (hitInfo.collider.gameObject.Equals (players [1])) { // 부딪힌 것이 1번 플레이면
					selected_player = players [1];
					selected_player_number = 1; //1번 플레이어가 선택된다. 
					rb = selected_player.GetComponent<Rigidbody> ();
				} else { // 플레이어를 선택하지 않을 때이다. 선택되어 있는 선수를 움직일 때 사용된다.
					hit_vec = hitInfo.point;
					Vector3 vec = hitInfo.point - rb.transform.position; // 선수로부터 터치한 방향의 벡터를 구한다.
					float x = vec.normalized.x; //
					float z = vec.normalized.z;
					//moveHorizontal = x;
					//moveVertical = z;
					float test = 1.0f;
					if (z < 0.0f)
						test = -1.0f;

					//플레이어가 터치한 곳으로 방향을 바로바로 바꾸도록 한 부분이다.
					//y축으로 적절한 각도만큼 회전(acos, radian, degree이용), -90.0f는 처음의 플레이어 방향 때문에 넣음
					rb.transform.rotation = Quaternion.Euler (0.0f, Mathf.Acos (-x) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
					rb.freezeRotation = true; // 플레이어가 계속해서 도는 것을 방지
					rb.velocity = vec.normalized * speed; //속도 조절
				}
			}
		} else if (cnt > 1 && start == -1)
			start = Time.time;
		///////////////////////////////////
		if (interval < 1 && interval >= 0) { // pass
			ball.GetComponent<Rigidbody> ().velocity = players_direction [curr_player_number] * speed * 2;
			curr_player_number = -1;
			interval = -1;
		} else if (interval >= 1) { // shoot
		}
    }
}
