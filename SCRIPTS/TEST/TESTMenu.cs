using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class TESTMenu : MonoBehaviour
    {
        [SerializeField] public Button _startBTN;
        [SerializeField] public Image _boyImage;

        [SerializeField] public TMP_Text _textHeader;
        [SerializeField] public TMP_Text _textBody;

        private void Awake()
        {
            // ПРИМЕР  game0bject. GetComponentInChildren<Image>().sprite = signConfig.img;
        }
    }
}