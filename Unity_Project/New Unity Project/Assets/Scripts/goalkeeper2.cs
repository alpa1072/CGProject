using UnityEngine;
using System.Collections;

public class goalkeeper2 : MonoBehaviour
{

    public GameObject keeper2;
    public GameObject ball2;
    private Rigidbody rb2;
    public float speed2;
    private Vector3 target;
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        rb2 = keeper2.GetComponent<Rigidbody>();
        Vector3 temp_vec;
        temp_vec.x = 530.0f;
        temp_vec.y = 22.0f;
        temp_vec.z = 150.0f;
        Vector3 vec = (ball2.transform.position - temp_vec).normalized;
        Vector3 vec2 = (ball2.transform.position - transform.position);

        float x = vec.normalized.x;
        vec.y = 0.0f;
        float z = vec.normalized.z;

        float x2 = vec2.normalized.x;
        vec2.y = 0.0f;
        float z2 = vec2.normalized.z;

        vec = vec * 50;
        target = temp_vec + vec;

        float test = 1.0f;
        if (z2 < 0.0f)
            test = -1.0f;

        transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x2) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
        rb2.freezeRotation = true; // 플레이어가 계속해서 도는 것을 방지
        rb2.velocity = (target - transform.position).normalized * speed2; //속도 조절
       
    }
}
