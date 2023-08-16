using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
  [SerializeField] private float rotSpeed = 200f;

  private float mx = 0;

  private void Update() 
  {
    if(GameManager.gm.GetGameState != EGameState.Run) return;
    
    float mouse_X = Input.GetAxis("Mouse X");

    mx += mouse_X * rotSpeed * Time.deltaTime;

    transform.eulerAngles = new Vector3(0, mx, 0);
  }
}
