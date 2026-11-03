using UnityEngine;
using UnityEngine.SceneManagement;

public class AppSceneManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Escribe aquí el nombre exacto de tu escena de juego")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    [Tooltip("Escribe aquí el nombre exacto de tu escena de menú principal")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameplaySceneName);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}