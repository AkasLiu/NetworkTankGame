using System;

public class Singleton<T> where T : class, new()
{
    private static T _instance;

    public static void CreateInstance()
    {
        if (_instance == null)
            _instance = Activator.CreateInstance<T>();
    }

    public static void DestroyInstance()
    {
        if (_instance != null)
            _instance = null;
    }

    public static T Instance
    {
        get
        {
            CreateInstance();
            return _instance;
        }
    }
}
