using UnityEngine;

public class SpawnerBomb : Spawner<Bomb>
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

    protected override void ActionOnGet(Bomb spawnedObject)
    {
        spawnedObject.ResetAlpha();
        base.ActionOnGet(spawnedObject);
        spawnedObject.transform.position = _nextSpawnPosition;
        spawnedObject.Released += Release;
        spawnedObject.Release();
    }

    protected override void ActionOnRelease(Bomb spawnedObject)
    {
        spawnedObject.Released -= Release;
        base.ActionOnRelease(spawnedObject);
    }

    private void SpawnObject(Vector3 spawnPosition)
    {
        _nextSpawnPosition = spawnPosition;
        Pool.Get();
    }  
}
