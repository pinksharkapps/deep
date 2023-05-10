using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace TEST
{
    public class ZadachiPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _progressStartPoint;
        private Vector3 _startPointPos;

        [SerializeField] private GameObject _progressItemPrefab;

        [SerializeField] public Button _button1;
        [SerializeField] public Button _button2;

        [SerializeField] public TMP_Text _questionText;

        private Question_config _currentConfig;

        private TestProgressItem _itemFile;
        public GameObject _imageObj;

        private Dictionary<int, GameObject> _bar_numbers_Dick;

        private const float OFFSET = 150f; // расстояние между квадратиками

        [HideInInspector] public Image _zadImage;

        [HideInInspector] public bool OneIsRight;
        [HideInInspector] public bool TwoIsRight;

        public event Action<bool> QuestionSolved;


        // -------------------------------------------------------------------------------------------------------------
        private void Awake()
        {
            _itemFile = FindObjectOfType<TestProgressItem>();
            _startPointPos = _progressStartPoint.transform.position;

            _bar_numbers_Dick = new Dictionary<int, GameObject>();
            // _zadRenderer = _imageObj.GetComponent<SpriteRenderer>();
            _zadImage = _zadImage.GetComponentInChildren<Image>();
        }

        //------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            _button1.onClick.AddListener(Button1_Pressed);
            _button2.onClick.AddListener(Button2_Pressed);

            //----- тестовое красит ВСЕ в игре в красный ---------------------
            // SolvedSetColor(1,true);
            // SolvedSetColor(2,true);
            // SolvedSetColor(3,false);
        }


        #region ========== Расставляем 1 2 3 4 5 6 7 8 9 10 ===========

        public void CreateProgressItems(int ITEMS_COUNT)
        {
            for (int i = 0; i < ITEMS_COUNT; i++)
            {
                Vector3 thisItemPos = new Vector3(_startPointPos.x + i * OFFSET, _startPointPos.y, 0f);

                // ставит квадратик с номером
                GameObject newItem = Instantiate(_progressItemPrefab,
                    thisItemPos,
                    Quaternion.identity,
                    _progressStartPoint.transform);

                _bar_numbers_Dick.Add(i, newItem);

                // задает текст номера (вопрос-кнопки)
                _bar_numbers_Dick[i].GetComponent<TestProgressItem>().SetNumber(i + 1);
                // задает цвет вопрос-кнопки по умолчанию TODO может убрать сделав 1м в префабе?
                _bar_numbers_Dick[i].GetComponent<TestProgressItem>().SetDefColor();
            }

            // Проба цветов ============
            // _bar_numbers_Dick[0].GetComponent<TestProgressItem>().SetSelectedColor();
            // _bar_numbers_Dick[2].GetComponent<TestProgressItem>().SetGreen();
        }
        
        #endregion

        //------------------------------------------------------------------------------------------------------------

        #region ========== Логика покраски кнопок =======

        // =================== Устанавливаем кнопки получивши из TestController, какая правильная ======================
        public void SetButtons(int whichOneRight)
        {
            OneIsRight = false;
            TwoIsRight = false;

            switch (whichOneRight)
            {
                case 1:
                    OneIsRight = true;
                    break;

                case 2:
                    TwoIsRight = true;
                    break;

                default:
                    Debug.Log(" ZadachiPanel: передали неправильный номер ответа ");
                    break;
            }
        }


        private void Button1_Pressed()
        {
            GameManager.Instance._audioManager.PlaySound("PRESS");

            if (OneIsRight) // Нажали 1 + и это Правильно
            {
                _button1.GetComponent<Image>().sprite = _button1.GetComponent<TestButton>()._greenButtonBg;
                _button1.GetComponent<TestButton>().ShowTick();
                QuestionSolved?.Invoke(true);
            }
            else
            {
                _button1.GetComponent<Image>().sprite = _button1.GetComponent<TestButton>()._redButtonBg;
                _button1.GetComponent<TestButton>().ShowCross();
                QuestionSolved?.Invoke(false);
            }
        }


        private void Button2_Pressed()
        {
            GameManager.Instance._audioManager.PlaySound("PRESS");

            if (TwoIsRight) // Нажали 2 + и это Правильно
            {
                _button2.GetComponent<Image>().sprite = _button2.GetComponent<TestButton>()._greenButtonBg;
                _button2.GetComponent<TestButton>().ShowTick();
                QuestionSolved?.Invoke(true);
                StartCoroutine(PlayDelayedSound("YES"));
            }
            else // Нажали 2 а это НЕправильно
            {
                _button2.GetComponent<Image>().sprite = _button2.GetComponent<TestButton>()._redButtonBg;
                _button2.GetComponent<TestButton>().ShowCross();
                QuestionSolved?.Invoke(false);
                StartCoroutine(PlayDelayedSound("NO"));
            }
        }

        //---- универсальный метод звука решенной задачи ---
        private static IEnumerator PlayDelayedSound(string whichSound)
        {
            yield return new WaitForSeconds(1f);
            GameManager.Instance._audioManager.PlaySound(whichSound);
        }

        //------------------------------------------------------------------------------------------------------------

        public void SetButtonsToDefault()
        {
            // Прячет крестик / галочку с проверкой (ОНА НАДО???)
            var bu1 = _button1.GetComponent<TestButton>();
            if (bu1.yesNoImage.IsActive())
            {
                bu1.HideCrossOrTick();
            } // спрячет картинку

            _button1.GetComponent<Image>().sprite = bu1._grayButtonBg;

            var bu2 = _button2.GetComponent<TestButton>();
            if (bu2.yesNoImage.IsActive())
            {
                bu2.HideCrossOrTick();
            } // спрячет картинку

            _button2.GetComponent<Image>().sprite = bu2._grayButtonBg;
        }

        #endregion

        //------------------------------------------------------------------------------------------------------------

        #region ========== Покраска 1 2 3 4 5 6 7 8 9 10 ==========

        public void SetColor_whenSolved(int number, bool isSolved)
        {
            if (isSolved)
            {
                _bar_numbers_Dick[number].GetComponent<TestProgressItem>().SetGreen();
            }
            else
            {
                _bar_numbers_Dick[number].GetComponent<TestProgressItem>().SetRed();
            }
        }

        public void SetColor_SelectedTask(int number)
        {
            _bar_numbers_Dick[number].GetComponent<TestProgressItem>().SetSelectedColor();
        }

        public void SetColor_Default(int number)
        {
            _bar_numbers_Dick[number].GetComponent<TestProgressItem>().SetDefColor();
        }
        
        #endregion

        // -------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            _button1.onClick.RemoveListener(Button1_Pressed);
            _button1.onClick.RemoveListener(Button2_Pressed);
        }
    }
}