using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Vector3 hit_vec;

	public GameObject ball;
	public GameObject player1;
	public GameObject player2;

	public float speed;
	private Vector3 offset;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		offset = transform.position - ball.transform.position;
		////////////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////////////
		rb = player1.GetComponent<Rigidbody>();
		hit_vec = rb.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = ball.transform.position + offset;
		////////////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////////////
		int cnt = Input.touchCount;

		if (cnt == 1)
		{
			Touch touch = Input.GetTouch(0);
			Vector2 pos = touch.position;

			Ray ray = Camera.main.ScreenPointToRay(pos);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, 10000f))
			{
				if (hitInfo.collider.gameObject.Equals(player1)) {
					rb = player1.GetComponent<Rigidbody> ();
				} else if (hitInfo.collider.gameObject.Equals(player2)) {
					rb = player2.GetComponent<Rigidbody> ();
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
