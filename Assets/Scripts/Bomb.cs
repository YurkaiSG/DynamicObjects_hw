using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
public class Bomb : SpawnableObject
{
    [SerializeField] private int _minFadeTimeValue = 2;
    [SerializeField] private int _maxFadeTimeValue = 5;
    [SerializeField] private float _interval = 0.1f;
    [SerializeField] private float _explosionForce = 80f;
    [SerializeField] private float _explosionRadius = 10f;
    private ColorChanger _colorChanger;

    public int MinFadeTimeValue => _minFadeTimeValue;
    public int MaxFadeTimeValue => _maxFadeTimeValue;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<Renderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void ResetAlpha()
    {
        _colorChanger.ChangeAlpha(Renderer);
    }

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in hits)
            if (hit.TryGetComponent(out SpawnableObject spawnedObject))
                spawnedObject.Rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
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
