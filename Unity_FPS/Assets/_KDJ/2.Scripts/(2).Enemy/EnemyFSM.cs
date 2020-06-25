//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//몬스터 유한상태 머신 
public class EnemyFSM : MonoBehaviour
{
    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idle,Move,Attack,Return,Damaged,Die,Skill1,Skill2
    }

    EnemyState state; //몬스터 상태 변수 
    public float enemySpeed=5.0f;

    /// <summary>
    /// 유용한 기능  슬래쉬 3번 하면 안에있는 내용은 자동 주석처리
    /// </summary>

    //유용한 기능 2 
    #region "Idle 상태에 필요한 변수들"
    //GameObject obj;
    #endregion
    #region "Move 상태에 필요한 변수들"
    //CharacterController controller;
    #endregion
    #region "Attack 상태에 필요한 변수들"
    //int damage;
    #endregion
    #region "Return 상태에 필요한 변수들"
    //public GameObject basicPt;
    #endregion
    #region "Damaged 상태에 필요한 변수들"
    //int enemyHp = 30;
    #endregion
    #region "Die 상태에 필요한 변수들"
    #endregion

    ///필요한 변수들 
    public float findRange = 15f;//플레이어를 찾는 범위
    public float moveRange = 30f;//시작지점에서 최대 이동 가능한 범위
    public float attackRange = 2f; //공격 가능 범위
    Vector3 startPoint;//몬스터 시작위치
    Quaternion startRotation; //몬스터 시작위치 
    Transform player;//플레이어를 찾기위해(안그러면 모든 몬스터에 다 드래그앤드랍 해줘야 한다 그냥 코드로 찾아서 처리하기)
    //CharacterController cc;//몬스터 이동을 위해 캐릭터컨트롤러 컴포넌트가 필요 

    //애니메이션을 제어하기 위한 애니메이터 컴포넌트 
    Animator anim;

    //네비게이션 
    NavMeshAgent nav;

    //몬스터 일반변수
    int hp = 100;//체력
    int att = 5;//공격력
    float speed = 5.0f;//이동속도

    //공격 딜레이 
    float attTime = 2f;//2초에 한번 공격
    float timer = 0f; //타이머 

    // Start is called before the first frame update
    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;
        //시작지점 저장
        startPoint = transform.position;
        //startRotation = transform.rotation;
        //플레이어 트랜스폼 컴포넌트
        player = GameObject.Find("Player").transform;
        //캐릭터 컨트롤러 컴포넌트 
        //cc = GetComponent<CharacterController>();
        //애니메이터 컴포넌트
        anim = GetComponentInChildren<Animator>();
        //네비게이션 컴포넌트
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
            default:
                break;
        }

    }//end of void Update

    private void Idle()
    {
        //Debug.Log("Idle상태");

        //1. 플레이어와 일정범위가 되면 이동상태로 변경 (탐지범위)
        //-  플레이어 찾기 (GameObject.Find("Player"))
        //obj = GameObject.Find("Player");
        //-  일정거리 20미터 (거리비교 : Distance,magnitude 아무거나) 
        //if(Vector3.Distance(obj.transform.position,this.transform.position)<=20)
        //{
        //    //-  상태 변경 
        //    state = EnemyState.Move;
        //    //-  상태전환 출력 
        //    Debug.Log("Move상태");
        //
        //}

        //magnitude 사용법
        //magnitude는 normalize 시키면 안된다 1이 되기 때문에 
        //distance는 vector3값이고 magnitude는 float 값이다 
        //추가 : 곱셈보다는 나눗셈이더 효율적이다 ex ) 어떤수에 나누기 2보다 0.5곱하는게 성능향상
        //Vector3 dir = (transform.position - player.position);
        //float distance = dir.magnitude;

        //1. 플레이어와 일정범위가 되면 이동상태로 변경 (탐지범위)
        
        //if(dir.magnitude<findRange)
        //if(distance<findRange)
        if(Vector3.Distance(transform.position,player.position)<findRange)
        {
            state = EnemyState.Move;
            print("상태전환 : Idle -> Move");

            //애니메이션
            anim.SetTrigger("Move");
        }




    }

    private void Move()
    {
        //controller = GetComponent<CharacterController>();
        ////1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경 
        ////2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경 
        ////- 플레이어 처럼 캐릭터컨트롤러를 이용 하기
        //
        //this.transform.LookAt(obj.transform);
        //Vector3 dir = this.transform.forward;
        //
        //float distance = Vector3.Distance(obj.transform.position, transform.position);
        //
        //controller.Move(dir.normalized * enemySpeed * Time.deltaTime);
        //
        ////- 공격범위 1미터 
        //if (distance <= 2.0f)
        //{
        //    this.transform.position = this.transform.position;
        //
        //    //- 상태변경
        //    state = EnemyState.Attack;
        //    //- 상태전환 출력 
        //    Debug.Log("Attack상태");
        //}

        //이동중 이동할 수 있는 최대범위에 들어왔을떄 
        if(Vector3.Distance(transform.position,startPoint)>moveRange)
        {
            state = EnemyState.Return;
            print("상태전환 : Move -> Return");
            anim.SetTrigger("Return");
        }
        //리턴상태가 아니면 플레이어를 추격해야 한다 
        else if(Vector3.Distance(transform.position,player.position)>attackRange)
        {
            nav.SetDestination(player.position);
            //플레이어를 추격
            //이동방향 (벡터의 뺄셈)
            //Vector3 dir = (player.position - transform.position).normalized;
            //캐릭터 컨트롤러를 이용해서 이동하기
            //cc.Move(dir * speed * Time.deltaTime);
            //몬스터가 벡스텝으로 쫒아온다
            //몬스터가 타겟을 바라보도록 하자 
            //방법1
            //transform.forward = dir;
            //방법2
            //transform.LookAt(player);

            //좀더 자연스럽게 회전처리를 하고 싶다
            //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
            //여기도 문제가 있다 지금 회전처리를 하면서 벡터의 러프를 사용한 상태라서 
            //타겟과 본인이 일직선상일경우 백덤블링으로 회전을 한다 

            //최종적으로 자연스런 회전처리를 하려면 결국 쿼터니온을 사용해야한다 
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

            //캐릭터 컨트롤러를 이용해서 이동하기
            //cc.Move(dir * speed * Time.deltaTime);
            //중력이 적용안되는 문제가 있다 
            
            //중력문제를 해결하기 위해서 심플무브를 사용한다 (충돌,중력이 내부적으로 들어가 있다)
            //심플무브는 최소한의 물리가 적용되어 중력문제를 해결할 수 있다
            //단 내부적으로 시간처리를 하기 때문에 Time.deltaTime을 사용하지 않는다 
            //cc.SimpleMove(dir * speed);
        }
        else //공격범위 안에 들어옴
        {
            state = EnemyState.Attack;
            print("상태전환 : Move -> Attack");
            anim.SetTrigger("Attack");
        }

    }

    private void Attack()
    {
        
        //damage = Random.Range(1, 5);
        //if(state==EnemyState.Attack)
        //{
        //    Debug.Log("데미지가" + damage + "만큼 들어갔습니다");
        //}
        ////1. 플레이어가 공격범위 안에 있따면 일정한 시간 간격으로 플레이어 공격 
        ////2. 플레이어가 공격범위를 벗어나면 이동상태(재추격)로 변경 
        ////- 공격범위 1미터 
        //if (Vector3.Distance(obj.transform.position,this.transform.position)>2.0f)
        //{
        //    //- 상태변경
        //    state = EnemyState.Move;
        //
        //    //- 상태전환 출력
        //    //StopAllCoroutines();
        //    
        //}

        //공격범위안에 들어옴
        if(Vector3.Distance(transform.position,player.position)<attackRange)
        {
            //일정 시간마다 플레이어를 공격하기 
            timer += Time.deltaTime;
            if(timer>attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트 컴포넌트를 가져와서 데미지를 주면 된다 
                //player.GetComponent<PlayerMove>().hitDamage(att);
                //타이머 초기화
                timer = 0f;
            }

        }
        else //현재상태를 무브로 전환하기 (재추격)
        {
            state = EnemyState.Move;
            print("상태전환 : Attack -> Move");
            //타이머 초기화
            timer = 0f;
            anim.SetTrigger("Move");
        }
    }

    private void Return()
    {
        

        //basicPt = GetComponent<GameObject>();
        ////1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴 
        ////- 처음위치에서 일정범위 30미터 
        //if(Vector3.Distance(this.transform.position,basicPt.transform.position)>=30.0f)
        //{
        //    //-상태 전환 출력 (트랜지션?)
        //    Debug.Log("Return상태");
        //    this.transform.position = basicPt.transform.position*enemySpeed*Time.deltaTime;
        //
        //    if(Vector3.Distance(this.transform.position,basicPt.transform.position)<=0.01f)
        //    {
        //        //- 상태변경 
        //        state = EnemyState.Idle;
        //    }
        //   
        //    
        //
        //}

        //시작위치까지 도달하지 않을떄는 이동
        //도착하면 대기상태로 변경
        if(Vector3.Distance(transform.position,startPoint)>0.1)
        {
            //Vector3 dir = (startPoint - transform.position).normalized;
            //cc.SimpleMove(dir * speed);
        }
        else
        {
            //위치값을 초기값으로 
            transform.position = startPoint;
            transform.rotation = Quaternion.identity;//startRotation;
            //Quaternion.identity => 쿼터니온 회전값을 0으로 초기화시켜준다.

            state = EnemyState.Idle;
            print("상태전환 : Return -> Idle");
            anim.SetTrigger("Idle");
        }
    }

    //플레이어쪽에서 충돌감지를 할 수 있으니 이함수는 퍼블릭으로 만들자 
    public void hitDamage(int value)
    {
        //예외처리 
        //피격상태이거나,죽은 상태일때는 데미지를 중첩으로 주지 않는다
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        //체력깍기
        hp -= value;

        //몬스터의 체력이 1이상이면 피격상태
        if(hp>0)
        {
            state = EnemyState.Damaged;
            print("상태전환 : AnyState -> Damaged");
            print("HP : " + hp);
            anim.SetTrigger("Damaged");
            Damaged();
        }
        //0이하이면 죽음 상태 
        else
        {
            state = EnemyState.Die;
            print("상태전환 : AnyState -> Die");
            anim.SetTrigger("Die");
            Die();
        }


    }

    //피격상태 (Any State)
    private void Damaged()
    {
        ////- 상태전환 출력 
        //Debug.Log("Damaged상태");
        //
        ////코루틴을 사용하자 
        ////1. 몬스터 체력이 1이상 
        ////2. 다시 이전상태로 변경
        //
        //
        //
        //StartCoroutine(DAMAGED());
        ////- 상태변경 
        //if (enemyHp<=0)
        //{
        //    state = EnemyState.Die;
        //}

        //피격 상태를 처리하기 위한 코루틴을 실행한다 
        StartCoroutine(DamageProc());
    }

    //피격상태 처리용 코루틴
    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기 
        yield return new WaitForSeconds(1.0f);
        //현재상태 
        state = EnemyState.Move;
        print("상태전환 : Damaged -> Move");
        anim.SetTrigger("Move");

    }

    //IEnumerator DAMAGED()
    //{
    //    //PlayerFire playerAttack;
    //    ////playerAttack.Fire();
    //    //Debug.Log("hp가" + enemyHp + "만큼 남았습니다");
    //    //
    //    //
    //    //yield return new WaitForSeconds(1.0f);
    //}

    //죽음상태 (Any State)
    private void Die()
    {
        ////코루틴을 사용하자
        ////1. 체력이 0이하
        ////2. 몬스터 오브젝트 삭제
        ////- 상태변경
        ////- 상태전환 출력 (죽었다)
        //Debug.Log("Die상태");
        //StopAllCoroutines();
        //StartCoroutine(DIE());

        //진행중인 모든 코루틴은 정지한다
        StopAllCoroutines();

        //죽음상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {
        //캐릭터컨트롤러 비활성화 
       // cc.enabled = false;

        //2초후에 자기자신을 제거한다 
        yield return new WaitForSeconds(2.0f);
        print("죽었다!!");
        Destroy(gameObject);
    }

    //IEnumerator DIE()
    //{
    //    //Debug.Log("적이 죽었습니다");
    //    //Destroy(this.gameObject);
    //    //yield return new WaitForSeconds(2.0f);
    //}

    private void OnDrawGizmos()
    {
        //공격 가능 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
        //플레이어를 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
        //이동 가능한 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }
}
