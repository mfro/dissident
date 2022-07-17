using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    public GameObject mainScreen, tutorialScreen, settingsScreen, creditsScreen;

    public Slider masterVolumeSlider, musicVolumeSlider, effectsVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        GoToMain();

        masterVolumeSlider.value = GameManager.gm.masterVolume;
        masterVolumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });

        musicVolumeSlider.value = GameManager.gm.musicVolume;
        musicVolumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });

        effectsVolumeSlider.value = GameManager.gm.effectsVolume;
        effectsVolumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
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
        SceneManager.LoadScene(1);
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
        GameManager.gm.musicVolume = musicVolumeSlider.value;
        GameManager.gm.effectsVolume = effectsVolumeSlider.value;
    }
}
