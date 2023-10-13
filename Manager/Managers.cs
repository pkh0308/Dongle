using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static GameManager _gameManager = new GameManager();
    static ResourceManager _resourceManager = new ResourceManager();
    static WfsManager _wfsManager = new WfsManager();
    static ObjectManager _objectManager = new ObjectManager();
    static UIManager _uiManager = new UIManager();
    static SoundManager _soundManager = new SoundManager();
    static DataBaseManager _databaseManager = new DataBaseManager();

    public static GameManager Game { get{ return _gameManager; } }
    public static ResourceManager Resc { get { return _resourceManager; } }
    public static WfsManager Wfs { get { return _wfsManager; } }
    public static ObjectManager Obj { get { return _objectManager; } }
    public static UIManager UI { get { return _uiManager; } }
    public static SoundManager Sound { get { return _soundManager; } }
    public static DataBaseManager DB { get { return _databaseManager; } }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // ������ ����
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        _gameManager.Init();
        _resourceManager.Init();
        _wfsManager.Init();
        _objectManager.Init();
        _uiManager.Init();
        _soundManager.Init();
        _databaseManager.Init();
    }
}
