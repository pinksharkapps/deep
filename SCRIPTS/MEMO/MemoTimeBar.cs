using DG.Tweening;
using MENU;
using TMPro;
using UnityEngine;

// Временна шкала на  баре
// У бара две функции: время или отображение названия дорожного знака

public class MemoTimeBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private GameObject _greenBar;
    [SerializeField] private GameObject _clock;

    private Vector2 defClockScale;

    private Vector2 defGreenBarScale;

    private AudioManager _audioManager;

    private Sequence _FadeSuggest; // для плавной смены текста
    private string stringFadeTo;

    // ---------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        // Скрыть зеленую панель бара
        _greenBar.SetActive(false);

        _audioManager = GameManager.Instance.GetComponent<AudioManager>();
        // Запоминаем scale картинки часов
        defClockScale = _clock.transform.localScale;
        // Запомнить scale зеленого бара таймера
        defGreenBarScale = _greenBar.transform.localScale;

        // Скрыть часы и зеленую панель таймера
        HideClock_and_GreenBar();
    }

    // ---------------------------------------------------------------------------------------------------------------

    // Установить текст на баре
    public void SetText(string whatTo)
    {
        _timeText.text = whatTo;
    }

    public void SetFadedText(string whatTo)
    {
        FadeTextChange(whatTo, 1f);
    }
    // ---------------------------------------------------------------------------------------------------------------

    // Запуск таймера
    public void TimeStartCountDown(float time)
    {
        // Отобразить часы и зеленую панель таймера
        ShowClock_and_GreenBar();


        _audioManager.PlaySound("CLOCK");

        // Таймер ползет и часики в конце исчезают
        DOTween.Sequence()
            .AppendInterval(0.25f)
            // Линейный отсчет времени
            .Append(_greenBar.transform.DOScaleX(0, time).OnComplete(HideClock_and_GreenBar).SetEase(Ease.Linear))
            .OnComplete(StopClock);
    }


    private void StopClock()
    {
        _audioManager.StopLoopedSound();
    }


    // ---------------------------------------------- PRIVATE --------------------------------------------------------

    // Отобразить часы и зеленую панель таймера
    private void ShowClock_and_GreenBar()
    {
        _greenBar.SetActive(true);
        DOTween.Sequence()
            .AppendInterval(0.25f)
            .Append(_clock.transform.DOScale(defClockScale, 1.0f).SetEase(Ease.InExpo));
    }

    // Скрыть часы, а также скрыть и восстановить scale зеленой панели таймера для следующего уровня
    private void HideClock_and_GreenBar()
    {
        _greenBar.SetActive(false);
        _greenBar.transform.DOScale(defGreenBarScale, 0);

        _clock.transform.DOScale(Vector2.zero, 0.8f).SetEase(Ease.InExpo);
    }


    //=====================================================================================================
    //============= ПЛАВНАЯ СМЕНА ТЕКСТА ==================================================================

    private void FadeTextChange(string to, float sec) // на какой текст, сколько ждем
    {
        SaveTextTo(to);

        _FadeSuggest = DOTween.Sequence()
            .Append(_timeText.DOFade(0, sec))
            .AppendCallback(ChangeText)
            .Append(_timeText.DOFade(225, sec));
    }

    private void SaveTextTo(string to)
    {
        stringFadeTo = to; // пришлось запоминать глобально потому что нельзя передавать переменную в дотвин
    }

    private void ChangeText()
    {
        _timeText.text = stringFadeTo; // КУДА СУЕМ ТЕКСТ
    }

    //===================== до сих ==================================================================
}