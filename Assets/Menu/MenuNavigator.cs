using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject tutorialScreen;
    public GameObject creditsScreen;

    Button lastSelected;
    EventSystem _es;

    // Start is called before the first frame update
    void Start()
    {
        GoToMain();

        _es = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMain();
        }

        if (!_es.currentSelectedGameObject)
        {
            lastSelected.Select();
        }

        lastSelected = _es.currentSelectedGameObject.GetComponent<Button>();
    }

    public void Play()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToCredits()
    {
        mainScreen.SetActive(false);
        creditsScreen.SetActive(true);
        creditsScreen.transform.Find("Back").GetComponent<Button>().Select();
    }

    public void GoToTutorial()
    {
        mainScreen.SetActive(false);
        tutorialScreen.SetActive(true);
        tutorialScreen.transform.Find("Back").GetComponent<Button>().Select();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void GoToMain()
    {
        tutorialScreen.SetActive(false);
        creditsScreen.SetActive(false);
        mainScreen.SetActive(true);

        mainScreen.transform.Find("Play").GetComponent<Button>().Select();
    }
}
