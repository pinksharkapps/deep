using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoBtn : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        Handheld.Vibrate();
        SceneManager.LoadScene("info");
    }
}