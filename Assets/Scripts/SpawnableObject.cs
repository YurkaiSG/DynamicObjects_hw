using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public abstract class SpawnableObject : MonoBehaviour
{
    public Rigidbody Rigidbody { get; protected set; }
    public Renderer Renderer { get; protected set; }
}
