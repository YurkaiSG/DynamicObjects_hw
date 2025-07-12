using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _spawnDelay = 0.5f;
    [SerializeField] private int _minReleaseDelay = 2;
    [SerializeField] private int _maxReleaseDelay = 5;
    [SerializeField] private int _poolCapacity = 30;
    [SerializeField] private int _poolMaxSize = 100;
    private ObjectPool<Cube> _pool;
    private Collider _collider;
    private Bounds _bounds;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
        _pool = new ObjectPool<Cube>(
            createFunc: () => CreateFunc(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );

        StartCoroutine(SpawnObject());
    }

    private Cube CreateFunc()
    {
        GameObject pooledObject = Instantiate(_prefab);
        pooledObject.TryGetComponent(out Cube cube);
        return cube;
    }

    private void ActionOnGet(Cube cube)
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(_bounds.min.x, _bounds.max.x),
            Random.Range(_bounds.min.y, _bounds.max.y),
            Random.Range(_bounds.min.z, _bounds.max.z));

        cube.Collided += ReleaseOnCollide;

        cube.transform.position = spawnPosition;
        cube.transform.rotation = Quaternion.identity;
        cube.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.transform.rotation = Quaternion.identity;
        cube.Rigidbody.velocity = Vector3.zero;
        cube.Collided -= ReleaseOnCollide;
    }

    private IEnumerator SpawnObject()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            _pool.Get();
            yield return wait;
        }
    }

    private void ReleaseOnCollide(Cube cube)
    {
        StartCoroutine(Release(cube));
    }

    private IEnumerator Release(Cube cube)
    {
        WaitForSeconds delay = new WaitForSeconds(Random.Range(_minReleaseDelay, _maxReleaseDelay + 1));
        yield return delay;
        _pool.Release(cube);
    }
}
