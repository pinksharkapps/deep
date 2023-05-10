using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

// Управляет - -  БОРДОЙ - - - только показать на старте все, перезапуск !
namespace MEMO
{
    public class MemoBoardController : MonoBehaviour
    {
        [SerializeField] private GameObject _memoItem;
        [SerializeField] private GameObject _startPoint;

        private MemoTimeBar _timeBar;
        
        private LevelConfig _currentLevelConfig;
        private float _pauseInLevel;
        //private Dictionary<int, GameObject> _instaDick; // массив для инстантиэйтов
        private MemoItem[] _instaArray;

        private Vector2 startPos;
        
        private float MEMO_SQWR_SIZE = 1.8f;
        private float MEMO_OFFSET = 1f;
        
        private int MemoHorizontCount = 4;
        private int MemoVerticalCount = 3;
        
        // Количество пар
        private int pairsCount;

        private bool signDesriptionShown;
        
        
        // -------------
       
        private const float PAUSE_BETWEEN_ALL_OPEN = 0.2f;

        public event Action AllPairsCompleted;
        public event Action CheckCompleted;

        public event Action <string> ShowSignDescription;

        // для работы проверки:
        private int pairsFound = 0;
        private string firstOpened = null;
        private string secondOpened;
        
        
        // ---------------------------------------------------------------------------------------
        private void Awake()
        {
            // _abstractDick = new Dictionary<int, GameObject>(HOW_MUCH_MEMOS); // инит словаря на 12 знаков
            //_realDick = FindObjectsOfType<DisplaySign>();
            startPos = _startPoint.transform.position;

            _timeBar = FindObjectOfType<MemoTimeBar>();
        }

        #region =============== Random + Instantiate Signs ========

        //===================== метод рандома + расстановки знаков ===================================================
        
        public void MemosPrepair_and_Instantiate(LevelConfig xxx)
        {
            _currentLevelConfig = xxx;

            // ОЧИСТИТЬ МАССИВ ПЕРЕД СОЗДАНИЕМ ------- TODO перенести в метод сноса борды
            _instaArray = new MemoItem[12];

            //--------- заполнить массив -------------
            Vector2 thisMemoPos = new Vector2();

            // --------- Перемешать знаки (логику перемешивания писал муж!)------------
            int memoCount;

            // Количество знаков
            memoCount = MemoVerticalCount * MemoHorizontCount;
            // Количество пар знаков
            pairsCount = memoCount / 2;

            // Пока найдено ноль пар
            pairsFound = 0;

            int[] pairs;
            pairs = new int[memoCount];

            // Пока знаки идут последовательно
            for (int i = 0; i < pairsCount; i++)
            {
                pairs[i * 2] = i;
                pairs[(i * 2) + 1] = i;
            }

            // Перемешиваем знаки
            for (int i = 0; i < MemoVerticalCount * MemoHorizontCount; i++)
            {
                int r = Random.Range(0, memoCount);
                int val1 = pairs[i];
                int val2 = pairs[r];
                // Обмен номерами знаков
                pairs[i] = val2;
                pairs[r] = val1;
            }

            // Расставить и запихать в в массив _instaArray
            for (int v = 0; v < MemoVerticalCount; v++)
            {
                for (int h = 0; h < MemoHorizontCount; h++)
                {
                    thisMemoPos = new Vector2(startPos.x + h * (MEMO_SQWR_SIZE + MEMO_OFFSET),
                        startPos.y - v * (MEMO_SQWR_SIZE + MEMO_OFFSET));
                    GameObject thisMemo = Instantiate(_memoItem, thisMemoPos, Quaternion.identity);
                    
                    // засовываем ее в словарь с 1 по 12
                    int sign_numb = h + (v * MemoHorizontCount);
                    
                    // присваиваем ему ID знака из конфига
                    string ID = _currentLevelConfig.SinsForThisLevel[pairs[sign_numb]].SignConfig.name;
                    thisMemo.GetComponentInChildren<DisplaySign>().mySignID = ID;
                    
                    // присваиваем ему нужную картинку знака из конфига
                    Sprite sprite = _currentLevelConfig.SinsForThisLevel[pairs[sign_numb]].SignConfig.img;
                    thisMemo.GetComponentInChildren<MemoSignImage>().GetComponentInChildren<SpriteRenderer>().sprite =
                        sprite;
                    
                    // присваиваем ему описание знака из конфига
                    string description =  _currentLevelConfig.SinsForThisLevel[pairs[sign_numb]].SignConfig.decription;
                    thisMemo.GetComponentInChildren<DisplaySign>().mySignDesription = description;
                    
                    // !!! Зановим в МАССИВ _instaArray !!!
                    //===================== класс MemoItem в самом низу =====
                    _instaArray[sign_numb] = new MemoItem();
                    _instaArray[sign_numb].thisId = ID;
                    _instaArray[sign_numb].thisDescription = description;
                    _instaArray[sign_numb].thisObject = thisMemo;
                    //============================================================

                    // на каждое открытие должна начинаться большая проверка
                    thisMemo.GetComponent<DisplaySign>().MemoOpened += CheckPairs;
                }
            }
        }
        #endregion

        // ----------------------------------------------------------------------------------------------------------
        
        #region ======== Check Pairs ========
        
        // =============================== ПРОВЕРКА НА СОВПАДЕНИЯ НАЗВАНИЙ ЗНАКА МОЯ ================================
            private void CheckPairs(string id_of_Clicked_Memo)
            {
                Find_SignDescription_and_Invoke(id_of_Clicked_Memo); // ПЕРЕПИСАНО, работает !!
                
                // BetonizeAll(); // не нажимается ничего пока проверяет или открыто ! TODO - КАК НЕ БЕТОНИРОВАТЬ МЕНЮ?

                // Если до этого не открывали, запоминаем, что открыли сейчас на следующий раз
                if (firstOpened == null) 
                { 
                    Debug.Log($" Открыли первую Memo {id_of_Clicked_Memo}...");
                    // присваиваем только что открытую в первую
                    firstOpened = id_of_Clicked_Memo;
                    return;
                    
                }

                // Если выбранная пара знаков не совпадает...
                if (id_of_Clicked_Memo != firstOpened)
                {
                    Debug.Log($"Выбранная пара знаков не совпала! Первый: {firstOpened} - Второй: {id_of_Clicked_Memo}");
                    // См. IfNotSolved_WrapToBack() в DisplaySign 
                    CheckCompleted?.Invoke();
                    firstOpened = null;

                    signDesriptionShown = true;
                    StartCoroutine(HideSignDescription()); // убираем описание знаков после паузы 2 сек ....
                    
                    return;
                    
                    IEnumerator HideSignDescription() 
                     {
                          yield return new WaitForSeconds(2f);
                          if (signDesriptionShown)
                          {
                             _timeBar.SetFadedText("..."); // убираем название знака, когда они закрытые// ТУТ ЛИ?????
                              signDesriptionShown = false;
                          }
                     }
                    
                }

                
            
                
                // Если выбранная пара знаков совпадает...
                if (id_of_Clicked_Memo == firstOpened)
                {
                    Debug.Log($"Выбранная пара знаков совпала! Первый: {firstOpened} - Второй: {id_of_Clicked_Memo}");
                    firstOpened = null;

                    // Пометить пару найденных Memo как найденные
                    MarkPairsMemo_as_Solved(firstOpened, id_of_Clicked_Memo);
                    
                    pairsFound++;
                    // Конец ли уровня?
                    if (pairsFound == pairsCount)
                        AllPairsCompleted?.Invoke();
                }

                // UnBetonizeAll();
            }

            private void MarkPairsMemo_as_Solved(string firstMemo, string secondMemo)
            {
                foreach (var i in _instaArray)
                {
                    if (i.thisId == firstMemo || i.thisId == secondMemo)
                    {
                        i.thisObject.GetComponent<DisplaySign>().isSolved = true;
                        i.thisObject.GetComponent<DisplaySign>().isInteractable = false;
                    }
                }
            }

            // ------------------------------------------------------------------------------------------------------

            public void BetonizeAll()
            {
                var evSys = FindObjectOfType<EventSystem>();
                evSys.enabled = false;
                
            }

            public void UnBetonizeAll()
            {
                
                var evSys = FindObjectOfType<EventSystem>();
                evSys.enabled = true;
            }

            #endregion
        
        // ----------------------------------------------------------------------------------------------------------

        private void SetItemSolved(int itemNumber)
        {
            // _instaDick[itemNumber].GetComponent<DisplaySign>().isSolved = true;
        }
        
        
        private void Find_SignDescription_and_Invoke(string id_of_Clicked_Memo)
        {
            //--- шерстит массив, найдя такой же айдишник - вызывает по нему установку текста
            for (int i = 0; i < _instaArray.Length; i++)
            {
                if (_instaArray[i].thisId == id_of_Clicked_Memo)
                {
                    ShowSignDescription?.Invoke(_instaArray[i].thisDescription);
                    break;
                }
            }
        }


        public void FlipAll_ToSign(float time)
        {
            for (int i = 0; i < _instaArray.Length; i++)
            {
                _instaArray[i].thisObject.GetComponent<DisplaySign>().TurnToSign();
                _instaArray[i].thisObject.GetComponent<DisplaySign>().TurnToBack(time);
            }
        }


        // --------- УДАЛЕНИЕ всех мемошиное - по удалению борды всех отписать -------------------------------
        public void DeleteBoard()
        {
            foreach (MemoItem item in _instaArray)
            {
                item.thisObject.GetComponent<DisplaySign>().MemoOpened -= CheckPairs;
                Destroy(item.thisObject);
            }
           
        }
    }
}


public class MemoItem
{
    public string thisId;
    public string thisDescription;
    public GameObject thisObject;

    public bool thisIsSolved;
    public bool thisIsOpened;
}