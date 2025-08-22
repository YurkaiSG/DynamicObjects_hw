using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 10.0f;
    [SerializeField] private float _explosionForce = 80.0f;

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hit in hits)
            if (hit.TryGetComponent(out SpawnableObject spawnedObject))
                spawnedObject.Rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
    }
}
