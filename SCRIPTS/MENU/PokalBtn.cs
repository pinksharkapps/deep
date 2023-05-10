using System;
using System.Collections;
using System.Collections.Generic;
using MENU;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PokalBtn : MonoBehaviour
{
    [SerializeField] private GameObject _prize1;
    [SerializeField] private GameObject _prize2;
    [SerializeField] private GameObject _prize3;
    

    private int _pokalState = 1;
    [SerializeField] private ParticleSystem _pokalParticles;

    
    
    private void Awake()
    {
        _pokalParticles.Stop();
        _prize1.SetActive(false);
        _prize2.SetActive(false);
        _prize3.SetActive(false);

        
    }
    
    // --------------------------------------------------------------
    private void Start()
    {
        GameManager.Instance.GetComponent<AudioManager>().PlaySound("WIN");
        _pokalState = GameManager.Instance.pokal_STATE;
        SetPokal(_pokalState); // установит кубок на нужное состояние
    }


    // ---- В максимальном состоянии у кубка частицы ----------------
    public void SetPokal(int xxx)
    {
        switch (xxx)
        {
            case 1: _prize1.SetActive(true);
                break;
            case 2: _prize2.SetActive(true);
                break;
            case 3: _prize3.SetActive(true); _pokalParticles.Play();
                break;

            default: Debug.Log($"<Color = Blue> Неверное число передали в кубок PokalBtn {xxx}</Color>");
                break;
        }
    }

    
    private void OnMouseUpAsButton()
    {
        Handheld.Vibrate();
        SceneManager.LoadScene("POKALS");
    }
}