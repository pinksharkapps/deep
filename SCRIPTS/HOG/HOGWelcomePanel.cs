using System;
using System.Collections;
using System.Collections.Generic;
using MENU;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HOG
{
    public class HOGWelcomePanel : BasePopPanel
    {
        //-------------------------------------------------------------------------------------------------
        
        [SerializeField] private TMP_Text _textMain;
        [SerializeField] private TMP_Text _textButton2;
        [SerializeField] private Button _againButton;
        
        // ---- по закрытию панели вызывается событие, его ждет HogController и начинает новый уровень
        public event Action HideHOGWelcomePanelEvent;
        public event Action AgainButtonTapped_HogWelcomePanel;
        
        //-------------------------------------------------------------------------------------------------
        
        public void ThisButtonTapped()
        {
            GameManager.Instance.GetComponent<AudioManager>().PlaySound("PRESS");
            HidePanel();
        }

        public void HideAgainButton() // HogContrlr вызывает если уровень 0 и после запуска сцены еще не играли в уровень 0
        {
            _againButton.gameObject.SetActive(false);
        }

        public void AgainButtonTapped()
        {
            GameManager.Instance.GetComponent<AudioManager>().PlaySound("PRESS");
            AgainButtonTapped_HogWelcomePanel?.Invoke();
            HidePanel();
        }

        public void SetTextOne(string xxx)
        {
            _textMain.text = $"{xxx}";
        }
    
        public void SetTextTwo(string xxx)
        {
            _textButton2.text = $"{xxx}";
        }
        
        protected override void AnimatePanelObj()
        {
            // TODO анимацию картинки которой еще нет
        }

        protected override void HidePanel() // NEW - - - -  проверяем че будет
        {
            Handheld.Vibrate(); //------------- НАСЛЕДОВАНИЕ - я всовывваю между ними и не убираю старые методы????
            HideHOGWelcomePanelEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
    
}