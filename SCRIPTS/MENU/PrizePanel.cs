using System;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using MENU;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// -------- НАСЛЕДУЕТСЯ от панели BasePopPanel
// ----- уже умеет закрываться и шевелить картинкой + добавлено уникальное событие
public class PrizePanel : BasePopPanel
{
    [SerializeField] private TMP_Text _textDescription;
    [SerializeField] private TMP_Text _textButton;

    [SerializeField] private Sprite REGachSprite;
    [SerializeField] private Sprite MEMOachSprite;
    [SerializeField] private Sprite TESTachSprite;
    [SerializeField] private Sprite HOGachSprite;
    [SerializeField] private Sprite ARachSprite;
    [SerializeField] private Sprite AR10achSprite;
    
    // private SpriteRenderer _currentRenderer;
    // [SerializeField] private SpriteRenderer _currentRenderer;
     [SerializeField] private Image _currentImage;
    
    //private Sequence _prizeSeq;

    

    //----------------------------------------------------------------------------------------------
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
       
    }


    public void SetTextOne(string xxx)
    {
        _textDescription.text = $"{xxx}";
    }

    public void SetTextTwo(string xxx)
    {
        _textButton.text = $"{xxx}";
    }

    protected override void AnimatePanelObj()
    {
        //_prizeSeq = DOTween.Sequence();
        //_prizeSeq.Append( _prize.transform.DOMoveY(_prizeSpawnPoint.y +0.3f, 2f)
        //.SetLoops(-1, LoopType.Yoyo));
    }

    protected override void HidePanel()
    {
        GameManager.Instance.GetComponent<AudioManager>().PlaySound("PRESS");
        Handheld.Vibrate();
        gameObject.SetActive(false);
    }


    public void SetPrizePicture(string xxx) // вызывает GameManager, ему приходит из любой игры
    {
        switch (xxx)
        {
            case "REG": _currentImage.sprite = REGachSprite;  break;
            case "MEMO": _currentImage.sprite = MEMOachSprite; break;
            case "TEST": _currentImage.sprite = TESTachSprite; break;
            case "HOG": _currentImage.sprite = HOGachSprite; break;
            case "AR": _currentImage.sprite = ARachSprite; break;
            case "AR10": _currentImage.sprite = AR10achSprite; break;
            
            default: Debug.Log($"Картинка дефолтная, передали {xxx}"); break;
            
            // TODO ADRESSABLE !!!
        }
    }

    private void OnDisable()
    {
        //_prizeSeq?.Kill(true);
    }
}