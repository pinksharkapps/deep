using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

// ===============================
// АНИМАЦИЯ ГОЛОВЫ СОБАКИ ПО КЛИКУ
// ===============================

namespace HOG
{
    public class Dog : MonoBehaviour
    {
        [SerializeField] private GameObject dogHead;
        [SerializeField] private float vertMoveOffset;

        private float _defPosY;
        private Sequence _seqDogTween;

        // ------------------------------------------

        private void Start()
        {
            _defPosY = dogHead.transform.position.y;
        }

        // ------------------------------------------

        // Анимация головы собаки вверх-вниз
        public void OnMouseUpAsButton()
        {
            // я долбалась 4 часа как всунуть изменяющийся параметр в дотвин, (??? .OnUpdate ???)
            // потом пришел муж и написал то, что ниже.
            // DoShake не так красиво затухает по синусоиде.

            // без active - ошибка дотвин киллед
            if (_seqDogTween != null && _seqDogTween.active && _seqDogTween.IsPlaying())
                return;

            _seqDogTween = DOTween.Sequence()
                .Append(dogHead.transform.DOMoveY(_defPosY - vertMoveOffset, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY - vertMoveOffset * 0.5f, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY - vertMoveOffset * 0.4f, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY - vertMoveOffset * 0.25f, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY - vertMoveOffset * 0.1f, 0.1f).SetEase(Ease.OutQuad))
                .Append(dogHead.transform.DOMoveY(_defPosY, 0.1f).SetEase(Ease.OutQuad));
        }
    }
    
}
