using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    //Vector2[] touchPos = new Vector2[5];

	Vector3 zero_Vec3 = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 num_Vec3 = new Vector3(15.5f, 0.0f, 15.5f);
    Vector3 hit_vec;

    public GameObject ball;
    private float distance;
    private Vector3 player_direction;
	private int first = 1;

    public float speed;
    private Rigidbody rb;


    void Start()
    {
        distance = 100.0f;
        player_direction = new Vector2(1.0f, 0.0f);
        //////////////////////////////////////////////////////
        rb = GetComponent<Rigidbody>();
        hit_vec = rb.transform.position;
    }

    void FixedUpdate()
    {
        float dis_x = ball.transform.position.x - rb.transform.position.x;
        float dis_z = ball.transform.position.z - rb.transform.position.z;
        distance = Mathf.Pow(dis_x * dis_x + dis_z * dis_z, 0.5f);

        if (rb.velocity.sqrMagnitude > 0.1f)
        {
            player_direction = rb.velocity;
            player_direction.y = 0.0f;
            player_direction = player_direction.normalized;
        }

		if (first == 1 && distance < 16.0f) {
			Vector3 temp = rb.transform.position;
			temp.y = ball.transform.position.y;
			temp.x = temp.x + player_direction.x * 20.0f;
			temp.z = temp.z + player_direction.z * 20.0f;
			ball.transform.position = temp;
			first = 0;
		} else if (first != 1) {
			Vector3 temp = rb.transform.position;
			temp.y = ball.transform.position.y;
			temp.x = temp.x + player_direction.x * 20.0f;
			temp.z = temp.z + player_direction.z * 20.0f;
			ball.transform.position = temp;
		}

        //////////////////////////////////////////////////////
        /*int cnt = Input.touchCount;

        if (cnt == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 pos = touch.position;

            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 10000f))
            {
                hit_vec = hitInfo.point;
                Vector3 vec = hitInfo.point - rb.transform.position;
                float x = vec.normalized.x;
                float z = vec.normalized.z;
                //moveHorizontal = x;
                //moveVertical = z;
                float test = 1.0f;
                if (z < 0.0f)
                    test = -1.0f;
                rb.transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
                rb.freezeRotation = true;
				rb.velocity = vec.normalized * speed;
            }
        }*/
        /*Vector3 temp_vec = hit_vec - rb.transform.position;
        if (temp_vec.x <= 1.0f && temp_vec.x >= -1.0f && temp_vec.z <= 1.0f && temp_vec.z >= -1.0f) {
            rb.velocity = zero_Vec3;
        }*/
        // 위의 주석 코드 : 찍은 위치 가면 멈추는 코드
        //////////////////////////////////////////////////////
        /*Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rb.AddForce(movement * 0, ForceMode.VelocityChange);
        rb.AddForce(movement * speed, ForceMode.VelocityChange);*/
    }
}