using System;

public interface ISpawner
{
    public abstract event Action Spawned;
    public abstract event Action Created;
    public abstract event Action<int> Active;
}
