using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
  [SerializeField] private int currentHP = 20;
  [SerializeField] private int maxHP = 100;
  [SerializeField] private float moveSpeed = 7f;
  [SerializeField] private float jumpPower = 10f;

  private CharacterController cc;
  private float gravity = -20f;
  private float yVelocity = 0;
  private bool bIsJumping = false;

  private void Start() 
  {
    cc = GetComponent<CharacterController>();
  }
  private void Update() 
  {
    if(cc.collisionFlags != CollisionFlags.Below)    
      yVelocity += gravity * Time.deltaTime;

    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");

    Vector3 dir = new Vector3(h, 0, v);
    dir = dir.normalized;

    dir = Camera.main.transform.TransformDirection(dir);
    if(bIsJumping && cc.collisionFlags == CollisionFlags.Below)
    {
      bIsJumping = false;
      yVelocity = 0;
    }

    if(Input.GetButtonDown("Jump") && !bIsJumping)
    {
      yVelocity = jumpPower;
      bIsJumping = true;      
    }    
    
    dir.y = yVelocity; 

    // transform.position += dir * moveSpeed * Time.deltaTime;
    cc.Move(dir * moveSpeed * Time.deltaTime);
  }

  public void DamageAction(int damage)
  {
    currentHP -= damage;
  }
}
