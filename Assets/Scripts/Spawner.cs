using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour, ISpawner where T : SpawnableObject
{
    [SerializeField] protected T Prefab;
    [SerializeField] protected float SpawnDelay = 0.5f;
    [SerializeField] protected int PoolCapacity = 30;
    [SerializeField] protected int PoolMaxSize = 100;
    protected ObjectPool<T> Pool;

    public event Action Spawned;
    public event Action Created;
    public event Action<int> Active;

    private void Awake()
    {
        Init();
    }

    protected virtual T CreateFunc()
    {
        T spawnedObject = Instantiate(Prefab);
        Created?.Invoke();
        return spawnedObject;
    }

    protected virtual void ActionOnGet(T spawnedObject)
    {
        spawnedObject.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        spawnedObject.gameObject.SetActive(true);
        Spawned?.Invoke();
        Active?.Invoke(Pool.CountActive);
    }

    protected virtual void ActionOnRelease(T spawnedObject)
    {
        spawnedObject.gameObject.SetActive(false);
        spawnedObject.Rigidbody.velocity = Vector3.zero;
        Active?.Invoke(Pool.CountActive);
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

    protected virtual void Release(T spawnedObject)
    {
        Pool.Release(spawnedObject);
    }
}
