using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Adds a looping two-layer city background to the Gameplay scene.
/// The city moves faster than the clouded sky to suggest forward flight.
/// </summary>
public sealed class ParallaxBackground : MonoBehaviour
{
    private const string GameplaySceneName = "Gameplay";
    private const float CityScrollSpeed = 3f;
    private const float SkyScrollSpeed = 0.45f;

    private LoopingLayer skyLayer;
    private LoopingLayer cityLayer;
    private Camera targetCamera;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterForSceneLoads()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.IsValid() || scene.name != GameplaySceneName)
            return;

        if (FindAnyObjectByType<ParallaxBackground>() != null)
            return;

        var backgroundObject = new GameObject("Gameplay Parallax Background");
        backgroundObject.AddComponent<ParallaxBackground>();
    }

    private void Awake()
    {
        targetCamera = Camera.main;
        if (targetCamera == null)
        {
            Debug.LogError("Parallax background could not find the Main Camera.", this);
            enabled = false;
            return;
        }

        Sprite skySprite = Resources.Load<Sprite>("Parallax/SkyLayer");
        Sprite citySprite = Resources.Load<Sprite>("Parallax/CityLayer");
        if (skySprite == null || citySprite == null)
        {
            Debug.LogError("Parallax sprites are missing from Assets/Resources/Parallax.", this);
            enabled = false;
            return;
        }

        float viewHeight = targetCamera.orthographicSize * 2f;
        float viewWidth = viewHeight * targetCamera.aspect;
        float scale = Mathf.Max(
            viewHeight / skySprite.bounds.size.y,
            viewWidth / skySprite.bounds.size.x);
        skyLayer = CreateLayer("Sky", skySprite, scale, -100, SkyScrollSpeed, targetCamera.transform.position.z + 9f);
        cityLayer = CreateLayer("City", citySprite, scale, -90, CityScrollSpeed, targetCamera.transform.position.z + 8f);
    }

    private void Update()
    {
        if (targetCamera == null)
            return;

        Vector3 cameraPosition = targetCamera.transform.position;
        float unscaledDeltaTime = Time.unscaledDeltaTime;
        skyLayer?.Update(unscaledDeltaTime, cameraPosition);
        cityLayer?.Update(unscaledDeltaTime, cameraPosition);
    }

    private LoopingLayer CreateLayer(string layerName, Sprite sprite, float scale, int sortingOrder, float speed, float z)
    {
        var first = CreateTile(layerName + " Tile A", sprite, scale, sortingOrder, z);
        var second = CreateTile(layerName + " Tile B", sprite, scale, sortingOrder, z);
        float width = sprite.bounds.size.x * scale;

        return new LoopingLayer(first, second, width, speed);
    }

    private Transform CreateTile(string tileName, Sprite sprite, float scale, int sortingOrder, float z)
    {
        var tile = new GameObject(tileName);
        tile.transform.SetParent(transform, false);
        tile.transform.localScale = Vector3.one * scale;

        var renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = sortingOrder;
        renderer.color = Color.white;

        Vector3 cameraPosition = targetCamera.transform.position;
        tile.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, z);
        return tile.transform;
    }

    private sealed class LoopingLayer
    {
        private readonly Transform firstTile;
        private readonly Transform secondTile;
        private readonly float width;
        private readonly float speed;
        private float offset;

        public LoopingLayer(Transform firstTile, Transform secondTile, float width, float speed)
        {
            this.firstTile = firstTile;
            this.secondTile = secondTile;
            this.width = width;
            this.speed = speed;
        }

        public void Update(float deltaTime, Vector3 cameraPosition)
        {
            offset = Mathf.Repeat(offset + speed * deltaTime, width);
            float leftTileX = cameraPosition.x - offset;

            firstTile.position = new Vector3(leftTileX, cameraPosition.y, firstTile.position.z);
            secondTile.position = new Vector3(leftTileX + width, cameraPosition.y, secondTile.position.z);
        }
    }
}
