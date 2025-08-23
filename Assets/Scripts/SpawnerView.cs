using TMPro;
using UnityEngine;

public abstract class SpawnerView<T> : MonoBehaviour where T : SpawnableObject
{
    [SerializeField] protected Spawner<T> _spawner;
    [SerializeField] protected TextMeshProUGUI _textSpawnedCount;
    [SerializeField] protected TextMeshProUGUI _textCreatedCount;
    [SerializeField] protected TextMeshProUGUI _textActiveCount;
    private string _textSpawned = "Spawned: ";
    private string _textCreated = "Created: ";
    private string _textActive = "Active: ";
    private int _spawnedCounter = 0;
    private int _createdCounter = 0;
    private int _activeCounter = 0;

    protected void ChangeSpawnedCounter()
    {
        _spawnedCounter++;
        ChangeUIInfo(_textSpawnedCount, _textSpawned, _spawnedCounter);
    }

    protected void ChangeCreatedCounter()
    {
        _createdCounter++;
        ChangeUIInfo(_textCreatedCount, _textCreated, _createdCounter);
    }

    protected void ChangeActiveCounter(int value)
    {
        _activeCounter = value;
        ChangeUIInfo(_textActiveCount, _textActive, _activeCounter);
    }

    private void ChangeUIInfo(TextMeshProUGUI textField, string text, int counter)
    {
        textField.text = text + counter;
    }
}
