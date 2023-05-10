using System;
using System.Collections;
using System.Diagnostics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

//-------------------- ТОЛЬКО ВЫВОД ТЕКСТА В МЕНЮ !!! внутри !!! LEVEL --------------------------------
//-------------- с корутиной подсказки на случай если юзер нашел пешехода и тупит ---------------------

namespace HOG
{
    public class HOGMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text pedCount;
        [SerializeField] private TMP_Text levelTmp;

        [SerializeField] private GameObject _HOGWelcomePanel; // GameObject - панель на первый запуск
        [SerializeField] private GameObject _HOGWinPanel; // GameObject - панель поздравления на win
        [SerializeField] private GameObject _eyePanel; // панелька "нашли пешехода"
        [SerializeField] private GameObject _TopPanel; // уровень и подсказка
        private HOGwinPanel _hogWinPanelFile; // компонент упраления панелью (наследник basePanel)
        private HOGWelcomePanel _hogWelcomePanelFile; // панель поздравления на win
        private HOGSmallYellowPanel _smallYellowPanelFile;

        private int allPedsCount = 0;
        private int foundPeds = 0;
        private int ostalosPeds = 0;

        private Sequence _FadeSuggest;
        private string stringFadeFrom;
        private string stringFadeTo;


        [SerializeField] private GameObject _eyeDropPos; // пустой, установить!
        private Vector2 eyePanelDefPOs;
        private Vector2 eyePanelDropPos;
        private Sequence _eyeSequence;
        private const float DROP_SPEED = 1f;
        private const float EYE_PANEl_READ_TIME = 5f;

        private void Awake()
        {
            _HOGWinPanel.SetActive(false);
            _hogWinPanelFile = _HOGWinPanel.GetComponent<HOGwinPanel>();
            _hogWelcomePanelFile = _HOGWelcomePanel.GetComponent<HOGWelcomePanel>();
            _smallYellowPanelFile = _eyePanel.GetComponent<HOGSmallYellowPanel>();


            eyePanelDefPOs = _eyePanel.gameObject.transform.position;
            eyePanelDropPos = _eyeDropPos.transform.position;
            _eyePanel.gameObject.SetActive(false);
        }


        #region ======== SET LEVEL ========

        // ---------------------------------------------------------------------------------------
        // вызывается из Level - у TOP панели своего файла управления нет, она просто выводин инфу
        public void SetTopPanel(int level, int allPeds) // выводит № уровня НА СТАРТЕ УРОВНЯ
        {
            allPedsCount = allPeds; // обнyновяем !!! <------------------------------------- ! !
            foundPeds = 0;
            ostalosPeds = allPedsCount; // пересчитываем пешиков

            _TopPanel.SetActive(true);
            levelTmp.text = $"Уровень {level + 1}";

            // запускает корутину подсказки на случай, если юзер тупит
            // SuggestHowMuchLeft(); 
        }


        //------------ текст на старте уровня "Найди столько-то" -----------------
        public void SayWhatToDo(int xxx)
        {
            allPedsCount = xxx;
            if (allPedsCount == 1)
            {
                pedCount.text = "Видишь пешехода?";
            }

            else
            {
                switch (allPedsCount)
                {
                    case 2:
                        pedCount.text = $"Найди двух пешеходов";
                        break;
                    case 3:
                        pedCount.text = $"Найди трёх пешеходов";
                        break;
                    case 4:
                        pedCount.text = $"Найди четырёх пешеходов";
                        break;
                    default:
                        pedCount.text = $"Найди {allPedsCount} пешеходов";
                        break;
                }
            }
        }


        //=================== подсказка с корутиной ?? =====================
        private void SuggestHowMuchLeft()
        {
            string whattoo = Line2Grammatics(ostalosPeds);
            StartCoroutine(WaitTillSuggest(whattoo)); // подсказка появляется если юзер тупит (whatto) секунд
        }

        private IEnumerator WaitTillSuggest(string whattoo)
        {
            yield return new WaitForSeconds(8f);
            FadeTextChange(whattoo, 2);
        }

        #endregion


        #region ======= Small Yellow Panel =====================================

        //================== ПОКАЗЫВАНИЕ ЖЕЛТОЙ МЕНЮШЕЧКИ ===================
        public void ShowSmallYellow(int foundPeds)
        {
            ostalosPeds = allPedsCount - this.foundPeds;

            _eyePanel.SetActive(true);

            // запомнили текст первой строки и вывели
            string xxx = "Заметил пешехода!";
            _smallYellowPanelFile.SetTextOne(xxx);

            // посчитали, сколько осталось, прогнали грамматику для 2 строки и вывели
            ostalosPeds = allPedsCount - foundPeds;
            string yyy = Line2Grammatics(ostalosPeds);
            _smallYellowPanelFile.SetTextTwo(yyy);


            // ------------------- АНИМАЦИЯ ОПУСКАНИЯ ПАНЕЛИ С ГЛАЗОМ ----------------------------------------------
            _eyeSequence = DOTween.Sequence();
            _eyeSequence.Append(_eyePanel.transform.DOMoveY(eyePanelDropPos.y, DROP_SPEED))
                .SetEase(Ease.OutBack);
            _eyeSequence.AppendInterval(EYE_PANEl_READ_TIME);
            _eyeSequence.AppendCallback(SayTailsToPlay); // ----- идет в желтую панель и включает анимашку хвостиков
            _eyeSequence.Append(_eyePanel.transform.DOMoveY(eyePanelDefPOs.y, DROP_SPEED * 0.5f));
            // _eyeSequence?.Kill(true); если тут кильнуть - 2й раз не запустит
        }

        private void SayTailsToPlay()
        { 
            _smallYellowPanelFile.PlayTails(); // ----- идет в желтую панель и включает анимашку хвостиков
        }



        //------------ СТРОКА 2 сколько осталось - грамматика -----------
        private string Line2Grammatics(int xxx)
        {
            string line2;

            if (ostalosPeds == 0) // если последний
            {
                line2 = "А пешеход тебя не заметил!";
            }

            else // иначе считаем грамматику
            {
                if (xxx is >= 1 and < 5)
                {
                    line2 = $"Найди ещё {xxx} пешехода";
                }


                else
                {
                    line2 = $"Найди ещё {xxx} пешеходов";
                }
            }

            return line2; // присвоили новый текст
        }

        #endregion


        #region ======= ПЛАВНАЯ СМЕНА ТЕКСТА =======

        //=====================================================================================================
        //============= ПЛАВНАЯ СМЕНА ТЕКСТА ==================================================================
        private void FadeTextChange(string to, float sec) // на какой текст, сколько ждем
        {
            SaveTextTo(to);

            _FadeSuggest = DOTween.Sequence()
                .Append(pedCount.DOFade(0, sec))
                .AppendCallback(ChangeText)
                .Append(pedCount.DOFade(225, sec));
        }

        private void SaveTextTo(string to)
        {
            stringFadeTo = to; // пришлось запоминать глобально потому что нельзя передавать переменную в дотвин
        }

        private void ChangeText()
        {
            pedCount.text = stringFadeTo; // КУДА СУЕМ ТЕКСТ
        }
        //===================== до сих ==================================================================

        #endregion


        #region ======= ПАНЕЛЬ Welcome =======

        //============= Показывается только при входе на сцену ! ==========================================
        public void ShowHogWelcomePanel(string givenText, string text4button)
        {
            _HOGWelcomePanel.SetActive(true); // 1 - включает
            _hogWelcomePanelFile.SetTextOne(givenText); // 2 - текст на панели

            if (text4button != null)
            {
                _hogWelcomePanelFile.SetTextTwo(text4button);
            } // 3 - меняет текст панели через функцию
        }


        //============= + скрыть оную =====================================================================
        public void HideHogWelcomePanel(string givenText, string text4button)
        {
            _HOGWelcomePanel.SetActive(false); // скрыли панель 
        }
        //=================================================================================================

        #endregion


        #region ======= Win Panel =======

        //============= МЕТОД ПОКАЗЫВАНИЯ - СИНЕЙ ПРИЗОВОЙ ПАНЕЛИ - ===============================
        public void ShowHogWinPanel(int lvl, int levelsCount)
        {
            _TopPanel.SetActive(false); // -------- эстетики ради, скрываем панель с номером уровня

            // РАССЧЕТ ТЕКСТА в зависимости от присланных уровня и levelsCount

            string text1 = null;
            string textOnButton = null;

            if (lvl <= 0) // --- ибо обнуляется
            {
                text1 = "Ты прошёл все уровни!";
                textOnButton = "Играть сначала"; // покажет панель 
            }

            else
            {
                text1 = "Заметил всех пешеходов!";
                textOnButton = $"Перейти на уровень {lvl + 1}!";
            }

            // ------ работа с самой панелью Win --------
            _HOGWinPanel.SetActive(true); // 1 - включает панель Win

            _hogWinPanelFile.SetTextOne(text1); // 2 - текст на панели
            _hogWinPanelFile.SetTextTwo(textOnButton); // 3 - меняет текст панели через функцию
        }
        //=================================================================================================
        #endregion


        private void OnDestroy()
        {
            _eyeSequence?.Kill(true);
            _FadeSuggest?.Kill(true); // кильнуть секвенцию ?!
        }
    }
}
