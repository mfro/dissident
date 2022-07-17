using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreLifeSystem : MonoBehaviour
{
  private int _scope;
  private int _currentLives;

  public int score
  {
    get => _scope;
    set
    {
      _scope = value;
      scoreText.text = "Score: " + value.ToString();
    }

  }
  public int currentLives
  {
    get => _currentLives;
    set
    {
      _currentLives = value;
      livesText.text = "Lives: " + _currentLives.ToString();

      if (_currentLives == 0 && !gameOverText.gameObject.activeSelf)
      {
        StartCoroutine(EndGame());
      }
    }
  }

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
    gameOverText.gameObject.SetActive(false);
    gameOverScoreText.gameObject.SetActive(false);
    returningToMenuText.gameObject.SetActive(false);

  }

  private IEnumerator EndGame()
  {
    gameOverText.gameObject.SetActive(true);
    gameOverText.text = "GAME OVER!";
    returningToMenuText.gameObject.SetActive(true);
    gameOverScoreText.text = "Score: " + score.ToString();
    gameOverScoreText.gameObject.SetActive(true);

    for (int i = 0; i < 5; i++)
    {
      int timeLeft = 5 - i;
      returningToMenuText.text = "Exiting in " + timeLeft.ToString();
      yield return new WaitForSeconds(1);
    }

    SceneManager.LoadScene(0);
  }
}
