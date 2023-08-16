using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
  [SerializeField] private float destroyTime = 1.5f;

  private float currentTime = 0;

  private void Update() 
  {
    if(currentTime > destroyTime)    
      Destroy(gameObject);

    currentTime += Time.deltaTime;
  }
}
