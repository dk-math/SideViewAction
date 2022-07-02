using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour

{
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] AudioClip gameClearSE;
    [SerializeField] AudioClip gameOverSE;
    AudioSource audioSource;

    const int MAX_SCORE = 9999;
    public int score = 0;

    private void Start() {
        scoreText.text = score.ToString() + " / 8";
        audioSource = GetComponent<AudioSource>();
    }

    public void AddScore(int val) {
        score += val;
        if (score > MAX_SCORE) {
            score = MAX_SCORE;
        }
        if (score < 8) {
            scoreText.text = score.ToString() + " / 8";
        } else {
            scoreText.text = "Go Home!";
        }
    }

    public void GameOver() {
        gameOverText.SetActive(true);
        audioSource.PlayOneShot(gameOverSE);
        Invoke("RestartScene", 1.5f);
    }
    public void GameClear() {
        gameClearText.SetActive(true);
        audioSource.PlayOneShot(gameClearSE);
        Invoke("RestartScene", 1.5f);
    }
    
    void RestartScene() {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
