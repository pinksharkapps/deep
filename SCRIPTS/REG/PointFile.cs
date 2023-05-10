using UnityEngine;

namespace REG
{
    // Висит на точках карты и хранит номер точки (задается руками)
    public class PointFile : MonoBehaviour
    {
        [SerializeField] public int PointNumber;
        [HideInInspector] public Vector2 Pos;

        // -----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            Pos = transform.position;
        }
    }
}