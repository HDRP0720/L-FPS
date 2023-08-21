using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingNextScene : MonoBehaviour
{
  [SerializeField] private int sceneNumber = 2;
  [SerializeField] private Slider loadingBar;
  [SerializeField] private TMP_Text loadingText;

  private void Start() 
  {
    loadingText.text = "";
    StartCoroutine(TransitionNextScene(sceneNumber));
  }
  private IEnumerator TransitionNextScene(int num)
  {
    AsyncOperation ao = SceneManager.LoadSceneAsync(num);
    ao.allowSceneActivation = false;

    while(!ao.isDone)
    {
      loadingBar.value = ao.progress;
      loadingText.text = (ao.progress * 100f).ToString() + "%";

      if(ao.progress >= 0.9f)
      {
        ao.allowSceneActivation = true;
      }

      yield return null;
    }
  }
}
