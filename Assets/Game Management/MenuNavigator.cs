using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public GameObject mainScreen, tutorialScreen, settingsScreen, creditsScreen;

    Slider masterVolumeSlider;

    Button lastSelected;
    EventSystem _es;

    // Start is called before the first frame update
    void Start()
    {
        GoToMain();

        _es = EventSystem.current;

        masterVolumeSlider = settingsScreen.GetComponentInChildren<Slider>();
        masterVolumeSlider.value = GameManager.gm.masterVolume;
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

    public void GoToMain()
    {
        DisableAll();
        mainScreen.SetActive(true);

        mainScreen.transform.Find("Play").GetComponent<Button>().Select();
    }

    public void GoToTutorial()
    {
        DisableAll();
        tutorialScreen.SetActive(true);
        tutorialScreen.transform.Find("Back").GetComponent<Button>().Select();
    }

    public void GoToSettings()
    {
        DisableAll();
        settingsScreen.SetActive(true);
        settingsScreen.transform.Find("Back").GetComponent<Button>().Select();
    }

    public void GoToCredits()
    {
        DisableAll();
        creditsScreen.SetActive(true);
        creditsScreen.transform.Find("Back").GetComponent<Button>().Select();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    void DisableAll()
    {
        mainScreen.SetActive(false);
        tutorialScreen.SetActive(false);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
    }

    public void OnVolumeChanged(float value)
    {
        GameManager.gm.UpdateMasterVolume(value);
    }
}
