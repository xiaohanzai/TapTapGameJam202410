using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace CombatSystem
{
    public class UIPlayerBuffRevealer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject buffPanel;
        [SerializeField] private TextMeshProUGUI buffText;

        public void SetHoverText(string s)
        {
            buffText.text = s;
        }

        // When the mouse enters the UI element
        public void OnPointerEnter(PointerEventData eventData)
        {
            buffPanel.SetActive(true);  // Show the text
        }

        // When the mouse exits the UI element
        public void OnPointerExit(PointerEventData eventData)
        {
            buffPanel.SetActive(false);  // Hide the text
        }

        private void Start()
        {
            buffPanel.SetActive(false);  // Ensure text is hidden initially
        }
    }
}