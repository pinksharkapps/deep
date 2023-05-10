using System;
using System.Collections;
using System.Collections.Generic;
using MENU;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


namespace REG
{
    public class Regulirovschik : MonoBehaviour
    {
        [Header("МАССИВ СОСТТОЯНИЙ vvv раскрыть")] [Serialize]
        public Sprite[] StateArray;

        private SpriteRenderer _regSpriteRenderer;
        public string StateName;
        private int curState;

        private AudioManager _audioManager;

        private void Awake()
        {
            _regSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _audioManager = GameManager.Instance.GetComponent<AudioManager>(); 
            // Инициализация позы гаиншника
            curState = 0;
        }

        private void Start()
        {
            StateName = _regSpriteRenderer.sprite.name;
            Debug.Log(StateName);

            RandomState();
            Debug.Log(StateName);
        }

        public void RandomState()
        {
            int tmp;
            int rnd = 0;

            do
            {
                tmp = Random.Range(0, 40);

                // Случайный выбор позы гаишника. Без повтора как у предыдущей позы. Самый низкий приоритет позам 0, 1, 2, 3
                switch (tmp)
                {
                    case 0 or 1 or 2 or 3:
                        rnd = tmp;
                        break;

                    case 4 or 5 or 6:
                        rnd = 4;
                        break;

                    case 7 or 8 or 9:
                        rnd = 5;
                        break;

                    case 10 or 11 or 12:
                        rnd = 6;
                        break;

                    case 13 or 14 or 15:
                        rnd = 7;
                        break;

                    case 16 or 17:
                        rnd = 8;
                        break;

                    case 18 or 19:
                        rnd = 9;
                        break;

                    case 20 or 21:
                        rnd = 10;
                        break;

                    case 22 or 23:
                        rnd = 11;
                        break;

                    case 24 or 25 or 26 or 27:
                        rnd = 12;
                        break;

                    case 28 or 29 or 30 or 31:
                        rnd = 13;
                        break;

                    case 32 or 33 or 34 or 35:
                        rnd = 14;
                        break;

                    case 36 or 37 or 38 or 39:
                        rnd = 15;
                        break;
                }
            } while (rnd == curState);

            curState = rnd;

            Sprite rndSprite = StateArray[rnd];
            string NewStateName = rndSprite.name;
            StateName = NewStateName;

            _regSpriteRenderer.sprite = rndSprite;
            
            _audioManager.PlaySound("SHUH");
        }
    }
}