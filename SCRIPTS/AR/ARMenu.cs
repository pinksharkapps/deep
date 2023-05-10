using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AR
{
    public class ARMenu : MonoBehaviour
    {
        [SerializeField] private Button easySigns;
        [SerializeField] private GameObject easySignsPanel;
        [SerializeField] private GameObject ArWarningPanel;
        
        [SerializeField] private TMP_Text _debugCountText;
        private DetectImages _detectImagesFile;
        private GameManager _gmFile;

        
        private void Awake()
        {
            _detectImagesFile = FindObjectOfType<DetectImages>();
            _detectImagesFile.SignDetected += UpdateSignsCount;
            
            _gmFile = GameManager.Instance;
            HideEasyPanel();
            
            //GameManager.Instance.CheckSceneForMenuBtn();
        }
        

        private void UpdateSignsCount(string name)
        {
            int xxx = _detectImagesFile._debugSignsDetected;
            _debugCountText.text = "Тех-инфа: rnd " + Random.Range(0, 100).ToString() + " / " + xxx.ToString() + $" Новый знак - {name}";

            if (xxx == 1)
            {
                string whatToPrint = "Вы нашли " + xxx.ToString() + " знак!";
                
                // тот случай, когда вывели Prize Panel но без ачивки:
                GameManager.Instance.ShowPrizePanel(whatToPrint, "Супер!", "ничего");
            }
            
            
            if (xxx == 3)
            {
                string whatToPrint = "Определено " + xxx.ToString() + " знака!";
                GameManager.Instance.ShowPrizePanel(whatToPrint, "Супер!", "AR");
                
                // ачивка 3
                GameManager.Instance.SaveAchieves("AR_Ach"); // тут сохраняет кол-во знаков
            }
            
            
            else if (xxx == 10)
            {
                string whatToPrint = "Ого, уже " + xxx.ToString() + "10 знаков!";
                GameManager.Instance.ShowPrizePanel(whatToPrint, "Супер!", "AR10");
                
                // ачивка 10
                GameManager.Instance.SaveAchieves("AR_Ach"); // тут сохраняет кол-во знаков
            }
            
        }

        private void Start()
        {
            ArWarningPanel.SetActive(true); // показать предупреждение
            
            string xxx = _gmFile.signsDetectedCount.ToString();
            UpdateSignsCount(xxx);
        }


        public void HideEasyPanel()
        {
            easySignsPanel.SetActive(false);
        }

        public void ShowEasyPanel()
        {
            easySignsPanel.SetActive(true);
        }

        private void OnDestroy()
        {
            _detectImagesFile.SignDetected -= UpdateSignsCount;
        }
    }
}