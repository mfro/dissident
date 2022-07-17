using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseNavigator : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseCanvas;
    public GameObject pauseMenu;
    public GameObject pauseSettings;

    Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas.SetActive(false);

        volumeSlider = pauseSettings.GetComponentInChildren<Slider>();
        volumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
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
    }

    public void OnResume()
    {
        TogglePause();
    }

    public void OnSettings()
    {
        pauseMenu.SetActive(false);
        pauseSettings.SetActive(true);
    }

    public void OnQuit()
    {
        SceneManager.LoadScene(1);
    }

    void UpdateVolume()
    {
        GameManager.gm.masterVolume = volumeSlider.value;
    }
}
