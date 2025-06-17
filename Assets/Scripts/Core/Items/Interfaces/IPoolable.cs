using System;

namespace Core.Items
{
    public interface IPoolable 
    {
        event Action<IPoolable> Despawned;
    }
}