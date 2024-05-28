using Unity.Netcode;
using UnityEngine;

public class NetMonoSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T _instance = null;
    protected static bool IsDestoryed = false;

    public static T Instance
    {
        get
        {
            if (IsDestoryed)
            {
                _instance = null;
            }

            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogWarning($"{typeof(T).Name} singleton is not exists!");
                }
                else
                {
                    IsDestoryed = false;
                }
            }

            return _instance;
        }
    }

    private void OnDisable()
    {
        IsDestoryed = true;
    }
}
