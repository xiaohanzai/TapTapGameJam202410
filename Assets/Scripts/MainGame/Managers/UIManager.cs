using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button startGameBtn;

    [SerializeField] private TextMeshProUGUI fairyEncounterText;
    [SerializeField] private TextMeshProUGUI countDownTxt;

    [SerializeField] private Button shareLightBtn;
    [SerializeField] private Button proceedBtn;
    [SerializeField] private Button auroraBtn;

    private int ind;

    public UnityEvent Evt_OnStartGameBtnPressed = new UnityEvent();
    public UnityEvent Evt_OnShareLightBtnPressed = new UnityEvent();
    public UnityEvent<int> Evt_OnProceedBtnPressed = new UnityEvent<int>();
    public UnityEvent Evt_OnAuroraBtnPressed = new UnityEvent();
    public UnityEvent Evt_OnFakeBossFightClicked = new UnityEvent();

    void Start()
    {
        startGameBtn.onClick.AddListener(() =>
        {
            mainMenuUI.SetActive(false);
            Evt_OnStartGameBtnPressed.Invoke();
        });

        shareLightBtn.onClick.AddListener(OnShareLightBtnPressed);
        proceedBtn.onClick.AddListener(OnProceedBtnPressed);
        auroraBtn.onClick.AddListener(OnAuroraBtnPressed);

        fairyEncounterText.text = "";
    }

    public void ShowShareLightBtn()
    {
        shareLightBtn.gameObject.SetActive(true);
        proceedBtn.gameObject.SetActive(false);
        auroraBtn.gameObject.SetActive(false);
    }

    public void HideShareLightBtn()
    {
        shareLightBtn.gameObject.SetActive(false);
    }

    public void ShowProceedBtn()
    {
        proceedBtn.gameObject.SetActive(true);
    }

    public void ShowAuroraBtn()
    {
        auroraBtn.gameObject.SetActive(true);
    }

    public void ShowFakeBossFightBtn()
    {
        Evt_OnFakeBossFightClicked.Invoke();
    }

    private void OnProceedBtnPressed()
    {
        Evt_OnProceedBtnPressed.Invoke(ind);
        ind = (ind + 1) % 2;
    }

    private void OnShareLightBtnPressed()
    {
        Evt_OnShareLightBtnPressed.Invoke();
    }

    private void OnAuroraBtnPressed()
    {
        Evt_OnAuroraBtnPressed.Invoke();
    }

    public void UpdateCountDownText(float t)
    {
        if (t > 0) countDownTxt.text = Mathf.CeilToInt(t) + " sec left to share light";
        else countDownTxt.text = "";
    }

    public void ShowFairyEncounterText(string s, float time)
    {
        fairyEncounterText.text = s;
        Invoke("DelayedResetFairyEncounterText", time);
    }

    private void DelayedResetFairyEncounterText()
    {
        fairyEncounterText.text = "";
    }

    public void ShowMainMenuUI()
    {
        mainMenuUI.SetActive(true);
    }
}
