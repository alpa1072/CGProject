using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	//Vector2[] touchPos = new Vector2[5];

    public float speed;
    private Rigidbody rb;
	private int width;
	private int height;

	Vector3 rayDir;

    void Start()
    {
		rb = GetComponent<Rigidbody>();
		width = Screen.width;
		height = Screen.height;
    }

    void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		//////////////////////////////////////////////////////
		int cnt = Input.touchCount;

		for (int i = 0; i < cnt; i++) {
			Touch touch = Input.GetTouch (i);
			Vector2 pos = touch.position;
			if (touch.phase == TouchPhase.Began) {
				Debug.Log (" start ");
			}
			else if (touch.phase == TouchPhase.Ended){
				Debug.Log (" end ");
				Ray ray = Camera.main.ScreenPointToRay(pos);
				rayDir = ray.direction;
				moveHorizontal = rayDir.x;
				moveVertical = rayDir.z;//(pos.y)/height;
			}
			else if (touch.phase == TouchPhase.Moved){
				Debug.Log (" moving ");
			}
		}
		//////////////////////////////////////////////////////
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rb.AddForce(movement * speed, ForceMode.Acceleration);

    }
}