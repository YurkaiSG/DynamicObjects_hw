using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private Color _baseColor;
    private bool _isCollided = false;

    public Rigidbody Rigidbody => _rigidbody;
    public bool IsCollided => _isCollided;
    public event Action<GameObject> Collided;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _baseColor = _renderer.material.color;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _isCollided = false;
    }

    private void OnDisable()
    {
        _renderer.material.color = _baseColor;
    }

    public void CollideWithPlatform()
    {
        _isCollided = true;
        _renderer.material.color = UnityEngine.Random.ColorHSV();
        Collided.Invoke(gameObject);
    }
}
