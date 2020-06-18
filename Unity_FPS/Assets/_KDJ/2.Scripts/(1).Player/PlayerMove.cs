using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed=5.0f; //이동속도 
    CharacterController controller;


    public float gravity = -20;
    float velocityY;//낙하속도 (벨로시티는 방향과 힘을 들고 있다)
    float jumpPower=10.0f;
    int jumpCount = 0;

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
        //Vector3 dir = new Vector3(h, 0, v);
        //dir.Normalize();
        //transform.Translate(dir * speed * Time.deltaTime);



        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        //카메라가 보는 방향으로 이동해야 한다 
        dir = Camera.main.transform.TransformDirection(dir);
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        //심각한 문제 : 하늘 날라다님,땅 뚫음, 충돌처리 안됨 
        //controller.Move(dir*speed*Time.deltaTime);

        //캐릭터 점프 
        //점프버튼을 누르면 수직속도에 점프파워를 넣는다 
        //땅에 닿으면 0으로 초기화
        //if(controller.isGrounded)//땅에 닿았냐
        //{
        //
        //
        //}
        //CollisionFlags.Above;
        //CollisionFlags.Below;
        //CollisionFlags.Sides;
        if(controller.collisionFlags==CollisionFlags.Below)//땅에 닿았냐
        {
            velocityY = 0;
            jumpCount = 0;
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }
        if(Input.GetButtonDown("Jump") && jumpCount<2)
        {
            velocityY = jumpPower;
            jumpCount++;
        }

        controller.Move(dir * speed * Time.deltaTime);
    }
}
