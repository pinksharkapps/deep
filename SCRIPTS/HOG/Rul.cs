using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
// using Cysharp.Threading.Tasks;

namespace HOG
{
    public class Rul : MonoBehaviour
    {
        private Cabine _cabine;
        
        private const float MOVE_OFFSET = 0.4f;
        private float _defaultPosX;

        private RulAndsHands _rulAndHands;
        private const int ROTATION = 10;
        private Vector3 _defaultRot; // повоторот тут измеряют квантерионами но пишем вектор :///

        private float _lastTimeClick_Left;
        private float _lastTimeClick_Right;

        // ----------------------------------------------------------------------------------------------
        
        private void Start()
        {
            // Кабина и ее начальная позиция X
            _cabine = FindObjectOfType<Cabine>();
            _defaultPosX = _cabine.transform.position.x;
            // Руль с руками
            _rulAndHands = FindObjectOfType<RulAndsHands>();

            // Запоминаем начальное время...
            _lastTimeClick_Right = Time.realtimeSinceStartup;
            _lastTimeClick_Left = Time.realtimeSinceStartup;
        }
        
        // ----------------------------------------------------------------------------------------------
        
        // Повернули руль влево
        public void CabineLeft()
        {
            // В течение 1 секунды было два клика:
            if (Time.realtimeSinceStartup - _lastTimeClick_Right < 1f)
            {
                Debug.Log("В течение 1 секунды было два клика. Тогда двигаем кабину из крайней правой позиции в крайнюю левую, минуя центр");
                _lastTimeClick_Right = Time.realtimeSinceStartup;
                // Гоним кабину и руль от крайне левого положения в крайнее правое
                MoveCar_and_RotateWheel(_defaultPosX - MOVE_OFFSET, ROTATION);
                return;
            }
            // Запоминаем время последнего клика
            _lastTimeClick_Right = Time.realtimeSinceStartup;
            
            // Если авто в центре или чуть левее от центра - 1 клик, проверка движется ли
            if (_cabine.transform.position.x <= _defaultPosX && _cabine.transform.position.x > (_defaultPosX - MOVE_OFFSET))
            {
                Debug.Log("Движение авто и руля из центра в крайнее левое положение");
                // Гоним кабину и руль в крайнее левое положение
                MoveCar_and_RotateWheel(_defaultPosX - MOVE_OFFSET, ROTATION);
                return;
            }
            
            // Если позиция кабины в правом положении, тогда перегоняем ее налево к центральной позиции ||
            if (_cabine.transform.position.x > _defaultPosX)
            {
                Debug.Log("Движение кабины и поворот руля справа налево до центральной позиции");
                // Гоним кабину и руль по центру
                MoveCar_and_RotateWheel(_defaultPosX, 0);
            }
        }

        // ----------------------------------------------------------------------------------------------

        // Повернули руль вправо
        public void CabineRight()
        {
            // В течение 1 секунды было два клика...
            if (Time.realtimeSinceStartup - _lastTimeClick_Left < 1f)
            {
                Debug.Log("В течение 1 секунды было два клика. Тогда двигаем кабину из крайней левой позиции в крайнюю правую, минуя центр");
                _lastTimeClick_Left = Time.realtimeSinceStartup;
                // Гоним кабину и руль от крайне левого положения в крайнее правое
                MoveCar_and_RotateWheel(_defaultPosX + MOVE_OFFSET, -ROTATION);
                return;
            }
            // Запоминаем время последнего клика
            _lastTimeClick_Left = Time.realtimeSinceStartup;
            
            // Если авто в центре или уже правее его - 1 клик, проверка движется ли
            if (_cabine.transform.position.x >= _defaultPosX && _cabine.transform.position.x < (_defaultPosX + MOVE_OFFSET))
            {
                Debug.Log("Движение авто и руля из центра в крайнее правое положение");
                // Гоним кабину и руль в крайнее правое положение
                MoveCar_and_RotateWheel(_defaultPosX + MOVE_OFFSET, -ROTATION);
                return;
            }
            
            // Если позиция кабины в левом положении, тогда перегоняем ее направо к центральной позиции ||
            if (_cabine.transform.position.x < _defaultPosX)
            {
                Debug.Log("Движение кабины и поворот руля слева направо до центральной позиции");
                // Гоним кабину и руль по центру
                MoveCar_and_RotateWheel(_defaultPosX, 0);
            }
        }

        
        // ===================== DOTween =============================================
        // Анимация кабины и руля
        private void MoveCar_and_RotateWheel(float newCabinePos, float NewCabineRotate)
        {
            DOTween.Sequence()
                .Append(_cabine.transform.DOMoveX(newCabinePos, 1f))
                .Join(_rulAndHands.transform.DORotate(new Vector3(0,0,NewCabineRotate), 1f));// установить позицию по умолчанию
        }

    }
}