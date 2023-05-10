using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestProgressItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _testNumber;
    [SerializeField] private Image _image;

    private Image _myImage;

    // -------?
    //public int itemID;
    //public bool isRight;

    //------------------------------------

    private void Awake()
    {
        _myImage = GetComponentInChildren<Image>();
    }

    public void SetRed()
    {
        _myImage.GetComponent<Image>().color = new Color32(212, 45, 78, 255);
    }

    public void SetGreen()
    {

        _myImage.GetComponent<Image>().color = new Color32(88, 131, 103, 255);
    }

    public void SetDefColor()
    {
        // Белый:
        //_myImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        _myImage.GetComponent<Image>().color = new Color32(237, 237, 237, 255);
        
    }

    public void SetSelectedColor()
    {
        // Желтый:
        // _myImage.GetComponent<Image>().color = new Color32(255, 225, 98, 255);
        // Белый:
        _myImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void SetNumber(int number)
    {
        _testNumber.text = number.ToString();
    }
}