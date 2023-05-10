using System;
using System.Collections.Generic;
using MENU;
using Unity.VisualScripting;
using UnityEngine;

namespace HOG
{
    //============== ОТДЕЛЬНЫЙ УРОВЕНЬ ХОГ ================
    public class HogLevel : MonoBehaviour
    {
        private Peshehod[] _pedestriansArray; // массив пешеходов на этом уровне
        
        private HOGMenu _hogmenuFile;
        private HOGController _hogControllerFile;
        // private SaveSystem _saveSystem; // для рекордов в будущем
        private int _totalPedsCount; // для ачивки

        private int currentLevel; // уровень приходит из HOG Controller и выводится в меню
        
        

        public int CurrentLevel
        {
            get { return CurrentLevel; } // что отправляем наружу
            set { currentLevel = value; } // что делаем с тем, что пришло извне 
        }

        private int howMuchPeds = 0;
        public int FoundPeds = 0;

        public event Action HogLevelWon; // идет в HogController
        //------------------------------------------------------------------------------------------------------------
        
        
        private void Awake()
        {
            _hogControllerFile = FindObjectOfType<HOGController>();
            currentLevel = _hogControllerFile.CurrentLevel;

            Debug.Log("Awake в уровне");
            _hogmenuFile = FindObjectOfType<HOGMenu>();

            _pedestriansArray = FindObjectsOfType<Peshehod>();

            _totalPedsCount = _hogControllerFile._totalPedsCount;

        }

        private void Start()
        { 
            GameManager.Instance.SplashScreen();  // сплеш от GameManager

            howMuchPeds = _pedestriansArray.Length;

            foreach (Peshehod ped in _pedestriansArray)
            {
                ped.FoundOnePed += FoundOnePed;
            }

            Debug.Log($"Натыкано {howMuchPeds} пешеходов");

            _hogmenuFile.SayWhatToDo(howMuchPeds);
            _hogmenuFile.SetTopPanel(currentLevel, howMuchPeds); // посылаем номер уровня в меню
        }

        
        
        
        //---------- НАШЛИ ОДНОГО ПЕШЕХОДА -------------------------------------------------------------------
        public void FoundOnePed()
        {
            FoundPeds++; // <--------------- СЧИТАЕТ НАЙДЕННЫХ ПЕШЕХОДОВ ---------
            _totalPedsCount++; // считает найденных за все уровни
            
            GameManager.Instance._audioManager.PlaySound("PUM");
            _hogmenuFile.ShowSmallYellow(FoundPeds);
            
            if (FoundPeds == howMuchPeds) // если нашли столько, сколько в массиве 
            {
                FoundAll(); // ниже!
            }

            if (_totalPedsCount == 10) // если их 20 - пошлет на проверку времени в ачивке (надо 10 за 20 секунд)
            {
                _hogControllerFile.Set10_pedsCounted();
            }

        }

        
        //---------- НАШЛИ ВСЕХ -------------------------------------------------------------------------------
        private void FoundAll()
        {
            HogLevelWon?.Invoke(); // идет в HogController
            Debug.Log(" Level: win, HogLevelWon?.Invoke");
            
            FoundPeds = 0;    // ????????????????????????????????????????????????????
            howMuchPeds = 0;  // ????????????????????????????????????????????????????
            _pedestriansArray = null; // new Peshehod[0];
        }

        
        private void OnDestroy()
        { 
            if (_pedestriansArray == null)
                return;
            
            foreach (Peshehod ped in _pedestriansArray) // отписываем удаляемых пешеходов от событий
            {
                ped.FoundOnePed -= FoundOnePed;
            }
            
            _pedestriansArray = null;
            Debug.Log(" очистили массив пешеходов и Destroy Lvl");
        }
    }
}