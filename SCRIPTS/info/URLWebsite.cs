using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MENU
{
    

public class URLWebsite : MonoBehaviour
{
    public void OpenURL(string link)
    {
        Application.OpenURL("http://ascha.info");
    }
}

}