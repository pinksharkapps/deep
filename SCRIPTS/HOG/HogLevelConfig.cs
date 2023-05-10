using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HOG
{

    [CreateAssetMenu(fileName = "Question_config", menuName = "Создать новый уровнь HOG")]
    public class HogLevelConfig  : ScriptableObject
    {
        [SerializeField] public AssetReference LevelGameObject; 
    }
}