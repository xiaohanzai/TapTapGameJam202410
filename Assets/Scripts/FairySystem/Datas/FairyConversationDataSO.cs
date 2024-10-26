using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FairySystem
{
    [CreateAssetMenu(fileName = "NewFairyConversationData", menuName = "SOs/Data/Fairy Conversation")]
    public class FairyConversationDataSO : ScriptableObject
    {
        [SerializeField] private List<ConversationData> _conversationDatas = new List<ConversationData>();
        public List<ConversationData> ConversationDatas => _conversationDatas;

        [SerializeField] private int _minHP;
        [SerializeField] private int _maxHP;
    }

    [System.Serializable]
    public class ConversationData
    {
        public Sprite image;
        public string text;
    }
}