using DG.Tweening;
using UnityEngine;
// весит на кнопке в UI
// отвечает только за анимацию, можно раздать всем куда надо
public class BtnWantBePressed: MonoBehaviour
{
    [SerializeField] private float PAUSE = 1f;   // пауза между циклами
    private float SCALE_ANIMATION = 1.05f; // до скольки увеличиваем
    private float TIME_BIGGER = 0.3f;      // время роста
    private float TIME_SMALLER = 1f;       // время ужимания
    private float PAUSE_INSIDE = 0f;       // пауза между циклами
    
    private Sequence _BTNsequence; // название у секвенции 
    
    
    public void OnEnable() //создали анимацию
    {
        Vector3 defaultScale = transform.localScale;
        
        _BTNsequence = DOTween.Sequence();
        _BTNsequence.AppendInterval(PAUSE);
        _BTNsequence.Append(transform.DOScaleX(defaultScale.x * SCALE_ANIMATION, TIME_BIGGER));
        _BTNsequence.AppendInterval(PAUSE_INSIDE);
        _BTNsequence.Append(transform.DOScaleX(defaultScale.x, TIME_SMALLER).SetEase(Ease.OutElastic));
        _BTNsequence.SetLoops(-1, LoopType.Restart);
        
    }

    private void OnDisable() //-------!!!-----------убрали анимацию------------------------------
    {
        _BTNsequence?.Kill(true); //------ не просто кильнуть, но и завершить!!! ТАК НАДО
        _BTNsequence = null;
    }
}
