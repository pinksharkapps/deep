using System;
using System.Collections;
using System.Collections.Generic;
using AR;
using UnityEngine;
using UnityEngine.UI;

public class CloseEasy : MonoBehaviour
{
    [SerializeField] private Button _closeEasyBtn;
    private ARMenu _arMenyFile;

    private void Awake()
    {
        _arMenyFile = FindObjectOfType<ARMenu>();
    }


    public void CloseEasyPanel()
    {
        _arMenyFile.HideEasyPanel();
    }

}
