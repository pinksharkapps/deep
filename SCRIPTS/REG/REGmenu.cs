using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace REG
{
    public class REGmenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rightAnswersCount;
        [SerializeField] private Button _podskazkaBTN;
        [SerializeField] private GameObject _podskazkaPanel;
        [SerializeField] private GameObject _smallTick;

        private RegController _regControllerFile;

        private event Action lookedPodskaka;
        
        private Vector2 defPos;
        private Sequence _tickSeq;
        private const float JUMP_SCALE = 1.5f;
        private const float JUMP_DURATION = 0.4f;


        // -----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            _podskazkaPanel.SetActive(false);
            _regControllerFile = FindObjectOfType<RegController>();
            defPos = _smallTick.transform.localScale; // позиция долбаной галочки
            
            // получаем текст по событию в RegController когда там меняется переменная
            _regControllerFile.RightAnswer += SetText;
            _regControllerFile.RightAnswer += JumpABit;
            
        }

        // -----------------------------------------------------------------------------------------------------------

        public void ShowPodskazka()
        {
            GameManager.Instance._menuButtonPanel.SetActive(false);
            _podskazkaPanel.SetActive(true);

            lookedPodskaka?.Invoke(); // для сложных уровней TODO
        }

        public void HidePodskazka()
        {
            GameManager.Instance._menuButtonPanel.SetActive(true);
            _podskazkaPanel.SetActive(false);
        }

        // -----------------------------------------------------------------------------------------------------------

        private void SetText(int count)
        {
            _rightAnswersCount.text = count.ToString();
        }

        
        
        private void JumpABit(int obj)
        {
            _tickSeq = DOTween.Sequence();
            _tickSeq.Append(_smallTick.transform.DOScale(defPos * JUMP_SCALE , JUMP_DURATION)).SetEase(Ease.OutBounce);
            _tickSeq.Append(_smallTick.transform.DOScale(defPos  , JUMP_DURATION));

        }
        // -----------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            _regControllerFile.RightAnswer -= SetText;
            _regControllerFile.RightAnswer -= JumpABit;
        }
    }
}