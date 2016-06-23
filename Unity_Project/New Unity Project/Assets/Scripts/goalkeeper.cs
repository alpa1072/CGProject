using UnityEngine;
using System.Collections;

public class goalkeeper : MonoBehaviour {

    public GameObject keeper;
    public GameObject ball;

    private float kx;
    private float kz;
    private float bx;
    private float bz;
    private Rigidbody rb;
    public float speed;

    private float temp_x = 530.0f;

	// Use this for initialization
	void Start () {
        rb = keeper.GetComponent<Rigidbody>();
        Vector3 temp_vec;
        temp_vec.x = 530.0f;
        temp_vec.y = 22.0f;
        temp_vec.z = 150.0f;
        Vector3 vec = ball.transform.position - temp_vec;
        float x = vec.normalized.x;
        float z = vec.normalized.z;

        float a = (ball.transform.position.z-temp_vec.z)/(ball.transform.position.x - temp_vec.x);
        float b = temp_vec.z - a*temp_vec.x;

        float t = (temp_vec.z - b);
        Vector3 vec1;
        vec1.x = t;
        vec1.y = 22.0f;
        vec1.z = 150.0f;

        rb.transform.position = vec1;
        float test = 1.0f;
        if (z < 0.0f)
            test = -1.0f;

        rb.transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
        rb.freezeRotation = true; // 플레이어가 계속해서 도는 것을 방지
       rb.velocity = vec.normalized * speed; //속도 조절
	}
	
	// Update is called once per frame
	void Update () {
        rb = keeper.GetComponent<Rigidbody>();
        Vector3 temp_vec;
        temp_vec.x = 530.0f;
        temp_vec.y = 22.0f;
        temp_vec.z = 150.0f;
        Vector3 vec = ball.transform.position - temp_vec;
        float x = vec.normalized.x;
        float z = vec.normalized.z;

        float a = (ball.transform.position.z - temp_vec.z) / (ball.transform.position.x - temp_vec.x);
        float b = temp_vec.z - a * temp_vec.x;

        float t = (temp_vec.z - b);
        t = (float) t /  (float) a;
        
        Vector3 vec1;
        vec1.x = t;
        vec1.y = 22.0f;
        vec1.z = 150.0f;
        //this.rb.transform.position = vec1;
        rb.position = vec1;
        float test = 1.0f;
        if (z < 0.0f)
            test = -1.0f;

        transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
        rb.freezeRotation = true; // 플레이어가 계속해서 도는 것을 방지
        rb.velocity = vec.normalized * speed; //속도 조절
	}
}
