using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
public class Cube : SpawnableObject
{
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private int MinReleaseDelay = 2;
    [SerializeField] private int MaxReleaseDelay = 5;
    private ColorChanger _colorChanger;
    private Color _baseColor;
    private bool _isCollided = false;

    public event Action<Cube> Released;

    private void Awake()
    {
        Init();
        _colorChanger = GetComponent<ColorChanger>();
        _baseColor = Renderer.material.color;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _isCollided = false;
    }

    private void OnDisable()
    {
        _colorChanger.Change(Renderer, _baseColor);
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((_platformLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (_isCollided == false)
            {
                _isCollided = true;
                _colorChanger.Change(Renderer);
                StartCoroutine(ReleaseAfterDelay());
            }
        }
    }

    private IEnumerator ReleaseAfterDelay()
    {
        WaitForSeconds delay = new WaitForSeconds(UnityEngine.Random.Range(MinReleaseDelay, MaxReleaseDelay + 1));
        yield return delay;
        RaiseReleasedEvent();
    }

    private void RaiseReleasedEvent() => Released?.Invoke(this);
}
