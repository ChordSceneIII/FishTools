using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    public class TableSelector : BaseHandler
    {
        public Transform guideButtons;
        public Transform contents;
        public bool keepPreference = false;
        [ConditionalField("keepPreference", true)] public int preferenceIndex;
        [ReadOnly, SerializeField] private int _curIndex = 0;
        [ReadOnly] public List<GameObject> objs;
        [ReadOnly] public List<Button> buttons;

        void Awake()
        {
            objs.Clear();
            buttons.Clear();
            foreach (Transform obj in guideButtons)
            {
                var btn = obj.GetComponent<Button>();
                buttons.Add(btn);
            }
            foreach (Transform obj in contents)
            {
                objs.Add(obj.gameObject);
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                int new_i = i;
                buttons[i].onClick.AddListener(() => Select(new_i));
            }
        }

        void OnEnable()
        {
            if (keepPreference)
                Select(preferenceIndex);
            else
                Select(_curIndex);
        }

        public void Select(int index)
        {

            objs.ForEach(x => x.SetActive(false));
            buttons.ForEach(x => x.interactable = true);

            if (index < 0 || index >= objs.Count) return;

            buttons[index].interactable = false;
            objs[index].SetActive(true);
            _curIndex = index;
        }
    }
}