using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MENU; //  мой неймспейс
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _prizePanel; // панель достигнутой ачивки или успеха
    [SerializeField] private GameObject _splashPanel; // черная панель для плавных переходов
    
    [SerializeField] public GameObject _menuButtonPanel; // панель с кнопкой МНЕНЮ
    private GameObject menuButton_standard;
    private GameObject menuButton_narrow;

    [SerializeField] public GameObject _menuButtonShrinkedTopPoint;
    private CanvasGroup _whatToFade;

    [SerializeField] public SaveSystem _saveSystem;
    private PrizePanel _prizePanelFile;
    private int[] _achieveArr; // в идеале массив типа ачивочка

    public AudioManager _audioManager;
    // public event Action GMsaysPrizePanelClosed;

    private const float MENU_SPLASH_DURATION = 1.0f; // длина флеш вспышки черного фона между событиями

    // -------- ачивочные переменные ----------------------
    public int REG_Ach; //
    public int HOG_Ach;
    public int TEST_Ach;
    public int MEMO_Ach; // сколько ачивок
    public int signsDetectedCount = 0; // сколько знаков

    public int pokalPoints;
    public int pokal_STATE = 0; // длля высчета состояния кубочка, по дефолту 1 !!!!

    //--------- Singleton ----------------------------------
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) // If there is an instance, and it's not me, delete myself.
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // --------- Singleton  до сих ----------------------

        _audioManager = GameManager.Instance.GetComponent<AudioManager>();

        // получаем рендерер для DOFade и скрываем панель
        //_spalshSpriteRenderer = _splashPanel.GetComponentInChildren<SpriteRenderer>();
        _whatToFade = _splashPanel.GetComponent<CanvasGroup>();
        _splashPanel.SetActive(false);

        // получаем компонент управления Призовой панелью и скрываем панель
        _prizePanelFile = _prizePanel.GetComponent<PrizePanel>();
        _prizePanel.SetActive(false);

        // она от интерфейса поэтому такое
        _saveSystem = new SaveSystem();

        _menuButtonPanel.SetActive(false);
        menuButton_standard = _menuButtonPanel.GetComponentInChildren<MenuBTN_Standard>().gameObject;
        menuButton_narrow = _menuButtonPanel.GetComponentInChildren<MenuBTN_Narrow>().gameObject;

        // LoadAchieves(); // загрузка ачивок 1 раз и рассчет кубка (Оно же стоит на загрузке сцены)
    }


    private void Start()
    {
        LoadAchieves(); // загрузка ачивок и рассчет кубка (Оно же стоит на загрузке сцены)
        HidePrizePanel();
        SplashScreen();
        SceneManager.activeSceneChanged += CheckSceneForMenuBtn;
    }


    // =========== РАССЧЕТ ДЛЯ ВЕЗДЕСУЩЕЙ КНОПКИ МЕНЮ ==========================================================
    public void CheckSceneForMenuBtn(Scene current, Scene next)
    {
        Scene currentScene = SceneManager.GetActiveScene(); // Create a temporary reference to the current scene.
        string sceneName = currentScene.name; // Retrieve the name of this scene.
        SplashScreen();

        if (sceneName == "MENU")
        {
            _menuButtonPanel.SetActive(false);
            LoadAchieves(); // на случай если наачивили в игре
        }
        else
        {
            _menuButtonPanel.SetActive(true);
            
        }
    }

    public void MoveMenuButtonForNarrowPlaces()
    {
        menuButton_standard.SetActive(false);
        menuButton_narrow.SetActive(true);
    }

    public void MoveMenuButtonToStandard()
    {
        menuButton_standard.SetActive(true);
        menuButton_narrow.SetActive(false);
    }

    public void PlusSignsDetected()
    {
        signsDetectedCount++;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ShowPrizePanel("Вы нажали на пробел!", "Обана!", "старый спрайт");
            SplashScreen();
        }
    }


    private void HidePrizePanel()
    {
        _prizePanel.SetActive(false);
    }


    // ============== УНИВЕРСАЛЬНАЯ ФУНКИЯ ПОП-АПА !! PRIZE (ачивки)==========================================
    public void ShowPrizePanel(string givenText, string text4button, string what)
    {
        // _prizePanelFile.ClosedPrizePanelEvent += WhenPrizePanelCloses; 
        _audioManager.PlaySound("WIN");

        _prizePanel.SetActive(true); // 1 - включает панель

        _prizePanelFile.SetTextOne(givenText); // 2 - текст на панели
        _prizePanelFile.SetTextTwo(text4button); // 3 - меняет текст панели через функцию
        _prizePanelFile.SetPrizePicture(what); // 4 - выводит нужную ачивку-картинку
    }
    // ========================================================================================================


    // ------------- показывание сплеш панели ------------------------------------------------------- 
    public void SplashScreen()
    {
        Debug.Log("Black splash - SHOW");

        _whatToFade.alpha = 1;
        _splashPanel.SetActive(true);

        _whatToFade.DOFade(0, MENU_SPLASH_DURATION).OnComplete(() =>
            {
                _splashPanel.SetActive(false);
                Debug.Log("Black splash - OUT");
            }
        );
    }
    // ------------- показывание сплеш панели -------------------------------------------------------


    // ======================= ЗАГРУЗКА АЧИВОК, нужных для кубка ====================================
    private void LoadAchieves()
    {
        REG_Ach = _saveSystem.LoadData("REG_Ach");
        Debug.Log($" Loaded REG ачивки {REG_Ach}");
        if (REG_Ach == 1)
        {
            pokalPoints++;
        }

        MEMO_Ach = _saveSystem.LoadData("MEMO_Ach");
        if (MEMO_Ach == 1)
        {
            pokalPoints++;
        }

        TEST_Ach = _saveSystem.LoadData("TEST_Ach");
        if (TEST_Ach == 1)
        {
            pokalPoints++;
        }

        HOG_Ach = _saveSystem.LoadData("HOG_Ach"); // сколько ачивок
        if (HOG_Ach == 1)
        {
            pokalPoints++;
        }

        signsDetectedCount = _saveSystem.LoadData("AR");
        Debug.Log($" Loaded signs detected {REG_Ach}");
        if (signsDetectedCount >= 3)
        {
            pokalPoints++;
        }

        if (signsDetectedCount >= 10)
        {
            pokalPoints++;
        }

        // рассчет состояния кубка
        switch (pokalPoints)
        {
            case 6:
                pokal_STATE = 3;
                break;
            case 1 or 2 or 3 or 4 or 5:
                pokal_STATE = 2;
                break;
            default:
                pokal_STATE = 1;
                break;
        }
    }


    // ============= УНИВЕРСАЛЬНЫЙ Метод сохранения в ачивку ========================================
    public void SaveAchieves(string _savingWhat)
    {
        switch (_savingWhat)
        {
            case "HOG_Ach":
                _saveSystem.SaveIntData("HOG_Ach", 1);
                break;
            case "MEMO_Ach":
                _saveSystem.SaveIntData("MEMO_Ach", 1);
                break;
            case "REG_Ach":
                _saveSystem.SaveIntData("REG_Ach", 1);
                break;
            case "AR_Ach":
                _saveSystem.SaveIntData("AR_Ach", signsDetectedCount);
                break;
            case "TEST_Ach":
                _saveSystem.SaveIntData("TEST_Ach", 1);
                break;

            default:
                Debug.Log(" GM SaveAchieves: Неправильный ключ к сохранению ачивки! ");
                break;
        }

        // TODO анимацию ачивки !!!!!!!!!!!!!!!!!!!!!!
        // TODO анимацию ачивки !!!!!!!!!!!!!!!!!!!!!!
        // TODO анимацию ачивки !!!!!!!!!!!!!!!!!!!!!!
    }

    public void ResetAllAchied()
    {
        signsDetectedCount = 0;

        REG_Ach = 0;
        HOG_Ach = 0;
        TEST_Ach = 0;
        MEMO_Ach = 0;

        pokalPoints = 0;
        pokal_STATE = 0;
    }
}