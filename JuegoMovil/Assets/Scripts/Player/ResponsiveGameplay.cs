using UnityEngine;

public class ResponsiveGameplay : MonoBehaviour
{
    public enum ScreenSide { Izquierda, Derecha }

    [Header("Ajuste a la Pantalla")]
    public ScreenSide ladoPantalla;

    public float margen = 1f;

    private void Start()
    {
        if (Camera.main == null) return;

        float viewportX = ladoPantalla == ScreenSide.Izquierda ? 0f : 1f;

        // Convierte ese borde a coordenadas del mundo 2D
        Vector3 bordeMundo = Camera.main.ViewportToWorldPoint(new Vector3(viewportX, 0f, Mathf.Abs(Camera.main.transform.position.z)));

        float nuevaPosX = ladoPantalla == ScreenSide.Izquierda ? bordeMundo.x + margen : bordeMundo.x - margen;

        transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
    }
}