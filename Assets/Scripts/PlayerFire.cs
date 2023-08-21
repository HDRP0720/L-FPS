using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFire : MonoBehaviour
{
  [SerializeField] private TMP_Text weaponModeTextField;
  [SerializeField] private int weaponPower = 5;

  [SerializeField] private Transform projectileTransform;
  [SerializeField] private GameObject projectileObject;
  [SerializeField] private float throwPower = 15f;

  [Space]
  [SerializeField] private GameObject bulletEffect;
  [SerializeField] private GameObject[] MuzzleEffects;

  [Header("UI")]
  [SerializeField] private GameObject assaultRifleIcon;
  [SerializeField] private GameObject sniperRifleIcon;
  [Space]
  [SerializeField] private GameObject assaultCrosshair;
  [SerializeField] private GameObject sniperCrosshair;
  [Space]
  [SerializeField] private GameObject grenadeIcon;
  [SerializeField] private GameObject zoomIcon;
  [Space]
  [SerializeField] private GameObject sniperZoom;

  private EWeaponMode weaponMode;
  private Animator animator;
  private ParticleSystem ps;

  private bool bIsAiming = false;
  private bool bIsZoomMode = false;
  private GameObject granade;

  private void Start() 
  {
    weaponModeTextField.text = "Normal Mode";
    weaponMode = EWeaponMode.Normal;
    ChangeWeaponUI(weaponMode);

    animator = GetComponentInChildren<Animator>();
    ps = bulletEffect.GetComponent<ParticleSystem>();
  }  
  private void Update() 
  {
    if(GameManager.gm.GetGameState != EGameState.Run) return;

    if(Input.GetKeyDown(KeyCode.Alpha1))
    {
      weaponMode = EWeaponMode.Normal;
      weaponModeTextField.text = "Normal Mode";
      Camera.main.fieldOfView = 60f;

      sniperZoom.SetActive(false);
      ChangeWeaponUI(weaponMode);
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      weaponMode = EWeaponMode.Sniper;
      weaponModeTextField.text = "Sniper Mode";

      ChangeWeaponUI(weaponMode);
    }

    if(Input.GetMouseButtonDown(0))
    {
      if(animator.GetFloat("MoveMotion") == 0)
      {
        animator.SetTrigger("Attack");
      }

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
    
      StartCoroutine(ShootEffect(0.05f));
    }

    if(Input.GetMouseButtonDown(1))
    {
      switch (weaponMode)
      {
        case EWeaponMode.Normal:
          bIsAiming = true;
          granade = Instantiate(projectileObject);
          Rigidbody rb = granade.GetComponent<Rigidbody>();
          rb.isKinematic = true;

          granade.transform.position = projectileTransform.position;
          granade.transform.SetParent(projectileTransform); 
          break;

        case EWeaponMode.Sniper:
          if(!bIsZoomMode)
          {
            Camera.main.fieldOfView = 15f;
            bIsZoomMode = true;
            sniperCrosshair.SetActive(false);
            sniperZoom.SetActive(true);
          }
          else
          {
            Camera.main.fieldOfView = 60f;
            bIsZoomMode = false;
            sniperCrosshair.SetActive(true);
            sniperZoom.SetActive(false);
          }
          break;
      }           
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

  private void ChangeWeaponUI(EWeaponMode weaponMode)
  {
    if(weaponMode == EWeaponMode.Normal)
    {
      assaultRifleIcon.SetActive(true);
      sniperRifleIcon.SetActive(false);
      assaultCrosshair.SetActive(true);
      sniperCrosshair.SetActive(false);
      grenadeIcon.SetActive(true);
      zoomIcon.SetActive(false);
    }
    else
    {
      assaultRifleIcon.SetActive(false);
      sniperRifleIcon.SetActive(true);
      assaultCrosshair.SetActive(false);
      sniperCrosshair.SetActive(true);
      grenadeIcon.SetActive(false);
      zoomIcon.SetActive(true);
    }
  }

  private IEnumerator ShootEffect(float duration)
  {
    int rand = Random.Range(0, MuzzleEffects.Length-1);
    MuzzleEffects[rand].SetActive(true);
    yield return new WaitForSeconds(duration);
    MuzzleEffects[rand].SetActive(false);
  }
}

public enum EWeaponMode { Normal, Sniper }
