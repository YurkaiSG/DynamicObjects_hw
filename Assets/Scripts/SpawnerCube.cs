using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnerCube : Spawner
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

    protected override SpawnableObject CreateFunc()
    {
        GameObject pooledObject = Instantiate(Prefab.gameObject);
        pooledObject.TryGetComponent(out Cube cube);
        return cube;
    }

    protected override void ActionOnGet(SpawnableObject spawnedObject)
    {
        Vector3 spawnPosition = new Vector3(
            UnityEngine.Random.Range(_bounds.min.x, _bounds.max.x),
            UnityEngine.Random.Range(_bounds.min.y, _bounds.max.y),
            UnityEngine.Random.Range(_bounds.min.z, _bounds.max.z));

        if (spawnedObject.TryGetComponent(out Cube cube))
            cube.Collided += ReleaseOnCollide;

        spawnedObject.transform.position = spawnPosition;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.gameObject.SetActive(true);
    }

    protected override void ActionOnRelease(SpawnableObject spawnedObject)
    {
        base.ActionOnRelease(spawnedObject);

        if (spawnedObject.TryGetComponent(out Cube cube))
            cube.Collided -= ReleaseOnCollide;
    }

    private void ReleaseOnCollide(Cube cube)
    {
        StartCoroutine(Release(cube));
    }

    protected override IEnumerator Release(SpawnableObject spawnedObject)
    {
        yield return StartCoroutine(base.Release(spawnedObject));
        Released?.Invoke(spawnedObject.transform.position);
    }
}
