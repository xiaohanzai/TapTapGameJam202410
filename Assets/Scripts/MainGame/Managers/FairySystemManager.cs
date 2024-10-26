using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FairySystemManager : MonoBehaviour
{
    [SerializeField] private GameObject mainBody;
    [SerializeField] private Button proceedBtn;

    [SerializeField] private FairySystem.ConversationManager conversationManager;

    public UnityEvent Evt_OnEncounterEnded = new UnityEvent();

    void Start()
    {
        proceedBtn.onClick.AddListener(OnProceedBtnPressed);
    }

    public void Activate()
    {
        mainBody.SetActive(true);
    }

    public void Deactivate()
    {
        mainBody.SetActive(false);
    }

    public void OnProceedBtnPressed()
    {
        Evt_OnEncounterEnded.Invoke();
    }

    public void ShowFairy(int i)
    {
        conversationManager.StartShowingSlides(i);
    }
}
