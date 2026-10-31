using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PauseUI : UIWindow
{
    // Método para el botón "Play" (Reanudar)
    public void OnPlayClicked()
    {
        // 1. Restaurar el tiempo PRIMERO para que el reloj vuelva a funcionar
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            Time.timeScale = 1f;
        }

        // 2. Ocultar usando la animación obligatoria de tu rúbrica
        Hide();
    }

    // Método para el botón "Menu"
    public void OnMenuClicked()
    {
        // Siempre hay que regresar el tiempo a la normalidad antes de cambiar de escena
        // Si no lo haces, el menú principal también estará congelado.
        Time.timeScale = 1f;

        // Cargamos la escena de inicio (asegúrate de que se llame exactamente "MainMenu")
        SceneManager.LoadScene("MainMenu");
    }

    // Método para el botón de Settings
    public void OnSettingsClicked()
    {
        if (UIManager.Instance != null)
        {
            // 1. Descongelamos el tiempo un instante para que la animación de Unity pueda fluir
            Time.timeScale = 1f;

            // 2. Llamamos a la animación oficial directamente sin GetWindow para evitar el error rojo
            UIManager.Instance.ShowWindow("settings");

            // 3. Empezamos a contar 0.5s para volver a congelar el juego
            StopAllCoroutines();
            StartCoroutine(PauseTimeAgainRoutine());
        }
        else
        {
            Debug.LogError("No se encontró el UIManager para abrir Settings.");
        }
    }

    // Corrutina para esperar a que termine la animación de Settings y volver a pausar
    private System.Collections.IEnumerator PauseTimeAgainRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f); // Lo que dura la animación (Show Duration)
        Time.timeScale = 0f; // Volvemos a congelar el mundo
    }

    #region Test Methods

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

    #endregion
}