using System;
using System.Collections;
using System.Collections.Generic;
using MENU;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace HOG
{
    public class HOGwinPanel : BasePopPanel
    {
          [SerializeField] private TMP_Text _textDescription;
          [SerializeField] private TMP_Text _textButton;
        
        // ---- по закрытию панели вызывается событие, его ждет HogController и начинает новый уровень
        public event Action HideHOGWinPanelEvent;

        //----- событие на старте панели, его ждет HogController и дистроит текущий уровень
        public event Action ShowHOGWinPanelEvent;


        //---------------------------------------------------------------------------------
        private void OnEnable()
        {
            ShowHOGWinPanelEvent?.Invoke();
            Debug.Log("Событие HOG-Win-панели послано");
        }

        
        public void SetTextOne(string xxx)
        {
            _textDescription.text = $"{xxx}";
        }
    
        public void SetTextTwo(string xxx)
        {
            _textButton.text = $"{xxx}";
        }
        
        
        
        
        public void ThisButtonTapped()
        {
            GameManager.Instance.GetComponent<AudioManager>().PlaySound("PRESS");
            HidePanel();
        }

        protected override void AnimatePanelObj()
        {
            // TODO анимацию картинки которой еще нет
        }

        protected override void HidePanel() // NEW - - - -  проверяем че будет
        {
            Handheld.Vibrate(); //------------- я всовывваю между ними и не убираю старые методы?
            HideHOGWinPanelEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}