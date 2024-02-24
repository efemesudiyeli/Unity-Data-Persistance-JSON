using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance;

    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private GameObject _buttonToStart;
    public string playerName = null;
    private bool _isGameStarted;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        if (!_isGameStarted)
        {
            if (playerName.Length > 1 && playerName.Length < 10 && !_nameInput.text.Contains(" "))
            {
                _buttonToStart.SetActive(true);
            }
            else
            {
                _buttonToStart.SetActive(false);
            }
        }
    }

    public void UpdateName()
    {
        playerName = _nameInput.text;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
        _isGameStarted = true;
    }
}
