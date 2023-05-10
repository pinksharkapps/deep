using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MENU;
using TMPro;
using UnityEngine;

namespace HOG
{


    public class HOGSmallYellowPanel : BasePopPanel
    {
        [SerializeField] private TMP_Text _textLine1;
        [SerializeField] private TMP_Text _textLine2;

        public Animator _animator;
        // [SerializeField] private GameObject _left_tail;
        // [SerializeField] private GameObject _right_tail;
        
        private Sequence _MySequence;
        private Vector2 _defPos;
        
        // private Vector2 leftTail_defPos;
        // private Vector2 rightTail_defPos;
        // private Guid uid; 
        // private float TAIL_OFFSET = 2f;
       
        // тексты в BASE полях
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void CantCallDirectlyThatsWhy()
        {
            PlayTails();
        }


        // ----------------- АНИМАЦИЯ хвостиков ленты с крутиной на возвращение -----------------
        public void PlayTails()
        {
             _animator.Play("tails");
             StartCoroutine(PlayTailsCor());
             _animator.Rebind();
        }
        private IEnumerator PlayTailsCor()
        {
            yield return new WaitForSeconds(1f);
            _animator.Rebind();
        }
        // ----------------------------------------------------------------------------------------

        public void SetTextOne(string xxx)
        {
            _textLine1.text = $"{xxx}";
        }
    
        public void SetTextTwo(string xxx)
        {
            _textLine2.text = $"{xxx}";
        }


        protected override void AnimatePanelObj()
        {
            // пока ну его 
        }

        protected override void HidePanel()
        {
            Debug.Log(" Панель с глазом прячется");
        }
        
    }
}
