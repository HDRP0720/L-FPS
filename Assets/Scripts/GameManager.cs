using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
  public static GameManager gm;

  [SerializeField] private EGameState gState;
  [Space]
  [SerializeField] private GameObject gameLabel;
  [SerializeField] private GameObject crosshair;

  private TMP_Text gameText;
  private PlayerMove player;

  // getter
  public EGameState GetGameState => gState;

  private void Awake() 
  {
    if(gm == null)
    {
      gm = this;
    }
  }
  private void Start() 
  {
    gState = EGameState.Ready;
    
    gameText = gameLabel.GetComponent<TMP_Text>();
    gameText.text = "Ready...";
    gameText.color = new Color32(255, 185, 0, 255);

    player = GameObject.Find("Player").GetComponent<PlayerMove>();

    StartCoroutine(ReadyToStart());
  }
  private void Update() 
  {
    if(gState == EGameState.Run)
    {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;

      crosshair.SetActive(true);
    }      
    else
    {
      crosshair.SetActive(false); 
    } 

    if(player.GetCurrentHP <= 0)
    {
      player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
      gameLabel.SetActive(true);
      gameText.text = "Game Over";
      gameText.color = new Color32(255, 0, 0, 255);
      gState = EGameState.GameOver;
    }
  }

  private IEnumerator ReadyToStart()
  {
    yield return new WaitForSeconds(2f);

    gameText.text = "Go!";

    yield return new WaitForSeconds(0.5f);

    gameLabel.SetActive(false);
    gState = EGameState.Run;
  }
}

public enum EGameState
{
  Ready, Run, GameOver
}
