using Cysharp.Threading.Tasks;
using UnityEngine;

// HOG делает экземпляр ее на старте
// она от интерфейса поэтому такое
namespace MENU
{
    public class SaveSystem : ISaveSystem
    {
        // --- УРОВНИ
        private const string SAVE_REG_KEY = "REG";
        private const string SAVE_TEST_KEY = "TEST";
        private const string SAVE_MEMO_KEY = "MEMO";
        private const string SAVE_HOG_KEY = "HOG";
        private const string SAVE_AR_KEY = "AR"; // количество знаков в AR
        // --- АЧИВКИ
        private const string SAVE_HOG_ASCHIEVE_KEY = "HOG_Ach";
        private const string SAVE_MEMO_ASCHIEVE_KEY = "MEMO_Ach";
        private const string SAVE_REG_ASCHIEVE_KEY = "REG_Ach";
        private const string SAVE_TEST_ASCHIEVE_KEY = "TEST_Ach";
        
        //------------------- БОЛЬШАЯ СОХРАНЯЛКА ---------------------------------
        public void SaveIntData (string what, int howmuch )
        {
            switch (what)
            { case "REG" : SavePrefsData(SAVE_REG_KEY,howmuch); break; 
              case "TEST" : SavePrefsData(SAVE_TEST_KEY,howmuch); break;
              case "MEMO" : SavePrefsData(SAVE_MEMO_KEY,howmuch); break;
              case "HOG" : SavePrefsData(SAVE_HOG_KEY,howmuch); break;
              case "AR" : SavePrefsData(SAVE_AR_KEY,howmuch); break;
              //---
              case "HOG_Ach" : SavePrefsData(SAVE_HOG_ASCHIEVE_KEY, howmuch); break;
              case "MEMO_Ach" : SavePrefsData(SAVE_MEMO_ASCHIEVE_KEY,howmuch); break;
              case "REG_Ach" : SavePrefsData(SAVE_REG_ASCHIEVE_KEY,howmuch); break;
              case "TEST_Ach" : SavePrefsData(SAVE_TEST_ASCHIEVE_KEY,howmuch); break;
              default: Debug.Log(" Ошибка ключа сохранения!"); break;
            }
        }

        //----- АЧИВКИ: 0 нэту / 1 есть ------------------------------------
        //----- ОСТАЛЬНОЕ: сохраняяем № уровня или кол-во определенных знаков
        private void SavePrefsData(string key, int gotLevel)
        {
            PlayerPrefs.SetInt(key, gotLevel); 
            Debug.Log($"{key}" + $" SaveSystem: saved {gotLevel}");
        }
        
        
            
        
             // ======================= БОЛЬШАЯ ЗАГРУЖАЛКА СОХРАНЕНИЙ ==========================
    

            public int LoadData(string what) // public UniTask <bool>
            {
                int xxx = 42; // на случай ошибки пусть выдает 42
                
                switch (what)
                {   case "REG" : xxx = LoadRegData(); Debug.Log($" SaveSystem: REG loaded {xxx}");break;
                    case "TEST" : xxx =LoadTestData(); break;
                    case "MEMO" : xxx =LoadMemoData(); break;
                    case "HOG" : xxx =LoadHogData(); break;
                    case "AR" : xxx =LoadArData(); break;

                    case "HOG_Ach":
                        xxx = LoadHogAch();
                        break;
                    case "MEMO_Ach":
                        xxx = LoadMemoAch();
                        break;
                    case "REG_Ach":
                        xxx = LoadRegAch();
                        break;
                    case "TEST_Ach":
                        xxx = LoadTestAch();
                        break;
                    default: Debug.Log(" Ошибка ключа загрузки savesystem!"); break;
                }

                return xxx;
            }
            
        
            private int LoadRegData()
            { int gotInt = PlayerPrefs.GetInt(SAVE_HOG_KEY, 0); return gotInt;}
            
            private int LoadTestData()
            { int gotInt = PlayerPrefs.GetInt(SAVE_MEMO_KEY, 0); return gotInt;}
    
            private int LoadMemoData()
            { int gotInt = PlayerPrefs.GetInt(SAVE_MEMO_KEY, 0); return gotInt;}
    
            private int LoadHogData()
            { int gotInt = PlayerPrefs.GetInt(SAVE_HOG_KEY, 0); return gotInt;}
    
            private int LoadArData()
            { int gotInt = PlayerPrefs.GetInt(SAVE_AR_KEY, 0); return gotInt;}
            
            
            //------- Load Achieves
            
            private int LoadHogAch()
            { int gotInt = PlayerPrefs.GetInt(SAVE_HOG_ASCHIEVE_KEY, 0); return gotInt; }
            
            private int LoadMemoAch()
            { int gotInt = PlayerPrefs.GetInt(SAVE_MEMO_ASCHIEVE_KEY, 0); return gotInt; }
            
            private int LoadRegAch()
            { int gotInt = PlayerPrefs.GetInt(SAVE_REG_ASCHIEVE_KEY, 0); return gotInt; }

            private int LoadTestAch()
            { int gotInt = PlayerPrefs.GetInt(SAVE_TEST_ASCHIEVE_KEY, 0); return gotInt; }
            
    }
    
//==============================================================================================================





    
//======================= можно вынести отдельно но пока незачем =====================================================================
    public interface ISaveSystem
    {
        // в интерфейсах все public
        // нельзя объявить переменные, приватный метод, нельзя тела {}
        // можно - сигнатуры метода (названия), Свойства, ИВЕНТЫ
            
        // public GameData GameData { get; }  // доступ на чтение
        // public UniTask Initialize(int defaultValue);
            
            public void SaveIntData(string what, int howmuch);
            public int LoadData(string what); // было UniTask <bool> 
            
    }
}