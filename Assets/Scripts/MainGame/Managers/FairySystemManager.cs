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

    private bool isFairyMet;
    public bool IsFairyMet => isFairyMet;

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
        isFairyMet = true;
        conversationManager.StartShowingSlides(i);
    }

    public (int, int) GetCurrentFairyHPs()
    {
        return conversationManager.GetCurrentFairyHPs();
    }

    public void ShowFairyUnseenUI()
    {
        isFairyMet = false;
        conversationManager.ShowFairyUnseenUI();
    }
}
