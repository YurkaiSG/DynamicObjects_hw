public class SpawnerCubeView : SpawnerView<Cube>
{
    private void OnEnable()
    {
        _spawner.Spawned += ChangeSpawnedCounter;
        _spawner.Created += ChangeCreatedCounter;
        _spawner.Active += ChangeActiveCounter;
    }

    private void OnDisable()
    {
        _spawner.Spawned -= ChangeSpawnedCounter;
        _spawner.Created -= ChangeCreatedCounter;
        _spawner.Active -= ChangeActiveCounter;
    }
}
