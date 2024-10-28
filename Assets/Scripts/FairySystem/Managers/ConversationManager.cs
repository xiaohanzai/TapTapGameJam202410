using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FairySystem
{
    public class ConversationManager : MonoBehaviour
    {
        [SerializeField] private FairyConversationDataSO[] fairyConversationDatas;

        [SerializeField] private Button nextBtn;
        [SerializeField] private Button proceedBtn;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI textMeshPro;

        [SerializeField] private GameObject slidesUI;
        [SerializeField] private GameObject fairyUnseenUI;

        [SerializeField] private float waitTime = 2f;

        private int ind;
        private int indSlide;

        void Start()
        {
            nextBtn.onClick.AddListener(ShowNextSlide);
            proceedBtn.onClick.AddListener(StartShowingSlides);

            ind = -1;

            StartShowingSlides();
        }

        public void StartShowingSlides(int i)
        {
            ind = i - 1;
            slidesUI.SetActive(true);
            fairyUnseenUI.SetActive(false);
            StartShowingSlides();
        }

        private void StartShowingSlides()
        {
            ind = (ind + 1) % fairyConversationDatas.Length;
            indSlide = 0;
            proceedBtn.gameObject.SetActive(false);
            if (ind < fairyConversationDatas.Length) ShowNextSlide();
        }

        private void ShowNextSlide()
        {
            //image.sprite = fairyConversationDatas[ind].ConversationDatas[indSlide].image;
            textMeshPro.text = fairyConversationDatas[ind].ConversationDatas[indSlide].text;
            nextBtn.gameObject.SetActive(false);
            indSlide++;
            if (indSlide < fairyConversationDatas[ind].ConversationDatas.Count) Invoke("DelayedShowNextBtn", waitTime);
            else Invoke("DelayedShowProceedBtn", waitTime);
        }

        private void DelayedShowNextBtn()
        {
            nextBtn.gameObject.SetActive(true);
        }

        private void DelayedShowProceedBtn()
        {
            proceedBtn.gameObject.SetActive(true);
        }

        public (int, int) GetCurrentFairyHPs()
        {
            return (fairyConversationDatas[ind].MinHP, fairyConversationDatas[ind].MaxHP);
        }

        public void ShowFairyUnseenUI()
        {
            fairyUnseenUI.SetActive(true);
            slidesUI.SetActive(false);
            proceedBtn.gameObject.SetActive(true);
        }
    }
}