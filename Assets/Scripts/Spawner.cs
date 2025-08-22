using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected SpawnableObject Prefab;
    [SerializeField] protected float SpawnDelay = 0.5f;
    [SerializeField] protected int MinReleaseDelay = 2;
    [SerializeField] protected int MaxReleaseDelay = 5;
    [SerializeField] protected int PoolCapacity = 30;
    [SerializeField] protected int PoolMaxSize = 100;
    protected ObjectPool<SpawnableObject> Pool;

    public abstract event Action Spawned;
    public abstract event Action Created;
    public abstract event Action<int> Active;

    private void Awake()
    {
        Init();
    }

    protected virtual SpawnableObject CreateFunc()
    {
        GameObject pooledObject = Instantiate(Prefab.gameObject);
        pooledObject.TryGetComponent(out SpawnableObject spawnedObject);
        return spawnedObject;
    }

    protected virtual void ActionOnGet(SpawnableObject spawnedObject)
    {
        spawnedObject.transform.position = transform.position;
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.gameObject.SetActive(true);

        StartCoroutine(Release(spawnedObject));
    }

    protected virtual void ActionOnRelease(SpawnableObject spawnedObject)
    {
        spawnedObject.gameObject.SetActive(false);
        spawnedObject.transform.rotation = Quaternion.identity;
        spawnedObject.Rigidbody.velocity = Vector3.zero;
    }

    protected void Init()
    {
        Pool = new ObjectPool<SpawnableObject>(
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

    protected virtual IEnumerator Release(SpawnableObject spawnedObject)
    {
        WaitForSeconds delay = new WaitForSeconds(UnityEngine.Random.Range(MinReleaseDelay, MaxReleaseDelay + 1));
        yield return delay;
        Pool.Release(spawnedObject);
    }
}
