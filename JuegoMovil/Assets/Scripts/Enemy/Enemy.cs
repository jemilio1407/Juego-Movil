using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int scoreValue = 1;

    private int currentHealth;
    private Animator animator;
    private bool isDead;
    private float reactionUntil;
    private string currentAnimation;

    private void Awake() => animator = GetComponent<Animator>();

    private void Start() => currentHealth = maxHealth;

    private void Update()
    {
        if (isDead) return;
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (Time.time >= reactionUntil) PlayAnimation("Enemy_Fly");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameManager.Instance.AddScore(scoreValue);
            Die();
            return;
        }
        reactionUntil = Time.time + 0.18f;
        PlayAnimation("Enemy_Hit");
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        PlayAnimation("Enemy_Death");
        Destroy(gameObject, 0.34f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.PlayHitAnimation();
            GameManager.Instance.TakeDamage(1);
            Die();
        }
        else if (other.CompareTag("BoundaryLimit")) Die();
    }

    private void PlayAnimation(string state)
    {
        if (animator == null || currentAnimation == state) return;
        animator.Play(state, 0, 0f);
        currentAnimation = state;
    }
}
