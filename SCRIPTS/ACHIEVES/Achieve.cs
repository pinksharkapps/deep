using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACH
{
    public class Achieve : MonoBehaviour
    {
        
        // private int id;
        // public bool isOpened;
        [SerializeField] public Sprite achSprite;
        [SerializeField] public ParticleSystem _achParticles;

        private SpriteRenderer _achSpriteRenderer;
        private Sprite _defaultSprtie;

        private const float FADE_RANGE = 0.2f;

        private void Awake()
        {
            _achSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _achSpriteRenderer.sprite = achSprite; // БЕРЕТ ЗАСУНУТУЮ РУКАМИ В СЕРИАЛИЗ АЧИВКУ!!!
        }

        private void OnEnable()
        {
            _achParticles.Stop();
            // FadeMe(); // на стате всех фейдим 
        }

        //================ !!! ФЕЙДИЛКА СПРАЙТРЕНДЕРА !!! =============
        // public void FadeOrNotToFade()
        // {
        //     if (!isOpened)
        //     {
        //         FadeMe();
        //     }
        // }

        public void ShoweMe()
        {
            Color tmpColor = _achSpriteRenderer.color;
            tmpColor.a = 1;
            _achSpriteRenderer.color = tmpColor;
        }
        
        
        public void FadeMe()
        {
            Color tmpColor = _achSpriteRenderer.color;
            tmpColor.a = FADE_RANGE;
            _achSpriteRenderer.color = tmpColor;
        }
        
    }
}