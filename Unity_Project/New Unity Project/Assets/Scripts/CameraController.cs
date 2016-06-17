using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Vector3 hit_vec;

	private float[] distance_xz = new float[2];
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

	// Use this for initialization
	void Start () {
		offset = transform.position - ball.transform.position;
		////////////////////////////////////////////////////////////////////////////////////////////
		distance_xz[0] = 100.0f;
		distance_xz[1] = 100.0f;
		players_direction[0] = new Vector2(1.0f, 0.0f);
		players_direction[1] = new Vector2(1.0f, 0.0f);
		// players_direction -> team1 -> +x / team2 -> -x
		curr_player_number = -1;
		////////////////////////////////////////////////////////////////////////////////////////////
		selected_player_number = 0;
		selected_player = players[0];
		rb = selected_player.GetComponent<Rigidbody>();
		hit_vec = rb.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		/* Camera position setting */
		transform.position = ball.transform.position + offset;
		////////////////////////////////////////////////////////////////////////////////////////////
		/* Get ball */
		float dis_x, dis_z;
		for (int i = 0; i < 2; i++) {
			dis_x = ball.transform.position.x - players[i].transform.position.x;
			dis_z = ball.transform.position.z - players[i].transform.position.z;
			distance_xz[i] = Mathf.Pow(dis_x * dis_x + dis_z * dis_z, 0.5f);
			if (players[i].GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.1f) {
				players_direction[i] = players[0].GetComponent<Rigidbody>().velocity;
				players_direction[i].y = 0.0f;
				players_direction[i] = players_direction[i].normalized;
			}
		}
		if (distance_xz [0] < distance_xz [1]) {
			nearest_player_number = 0;
		} else {
			nearest_player_number = 1;
		}

		if (distance_xz[nearest_player_number] < 16.0f && curr_player_number != nearest_player_number) {
			curr_player_number = nearest_player_number;
			Vector3 temp = players[curr_player_number].GetComponent<Rigidbody>().transform.position;
			temp.y = ball.transform.position.y;
			temp.x = temp.x + players_direction[curr_player_number].x * 20.0f;
			temp.z = temp.z + players_direction[curr_player_number].z * 20.0f;
			ball.transform.position = temp;
			first = 0;
		} else if (curr_player_number != -1) {
			Vector3 temp = players[curr_player_number].GetComponent<Rigidbody>().transform.position;
			temp.y = ball.transform.position.y;
			temp.x = temp.x + players_direction[curr_player_number].x * 20.0f;
			temp.z = temp.z + players_direction[curr_player_number].z * 20.0f;
			ball.transform.position = temp;
		}

		////////////////////////////////////////////////////////////////////////////////////////////
		/* Movement */
		int cnt = Input.touchCount;

		if (cnt == 1)
		{
			Touch touch = Input.GetTouch(0);
			Vector2 pos = touch.position;

			Ray ray = Camera.main.ScreenPointToRay(pos);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, 10000f))
			{
				if (hitInfo.collider.gameObject.Equals(players[0])) {
					selected_player = players[0];
					selected_player_number = 0;
					rb = selected_player.GetComponent<Rigidbody> ();
				} else if (hitInfo.collider.gameObject.Equals(players[1])) {
					selected_player = players[1];
					selected_player_number = 1;
					rb = selected_player.GetComponent<Rigidbody> ();
				} else {
					hit_vec = hitInfo.point;
					Vector3 vec = hitInfo.point - rb.transform.position;
					float x = vec.normalized.x;
					float z = vec.normalized.z;
					//moveHorizontal = x;
					//moveVertical = z;
					float test = 1.0f;
					if (z < 0.0f)
						test = -1.0f;
					rb.transform.rotation = Quaternion.Euler (0.0f, Mathf.Acos (-x) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
					rb.freezeRotation = true;
					rb.velocity = vec.normalized * speed;
				}
			}
		}
    }
}
