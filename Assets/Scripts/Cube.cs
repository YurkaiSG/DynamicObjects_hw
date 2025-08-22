using System;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
public class Cube : SpawnableObject
{
    private ColorChanger _colorChanger;
    private Color _baseColor;
    private bool _isCollided = false;

    public event Action<Cube> Collided;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<Renderer>();
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
        if (other.gameObject.TryGetComponent(out Cube _) == false && other.gameObject.TryGetComponent(out Bomb _) == false)
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
