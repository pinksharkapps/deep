using UnityEngine;

// ScriptableObject для нескольких наборов дорожных знаков
namespace MEMO
{
    [CreateAssetMenu(fileName = "Signs", menuName = "Add new - one sign config")]
    
    public class SignConfig : ScriptableObject
    {
        // Номер знака
        public new string name;

        // Описание знака
        public string decription;

        // Картинка знака
        public Sprite img;

        // Сложность знака
        public int level;  // 0, 1, 2;
    }
}