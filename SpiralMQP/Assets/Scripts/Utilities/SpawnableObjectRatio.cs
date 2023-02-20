[System.Serializable]
public class SpawnableObjectRatio<T>
{
    public T dungeonObject; // This spawnable object with the generic type T
    
    // Spawn ratio for this spawnable object. The value itself is not important, the relative ratio compared to other spawnable object in the same level is important.
    public int ratio; 
}
