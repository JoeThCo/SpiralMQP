using UnityEngine;

/// <summary>
/// This is an abstract generic class for singleton pattern design
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonAbstract<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }
    
    protected virtual void Awake() 
    {
        if (instance == null)
        {
            instance = this as T;
        }   
        else
        {
            Destroy(gameObject);
        }
    }
}
