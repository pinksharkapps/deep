using System;
using UnityEngine;

namespace MENU
{
    public class InfoController : MonoBehaviour
    {
        private void Start()
        {
           GameManager.Instance.MoveMenuButtonToStandard();
        }
    }
}