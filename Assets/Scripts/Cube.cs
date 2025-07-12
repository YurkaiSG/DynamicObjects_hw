using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ColorChanger))]
public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private ColorChanger _colorChanger;
    private Color _baseColor;
    private bool _isCollided = false;

    public Rigidbody Rigidbody => _rigidbody;
    public Renderer Renderer => _renderer;
    public event Action<Cube> Collided;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _colorChanger = GetComponent<ColorChanger>();
        _baseColor = _renderer.material.color;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _isCollided = false;
    }

    private void OnDisable()
    {
        _colorChanger.Change(_renderer, _baseColor);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Cube _) == false)
        {
            if (_isCollided == false)
            {
                _isCollided = true;
                _colorChanger.Change(_renderer);
                Collided.Invoke(this);
            }
        }
    }
}
