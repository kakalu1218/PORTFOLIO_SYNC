using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance; // 유일성이 보장된다
    private static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    #region Manager
    private NetworkManager _network = new NetworkManager();
    private ObjectManager _object = new ObjectManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();

    public static NetworkManager Network { get { return Instance._network; } }
    public static ObjectManager Object { get { return Instance._object; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    #endregion

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _network.Update();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject gameObject = GameObject.Find("@Managers");
            if (gameObject == null)
            {
                gameObject = new GameObject { name = "@Managers" };
                gameObject.AddComponent<Managers>();
            }

            DontDestroyOnLoad(gameObject);
            s_instance = gameObject.GetComponent<Managers>();

            // Init
            s_instance._network.Init();
            s_instance._pool.Init();
        }
    }

    public static void Clear()
    {
        // Clear
        s_instance._scene.Clear();
        s_instance._pool.Clear();
    }
}
