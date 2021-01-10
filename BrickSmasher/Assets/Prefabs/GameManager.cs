using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float leftScreenEdge, rightScreenEdge;
    public int lives;
    public static int score;
    int keepScore;
    public int livesPoints;
    public int points_brick;
    public bool gameover;
    public bool gamewon;
    public bool cursorShow;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject pauseMenu;
    public TextMeshProUGUI finalScoreTextLoss;
    public TextMeshProUGUI finalScoreTextWin;
    public GameObject otherGUI_score;
    public GameObject otherGUI_lives;
    public GameObject bricks;
    public Transform barWidth;
    public bool toDropBar_lifePlus = true, toDropBar_lifeMinus = true, toDropBar_widthPlus = true, toDropBar_widthMinus = true;
    public bool isActivated_minus = false, isActivated_plus = false;

    AudioSource gameOver_sound, menuButton_sound;
    int brickCount;
    public float initBarWidth = 14f;

    public bool isGamePaused = false;
    void Start()
    {
        //resetHighScore();
        keepScore = score;
        Cursor.lockState = CursorLockMode.Confined;
        var aSources = GetComponents<AudioSource>();
        gameOver_sound = aSources[0];
        menuButton_sound = aSources[1];
        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score; 
        brickCount = bricks.transform.childCount;
    }

    void Update()
    {
        leftScreenEdge = -(barWidth.localScale.x/2.5f);

        if(initBarWidth > barWidth.localScale.x)
        {
            leftScreenEdge -= barWidth.localScale.x / 2 - 0.15f;
        }
        else
        {
            if(initBarWidth < barWidth.localScale.x)
            {
                leftScreenEdge += barWidth.localScale.x / 4 - 0.45f;
            }
        }

        rightScreenEdge = -leftScreenEdge;

        if(!cursorShow)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public IEnumerator StartCountdown(float countdownValue, int positive)
    {
        while (countdownValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            countdownValue--;
        }
        if(positive > 0)
        {
            toDropBar_widthPlus = true;
            isActivated_plus = false;
        }
        else
        {
            toDropBar_widthMinus = true;
            isActivated_minus = false;
        }
        UpdateBarWidth(-positive);
    }

    public void UpdateLives(int change)
    {
        lives += change;

        if(lives <= 0)
        {
            lives = 0;
            gameOver_sound.Play();
            gameOver();
        }

        livesText.text = "Lives: " + lives;
    }

    public void UpdateScore(int change)
    {
        brickCount --;
        score += change;
        scoreText.text = "Score: " + score;

        if(brickCount == 0)
        {
            gameWon();
        }
    }

    void gameWon()
    {
        victoryPanel.SetActive(true);
        score += lives * livesPoints;
        finalScoreTextWin.text = "Score: " + score;
        highScore_update(score);
        otherGUI_lives.SetActive(false);
        otherGUI_score.SetActive(false);
        gamewon = true;
        cursorShow = true;
    }

    void gameOver()
    {
        gameOverPanel.SetActive(true);
        otherGUI_lives.SetActive(false);
        otherGUI_score.SetActive(false);
        highScore(score);
        score = 0;
        gameover = true;
        cursorShow = true;
    }

    public void retryLevel()
    {
        score = keepScore;
        cursorShow = false;
        menuButton_sound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextLevel()
    {
        keepScore = score;
        cursorShow = false;
        menuButton_sound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void enterMainMenu()
    {
        cursorShow = true;
        menuButton_sound.Play();
        score = 0;
        SceneManager.LoadScene(0);

    }

    void Resume()
    { 
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void resetGameActivity()
    {
        isGamePaused = true;
        Time.timeScale = 1f;
    }

    void highScore (int currentScore)
    {
        int highScore = PlayerPrefs.GetInt("HIGHSCORE");

        if(currentScore > highScore)
        {
            PlayerPrefs.SetInt("HIGHSCORE", currentScore);
            finalScoreTextLoss.text = "New High Score: " + currentScore;
        }
        else
        {
            finalScoreTextLoss.text = "Score: " + currentScore;
        }
    }

    void highScore_update (int currentScore)
    {
        int highScore = PlayerPrefs.GetInt("HIGHSCORE");

        if(currentScore > highScore)
        {
            PlayerPrefs.SetInt("HIGHSCORE", currentScore);
        }
    }

    public void resetHighScore()
    {
        PlayerPrefs.SetInt("HIGHSCORE",0);
    }

    public void UpdateBarWidth(int positive)
    {
        float ratio;
        if(positive > 0)
        {
            ratio = 2;
        }
        else
        {
            ratio = 0.5f;
        }
        barWidth.localScale = new Vector2 (ratio * barWidth.localScale.x, barWidth.localScale.y);
    }
}
