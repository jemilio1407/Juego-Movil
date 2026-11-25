using UnityEngine;
using UnityEngine.EventSystems;
using Dino.Utility.Audio;

public class PlayerController : MonoBehaviour
{
    private static readonly int IdleStateHash = Animator.StringToHash("Player_Idle");
    private static readonly int FlyStateHash = Animator.StringToHash("Player_Fly");
    private static readonly int FallStateHash = Animator.StringToHash("Player_Fall");
    private static readonly int ShootStateHash = Animator.StringToHash("Player_Shoot");
    private static readonly int HitStateHash = Animator.StringToHash("Player_Hit");

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 3f;

    [Header("Combat Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;

    private float nextFireTime;
    private bool isMovingUp;
    private Animator animator;
    private float animationLockUntil;
    private int currentAnimationHash;

    private void Awake() => animator = GetComponent<Animator>();

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        HandleInput();
        MovePlayer();
        UpdateMovementAnimation();
    }

    private void HandleInput()
    {
#if UNITY_EDITOR
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
#endif
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId)) continue;
            if (touch.phase != TouchPhase.Began) continue;
            if (touch.position.x < Screen.width / 2f) isMovingUp = !isMovingUp;
            else TryShoot();
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width / 2f) isMovingUp = !isMovingUp;
            else TryShoot();
        }
#endif
    }

    private void MovePlayer()
    {
        float currentSpeed = isMovingUp ? moveSpeed : -fallSpeed;
        Vector3 newPosition = transform.position + Vector3.up * currentSpeed * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }

    private void UpdateMovementAnimation()
    {
        if (Time.time < animationLockUntil) return;

        bool reachedUpperLimit = isMovingUp && transform.position.y >= maxY;
        if (reachedUpperLimit)
        {
            PlayAnimation(IdleStateHash);
            return;
        }

        PlayAnimation(isMovingUp ? FlyStateHash : FallStateHash);
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime) return;
        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        nextFireTime = Time.time + fireRate;
        PlayLockedAnimation(ShootStateHash, 0.24f);
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySound("Shoot");
    }

    public void PlayHitAnimation() => PlayLockedAnimation(HitStateHash, 0.36f);

    private void PlayLockedAnimation(int stateHash, float duration)
    {
        animationLockUntil = Time.time + duration;
        PlayAnimation(stateHash);
    }

    private void PlayAnimation(int stateHash)
    {
        if (animator == null || currentAnimationHash == stateHash) return;
        animator.Play(stateHash, 0, 0f);
        currentAnimationHash = stateHash;
    }
}
