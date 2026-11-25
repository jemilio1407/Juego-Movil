using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static readonly int FlyStateHash = Animator.StringToHash("Enemy_Fly");
    private static readonly int HitStateHash = Animator.StringToHash("Enemy_Hit");
    private static readonly int DeathStateHash = Animator.StringToHash("Enemy_Death");

    [Header("Stats")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private float maxLifetime = 15f;

    private int currentHealth;
    private Animator animator;
    private Collider2D enemyCollider;
    private bool isDead;
    private float reactionUntil;
    private int currentAnimationHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        if (isDead) return;
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (Time.time >= reactionUntil) PlayAnimation(FlyStateHash);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (GameManager.Instance != null) GameManager.Instance.AddScore(scoreValue);
            Die();
            return;
        }
        reactionUntil = Time.time + 0.18f;
        PlayAnimation(HitStateHash);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        if (enemyCollider != null) enemyCollider.enabled = false;
        PlayAnimation(DeathStateHash);
        Destroy(gameObject, 0.34f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.PlayHitAnimation();
            if (GameManager.Instance != null) GameManager.Instance.TakeDamage(1);
            Die();
        }
        else if (other.CompareTag("BoundaryLimit"))
        {
            isDead = true;
            if (enemyCollider != null) enemyCollider.enabled = false;
            Destroy(gameObject);
        }
    }

    private void PlayAnimation(int stateHash)
    {
        if (animator == null || currentAnimationHash == stateHash) return;
        animator.Play(stateHash, 0, 0f);
        currentAnimationHash = stateHash;
    }
}
