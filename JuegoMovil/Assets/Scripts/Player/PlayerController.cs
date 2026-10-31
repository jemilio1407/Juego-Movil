using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    private bool isMovingUp = false;

    private void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        // Input móvil
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < Screen.width / 2f)
                {
                    isMovingUp = !isMovingUp;
                }
                else if (touch.position.x >= Screen.width / 2f)
                {
                    TryShoot();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width / 2f)
            {
                isMovingUp = !isMovingUp;
            }
            else if (Input.mousePosition.x >= Screen.width / 2f)
            {
                TryShoot();
            }
        }
    }

    private void MovePlayer()
    {
        float currentSpeed = isMovingUp ? moveSpeed : -fallSpeed;
        Vector3 newPosition = transform.position + Vector3.up * currentSpeed * Time.deltaTime;

        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }

    private void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }
    }
}