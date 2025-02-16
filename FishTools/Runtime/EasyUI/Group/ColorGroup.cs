
using UnityEngine;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    public class ColorGroup : MonoBehaviour
    {
        public ObserveField<Color> color = new ObserveField<Color>(Color.white);

        public Color colorA = Color.green;
        public Color colorB = Color.white;
        public Image[] images;

        private void Awake()
        {
            var image = GetComponent<Image>();
            if (image != null)
            {
                image.color = color.field;
            }
        }

        private void OnEnable()
        {
            color.AddListener(ChangeColor);
        }

        private void OnDisable()
        {
            color.RemoveListener(ChangeColor);
        }

        public void ChangeColor(Color color)
        {
            foreach (var image in images)
            {
                image.color = color;
            }
        }
        public void SetColorA()
        {
            ChangeColor(colorA);
        }
        public void SetColorB()
        {
            ChangeColor(colorB);
        }

    }
}