using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonController : MonoBehaviour
{
    public Button button;
    public string SceneName;

    private void Awake()
    {
        button.onClick.AddListener(SceneLoader);
    }

    private void SceneLoader()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(SceneName);
    }
}
