using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIHoverRevealer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverPanel;
    [SerializeField] private TextMeshProUGUI hoverText;

    public void SetHoverText(string s)
    {
        hoverText.text = s;
    }

    // When the mouse enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverPanel.SetActive(true);  // Show the text
    }

    // When the mouse exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverPanel.SetActive(false);  // Hide the text
    }

    private void Start()
    {
        hoverPanel.SetActive(false);  // Ensure text is hidden initially
    }
}
