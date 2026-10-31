using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro
using System.Collections; // NUEVO: Necesario para las Corrutinas (Temporizadores)

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText; // Referencia a tu texto de Score en el Canvas
    [SerializeField] private GameObject[] hearts; // Array para guardar las imágenes de los corazones

    [Header("Session Stats")]
    [SerializeField] private int playerHealth = 3;
    private int score = 0;

    // Añadimos esta variable para saber si ya perdimos
    private bool isGameOver = false;

    private void Awake()
    {
        // Singleton para poder llamar al GameManager desde cualquier otro script
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        // Actualizamos el texto del score apenas inicia el juego (para que diga 000)
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // El ToString("000") fuerza a que el número siempre tenga 3 dígitos (ej: 007, 015, 150)
            scoreText.text = score.ToString("000");
        }
        else
        {
            Debug.LogWarning("Falta asignar el Score Text en el GameManager.");
        }
    }

    public void TakeDamage(int amount)
    {
        // Si ya perdimos, ignoramos cualquier otro golpe para no repetir el Game Over
        if (isGameOver) return;

        playerHealth -= amount;

        // Actualizamos visualmente los corazones en la pantalla
        UpdateHeartsUI();

        if (playerHealth <= 0)
        {
            isGameOver = true; // Marcamos que el juego terminó

            // NUEVO: Le pedimos a TU UIManager que busque la ventana y le pase los puntos 
            // Esto asegura que actualice a la UI correcta de la escena
            if (UIManager.Instance != null)
            {
                // Buscamos la ventana por su ID "gameover" y la tratamos como tipo GameOverUI
                GameOverUI goUI = UIManager.Instance.GetWindow("gameover") as GameOverUI;

                if (goUI != null)
                {
                    goUI.SetupScore(score); // Le pasamos los puntos reales
                }

                UIManager.Instance.ShowWindow("gameover"); // La mostramos en pantalla
            }
            else
            {
                Debug.LogError("¡No se encontró el UIManager en la escena!");
            }

            // En lugar de pausar el tiempo, apagamos al Jugador y al Spawner
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) player.gameObject.SetActive(false); // Desaparece al jugador

            EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
            if (spawner != null) spawner.gameObject.SetActive(false); // Apaga el generador de enemigos

            // NUEVO: Buscamos a todos los enemigos que quedaron en pantalla y los destruimos
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    private void UpdateHeartsUI()
    {
        // Si no hemos asignado corazones, no hacemos nada para evitar errores
        if (hearts == null || hearts.Length == 0) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < playerHealth);
        }
    }

    // Función para pausar el juego respetando la rúbrica de animaciones
    public void PauseGame()
    {
        if (isGameOver) return;

        // 1. Mostramos la ventana usando tu sistema UIManager directamente (Sin buscarla para evitar el error rojo)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowWindow("pause");
        }

        // 2. Iniciamos un temporizador para esperar a que termine la animación
        StopAllCoroutines();
        StartCoroutine(PauseTimeRoutine());
    }

    public void ResumeGame()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
    }

    private IEnumerator PauseTimeRoutine()
    {
        // Esperamos 0.5 segundos reales (lo que dura tu animación Show Duration)
        yield return new WaitForSecondsRealtime(0.5f);

        // 3. Una vez que el menú ya es 100% visible, congelamos el juego
        Time.timeScale = 0f;
    }
}