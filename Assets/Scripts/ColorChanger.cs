using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void Change(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }

    public void Change(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }
}
