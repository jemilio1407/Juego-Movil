using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro
using System.Collections; // NUEVO: Necesario para las Corrutinas (Temporizadores)
using Dino.Utility.Audio; // NUEVO: Importamos el sistema de audio

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText; 
    [SerializeField] private GameObject[] hearts; 

    [Header("Session Stats")]
    [SerializeField] private int playerHealth = 3;
    private int score = 0;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        UpdateScoreUI();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("BGM");
        }
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
            scoreText.text = score.ToString("000");
        }
        else
        {
            Debug.LogWarning("Falta asignar el Score Text en el GameManager.");
        }
    }

    public void TakeDamage(int amount)
    {
        if (isGameOver) return;

        playerHealth -= amount;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Damage");
        }

        UpdateHeartsUI();

        if (playerHealth <= 0)
        {
            isGameOver = true;

            if (UIManager.Instance != null)
            {
                GameOverUI goUI = UIManager.Instance.GetWindow("gameover") as GameOverUI;

                if (goUI != null)
                {
                    goUI.SetupScore(score); 
                }

                UIManager.Instance.ShowWindow("gameover");
            }
            else
            {
                Debug.LogError("¡No se encontró el UIManager en la escena!");
            }
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null) player.gameObject.SetActive(false); 

            EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
            if (spawner != null) spawner.gameObject.SetActive(false);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    private void UpdateHeartsUI()
    {
        if (hearts == null || hearts.Length == 0) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < playerHealth);
        }
    }

    public void PauseGame()
    {
        if (isGameOver) return;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowWindow("pause");
        }

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
        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 0f;
    }
}