using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;
using Vector2 = UnityEngine.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace HOG
{
    // ------------ задача - настроить прозрачность верхнему спрайту пешехода -------------------

    public class Peshehod : MonoBehaviour
    {
        private ParticleSystem _stars;

        [SerializeField] private GameObject _outLine; // 17 кб png 1 цвет
        [SerializeField] private GameObject _mainSprite;

        [SerializeField] private float OFFSET = 0.05f; // на сколько растягиваем контур в анимации
        [SerializeField] private int OUTLINELOOPS = 4;
        // private const string toText = "Вы заметили пешехода!";

        private Vector2 defScale; // размер контура в статике
        private Sprite _mainImage;

        private Sequence peshSeq;
        private Guid uid;
        
        private bool isFound = false;
        
        public event Action FoundOnePed;

        //--------------------------------------------------
        private void Awake()
        {
            _stars = GetComponentInChildren<ParticleSystem>();
            _stars.Stop();
            
            _outLine.SetActive(false);
            
            defScale = _outLine.transform.localScale;
        }

        
        

        //================== все что происходит по нажатию =========================
        public void OnMouseUpAsButton()
        {
            if (isFound)
                return;

            Handheld.Vibrate();
            PlayStarsPartcl(); //------------------------ 1  звездочки
            StartCoroutine(OutlinePlay()); //------ 2  вызываем иенумератор на контур пешехода
        }
        
        private void PlayStarsPartcl()
        {
            _stars.Play(); //-----------------------------2  звездочки
        }

        private IEnumerator OutlinePlay()
        {
            _outLine.SetActive(true); // достали обводку
            Vector2 scaleTo = defScale + new Vector2(OFFSET, OFFSET); // расширяется в обе стороны
            Vector2 finalScale = defScale;

            // ----------------------------------------------------------------------------------------
            // ############################# показательный твин #######################################
            // ----------------------------------------------------------------------------------------

            // Создаем UID для DOTween
            peshSeq = DOTween.Sequence();
            uid = System.Guid.NewGuid();
            peshSeq.id = uid; // чтобы выпилить дотвин 

            yield return peshSeq
                .Append(
                    _outLine.transform.DOScale(scaleTo, 0.3f).SetEase(Ease.InOutBack)
                        .SetLoops(OUTLINELOOPS, LoopType.Restart)
                ) // это одна штука
                .Append(
                    _outLine.transform.DOScale(finalScale, 0.3f).SetEase(Ease.OutBack)
                ) // это назад до начального размера
                .AsyncWaitForCompletion();
            
            FadePeshehod();
            isFound = true; // шоб не нажимался более

            //-------------------------
            // _hogLevelFile.FoundOnePed(); - фу
            FoundOnePed?.Invoke();
        }


        //############################ НЕ РАБОТАЕТ ##############################################
        private void FadePeshehod()
        {
            // _outlneSpriteRenderer.DOFade(0, 0.5f);
        }

        
        private void OnDestroy()
        {
            // Удалить DOTween
            DOTween.Kill(uid);
            peshSeq = null;
            // ---------------
            Debug.Log("Пешеход: Destroy!");
        }
    }
}