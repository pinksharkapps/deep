using System;
using UnityEngine;
using DG.Tweening;
using MENU;
using UnityEngine.Serialization;

namespace MEMO
{
    public class DisplaySign : MonoBehaviour
    {
        // public SignConfig _signConfig; // КОНФИГ ЗНАКА
        // private string thisSignName;
        [SerializeField] private GameObject backside;
        [SerializeField] private GameObject frame;
        [SerializeField] private GameObject imageObj;
        [SerializeField] private ParticleSystem _animation;

        private Sprite _thisSignSprite;

        private MemoBoardController _board;

        private Rigidbody2D _myRigidbody2D;
        private bool isOpened = false;
        public bool isInteractable = true; // чтобы блокировать всех на время проверки
        public bool isSolved = false; // !!!! ВКЛ если совпали !!!!

        private const float OPENED_TIME = 0.8f;
        private float givenTimeToOpen;

        public event Action<string> MemoOpened;
        public string mySignID;

        [FormerlySerializedAs("signDesription")]
        public string mySignDesription;

        private AudioManager _audioManager;

        // ----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            frame.SetActive(false);
            backside.SetActive(true);

            _thisSignSprite = imageObj.GetComponentInChildren<SpriteRenderer>().sprite;
            _board = FindObjectOfType<MemoBoardController>();
            
            _audioManager = GameManager.Instance.GetComponent<AudioManager>(); 

        }

        public void SetSprite(Sprite toSet, string id, string description)
        {
            _thisSignSprite = toSet;
            mySignID = id;
            mySignDesription = description;
            Debug.Log($"Мемо получила ID {mySignID}");
        }

        // ------------------------------------- на клик + Rigidbody + Collider2D -----------------------------------

        private void OnMouseUpAsButton()
        {
            if (!isInteractable || isSolved)
            {
                Debug.Log($"Мемо решена или недоступна: {mySignID}");
                return;
            }

            // открытые не кликаются
            if (isOpened)
            {
                Debug.Log($"Мемо уже была открыта: {mySignID}. Игнорируем...");
                return;
            }

            Debug.Log($"Открыть Мемо: {mySignID}");

            isOpened = true;
            Handheld.Vibrate();

            // ждет события проверки чтобы перевернуться
            _board.CheckCompleted += IfNotSolved_WrapToBack;
            
            MemoOpened?.Invoke(mySignID); // идет в борду проверяться

            TurnToSign();
        }

        // Закрыть Memo
        private void IfNotSolved_WrapToBack()
        {
            if (!isSolved)
            {
                Debug.Log($"Закрыть Мемо: {mySignID}");
                TurnToBack(OPENED_TIME);
            }
            
            // Отписаться от закрытия Memo
            _board.CheckCompleted -= IfNotSolved_WrapToBack;
        }
        

        // ------------------------------------------- Эффекты Старт/Стоп -------------------------------------------

        public void PlayParticles()
        {
            _animation.Play();
        }

        public void StopParticles()
        {
            _animation.Stop();
        }


        // --------------------------------------- методы анимациии перевроачивания ---------------------------------

        public void TurnToSign()
        {
            isOpened = true;

            _audioManager.PlaySound("PRESS");
            DOTween.Sequence()
                .AppendCallback(PlayParticles) //TODO - ПОЧЕМУ НЕ РАБОТАЮТ?
                .Append(backside.transform.DOScaleX(0, 0.2f))
                .Join(imageObj.transform.DOScaleX(0, 0f))
                .Join(imageObj.transform.DOScaleX(1, 0.2f))
                .OnComplete(StopParticles);
        }

        public void TurnToBack(float time)
        {
            Debug.Log($"Пытаемся перевернуть {mySignID} через {time} секунд");
            _audioManager.PlaySound("PRESS");
            DOTween.Sequence()
                .AppendInterval(time)
                .Append(backside.transform.DOScaleX(1, 0.2f))
                .Join(imageObj.transform.DOScaleX(0, 0.2f))
                .AppendCallback(SetOpenedFalse);
        }

        private void SetOpenedFalse()
        {
            isOpened = false;
        }

        private void OnDestroy()
        {
            _board.CheckCompleted -= IfNotSolved_WrapToBack;
        }
    }
}