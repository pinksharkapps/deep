using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MENU;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Главный файл в игре МЕМО - управление бородой и уровни тут!!!!
namespace MEMO
{
    public class MemoController : MonoBehaviour
    {
        private MemoGaishnik _gaishnik;
        private MemoBoardController _board;

        [SerializeField] private MemoTimeBar _bar;

        [SerializeField] private TMP_Text _levelNumberTMP; // текстовое поле номер уровня
        [SerializeField] private TMP_Text _levelNameTMP; // текстовое поле название уровня

        [SerializeField] private LevelConfig[] _LevelsArray; // массив, куда руками напихать уровни
        private int _currtentlevel = 0;

        [SerializeField] private GameObject _StartButtonsPanel;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _start_FirstLevel_Button;


        [SerializeField] private GameObject _appearPoint;
        private Vector2 _appearPointPos;
        private Vector2 _startPointPos;

        // private float PAUSE_ON_START = 0.5f; // запасная пауза...
        // private float PAUSE_AFTER_TIMER_FALLS = 1f;

        private float PAUSE_WHILE_TIMER_FALLS = 1.0f;


        private float TIME_TO_REMEMBER_SIGNS; // пауза в уровне берется из конфиг

        //-----------------------------------------------------------------------------------------

        private void Awake()
        {
            _gaishnik = FindObjectOfType<MemoGaishnik>();
            _board = FindObjectOfType<MemoBoardController>();
            
            _currtentlevel = GameManager.Instance._saveSystem.LoadData("HOG");
            GameManager.Instance.MoveMenuButtonToStandard();

            _startPointPos = _bar.transform.position; // позиции старта и выпадения тайм-бара
            _appearPointPos = _appearPoint.transform.position;

            // прячем борду и показываем кнопки
            _board.gameObject.SetActive(false);
            _StartButtonsPanel.SetActive(true);

            _board.AllPairsCompleted += LevelCompleted;
            _board.ShowSignDescription += SetSignDescription;

            // Спокойное лицо гаишника
            _gaishnik.State_Serious();
            
        }

        private void Start() // Общий ПЕРВЫЙ запуск
        {
            _gaishnik.BubbleAnmationIn(1.0f, 1.5f, "Поиграем в игру \"Память\"!");
        }

        // ----------------------------------------------------------------------------------------------------------

        // Кнопка "Играть сначала"
        public void StartNewGame()
        {
            GameManager.Instance.GetComponent<AudioManager>().PlaySound("PRESS");
            _currtentlevel = 0;
            StartNewLevel();
        }

        // Кнопка "Продолжить играть..."
        public void StartNewLevel() // Назначено на кнопку "играть!"
        {
            GameManager.Instance.GetComponent<AudioManager>().PlaySound("PRESS");
            _StartButtonsPanel.SetActive(false); // спрятать кнопки
            _board.gameObject.SetActive(true); // показать борду

            // Анимация гаишника
            _gaishnik.BubbleAnmationIn(0, 1.5f, "Найди одинаковые знаки!");

            // Подготовка уровня
            SetupLevel(_currtentlevel);
        }

        //-----------------------------------------------------------------------------------------------------------

        // Подготовка уровня
        private async void SetupLevel(int xxx)
        {
            if (xxx > _LevelsArray.Length) // обнуляет, если остановились на последнем уровне
            { xxx = 0; }

            // Отобразить номер и назване уровня
            _levelNumberTMP.text = "Уровень " + _LevelsArray[xxx].levelNumber;
            _levelNameTMP.text = _LevelsArray[xxx].levelName;
            // Подсказка на баре
            _bar.SetText("Запоминай...");

            // Время открытых знаков
            TIME_TO_REMEMBER_SIGNS = _LevelsArray[xxx].timeToShow;

            // Появляются квадраты
            _board.MemosPrepair_and_Instantiate(_LevelsArray[xxx]);

            // Временно перевернуть знаки для запоминания
            _board.FlipAll_ToSign(TIME_TO_REMEMBER_SIGNS);

            // Дать время на запоминание
            TimeBarDropdown();
            
            // Пауза
            await UniTask.Delay(TimeSpan.FromSeconds(TIME_TO_REMEMBER_SIGNS));

            // Подсказка на баре
            _bar.SetText("Переверни знак!");
        }


        // Уровень успешно завершен
        private void LevelCompleted()
        {
            // Удаление предыдущего уровня
            _board.DeleteBoard();
            
            _currtentlevel++;
            GameManager.Instance._saveSystem.SaveIntData("HOG", _currtentlevel);

            // Пройдены ли все уровни...
            if (_currtentlevel > _LevelsArray.Length - 1)
            {
                _currtentlevel = 0;
                MemoGiveAchieve();
            }
            
            _StartButtonsPanel.SetActive(true);
        }


        // Отобразить описание знака
        private void SetSignDescription(string SignDescr) 
        {
            _bar.SetText(SignDescr);
        }

        // ----------------------------------------------------------------------------------------------------------

        // ----- функция анимации выпадания вниз тайм бара
        private void TimeBarDropdown()
        {
            _bar.transform.DOMoveY(_appearPointPos.y, PAUSE_WHILE_TIMER_FALLS)
                .SetEase(Ease.OutQuad);

            // ставит бару время взятое из левел-конфига
            _bar.TimeStartCountDown(TIME_TO_REMEMBER_SIGNS);
        }


        // использовать в конце уровня: взлетание полоски времени наверх ------------
        private void TimeBarFlyUp()
        {
            _bar.transform.DOMoveY(_startPointPos.y, 0.4f);
        }


        // --------------------------------------------- Выдача ачивки ----------------------------------------------

        private void MemoGiveAchieve()
        {
            GameManager.Instance.ShowPrizePanel("Прошёл все уровни. У тебя крутая память!", "Играть ещё!", "MEMO");
            GameManager.Instance._saveSystem.SaveIntData("MEMO_Ach", 1);
        }

        // ----------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            _board.AllPairsCompleted -= LevelCompleted;
            _board.ShowSignDescription -= SetSignDescription;
        }
        
    }
}