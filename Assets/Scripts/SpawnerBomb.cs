using System;
using System.Collections;
using UnityEngine;

public class SpawnerBomb : Spawner<Bomb>
{
    [SerializeField] private SpawnerCube _spawnerCube;
    private Vector3 _nextSpawnPosition;

    public override event Action Spawned;
    public override event Action Created;
    public override event Action<int> Active;

    private void OnEnable()
    {
        _spawnerCube.Released += SpawnObject;
    }

    private void OnDisable()
    {
        _spawnerCube.Released -= SpawnObject;
    }

    protected override Bomb CreateFunc()
    {
        GameObject pooledObject = Instantiate(Prefab.gameObject);
        pooledObject.TryGetComponent(out Bomb bomb);
        Created?.Invoke();
        return bomb;
    }

    protected override void ActionOnGet(Bomb spawnedObject)
    {
        spawnedObject.transform.position = _nextSpawnPosition;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.gameObject.SetActive(true);
        
        if (spawnedObject.TryGetComponent(out Bomb bomb))
        {
            bomb.ResetAlpha();
            StartCoroutine(Release(spawnedObject));
        }

        Spawned?.Invoke();
        Active?.Invoke(Pool.CountActive);
    }

    protected override IEnumerator Release(Bomb spawnedObject)
    {
        if (spawnedObject.TryGetComponent(out Bomb bomb))
        {
            yield return StartCoroutine(bomb.FadeOutSmoothly(UnityEngine.Random.Range(bomb.MinFadeTimeValue, bomb.MaxFadeTimeValue)));
            bomb.Explode();
        }

        Pool.Release(spawnedObject);
        Active?.Invoke(Pool.CountActive);
    }

    private void SpawnObject(Vector3 spawnPosition)
    {
        _nextSpawnPosition = spawnPosition;
        Pool.Get();
    }  
}
