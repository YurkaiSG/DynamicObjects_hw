using UnityEngine;

public class SpawnerReference : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _spawnerMonoBehaviour;

    public ISpawner Spawner => _spawnerMonoBehaviour as ISpawner;

    private void OnValidate()
    {
        if(_spawnerMonoBehaviour != null && (_spawnerMonoBehaviour is ISpawner) == false)
        {
            Debug.LogError($"{_spawnerMonoBehaviour.name} does not implement {nameof(ISpawner)}");
            _spawnerMonoBehaviour = null;
        }
    }
}
