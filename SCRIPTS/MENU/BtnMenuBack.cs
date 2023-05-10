using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace MENU
{
    public class BtnMenuBack : MonoBehaviour
    {
        private Button _fgBtn;

        private void Awake()
        {
            _fgBtn = GetComponentInChildren<Button>();
        }

        private void Start()
        {
            _fgBtn.onClick.AddListener(MenuBtnClicked);
        }

        // ----- метод для несчастной кнопки МЕНЮ (назад) -----
        public void MenuBtnClicked()
        {
            DOTween.Clear(true);
            
            Debug.Log("КНОПКУ меню нажали -  clicked");
            SceneManager.LoadScene("MENU");
        }

        private void OnDestroy()
        {
            _fgBtn.onClick.RemoveListener(MenuBtnClicked);
        }
    }
}