
using MENU;
using UnityEngine;



// -------- НАСЛЕДУЕТСЯ от панели BasePopPanel
// потому, что может.

namespace AR
{
    public class ArWarningPanel : BasePopPanel
    {

        //----------------- скрипт нужен для закрывания по щелчку по ней или кнопке ----------------------------------------
        public void HideWarningPanel()
        {
            HidePanel();
        }

        protected override void AnimatePanelObj()
        {
            // там у меня анимация аниматором
        }

        protected override void  HidePanel() 
        {
            Handheld.Vibrate();
            gameObject.SetActive(false);
        }
        
    }


}