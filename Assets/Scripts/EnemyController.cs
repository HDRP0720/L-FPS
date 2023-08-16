using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [SerializeField] private int currentHP = 15;
  [SerializeField] private int maxHP = 100;
  [SerializeField] private int attackPower = 3;

  [SerializeField] private float moveDistance = 20f;
  [SerializeField] private float findDistance = 8f;
  [SerializeField] private float attackDistance = 2f;

  [SerializeField] private float moveSpeed = 5f;
 
  private EnemyState state;
  private CharacterController cc;
  private float currentTime = 0;
  private float attackDelay = 2f;
  private Vector3 originPos;
  private Transform player;

  private void Start() 
  {
    currentHP = maxHP;
    state = EnemyState.Idle;
    cc = GetComponent<CharacterController>();
    originPos = transform.position;
    player = GameObject.Find("Player").transform;
  }
  private void Update() 
  {
    switch(state)
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
        // Damaged();
        break;

      case EnemyState.Die:
        // Die();
        break;
    }
  }

  private void Idle()
  {
    if(Vector3.Distance(transform.position, player.position) < findDistance)
    {
      state = EnemyState.Move;
      Debug.Log("상태전환: Idle -> Move");
    }
  }
  private void Move()
  {
    if(Vector3.Distance(transform.position, originPos) > moveDistance)
    {
      state = EnemyState.Return;
      Debug.Log("상태전환: Move -> Return");
    }
    else if(Vector3.Distance(transform.position, player.position) > attackDistance)
    {
      Vector3 dir = (player.position - transform.position).normalized;
      cc.Move(dir * moveSpeed * Time.deltaTime);
    }
    else
    {
      state = EnemyState.Attack;
      currentTime = attackDelay;
      Debug.Log("상태전환: Move -> Attack");
    }
  }
  private void Attack()
  {
    if(Vector3.Distance(transform.position, player.position) < attackDistance)
    {
      currentTime += Time.deltaTime;
      if(currentTime > attackDelay)
      {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
        Debug.Log("Attack");
        currentTime = 0;
      }
    }
    else
    {
      state = EnemyState.Move;
      currentTime = 0;
      Debug.Log("상태전환: Attack -> Move");
    }
  }
  private void Return()
  {
    if(Vector3.Distance(transform.position, originPos) > 0.1f)
    {
      Vector3 dir = (originPos - transform.position).normalized;
      cc.Move(dir * moveSpeed * Time.deltaTime);
    }
    else
    {
      transform.position = originPos;
      currentHP = maxHP;
      state = EnemyState.Idle;
      Debug.Log("상태전환: Return -> Idle");
    }
  }
  private void Damaged()
  {
    StartCoroutine(DamageProcess());
  }
  private IEnumerator DamageProcess()
  {
    yield return new WaitForSeconds(0.5f);
    state = EnemyState.Move;
    Debug.Log("상태전환: Damaged -> Move");
  }

  private void Die()
  {
    StopAllCoroutines();

    StartCoroutine(DieProcess());
  }
  private IEnumerator DieProcess()
  {
    cc.enabled = false;
    yield return new WaitForSeconds(2.0f);
    Debug.Log("소멸");
    Destroy(gameObject);
  }
  

  public void HitEnemy(int damage)
  {
    if(state == EnemyState.Damaged || state == EnemyState.Die || state == EnemyState.Return)
      return;

    currentHP -= damage;
    if(currentHP > 0)
    {
      state = EnemyState.Damaged;
      Damaged();
      Debug.Log("상태전환: Any State -> Move");
    }
    else
    {
      state = EnemyState.Die;
      Die();
      Debug.Log("상태전환: Any State -> Die");
    }
  }

  private void OnDrawGizmosSelected() 
  {
    DrawColoredWireSphere(transform.position, moveDistance, Color.blue);
    DrawColoredWireSphere(transform.position, findDistance, Color.yellow);
    DrawColoredWireSphere(transform.position, attackDistance, Color.red);
  }
  private void DrawColoredWireSphere(Vector3 center, float radius, Color color)
  {
    Gizmos.color = color;
    int segments = 36; // Number of segments for the wire sphere
    float angleIncrement = 360.0f / segments;
    Vector3 prevPoint = center + new Vector3(radius, 0, 0); // Starting point

    for (int i = 1; i <= segments; i++)
    {
      float angle = i * angleIncrement;
      Vector3 newPoint = center + Quaternion.Euler(0, angle, 0) * (Vector3.forward * radius);

      Gizmos.DrawLine(prevPoint, newPoint);

      prevPoint = newPoint;
    }
  }
}

public enum EnemyState
{
  Idle, Move, Attack, Return, Damaged, Die
}
