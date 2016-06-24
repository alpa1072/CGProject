using UnityEngine;
using System.Collections;

/* 공과 골키퍼의 벡터를 구하고 골대와 공의 벡터를 구한다.
  그러고나서 골대를 기준으로 공의 위치를 파악해서 골대로부터 적당히 떨어진 위치에 골키퍼가 위치할 수 있도록 한다.
  골키퍼는 항상 공을 바라보고 있다. */

public class goalkeeper : MonoBehaviour {

    public GameObject keeper; //골키퍼 오브젝트
    public GameObject ball; // 공 오브젝트
    private Rigidbody rb;
    public float speed;
    private Vector3 target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        rb = keeper.GetComponent<Rigidbody>();

        //기준이 되는 위치이다. 이 위치와 공과의 벡터를 구해서 골키퍼의 이동과 방향을 결정한다. 
        Vector3 temp_vec;
        temp_vec.x = -30.0f; 
        temp_vec.y = 22.0f;
        temp_vec.z = 150.0f;

        Vector3 vec = (ball.transform.position - temp_vec).normalized; // 공과 골대(기준)의 벡터
        Vector3 vec2 = (ball.transform.position - transform.position); // 공과 골키퍼의 벡터
        
        
        float x = vec.normalized.x;
        vec.y = 0.0f;
        float z = vec.normalized.z;

        float x2 = vec2.normalized.x;
        vec2.y = 0.0f;
        float z2 = vec2.normalized.z;

        
        vec = vec * 50;
        target = temp_vec + vec; //골대(기준)에서 50을 곱한 벡터의 위치에 골키퍼가 항상 자리잡도록 한다. 
        
        float test = 1.0f;
        if (z2 < 0.0f)
            test = -1.0f;

        transform.rotation = Quaternion.Euler(0.0f, Mathf.Acos(-x2) * 180.0f * test / Mathf.PI - 90.0f, 0.0f);
        rb.freezeRotation = true; // 플레이어가 계속해서 도는 것을 방지
        rb.velocity = (target - transform.position).normalized * speed; //속도 조절
       
	}
}
