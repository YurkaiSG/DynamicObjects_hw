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

    public void ChangeAlpha(Renderer renderer, float targetAlphaValue = 100)
    {
        Color color = renderer.material.color;
        color.a = targetAlphaValue;
        renderer.material.color = color;
    }
}
