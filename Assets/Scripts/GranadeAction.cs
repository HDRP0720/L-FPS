using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeAction : MonoBehaviour
{
  [SerializeField] private GameObject granadeEffect;
  [SerializeField] private int attackPower = 10;
  [SerializeField] private float explosionRadius = 5f;

  private void OnCollisionEnter(Collision other) 
  {
    Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8);
    for (int i = 0; i < colliders.Length; i++)
    {
      colliders[i].GetComponent<EnemyController>().HitEnemy(attackPower);
    }
    GameObject effect = Instantiate(granadeEffect);
    effect.transform.position = transform.position;

    Destroy(gameObject);
  }
}
