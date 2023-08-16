using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
  [SerializeField] private int weaponPower = 5;

  [SerializeField] private Transform projectileTransform;
  [SerializeField] private GameObject projectileObject;
  [SerializeField] private float throwPower = 15f;

  [Space]
  [SerializeField] private GameObject bulletEffect;

  private bool bIsAiming = false;
  private GameObject granade;
  private ParticleSystem ps;

  private void Start() 
  {
    ps = bulletEffect.GetComponent<ParticleSystem>();
  }
  private void Update() 
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

    if(Input.GetMouseButtonDown(0))
    {
      Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
      RaycastHit hit = new RaycastHit();

      if(Physics.Raycast(ray, out hit))
      {
        if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
          EnemyController enemyController = hit.transform.GetComponent<EnemyController>();
          enemyController.HitEnemy(weaponPower);
        }
        else
        {       
          bulletEffect.transform.position = hit.point;
          bulletEffect.transform.forward = hit.normal;

          ps.Play();
        }        
      }
    }

    if(Input.GetMouseButtonDown(1))
    {
      bIsAiming = true;
      granade = Instantiate(projectileObject);
      Rigidbody rb = granade.GetComponent<Rigidbody>();
      rb.isKinematic = true;

      granade.transform.position = projectileTransform.position;
      granade.transform.SetParent(projectileTransform);      
    }
    
    if(Input.GetMouseButtonUp(1) && bIsAiming)
    {
      bIsAiming = false;
      granade.transform.SetParent(null); 

      Rigidbody rb = granade.GetComponent<Rigidbody>();
      rb.isKinematic= false;
      rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
    }
  }
}
