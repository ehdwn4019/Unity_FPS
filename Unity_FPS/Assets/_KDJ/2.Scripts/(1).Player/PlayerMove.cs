using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed=5.0f; //이동속도 
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        //캐릭터컨트롤러 컴포넌트 가져오기
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        //pos = (Vector3.forward * v) + (Vector3.right * h);
        //this.transform.Translate(pos * speed * Time.deltaTime);
    }

    private void move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        transform.Translate(dir * speed * Time.deltaTime);



        //카메라가 보는 방향으로 이동해야 한다 
        dir = Camera.main.transform.TransformDirection(dir);
        //transform.Translate(dir * speed * Time.deltaTime);

        //심각한 문제 : 하늘 날라다님,땅 뚫음, 충돌처리 안됨 
        controller.Move(dir*speed*Time.deltaTime);



    }
}
