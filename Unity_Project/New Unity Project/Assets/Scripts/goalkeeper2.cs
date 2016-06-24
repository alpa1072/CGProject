using UnityEngine;
using System.Collections;

/* 공과 골키퍼의 벡터를 구하고 골대와 공의 벡터를 구한다.
  그러고나서 골대를 기준으로 공의 위치를 파악해서 골대로부터 적당히 떨어진 위치에 골키퍼가 위치할 수 있도록 한다.
  골키퍼는 항상 공을 바라보고 있다. */

public class goalkeeper2 : MonoBehaviour
{

    public GameObject keeper2; // 반대 편 골키퍼 오브젝트 
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
        temp_vec.x = 530.0f; // 기준이 되는 골대의 위치이다.
        temp_vec.y = 22.0f;
        temp_vec.z = 150.0f;

        Vector3 vec = (ball2.transform.position - temp_vec).normalized; // 공과 골대의 벡터
        Vector3 vec2 = (ball2.transform.position - transform.position); // 공과 골키퍼의 벡터

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
