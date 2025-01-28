using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerGUI : MonoBehaviour
    {
        [SerializeField] private Image staminaLine;
        public Image StaminaLine => staminaLine;
    }
}