using System.Collections;
using UnityEngine;

public class SpawnerBomb : Spawner
{
    [SerializeField] private SpawnerCube _spawnerCube;
    private Vector3 _nextSpawnPosition;

    private void OnEnable()
    {
        _spawnerCube.Released += SpawnObject;
    }

    private void OnDisable()
    {
        _spawnerCube.Released -= SpawnObject;
    }

    protected override SpawnableObject CreateFunc()
    {
        GameObject pooledObject = Instantiate(Prefab.gameObject);
        pooledObject.TryGetComponent(out Bomb bomb);
        return bomb;
    }

    protected override void ActionOnGet(SpawnableObject spawnedObject)
    {
        spawnedObject.transform.position = _nextSpawnPosition;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.gameObject.SetActive(true);
        
        if (spawnedObject.TryGetComponent(out Bomb bomb))
        {
            bomb.ResetAlpha();
            StartCoroutine(Release(spawnedObject));
        }
    }

    protected override IEnumerator Release(SpawnableObject spawnedObject)
    {
        if (spawnedObject.TryGetComponent(out Bomb bomb))
        {
            yield return StartCoroutine(bomb.FadeOutSmoothly(Random.Range(bomb.MinFadeTimeValue, bomb.MaxFadeTimeValue)));
            bomb.Explode();
        }

        Pool.Release(spawnedObject);
    }

    private void SpawnObject(Vector3 spawnPosition)
    {
        _nextSpawnPosition = spawnPosition;
        Pool.Get();
    }  
}
