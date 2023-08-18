using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
  [SerializeField] private int currentHP = 15;
  [SerializeField] private int maxHP = 100;
  [SerializeField] private Slider hpSlider;
  [Space]
  [SerializeField] private int attackPower = 3;
  [Space]
  [SerializeField] private float moveDistance = 20f;
  [SerializeField] private float findDistance = 8f;
  [SerializeField] private float attackDistance = 2f;

  [SerializeField] private float moveSpeed = 5f;
 
  private EEnemyState state;
  private CharacterController cc;
  private NavMeshAgent agent;
  private Animator animator;
  private float currentTime = 0;
  private float attackDelay = 2f;

  private Vector3 originPos;
  private Quaternion originRot;

  private Transform player;

  private void Start() 
  {
    currentHP = maxHP;
    state = EEnemyState.Idle;
    cc = GetComponent<CharacterController>();
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponentInChildren<Animator>();

    originPos = transform.position;
    originRot = transform.rotation;

    player = GameObject.Find("Player").transform;
  }
  private void Update() 
  {
    hpSlider.value = (float)currentHP / (float)maxHP;
    switch(state)
    {
      case EEnemyState.Idle:
        Idle();
        break;

      case EEnemyState.Move:
        Move();
        break;

      case EEnemyState.Attack:
        Attack();
        break;

      case EEnemyState.Return:
        Return();
        break;

      case EEnemyState.Damaged:
        // Damaged();
        break;

      case EEnemyState.Die:
        // Die();
        break;
    }
  }

  private void Idle()
  {
    if(Vector3.Distance(transform.position, player.position) < findDistance)
    {
      state = EEnemyState.Move;
      animator.SetTrigger("IdleToMove");
      Debug.Log("State: Idle -> Move");
    }
  }
  private void Move()
  {
    if(Vector3.Distance(transform.position, originPos) > moveDistance)
    {
      state = EEnemyState.Return;
      Debug.Log("State: Move -> Return");
    }
    else if(Vector3.Distance(transform.position, player.position) > attackDistance)
    {      
      agent.isStopped = true;
      agent.ResetPath();
      agent.stoppingDistance = attackDistance;
      agent.destination = player.position;
    }
    else
    {
      state = EEnemyState.Attack;
      currentTime = attackDelay;
      animator.SetTrigger("MoveToAttackDelay");
      Debug.Log("State: Move -> Attack");
    }
  }
  private void Attack()
  {
    if(Vector3.Distance(transform.position, player.position) < attackDistance)
    {
      currentTime += Time.deltaTime;
      if(currentTime > attackDelay)
      {
        animator.SetTrigger("StartAttack");
        Debug.Log("Attack");
        currentTime = 0;
      }
    }
    else
    {
      state = EEnemyState.Move;
      currentTime = 0;
      animator.SetTrigger("AttackToMove");
      Debug.Log("State: Attack -> Move");
    }
  }
  private void Return()
  {
    if(Vector3.Distance(transform.position, originPos) > 0.1f)
    { 
      agent.destination = originPos;
      agent.stoppingDistance = 0;
    }
    else
    {
      agent.isStopped = true;
      agent.ResetPath();

      transform.position = originPos;
      transform.rotation = originRot;
      currentHP = maxHP;
      state = EEnemyState.Idle;
      animator.SetTrigger("MoveToIdle");
      Debug.Log("State: Return -> Idle");
    }
  }
  private void Damaged()
  {
    StartCoroutine(DamageProcess());
  }
  private IEnumerator DamageProcess()
  {
    yield return new WaitForSeconds(1f);
    state = EEnemyState.Move;
    Debug.Log("State: Damaged -> Move");
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
    Debug.Log("Perishment");
    Destroy(gameObject);
  }  

  public void HitEnemy(int damage)
  {
    if(state == EEnemyState.Damaged || state == EEnemyState.Die || state == EEnemyState.Return)
      return;

    hpSlider.gameObject.SetActive(true);

    currentHP -= damage;
    agent.isStopped = true;
    agent.ResetPath();
    
    if(currentHP > 0)
    {
      state = EEnemyState.Damaged;
      animator.SetTrigger("Damaged");
      Damaged();
      Debug.Log("State: Any State -> Move");
    }
    else
    {
      state = EEnemyState.Die;
      hpSlider.gameObject.SetActive(false);
      animator.SetTrigger("Die");
      Die();
      Debug.Log("State: Any State -> Die");
    }
  }

  public void AttackAction()
  {
    player.GetComponent<PlayerMove>().DamageAction(attackPower);
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

public enum EEnemyState
{
  Idle, Move, Attack, Return, Damaged, Die
}