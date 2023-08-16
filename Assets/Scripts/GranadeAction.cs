using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeAction : MonoBehaviour
{
  [SerializeField] private GameObject granadeEffect;

  private void OnCollisionEnter(Collision other) 
  {
    GameObject effect = Instantiate(granadeEffect);
    effect.transform.position = transform.position;

    Destroy(gameObject);
  }
}
