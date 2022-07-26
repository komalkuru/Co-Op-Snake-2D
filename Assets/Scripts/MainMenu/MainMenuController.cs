using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject object1;
    [SerializeField] private GameObject object2;

    public void Quit()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Application.Quit();
    }

    public void ModeSelect()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        object1.SetActive(true);
        object2.SetActive(true);
    }

    public void BackToMenu()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        object1.SetActive(true);
        object2.SetActive(false);
    }
}
