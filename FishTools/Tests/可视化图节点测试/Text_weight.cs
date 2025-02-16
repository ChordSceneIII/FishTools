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
        public InputField inputf;

        private void Awake()
        {
            connectionUI.weight.AddListener(() => inputf.text = connectionUI.weight.field.ToString());
            connectionUI.weight.Refresh();
            inputf.onEndEdit.AddListener((s) => connectionUI.weight.field = int.Parse(s));

        }
    }
}
