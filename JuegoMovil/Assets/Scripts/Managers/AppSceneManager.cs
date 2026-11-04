using UnityEngine;
using UnityEngine.SceneManagement;
using Dino.Utility.Audio;

public class AppSceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        bool isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        if (isMusicOn && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("BGM");
        }
    }

    public void PlayGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound("click1");
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameplaySceneName);
    }

    public void GoToMainMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound("click1");
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OpenSettings()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound("click1");
        if (UIManager.Instance != null) UIManager.Instance.ShowWindow("settings");
    }

    public void OpenCredits()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound("click1");
        if (UIManager.Instance != null) UIManager.Instance.ShowWindow("credits");
    }

    public void QuitGame()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}