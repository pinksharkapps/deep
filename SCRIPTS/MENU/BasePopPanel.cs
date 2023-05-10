using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

// ------------------- ОТ НЕЕ НАСЛЕДУЮТСЯ ВСЕ ПАНЕЛИ ПАУЗА / ПОЗДРАВЛЕНИЕ -------------
namespace MENU
{
    public abstract class BasePopPanel : MonoBehaviour
    {
        [SerializeField] public GameObject _prize;

        // [SerializeField] private Button _PopButton;
        private TextMeshProUGUI _text1;
        private TextMeshProUGUI _text2;

        public Vector3 _prizeSpawnPoint;
        private const float MOVE_PRIZE_OFFSET = 1f;
        private Sequence _prizeSeq;

        // а экшкны у деток разные ---------------------------------------


        private void Awake()
        {
            // _text1File = FindObjectOfType<Text1>(); - НЕТ
            //_text1 = GetComponentInChildren<Text1>().GetComponent<TextMeshProUGUI>(); - НЕТ
            //_text1 = _text1File.GetComponent<TMP_Text>(); - НЕТ

            // --------- ПОТОМ ПРИГОДИТСЯ -----------------
            _prizeSpawnPoint = _prize.transform.position;
        }

        private void Start()
        {
            AnimatePanelObj(); // анимация графики на панели
        }

        //-----------------------------------------------------------------
        protected abstract void AnimatePanelObj();
        // также нафиг наследование анимации
        //{
            // _prizeSeq = DOTween.Sequence();
            //_prizeSeq.Append(_prize.transform.DOMoveY(_prizeSpawnPoint.y + MOVE_PRIZE_OFFSET, 3f)
            //    .SetLoops(-1, LoopType.Yoyo));
        //}

        #region ====== ПОЧЕМУ не получается наследовать методы, связанные с TMP_Text? ===

        //------- пыталась заставить наследовать, но обломалось ------------
        // НУ ПОЧЕМУ ЭТО НЕ РАБОТАЕТ ЕСЛИ ВЫЗЫВАТЬ В GM ???????????????????? 
        // В гугле говорят, что с монобехами нельзя,
        // Юнити Выдает ошибку "много всяких наследует одгно и то же поле"

        // protected void SetTextOne(string xxx) - протектед не доступно из внешних классов!!!

        // public void SetTextOne(string xxx) - пробовала
        //{
        // _text1.text = $"{xxx}";
        //}

        //protected void SetTextTwo(string xxx)
        //{
        //}

        //------------------------------------------------------------------

        #endregion


        protected abstract void HidePanel();


        private void OnDestroy()
        {
            _prizeSeq?.Kill(true);
            _prizeSeq = null;
        }
    }
}