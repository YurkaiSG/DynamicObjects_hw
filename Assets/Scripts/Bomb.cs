using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ColorChanger))]
public class Bomb : SpawnableObject
{
    [SerializeField] private int minFadeTimeValue = 2;
    [SerializeField] private int maxFadeTimeValue = 5;
    [SerializeField] private float _fadeSpeed = 10f;
    [SerializeField] private float _interval = 0.5f;
    private ColorChanger _colorChanger;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
    }

    private IEnumerator FadeOutSmoothly(float fadeTime)
    {
        WaitForSeconds waitInterval = new WaitForSeconds(_interval);
        float currentTime = 0f;

        while (Renderer.material.color.a != 0)
        {

            yield return waitInterval;
            currentTime += _interval;
        }
    }
}
