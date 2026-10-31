using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : UIWindow
{
    [Header("Game Over Elements")]
    [SerializeField] private TMP_Text finalScoreText;

    public void SetupScore(int score)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + score.ToString("000");
        }
    }

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;

        Hide();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }


    [Button("Test Show")]
    private void TestShow()
    {
        Show();
    }

    [Button("Test Hide")]
    private void TestHide()
    {
        Hide();
    }

}