using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
  [SerializeField] private float rotSpeed = 200f;

  private float mx = 0;
  private float my = 0;

  private void Update() 
  {
    if(GameManager.gm.GetGameState != EGameState.Run) return;
    
    float mouse_X = Input.GetAxis("Mouse X");
    float mouse_Y = Input.GetAxis("Mouse Y");

    mx += mouse_X * rotSpeed * Time.deltaTime;
    my += mouse_Y * rotSpeed * Time.deltaTime;

    my = Mathf.Clamp(my, -90, 90);

    transform.eulerAngles = new Vector3(-my, mx, 0);
  }
}
