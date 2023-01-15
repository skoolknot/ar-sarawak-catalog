using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToSceneManager : MonoBehaviour
{
    [SerializeField]
    private string _sceneName;

    private Button _panel;

    private void OnEnable()
    {
        _panel = GetComponent<Button>();
        _panel.onClick.AddListener(() =>
        {
            if (_sceneName != "")
                SceneManager.LoadScene(_sceneName);
        });
    }

    private void OnDisable() => _panel.onClick.RemoveAllListeners();
}
