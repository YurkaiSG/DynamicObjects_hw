using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpawnerReference))]
public class SpawnerInfoView : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI _textSpawnedCount;
    [SerializeField] private TextMeshProUGUI _textCreatedCount;
    [SerializeField] private TextMeshProUGUI _textActiveCount;
    private SpawnerReference _spawnerReference;
    private string _textSpawned = "Spawned: ";
    private string _textCreated = "Created: ";
    private string _textActive = "Active: ";
    private int _spawnedCounter = 0;
    private int _createdCounter = 0;
    private int _activeCounter = 0;

    private void Awake()
    {
        _spawnerReference = GetComponent<SpawnerReference>();
    }

    private void OnEnable()
    {
        _spawnerReference.Spawner.Spawned += ChangeSpawnedCounter;
        _spawnerReference.Spawner.Created += ChangeCreatedCounter;
        _spawnerReference.Spawner.Active += ChangeActiveCounter;
    }

    private void OnDisable()
    {
        _spawnerReference.Spawner.Spawned -= ChangeSpawnedCounter;
        _spawnerReference.Spawner.Created -= ChangeCreatedCounter;
        _spawnerReference.Spawner.Active -= ChangeActiveCounter;
    }

    private void ChangeSpawnedCounter()
    {
        _spawnedCounter++;
        ChangeUIInfo(_textSpawnedCount, _textSpawned, _spawnedCounter);
    }

    private void ChangeCreatedCounter()
    {
        _createdCounter++;
        ChangeUIInfo(_textCreatedCount, _textCreated, _createdCounter);
    }

    private void ChangeActiveCounter(int value)
    {
        _activeCounter = value;
        ChangeUIInfo(_textActiveCount, _textActive, _activeCounter);
    }

    private void ChangeUIInfo(TextMeshProUGUI textField, string text, int counter)
    {
        textField.text = text + counter;
    }
}
