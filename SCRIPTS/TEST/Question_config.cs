using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TEST
{
    // ==== СОЗДАВАЛКА 1 задачи =============================================
    // ---- меню для создания:
    [CreateAssetMenu(fileName = "Question_config", menuName = "Создать новую задачу")]
    public class Question_config : ScriptableObject
    {
        [SerializeField] public int ques_Number; // общий порядковый номер задачи ОТ ОЛЕГА

        [Space] [Header("Картинка задачи")] [SerializeField]
        public AssetReferenceSprite ques_Image; // картинка JPG к задаче ЧЕРЕЗ АДРЕССЭЙБЛЫ

        // ВНИМАНИЕ, ПРОБУЮ СПРАЙТЫ!!!!
        [Space] [SerializeField] bool ques_DoesImage_A_exist; // параметр, имеется ли анимация к задаче?
        [SerializeField] public AssetReferenceSprite ques_Image_A; // картинка для анимации

        [Space] [Space] [Header("Текст вопроса задачи:")] [SerializeField] [TextArea(2, 5)]
        public string ques_Text;

        [Space] [Header("Вариант ответа 1")] [SerializeField] [TextArea(2, 5)]
        public string ques_Var1;

        [Header("Вариант ответа 2")] [SerializeField] [TextArea(2, 5)]
        public string ques_Var2;

        [Header("Вариант ответа 3")] [SerializeField] [TextArea(2, 5)]
        public string ques_Var3; // ЕСЛИ ВДРУГ БУДЕТ ??????

        [Space] [Header("Какой вариант правильный?")] [SerializeField]
        public int ques_Answer; // Текст вопроса 
    }
}