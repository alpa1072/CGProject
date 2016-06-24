using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour
{
    void OnTriggerEnter(Collider col) // trigger가 발생하면 (충돌하면)
    {
        if (col.gameObject.tag == "teamA" || col.gameObject.tag == "teamB") // 태그를 파악한다. 
        {
            //현재 속도를 이용해서 임시 벡터에 반대편으로 50만큼의 속도를 곱한 속도를 저장
            Vector3 vec = col.gameObject.GetComponent<Rigidbody>().velocity;
            vec.x = vec.x * -50;
            vec.z = vec.z * -50;
            vec.y = 0f;

            //위 벡터를 다시 속도로 저장
            //충돌하면 선수 튕겨나가도록 구현.
            col.gameObject.GetComponent<Rigidbody>().velocity = vec;
            
            //선수와 충돌하면 계속 랜덤으로 폭탄 생성(운동장의 좌표 안에서 생성되도록 한다.)
            this.gameObject.transform.position = new Vector3(Random.Range(20,480), 250,Random.Range(30,270));
        }
    }
}
