using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

// методы внутри юного гаишника: смена состояний, вывод подсказки

namespace MEMO
{
    public class MemoGaishnik : MonoBehaviour
    {
        [SerializeField] private GameObject _gaishnikIDLE;
        [SerializeField] private GameObject _gaishnikHAPPY;
        [SerializeField] public GameObject _gaishnikSPEACH;

        private TMP_Text _gaishnikText;

        private Sequence _bubbleSeq;
        private Vector2 _bullbeDefScale;

        private const float PauseBeforeGaishnikSays = 0.2f;

        // ----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            _gaishnikText = _gaishnikSPEACH.GetComponentInChildren<TMP_Text>();
            
            // Скрыть подсказку
            BubbleMakeInvisible();

            // По умочанию покойное лицо гаишника
            State_Serious();

            // Запомнить scale подсказки гаишника
            _bullbeDefScale = _gaishnikSPEACH.transform.localScale;
        }

        // ----------------------------------------------------------------------------------------------------------

        // Счастливое лицо гаишника
        public void State_Happy()
        {
            _gaishnikIDLE.SetActive(false);
            _gaishnikHAPPY.SetActive(true);
        }

        // Спокойное лицо гаишника
        public void State_Serious()
        {
            _gaishnikIDLE.SetActive(true);
            _gaishnikHAPPY.SetActive(false);
        }

        // ------------------------------------- Подсказка от юного гаишника -----------------------------------------

        // Скрыть подсказку
        public void BubbleMakeInvisible()
        {
            // Скрыть подсказку
            _gaishnikSPEACH.SetActive(false);
            _gaishnikSPEACH.transform.DOScale(_bullbeDefScale, 0f);
        }

        // Отобразить подсказку гаишника на howLongShow секунд
        public void BubbleAnmationIn(float starDelay, float howLongShow, string whatHeSays)
        {
            _gaishnikText.text = whatHeSays; // задает текст

            _bubbleSeq = DOTween.Sequence()
                .AppendInterval(starDelay)
                .AppendCallback(() => _gaishnikSPEACH.SetActive(true))
                .AppendCallback(State_Happy) // вылезет радостный
                .Append(_gaishnikSPEACH.transform.DOScale(Vector3.zero, 0f))
                .Append(_gaishnikSPEACH.transform.DOScale(_bullbeDefScale, 1f).SetEase(Ease.OutBounce))
                .AppendInterval(howLongShow)
                .Append(_gaishnikSPEACH.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutQuint))
                .AppendCallback(State_Serious) // после подсказки - спокойное лицо 
                .AppendCallback(BubbleMakeInvisible);
        }

        // ----------------------------------------------------------------------------------------------------------
        private void OnDestroy() // на Disable не надо пробовать
        {
            _bubbleSeq?.Kill(true);
            _bubbleSeq = null;
        }
    }
}