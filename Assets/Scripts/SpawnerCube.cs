using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnerCube : Spawner<Cube>
{
    private Collider _collider;
    private Bounds _bounds;

    public event Action<Vector3> Released;
    public override event Action Spawned;
    public override event Action Created;
    public override event Action<int> Active;

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

    protected override Cube CreateFunc()
    {
        GameObject pooledObject = Instantiate(Prefab.gameObject);
        pooledObject.TryGetComponent(out Cube cube);
        Created?.Invoke();
        return cube;
    }

    protected override void ActionOnGet(Cube spawnedObject)
    {
        Vector3 spawnPosition = new Vector3(
            UnityEngine.Random.Range(_bounds.min.x, _bounds.max.x),
            UnityEngine.Random.Range(_bounds.min.y, _bounds.max.y),
            UnityEngine.Random.Range(_bounds.min.z, _bounds.max.z));

        spawnedObject.Collided += ReleaseOnCollide;
        spawnedObject.transform.position = spawnPosition;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.gameObject.SetActive(true);
        Spawned?.Invoke();
        Active?.Invoke(Pool.CountActive);
    }

    protected override void ActionOnRelease(Cube spawnedObject)
    {
        base.ActionOnRelease(spawnedObject);
        spawnedObject.Collided -= ReleaseOnCollide;
        Active?.Invoke(Pool.CountActive);
    }

    private void ReleaseOnCollide(Cube cube)
    {
        StartCoroutine(Release(cube));
    }

    protected override IEnumerator Release(Cube spawnedObject)
    {
        yield return StartCoroutine(base.Release(spawnedObject));
        Released?.Invoke(spawnedObject.transform.position);
    }
}
