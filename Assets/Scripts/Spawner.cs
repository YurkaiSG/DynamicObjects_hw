using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour, ISpawner where T : SpawnableObject
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected float SpawnDelay = 0.5f;
    [SerializeField] protected int MinReleaseDelay = 2;
    [SerializeField] protected int MaxReleaseDelay = 5;
    [SerializeField] protected int PoolCapacity = 30;
    [SerializeField] protected int PoolMaxSize = 100;
    protected ObjectPool<T> Pool;

    public abstract event Action Spawned;
    public abstract event Action Created;
    public abstract event Action<int> Active;

    private void Awake()
    {
        Init();
    }

    protected virtual T CreateFunc()
    {
        GameObject pooledObject = Instantiate(Prefab.gameObject);
        pooledObject.TryGetComponent(out T spawnedObject);
        return spawnedObject;
    }

    protected virtual void ActionOnGet(T spawnedObject)
    {
        spawnedObject.transform.position = transform.position;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.gameObject.SetActive(true);

        StartCoroutine(Release(spawnedObject));
    }

    protected virtual void ActionOnRelease(T spawnedObject)
    {
        spawnedObject.gameObject.SetActive(false);
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.Rigidbody.velocity = Vector3.zero;
    }

    protected void Init()
    {
        Pool = new ObjectPool<T>(
            createFunc: () => CreateFunc(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: PoolMaxSize
        );
    }

    protected virtual void InitiateSpawn()
    {
        StartCoroutine(SpawnObject());
    }

    protected virtual IEnumerator SpawnObject()
    {
        WaitForSeconds wait = new WaitForSeconds(SpawnDelay);

        while (enabled)
        {
            Pool.Get();
            yield return wait;
        }
    }   

    protected virtual IEnumerator Release(T spawnedObject)
    {
        WaitForSeconds delay = new WaitForSeconds(UnityEngine.Random.Range(MinReleaseDelay, MaxReleaseDelay + 1));
        yield return delay;
        Pool.Release(spawnedObject);
    }
}
