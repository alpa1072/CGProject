using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    //Vector2[] touchPos = new Vector2[5];

    public float speed;
    private Rigidbody rb;
    private int width;
    private int height;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        width = Screen.width;
        height = Screen.height;
    }

    void Update()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //////////////////////////////////////////////////////
        int cnt = Input.touchCount;

        if (cnt == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 pos = touch.position;

            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 10000f))
            {
                Vector3 vec = hitInfo.point - rb.transform.position;
                float x = vec.normalized.x;
                float z = vec.normalized.z;
                moveHorizontal = x;
                moveVertical = z;
                float test = 1.0f;
                if (z < 0.0f)
                    test = -1.0f;
                rb.transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x) * 180.0f * test / Mathf.PI, 0.0f);
                rb.freezeRotation = true;
            }
        }
        //////////////////////////////////////////////////////
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed, ForceMode.VelocityChange);
    }
}