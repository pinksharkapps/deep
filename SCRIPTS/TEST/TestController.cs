using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TEST
{
    public class TestController : MonoBehaviour
    {
        [SerializeField] private GameObject PanelForZadacha;
        [SerializeField] private GameObject start_Panel;

        // нужные задачи суем руками:
        [SerializeField] private Question_config[] _zadachi_Array;

        private TESTMenu _startMenuFile;
        private ZadachiPanel _zadachiPanelFile;

        public int zadachiCount = 1;
        private int currentZadacha = 0; // текущая задача, на старте ноль
        private int RightSolvedCount = 0;
        private int whichOneRight; // для отписок на стирании задачи


        // -------------- из урока ----------------
        public AssetReferenceSprite newSprite;
        private SpriteRenderer spriteRenderer;
        private AsyncOperationHandle<Sprite> spriteOperation;

        private const float PAUSE_SEE_IF_REIGHT_SOLVED = 2f;
        private const float PAUSE_BETWEEN_QUESTIONS = 1f;

        private int TEST_Ach;
        private int getWhereStopped;
        private bool StartedNotFromFirst;

        //------------------------------------------------------------------------------------------------------------
        
        private void Awake()
        {
            PanelForZadacha.SetActive(false);
            _zadachiPanelFile = PanelForZadacha.GetComponent<ZadachiPanel>();
            zadachiCount = _zadachi_Array.Length;
            
            GameManager.Instance.MoveMenuButtonToStandard();

            // создали новый словарь
            _zadachiPanelFile.QuestionSolved += OnQuestionSolved;

            TEST_Ach = GameManager.Instance._saveSystem.LoadData("TEST_Ach");

            getWhereStopped = GameManager.Instance._saveSystem.LoadData("TEST");

            // если остановились не на первой или последней задаче
            if (getWhereStopped != 0 && getWhereStopped != zadachiCount)
            {
                currentZadacha = getWhereStopped;  // ???
            }
        }
        
        //------------------------------------------------------------------------------------------------------------

        // НА КНОПКЕ "Решать" на панели старта
        public void StartGame()
        {
            PanelForZadacha.SetActive(true);
            GameManager.Instance.MoveMenuButtonForNarrowPlaces();

            // говорит панель задачи, сколько нахреначить квадратиков
            _zadachiPanelFile.CreateProgressItems(zadachiCount);

            // Пока задачи не решены
            RightSolvedCount = 0;
            
            // Выделить первую задачу
            currentZadacha = 0;
            _zadachiPanelFile.SetColor_SelectedTask(currentZadacha);
            
            // Открыть первую задачу из конфига
            NextZadacha();
        }


        // Экран приветсвия - игра СНАЧАЛА если задачи закончились
        public void ShowScreenPlayAgain()
        {
            PanelForZadacha.SetActive(false);
            GameManager.Instance.MoveMenuButtonToStandard();

            start_Panel.GetComponent<TESTMenu>()._textHeader.text = "Хочешь решить еще раз?";

            // Если до этого решили правильно 10 задач
            if (RightSolvedCount == _zadachi_Array.Length)
            {
                start_Panel.GetComponent<TESTMenu>()._textBody.text =
                    "Получи достижение в коллекцию, если все задачи будут решены правильно!";
            }

            // Игра запустится по нажатию на конпку - StartGame()
        }
        
        //------------------------------------------------------------------------------------------------------------

        #region ============= КОГДА ЗАДАЧУ РЕШИЛИ ==============

        private void OnQuestionSolved(bool SolvedRight) // вызываается по событию из ZadachiPanel
        {
            // Задача решена правильно
            if (SolvedRight)
            {
                _zadachiPanelFile.SetColor_whenSolved(currentZadacha, true);
                RightSolvedCount++;
            }
            else // ...иначе задача решена неправильно
            {
                _zadachiPanelFile.SetColor_whenSolved(currentZadacha, false);
            }

            // Выбрать и выделить следующую задачу
            currentZadacha++;
            
            // Проверка завершения решения билета...
            if (currentZadacha < _zadachi_Array.Length)
            {
                // Очистить результат через 2 секунды
                Invoke("ClearZadachu", PAUSE_BETWEEN_QUESTIONS);
                // Следующая задача
                Invoke("NextZadacha", PAUSE_BETWEEN_QUESTIONS);
            }
            else // иначе все задачи решены
            {
                // ------ если ачивки не было и все правильные, выводим ВЫВОДИМ АЧИВКУ и поздравление ------------------
                // TODO придумать чего получше

                if (TEST_Ach != 1 && !StartedNotFromFirst)
                {
                    string giventext = "Все задачи решены правильно!";
                    string btntext = "Здорово!";
                    GameManager.Instance.ShowPrizePanel(giventext, btntext, "TEST");
                    GameManager.Instance._saveSystem.SaveIntData("REG_Ach", 1);
                }

                // Играть еще?
                Invoke("ShowScreenPlayAgain", PAUSE_BETWEEN_QUESTIONS);
                // ShowScreenPlayAgain();
            }
            
        }

        #endregion


        #region ======== 1 задача ===========================================

        // СЕТАПИТ 1 задачу из конфига
        private async void NextZadacha()
        {
            // Выделить вопрос-кнопку следующей задачи
            _zadachiPanelFile.SetColor_SelectedTask(currentZadacha);
            
            // ===== РАЗДАЧА из конфига ======================================

            // ----- ставит в задачу текст из конфига

            string giveText = _zadachi_Array[currentZadacha].ques_Text;
            _zadachiPanelFile._questionText.text = giveText;

            // -----  выдает текст варианта 1

            string giveVar1 = _zadachi_Array[currentZadacha].ques_Var1;
            _zadachiPanelFile._button1.GetComponent<TestButton>().SetVarText(giveVar1);

            // -----  выдает текст варианта 2

            string giveVar2 = _zadachi_Array[currentZadacha].ques_Var2;
            _zadachiPanelFile._button2.GetComponent<TestButton>().SetVarText(giveVar2);

            // ----- выдаем и присваиваем картинку в задачу (из адрессейблов )
            // ----------- какая-то жесть из урока, но работает --------------------------------------------
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            // spriteOperation = newSprite.LoadAssetAsync();

            spriteOperation = Addressables.LoadAssetAsync<Sprite>(_zadachi_Array[currentZadacha].ques_Image);
            await spriteOperation;

            spriteOperation.Completed += SpriteLoaded;

            void SpriteLoaded(AsyncOperationHandle<Sprite> obj)
            {
                switch (obj.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        _zadachiPanelFile._zadImage.sprite = obj.Result;
                        break;
                    case AsyncOperationStatus.Failed:
                        Debug.LogError("Из урока: Sprite load failed.");
                        break;
                    default: // case AsyncOperationStatus.None:
                        break;
                }
            }
            // ==========================================================================================================

            // ----- берем из конфига правильный вариант и передаем в задачу, там считает варианты событий---------------
            whichOneRight = _zadachi_Array[currentZadacha].ques_Answer;
            _zadachiPanelFile.SetButtons(whichOneRight);
        }

        // =============================================================================================================

        #endregion
        
        // ----------------------------------------------- ОЧИСТКА ЗАДАЧИ -------------------------------------------
        
        private void ClearZadachu()
        {
            // убрать картинку
            if (spriteOperation.IsValid())
            {
                Addressables.Release(spriteOperation);
                Debug.Log("Урок: Successfully released sprite load operation.");
            }

            _zadachiPanelFile.SetButtonsToDefault(); // убрать крестики и сделать серый фон
        }

        //-----------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            _zadachiPanelFile.QuestionSolved -= OnQuestionSolved;
            GameManager.Instance._saveSystem.SaveIntData("TEST", currentZadacha);
        }
    }
}