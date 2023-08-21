using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
  [SerializeField] private TMP_InputField id;
  [SerializeField] private TMP_InputField password;

  [SerializeField] private TMP_Text notify;

  private void Start() 
  {
    notify.text = "";
  }

  private bool CheckInput(string id, string pw)
  {
    if(id == "" || pw == "")
    {
      notify.text = "Please enter your ID or password.";
      return false;
    }
    else
    {
      return true;
    }
  }

  public void SaveUserData()
  {
    if(!CheckInput(id.text, password.text)) return;

    if(!PlayerPrefs.HasKey(id.text))
    {
      PlayerPrefs.SetString(id.text, password.text);
      notify.text = "Registration complete!";
    }
    else
    {
      notify.text = "The ID already exists.";
    }
  }  

  public void CheckUserData()
  {
    if(!CheckInput(id.text, password.text)) return;

    string pass = PlayerPrefs.GetString(id.text);

    if(password.text == pass)
    {
      SceneManager.LoadScene(1);
    }
    else
    {
      notify.text = "ID or password is not correct";
    }
  }
}
