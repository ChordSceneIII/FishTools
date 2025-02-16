using FishTools.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace FishTools.Tests
{
    public class Text_node : MonoBehaviour
    {
        public Text text;
        public NodeUI nodeUI;
        private void Awake()
        {
            nodeUI = GetComponentInParent<NodeUI>();
            text = GetComponent<Text>();
        }
        private void Update()
        {
            text.text = nodeUI.NID.ToString();
        }
    }
}