using System.Collections;
using System.Collections.Generic;
using MENU;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button menuRegBTN;
    [SerializeField] private Button menuMemoBTN;
    [SerializeField] private Button menuTestBTN;
    [SerializeField] private Button menuHogBTN;
    [SerializeField] private Button menuARBTN;
    
    private AudioManager _audioManager;
    void Start()
    {
        _audioManager = GameManager.Instance.GetComponent<AudioManager>(); 

    }
    
    void Update()
    {
        
    }

    public void StartRegGame()
    {
        _audioManager.PlaySound("PRESS");
        SceneManager.LoadScene("REG");
    }
    
    
    public void StartMemoGame()
    {
        _audioManager.PlaySound("PRESS");
        SceneManager.LoadScene("MEMO");
    }
    
    
    public void StartHogGame()
    {
        _audioManager.PlaySound("PRESS");
        SceneManager.LoadScene("HOG");
    }
    
    public void StartTestGame()
    {
        _audioManager.PlaySound("PRESS");
        SceneManager.LoadScene("Test");
    }
    
    public void StartARGame()
    {
        _audioManager.PlaySound("PRESS");
        SceneManager.LoadScene("AR");
    }
}
