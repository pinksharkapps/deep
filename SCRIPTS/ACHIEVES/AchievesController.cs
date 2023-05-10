using System.Collections;
using ACH;
using MENU;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievesController : MonoBehaviour
{
    [SerializeField] private GameObject _hogAch;  // Регулировщик без ошибок
    [SerializeField] private GameObject _memoAch;// Распознал 5 знаков
    [SerializeField] private GameObject _regAch; // Решил мемо без ошибочных кликов
    [SerializeField] private GameObject _arAch; // Решил тесты без ошибок
    [SerializeField] private GameObject _ar10Ach; // 3 регулировщика подряд без ошибок
    [SerializeField] private GameObject _testAch; // Нашел всех пешеходов во всех уровнях

    [SerializeField] private TMP_Text _signsCountText;
    // переменные, куда присвоим из savesystem: 1 - открытая, 0 - закрытая
    private int hogAch = 0;
    private int memoAch = 0;
    private int regAch = 0;
    private int testAch = 0;
    private int ar = 0;
    
    private Achieve _achieveFile;
    private Achieve[] _achievesArray;

    private Button _resetButton;
    

    private void Awake()
    {
        LoadSavedAchieves(); // лезем в сохранение
        
        _achievesArray = FindObjectsOfType<Achieve>(); // собираем массив со сцены
        _resetButton = GetComponentInChildren<Button>();
        GameManager.Instance.MoveMenuButtonToStandard();
    }

    private void Start()
    {
        SetupAchieves(); // раздаем вкл/выкл из сохранени
        
        // --------- отключаем кнопку ресета если нету ачивок ----
        if (GameManager.Instance.pokalPoints == 0)
        {  _resetButton.interactable = false; }
    }


    //-----------------------------------------------------------
    private void LoadSavedAchieves() // грузит инфу из сохранений
    {
        hogAch = GameManager.Instance.HOG_Ach;
        memoAch = GameManager.Instance.MEMO_Ach;
        regAch = GameManager.Instance.REG_Ach;
        testAch = GameManager.Instance.TEST_Ach;
        ar = GameManager.Instance.signsDetectedCount;
    }
    
    
    // ====== хардкод, который стабильно работает в отличие от выкрунтасов =============
    private void SetupAchieves() // вкл/выкл в зависимости от того, что загрузилось d GM
    {
        if (hogAch == 0) { _hogAch.GetComponent<Achieve>().FadeMe();} 
        else if (hogAch == 1) { _hogAch.GetComponent<Achieve>()._achParticles.Play();}
        
        if (memoAch == 0) { _memoAch.GetComponent<Achieve>().FadeMe();}
        else if (memoAch == 1){ _memoAch.GetComponent<Achieve>()._achParticles.Play();}
        
        if (regAch == 0) { _regAch.GetComponent<Achieve>().FadeMe();}
        else if (regAch == 1){ _regAch.GetComponent<Achieve>()._achParticles.Play();}
        
        if (testAch == 0) { _testAch.GetComponent<Achieve>().FadeMe();}
        else if (testAch == 1){ _testAch.GetComponent<Achieve>()._achParticles.Play();}

        _signsCountText.text = "Найдено знаков: " + $"{ar}";
        
        if (ar <= 3) { _arAch.GetComponent<Achieve>().FadeMe();}
        else if (ar >= 3){ _arAch.GetComponent<Achieve>()._achParticles.Play();}
        
        if (ar <= 10) { _ar10Ach.GetComponent<Achieve>().FadeMe(); }
        else if (ar >= 10){ _ar10Ach.GetComponent<Achieve>()._achParticles.Play();}
    }

    private void HideAllAchieves()
    {
        foreach (var ach in _achievesArray)
        {
            ach.gameObject.SetActive(false);
        }
    }
    
    private void ShowAllAchieves()
    {
        foreach (var ach in _achievesArray)
        {
            StartCoroutine(ShowAfterPause());

            IEnumerator ShowAfterPause()
            {
                yield return new WaitForSeconds(0.5f);
                ach.gameObject.SetActive(true);
            }
        }
    }


    // ----------- На кнопку "сбросить рекорды" ---------- 
    public void ResetAll()
    {
        _resetButton.interactable = false;
        
        PlayerPrefs.DeleteAll();
        
        // ------ обнуляем переменную в GM ------------
        GameManager.Instance.ResetAllAchied();
        
        HideAllAchieves();
        LoadSavedAchieves();
        SetupAchieves();
        ShowAllAchieves();
    }




}
