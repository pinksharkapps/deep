using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace REG
{
    public class PedBoy : MonoBehaviour
    {
        [SerializeField] private GameObject _effectRight;
        [SerializeField] private GameObject _effectWrong;

        private Animator _animator;
        private float _boySpeed;

        private const string ISMOVING = "IsMoving";
        private const string ISFRONTFACE = "IsFrontFace";

        // Время отображения результата
        private const float EFFECT_SHOW_TIME = 5f;

        // -----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();

            // По умолчанию пешеход стоит
            _animator.SetBool(ISMOVING, false);

            // Скрыть ответы
            HideEffects();
        }

        // ---------------------------------------- ПОКАЗАТЬ / СПРЯТАТЬ ОТВЕТЫ  -------------------------------------
        public void ShowRightAnswer()
        {
            _effectRight.SetActive(true);
            Invoke("HideEffects", EFFECT_SHOW_TIME); // спраятать эффекты через 5 с
        }

        public void ShowWrongAnswer()
        {
            _effectWrong.SetActive(true);
            Invoke("HideEffects", EFFECT_SHOW_TIME); // спраятать эффекты через 5 с
        }

        // -----------------------------------------------------------------------------------------------------------

        private void HideEffects()
        {
            _effectRight.SetActive(false);
            _effectWrong.SetActive(false);
        }

        // -----------------------------------------------------------------------------------------------------------

        public void SetBoyGoing()
        {
            _animator.SetBool(ISMOVING, true);
        }

        public void SetBoyStanding()
        {
            _animator.SetBool(ISMOVING, false);
        }
        
        // -----------------------------------------------------------------------------------------------------------

        // Пешеход повернут к нам лицом или затылком
        public void SetBoyFrontFace()
        {
            _animator.SetBool(ISFRONTFACE, true);
        }

        public void SetBoyBackFace()
        {
            _animator.SetBool(ISFRONTFACE, false);
        }
    }
}