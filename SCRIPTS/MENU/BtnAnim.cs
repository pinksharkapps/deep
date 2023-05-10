using DG.Tweening;
using UnityEngine;

    // весит на кнопке в UI
    // отвечает только за анимацию, можно раздать всем куда надо
    public class BtnAnim : MonoBehaviour
    {
        private Sequence _BTNsequence; //ну вот такое название у секвенции 

        private const float SCALE_OUT_SIZE = 0.8f;
        private const float DURATION = 0.2f;
        public void PlayAnim() //создали анимацию
        {
            _BTNsequence = DOTween.Sequence();
            _BTNsequence.Append(transform.DOScale(Vector3.one * SCALE_OUT_SIZE, DURATION).SetEase(Ease.InQuad));
            _BTNsequence.Append(transform.DOScale(Vector3.one, DURATION).SetEase(Ease.OutBounce)); //.one - это Vector3 (1,1,1)
            _BTNsequence.Append(transform.DOScale(Vector3.one * SCALE_OUT_SIZE, DURATION * 0.5f).SetEase(Ease.InQuad));
            _BTNsequence.Append(transform.DOScale(Vector3.one, DURATION * 0.5f).SetEase(Ease.OutBounce)); 
            //_sequence.SetLoops(1, );
        }

        private void OnDisable() //-------!!!-----------убрали анимацию---------
        {
            _BTNsequence?.Kill(true); //------ не просто кильнуть, но и завершить!!! ТАК НАДО
            _BTNsequence = null;
        }
    }