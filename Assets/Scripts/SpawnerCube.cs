using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnerCube : Spawner<Cube>
{
    private Collider _collider;
    private Bounds _bounds;

    public event Action<Vector3> Released;

    private void Awake()
    {
        Init();
        _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }

    private void Start()
    {
        InitiateSpawn();
    }

    protected override void ActionOnGet(Cube spawnedObject)
    {
        Vector3 spawnPosition = new Vector3(
            UnityEngine.Random.Range(_bounds.min.x, _bounds.max.x),
            UnityEngine.Random.Range(_bounds.min.y, _bounds.max.y),
            UnityEngine.Random.Range(_bounds.min.z, _bounds.max.z));

        base.ActionOnGet(spawnedObject);
        spawnedObject.transform.position = spawnPosition;
        spawnedObject.Released += Release;
    }

    protected override void ActionOnRelease(Cube spawnedObject)
    {
        spawnedObject.Released -= Release;
        Released?.Invoke(spawnedObject.transform.position);
        base.ActionOnRelease(spawnedObject);
    }
}
