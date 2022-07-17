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

    // Start is called before the first frame update
    void Start()
    {
        GoToMain();

        masterVolumeSlider = settingsScreen.GetComponentInChildren<Slider>();
        masterVolumeSlider.value = GameManager.gm.masterVolume;
        masterVolumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMain();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(0);
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
    }

    public void GoToMain()
    {
        DisableAll();
        mainScreen.SetActive(true);
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
    }

    public void GoToTutorial()
    {
        DisableAll();
        tutorialScreen.SetActive(true);
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
    }

    public void GoToSettings()
    {
        DisableAll();
        settingsScreen.SetActive(true);
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
    }

    public void GoToCredits()
    {
        DisableAll();
        creditsScreen.SetActive(true);
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
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

    void UpdateVolume()
    {
        GameManager.gm.masterVolume = masterVolumeSlider.value;
    }
}
