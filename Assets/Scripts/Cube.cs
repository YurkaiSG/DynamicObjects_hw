using System;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
public class Cube : SpawnableObject
{
    [SerializeField] private LayerMask _platformLayer;
    private ColorChanger _colorChanger;
    private Color _baseColor;
    private bool _isCollided = false;

    public event Action<Cube> Collided;

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
                Collided?.Invoke(this);
            }
        }
    }
}
