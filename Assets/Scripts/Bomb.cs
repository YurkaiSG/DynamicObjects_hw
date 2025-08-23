using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger), typeof(Exploder))]
public class Bomb : SpawnableObject
{
    [SerializeField] private int _minFadeTimeValue = 2;
    [SerializeField] private int _maxFadeTimeValue = 5;
    [SerializeField] private float _interval = 0.1f;
    private Exploder _exploder;
    private ColorChanger _colorChanger;

    public int MinFadeTimeValue => _minFadeTimeValue;
    public int MaxFadeTimeValue => _maxFadeTimeValue;

    private void Awake()
    {
        Init();
        _exploder = GetComponent<Exploder>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void ResetAlpha()
    {
        _colorChanger.ChangeAlpha(Renderer);
    }

    public void Explode()
    {
        _exploder.Explode();
    }

    public IEnumerator FadeOutSmoothly(float fadeTime)
    {
        WaitForSeconds waitInterval = new WaitForSeconds(_interval);
        float currentTime = fadeTime;

        while (Renderer.material.color.a != 0)
        {
            yield return waitInterval;
            currentTime -= _interval;
            _colorChanger.ChangeAlpha(Renderer ,Mathf.InverseLerp(0f, fadeTime, currentTime));
        }
    }
}
