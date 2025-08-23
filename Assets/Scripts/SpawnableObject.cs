using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public abstract class SpawnableObject : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public Renderer Renderer { get; private set; }

    protected void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<Renderer>();
    }
}
