using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    // Висит на КНОПКАХ варанта ответа. сам ничего не решает ! только выводит инфу и реагирует
    // Анимацию кнопок писать извне
    public class TestButton : MonoBehaviour
    {

        [SerializeField] private TMP_Text _variantText;
        
        [SerializeField] private GameObject _YesNoObject;
        [SerializeField] private Sprite _Tick; // галочка
        [SerializeField] private Sprite _Cross; // крестик
        
        [SerializeField] public Sprite _grayButtonBg;
        [SerializeField] public Sprite _redButtonBg; // красный фон 
        [SerializeField] public Sprite _greenButtonBg; // 
        
        public Image yesNoImage;
        
        //-----------------------------------------------------------------------------------------------------------
        
        private void Awake()
        {
            
            _YesNoObject.SetActive(false); // спрятать крестик - галочку
            yesNoImage = _YesNoObject.GetComponentInChildren<Image>();
        }

        public void OnClick()
        {
            Debug.Log(yesNoImage.sprite.name);
            Handheld.Vibrate();
        }
        
        // ========================== МЕТОДЫ с инфой извне =====================================

        // 1) поставить полученый текст 
        public void SetVarText(string givenText)
        {
            _variantText.text = givenText;
        }
        
        // -------------------------------------------------- X / V -------------------------------------------------
        public void ShowTick()
        {
            if (yesNoImage.sprite != _Tick)
            { 
                // показывает галку
                yesNoImage.sprite = _Tick; // ставит галочку
            }
            _YesNoObject.SetActive(true);
        }

        public void ShowCross()
        {
            if (yesNoImage.sprite != _Cross)
            {
                yesNoImage.sprite = _Cross; // ставит крестик
            }
            _YesNoObject.SetActive(true);
        }
        
        //-----------------------------------------------------------------------------------------------------------
        
        public void HideCrossOrTick()
        {
            _YesNoObject.SetActive(false);
        }
    }
}
