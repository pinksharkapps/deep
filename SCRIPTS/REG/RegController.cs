using System;
using System.Collections;
using System.Drawing;
using DG.Tweening;
using MENU;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace REG
{
    public class RegController : MonoBehaviour
    {
        [SerializeField] private GameObject _regulirovschik;
        private Regulirovschik _regFile;

        [SerializeField] private GameObject _yesNoButtons;
        [SerializeField] private GameObject _boy;
        private PedBoy _boyFile;
        private const float PED_SPEED = 3f;

        // точки на карте
        [SerializeField] private GameObject point_1;
        [SerializeField] private GameObject point_2;
        [SerializeField] private GameObject point_3;
        [SerializeField] private GameObject point_4;
        [SerializeField] private GameObject point_5;
        [SerializeField] private GameObject point_6;
        [SerializeField] private GameObject point_7;
        [SerializeField] private GameObject point_8;

        private PointFile[] _pointsArray;

        private AudioManager _audioManager;

        private RegLogic _regLogic;
        private int regPosition;
        private bool GoUp;
        private bool GoRight;

        public event Action<int> RightAnswer;
        private event Action Answered;

        // private bool podskazkaShownThisTime; // будет использоваться на сложных уровнях
        private const float PAUSE_BETWEEN_TASKS = 2f;

        private int _rightAnswersCount = 0;
        private int _currentPoint = 0;

        // -----------------------------------------------------------------------------------------------------------

        public int RightAnswersCount
        {
            get { return _rightAnswersCount; } // отдаем наружу другим файлам 

            set
            {
                _rightAnswersCount = value;
                RightAnswer?.Invoke(RightAnswersCount); //инвокает в меню на каждое изменение переменной
                
                if (_rightAnswersCount == 10) // Вызывает ачивку "10 правильных"
                {
                    RegAchieves();
                }
            }
        }

        // -----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            _audioManager = GameManager.Instance.GetComponent<AudioManager>(); 
            GameManager.Instance.MoveMenuButtonToStandard();
            
            
            _boyFile = _boy.GetComponent<PedBoy>();

            _pointsArray = new PointFile[8];
            _pointsArray[0] = point_1.GetComponent<PointFile>();
            _pointsArray[1] = point_2.GetComponent<PointFile>();
            _pointsArray[2] = point_3.GetComponent<PointFile>();
            _pointsArray[3] = point_4.GetComponent<PointFile>();
            _pointsArray[4] = point_5.GetComponent<PointFile>();
            _pointsArray[5] = point_6.GetComponent<PointFile>();
            _pointsArray[6] = point_7.GetComponent<PointFile>();
            _pointsArray[7] = point_8.GetComponent<PointFile>();
            
            _regLogic = new RegLogic();
            _regFile = _regulirovschik.GetComponent<Regulirovschik>();
            Answered += ThisAnsweredStartNextTask;

            // Пешеход в первой позиции
            _boy.transform.position = point_1.transform.position;
        }


        private void Start()
        {
            
        }

        private void ThisAnsweredStartNextTask()
        {
            Invoke("NextTask", PAUSE_BETWEEN_TASKS);
        }


        //================= МЕТОДЫ НА КНОПКАХ идти/стоять ===================
        public void IGoPressed()
        {
            bool canHeMove = CheckLogic();

            if (canHeMove)
            {
                MovePed();
                _boyFile.ShowRightAnswer();
                _audioManager.PlaySound("YES");
                RightAnswersCount++;
                return;
            }

            _boyFile.ShowWrongAnswer();
            _audioManager.PlaySound("NO");
            _rightAnswersCount = 0;
            Answered?.Invoke();
        }


        public void IStandPressed()
        {
            bool canHeMove = CheckLogic();

            if (canHeMove)
            {
                _boyFile.ShowWrongAnswer();
                _audioManager.PlaySound("NO");
                _rightAnswersCount = 0;
                Answered?.Invoke();
                return;
            }

            _boyFile.ShowRightAnswer();
            _audioManager.PlaySound("YES");
            // _rightAnswersCount++;
            RightAnswersCount++;
            Answered?.Invoke();
        }

        private void ShowHide_YesNoButtons()
        {
            _yesNoButtons.SetActive(!_yesNoButtons.activeSelf);
        }

        //-----------------------------------------------------------------------------------------------------------

        // ПРОВЕРЯЕТ и РЕАГИРУЕТ
        public bool CheckLogic()
        {
            // прячет кнопки
            ShowHide_YesNoButtons();

            // идет проверяет логику
            bool canHeMove = _regLogic.CalculateLogic(_regFile.StateName, _currentPoint);
            return canHeMove;
        }

        //-----------------------------------------------------------------------------------------------------------

        #region ============================ СМЕНА ЗАДАНИЯ ============================

        private void NextTask()
        {
            // ----- Новая позиция регулировщику ---
            _regFile.RandomState();

            // ТЕЛЕПОРТ В НОВУЮ ТОЧКУ
            // Начало с 0...
            // Следующая - 1
            Vector2 whereToTeleport = _pointsArray[_currentPoint].Pos;
            
            {
                _boy.transform.position = whereToTeleport;

                // Пешеход повернут к нам лицом или затылком
                if (_currentPoint is 4 or 6)
                    _boyFile.SetBoyFrontFace();
                else _boyFile.SetBoyBackFace();
                
                // ----- FLIP для пешехода  -----
                Vector3 newScale = _boy.transform.localScale;
                if (_currentPoint is 0 or 4)
                    newScale.x = Mathf.Abs(newScale.x); // Возвращает только положительное число
                else newScale.x = -1 * Mathf.Abs(newScale.x);
                
                _boy.transform.localScale = newScale;
            }

            // podskazkaShownThisTime = false; // очистить ожидание подсказки

            ShowHide_YesNoButtons(); // показать кнопки да / нет
        }

        #endregion


        #region ============================ Pesheod moves ============================

        // -------- ПРОГРАММА ДВИЖЕНИЯ ПЕШЕХОДА 
        private void MovePed()
        {
            _boyFile.SetBoyGoing();
            _audioManager.StartTopat(); // ставит флаг "можно топать" для залупливания;
            _audioManager.PlaySound("STEPS"); // ЗВУК

            // ПО УМОЛЧАНИЮ СМОТРИТ ------>  x: 1
            switch (_currentPoint)
            {
                case 0:
                    // anim: UP   ToLeft
                    //FlipOrNot(!GoRight);
                    _boy.transform.DOMove(point_2.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                case 1:
                    // anim: DOWN  ToRight
                    // FlipOrNot(GoRight);
                    _boy.transform.DOMove(point_1.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                case 2:
                    // anim: UP   ToRight
                    // FlipOrNot(!GoRight);
                    _boy.transform.DOMove(point_4.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                case 3:
                    // anim: DOWN   ToRight
                    //FlipOrNot(GoRight);
                    _boy.transform.DOMove(point_3.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                case 4:
                    // anim: DOWN  ToLeft
                    // FlipOrNot(!GoRight);
                    _boy.transform.DOMove(point_6.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                case 5:
                    // anim:  UP  ToRight
                    // FlipOrNot(GoRight);
                    _boy.transform.DOMove(point_5.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                case 6:
                    // anim:   DOWN  ToRight
                    // FlipOrNot(GoRight);
                    _boy.transform.DOMove(point_8.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                case 7:
                    // anim:   UP  ToLeft
                    // FlipOrNot(!GoRight);
                    _boy.transform.DOMove(point_7.transform.position, PED_SPEED).SetEase(Ease.Linear)
                        .OnComplete(() => BoyStopping());
                    break;
                default:
                    Debug.Log("Неправильный кур-поинт");
                    break;
            }
        }

        #endregion

        private void BoyStopping()
        {
            _boyFile.SetBoyStanding();
            _audioManager.StopTopat(); // ЗВУК - остановка цикла

            // Следующая точка - идем по перекрестку против часовой стрелки
            _currentPoint += 2;
            if (_currentPoint > _pointsArray.Length - 1)
                _currentPoint = 0;
            
            Answered?.Invoke();
        }


        public void RegAchieves()
        {
            GameManager.Instance.ShowPrizePanel("10 правильных ответов подряд!", "Чудно!", "REG");
            GameManager.Instance._saveSystem.SaveIntData("REG_Ach", 1);
        }


        private void OnDestroy()
        {
            Answered -= ThisAnsweredStartNextTask;
        }
    }
}