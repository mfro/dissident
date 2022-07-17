using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreLifeSystem : MonoBehaviour
{

    public long score;
    public int currentLives;
    public int maxLives;

    [SerializeField] PixelText scoreText;
    [SerializeField] PixelText livesText;
    [SerializeField] PixelText gameOverText;
    [SerializeField] PixelText gameOverScoreText;
    [SerializeField] PixelText returningToMenuText;

    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        currentLives = maxLives;
        gameOverText.enabled = false;
        gameOverScoreText.enabled = false;
        returningToMenuText.enabled = false;

    }

    private IEnumerator EndGame()
    {
        gameOverText.enabled = true;
        gameOverText.text = "GAME OVER!";
        returningToMenuText.enabled = true;
        gameOverScoreText.text = "Score: " + score.ToString();
        gameOverScoreText.enabled = true;

        for (int i = 0; i < 5; i++)
        {
            int timeLeft = 5 - i;
            returningToMenuText.text = "Exiting in " + timeLeft.ToString();
            yield return new WaitForSeconds(1);
        }

        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        livesText.text = "Lives: " + currentLives.ToString();

        if(currentLives == 0)
        {
            StartCoroutine(EndGame());
        }
    }
}
