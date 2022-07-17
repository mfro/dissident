using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseNavigator : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseCanvas, pauseMenu, pauseSettings;

    public Slider masterVolumeSlider, musicVolumeSlider, effectsVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas.SetActive(false);

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
            TogglePause();
        }
    }

    void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        if (paused) { SetupPause(); }
        pauseCanvas.SetActive(paused);
    }

    public void SetupPause()
    {
        pauseMenu.SetActive(true);
        pauseSettings.SetActive(false);
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
    }

    public void OnResume()
    {
        TogglePause();
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
    }

    public void OnSettings()
    {
        pauseMenu.SetActive(false);
        pauseSettings.SetActive(true);
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
    }

    public void OnQuit()
    {
        GameManager.gm.PlaySound(GameManager.SoundEffects.cardInspect);
        SceneManager.LoadScene(1);
    }

    void UpdateVolume()
    {
        GameManager.gm.masterVolume = masterVolumeSlider.value;
        GameManager.gm.musicVolume = musicVolumeSlider.value;
        GameManager.gm.effectsVolume = effectsVolumeSlider.value;
    }
}
