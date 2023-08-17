using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
  [SerializeField] private EnemyController enemyController;

  public void PlayerHit()
  {
    enemyController.AttackAction();
  }
}
