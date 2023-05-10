using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MEMO
{
    // Массив видов наших объектов

    [CreateAssetMenu(fileName = "Signs", menuName = "Signs Level", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        // заводим настраиваемый из редактора конфиг
        // маасив инкапсулируем alt-enter read only
        [SerializeField] public int levelNumber;   // номер уровня для называия
        [SerializeField] public string levelName;  // название уровня, например "Знаки сервиса"
        [SerializeField] public int memosCount;
        [SerializeField] public float timeToShow;

        [FormerlySerializedAs("items")] [SerializeField]
        private SignsLevel[] sinsForThisLevel;

        // ------------------------------------------ Инкапсуляция массива ------------------------------------------
        
        public SignsLevel[] SinsForThisLevel => sinsForThisLevel;

        // АК обычно сразу создает публичный метод который возвращает элемент конфига по какому-то запросу
        // тут по запросу логического ключа
        public SignsLevel GetItemByKey(string key)
        {
            // возвращает LINQ перебор коллекции - дай первый у кого ключ как мы передали в скобках
            // если не найдет вернят null
            return sinsForThisLevel.FirstOrDefault(item => item.Key == key);
        }
    }
    
    // ------------------------------------------- отдельный класс --------------------------------------------------

    // связка (картинка) конфиг одного знака + ЛОГИЧЕСКИЙ айдишник ключ 
    [Serializable]
    public class SignsLevel
    {
        [SerializeField] private string key; // ключ
        [SerializeField] private SignConfig signConfig; // конфиг одного знака

        public string Key => key;
        public SignConfig SignConfig => signConfig;
    }
}