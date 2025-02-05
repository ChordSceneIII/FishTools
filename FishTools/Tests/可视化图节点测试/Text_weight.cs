using System.Collections;
using System.Collections.Generic;
using FishTools.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace FishTools.Tests

{
    public class Text_weight : MonoBehaviour
    {
        public ConnectionUI connectionUI;
        public Text text;

        private void Awake()
        {
            connectionUI.weight.AddListener(() => text.text = connectionUI.weight.field.ToString());
            connectionUI.weight.Refresh();
        }
    }
}
