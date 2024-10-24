using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CombatSystem
{
    public class UIWinPanel : MonoBehaviour
    {
        [SerializeField] private Button getLightButton;
        [SerializeField] private Button getBuffButton;
        [SerializeField] private Button doNothingButton;

        public UnityEvent Evt_BtnOnClickUniversal = new UnityEvent();
        public UnityEvent<int> Evt_OnGetLightClicked = new UnityEvent<int>();
        public UnityEvent<BuffType> Evt_OnGetBuffClicked = new UnityEvent<BuffType>();

        private int lightAmount;
        private BuffType newBuff;

        // Start is called before the first frame update
        void Start()
        {
            getLightButton.onClick.AddListener(() => Evt_OnGetLightClicked.Invoke(lightAmount));
            getBuffButton.onClick.AddListener(() => Evt_OnGetBuffClicked.Invoke(newBuff));

            getLightButton.onClick.AddListener(BtnOnClickUniversal);
            getBuffButton.onClick.AddListener(BtnOnClickUniversal);
            doNothingButton.onClick.AddListener(BtnOnClickUniversal);
        }

        private void BtnOnClickUniversal()
        {
            Invoke("DelayedBtnOnClickUniversal", 0.1f);
        }

        private void DelayedBtnOnClickUniversal()
        {
            Evt_BtnOnClickUniversal.Invoke();
        }

        public void SetUpWinPanel(int light, BuffType enemyBuff, string currentBuffDescription, string newBuffDescription)
        {
            lightAmount = light;
            newBuff = enemyBuff;

            getLightButton.GetComponent<UIHoverRevealer>().SetHoverText("Get " + lightAmount.ToString() + " amount of light from defeating this enemy");
            getBuffButton.GetComponent<UIHoverRevealer>().SetHoverText("<color=\"red\">Change from:</color>" + currentBuffDescription + "\n" + "<color=\"red\">To:</color>" + newBuffDescription);
        }
    }
}