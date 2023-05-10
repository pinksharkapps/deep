using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MENU;
using UniRx;
using Unity.VisualScripting; // !!! 
using UnityEngine;
using UnityEngine.AddressableAssets;
using Observable = UnityEngine.InputSystem.Utilities.Observable;

// уровни у меня по кругу
// пасть коммменты только к текущей строке, что там делается!
namespace HOG
{
    public class HOGController : MonoBehaviour
    {
        [SerializeField] private HogLevelConfig[] _hogLevelslArr; // натыкано руками на HOLDER

        private AssetReference _curAssetRef;
         
        private HOGwinPanel _hogWinPanel;
        private HOGWelcomePanel _hogWelcomePanel;
        private HOGMenu _hogmenuFile;
        private HOGSmallYellowPanel _eyePanel; // она была раньше желтая поэтому называется так

        private SaveSystem _saveSystem; // Для сохранения и загрузки номера текущего уровня
       
        private GameObject FileHOGlevelCreated; // null при первом запуске?
        private HogLevel _currentHogLevelFile; 
        
        private int _totalLevelsCount; // Для подсчета уровней в игре "Найди пешеходов"
        public int _totalPedsCount; // для выдачи левелу кол-ва уже нащелканных пешеходов
        private bool pedsCounted_10 = false;

        private bool FLAG_Level0_done_thistime = false; // ФЛАГ для вопроса играть ли сначала????
        private const float PAUSE_BEFORE_WINPANEL = 2f;
        

        private CompositeDisposable _disposable = new CompositeDisposable(); // для таймера
        // ----------------------------------------------------------------------------------------
        private int _currentLevel; // ГЛАВНАЯ ПЕРЕМЕННАЯ в HOG СЦЕНЕ, ГДЕ ХРАНИТСЯ ТЕКУЩИЙ УРОВЕНЬ 

        public int CurrentLevel // геттер и сеттер 
        {
            get { return _currentLevel; } // отдаем наружу другим файлам 
            set
            {
                _currentLevel = value;
                if (_currentLevel >= _totalLevelsCount) // по кругу 
                {
                    _currentLevel = 0;
                    FLAG_Level0_done_thistime = true;
                }
            }
        }


        //=========================================================================================================
        private void Awake()
        {
            // --------- Смотрит, сколько насовали руками уровней в массив (массив висит на HOLDER) 
            _totalLevelsCount = _hogLevelslArr.Length;

            // --------- Текущий уровень взять из сохранения -------------------------------------- 
            // _saveSystem = new SaveSystem(); // было !!!
            _saveSystem = GameManager.Instance._saveSystem;
            _currentLevel = _saveSystem.LoadData("HOG");

            // --------- Находит управление менюшки и управление призовой панели ------------------
            _hogmenuFile = FindObjectOfType<HOGMenu>();
            
            GameManager.Instance.MoveMenuButtonToStandard(); // кнопка "в меню"


            _hogWelcomePanel = FindObjectOfType<HOGWelcomePanel>();
            // подписывает ее на событие закрытия панели
            _hogWelcomePanel.HideHOGWelcomePanelEvent += StartAfterWelcome;
            _hogWelcomePanel.AgainButtonTapped_HogWelcomePanel += OnAgainTapped;


            _hogWinPanel = FindObjectOfType<HOGwinPanel>();
            // --------- ПОДПИСКА: Панель появится - дистроим старый уровень (если есть) ----------
            // _hogWinPanel.ShowHOGWinPanelEvent += DestroyLevel;
            // --------- ПОДПИСКА: Панель закроют - делаем новый уровень --------------------------
            _hogWinPanel.HideHOGWinPanelEvent += NewGame;
            _hogWinPanel.gameObject.SetActive(false); //!!!!!!!!!!!!!!!!!!

            //GameManager.Instance.CheckSceneForMenuBtn();
        }


        #region ================== WELCOM PANEL - первый запуск ===========================

        // ----------WELCOME panel только на первый запуск сцены ----------------------
        private void Start()
        {
            Debug.Log(" Hog Colntroller: Start");

            // ============ УНИВЕРСАЛЬНЫЙ ВЫЗОВ СПЛЕШ СКРИНА ======================================
            GameManager.Instance.SplashScreen();
            // ====================================================================================
            GameManager.Instance.MoveMenuButtonToStandard();
            

            // --- готовит текст приветствия 
            string textMsg;
            string textButton;
            WelcomText_SavedLevelNumber(out textMsg, out textButton);

            // открывает меню приветствия
            _hogmenuFile.ShowHogWelcomePanel(textMsg, textButton);

            if (_currentLevel == 0 && !FLAG_Level0_done_thistime)
                _hogWelcomePanel.HideAgainButton();


            // ТАЙМЕР для ачивки -------------------------------------------
            DOVirtual.DelayedCall(20, HogAchieve);
            // -------- а это не работает, "Timer" красный: ----------------
            // Observable.Timer(TimeSpan.FromSeconds(20f)) // надо 20 секунд 
            //     .Subscribe ( _ => // подписываемся
            //     {
            //         HogAchieve(); // что выполняем
            //         _disposable.Clear();
            //     }).AddTo( _disposable); // отписка
        }


        //================ ПОДГОТАВЛИВАЕТ текст на Welcome Panel в зависимости от сохраненного уровня ==============
        private void WelcomText_SavedLevelNumber(out string text, out string buttonText)
        {
            if (_currentLevel == 0 && !FLAG_Level0_done_thistime) // если на этом запуске уровень 0 еще играли
            {
                // надо в начале определить дефолтное OUT, у нас тут: ---  _currentLevel = 0 -------------------
                text = "Легко ли водителю заметить пешехода?";
                buttonText = "Играть";
                Debug.Log("Hog Controller: рассчитал что уровень = 0 --- !");
            }

            else //------ уровни еще есть ---------
            {
                text = "Продолжить игру в водителя:";
                buttonText = $"Играть уровень {_currentLevel + 1}";
            }
        }


        // ---------- запускается по событию HideHOGWelcomePanelEvent изнутри Welcome Hog Panel ---
        private void StartAfterWelcome() // запускает игру после Welcome
        {
            NewGame();
        }

        //---------- НАЖАТИЕ НА КНОПКУ "играть с начала" на Welcome Panel
        private void OnAgainTapped()
        {
            _currentLevel = 0;
        }

        #endregion


        // ========= новая игра ПО СОБЫТИЮ - ЗАКРЫТИЮ win Panel =============================================
        // --- лишняя функция из-за того, что нельзя засунуть параметр внутрь подписки-----------------------
        private void NewGame()
        {
            DestroyLevel();
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!// 
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!// 
            
            //------- СОЗДАТЬ УРОВЕНЬ
            // StartCoroutine(PauseBetween()); БЫЛО
            PauseBetween();
        }

        private async void PauseBetween() // ПАУЗА перед началом нового уровня!
        {
            // yield return new WaitForEndOfFrame(); //
            // yield return new WaitForSeconds(1f);
            CreateLevel();
            
            // TODO  ????????????????
            // ЧТО И ГДЕ ДОПИСАТЬ, ЧТОБЫ ОНО ЖДАЛО, ПОКА ПОДГРУЗИСЯ УРОВЕНЬ из адрессейблов?????
            // TODO  ????????????????
        }


        // ---------------------------------- Универсальный создаватель уровня -------------------------------
        private async void CreateLevel()
        {
            Debug.Log($" Hog Colntrooler:  Create level {_currentLevel}");
            
            GameManager.Instance.MoveMenuButtonForNarrowPlaces();
            // что инстейтиэйтить - берет из конфига, которые натыканы руками в массив
             FileHOGlevelCreated = await Addressables.InstantiateAsync(_hogLevelslArr[_currentLevel].LevelGameObject);
            
            // получает ссылку на управление созданным объектом
           _currentHogLevelFile = FileHOGlevelCreated.GetComponent<HogLevel>();
            
           _currentHogLevelFile.HogLevelWon += WhenLevelWon; // Подписка на событие Win в созданном уровне
           
        }


        // -------------------- Однозначный удалятель уровня ПО СОБЫТИЮ закрытия панели HOGWin ---------------
        private void DestroyLevel()
        {
            if (FileHOGlevelCreated != null)
            {
                Debug.Log($" Hog Colntrooler: Destroy level {FileHOGlevelCreated}");
                _currentHogLevelFile.HogLevelWon -= WhenLevelWon; // отписались на всякий случай перед дестроем

                // Destroy(FileHOGlevelCreated); //------ НЕ УДАЛЯЕТ ПРОВЕРЯЛА - ПОЧЕМУ????? а что это бы делало вообще?
                // Destroy(FileHOGlevelCreated.gameObject);
                Addressables.ReleaseInstance(FileHOGlevelCreated); //(_hogLevelslArr[_currentLevel].LevelGameObject);

                FileHOGlevelCreated = null;
            }
        }

        #region ======== WHEN LEVEL WON ============

        // ------------ Вызывается по event HogLevelWon из отдельного уровня вместе с панелькой поздравления -----------
        private void WhenLevelWon()
        {
            StartCoroutine(PauseBeforePanel()); // пауза
            // вынесла все остальное в корутину 
        }

        //-------------------- ПАУЗА и ВЫЗОВ ПАНЕЛИ WIN --------------------------------
        private IEnumerator PauseBeforePanel()
        {
            //------- ДЛЯ АЧИВКИ: храним тут кол-во всех найденных пешеходов
            int pedsToAdd = _currentHogLevelFile.FoundPeds;
            _totalPedsCount += pedsToAdd;
            

            CurrentLevel++; // <-------------------------------- ЛЕВЕЛ АП ! ! !
            _saveSystem.SaveIntData("HOG", CurrentLevel);

            // ! задаем ПОСЛЕ 
            yield return new WaitForSeconds(PAUSE_BEFORE_WINPANEL); //============= ПАУЗА

            _hogmenuFile.ShowHogWinPanel(CurrentLevel, _totalLevelsCount);
            GameManager.Instance.MoveMenuButtonToStandard();
            GameManager.Instance._audioManager.PlaySound("WIN");
        }

        #endregion


        //======================+==== АЧИВКА 10 пешиков за 20 секунд ====================
        public void Set10_pedsCounted()
        {
            pedsCounted_10 = true;
        }

        public void HogAchieve()
        {
            if (!pedsCounted_10)
            {
                Debug.Log("20 сек прошло, а 10 не накликали");
                return;
            } // проверка, послал ли нам левел, что ствло 20 штук
        
            GameManager.Instance.ShowPrizePanel("10 пешеходов за 20 секунд!", "Я могу!", "HOG");
            GameManager.Instance._saveSystem.SaveIntData("HOG_Ach", 1);
        }

        
        
        
        private void OnDestroy()
        {
            _hogWelcomePanel.HideHOGWelcomePanelEvent -= StartAfterWelcome;
            _hogWelcomePanel.AgainButtonTapped_HogWelcomePanel -= OnAgainTapped;

            _hogWinPanel.HideHOGWinPanelEvent -= NewGame;
            
        }
    }
}