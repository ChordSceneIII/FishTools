using UnityEngine;
using UnityEngine.UI;

namespace FishTools.EasyUI
{
    public static class ViewUtils
    {
        public enum Direction
        {
            none = -1,
            up = 0,
            down = 1,
            left = 2,
            right = 3,
        }

        public static void GetDirection(out Direction direction)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Direction.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Direction.right;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Direction.up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Direction.down;
            }
            else
            {
                direction = Direction.none;
            }
        }


        public static GameObject CreateNewImage(string name = "newImage", Transform parent = null, Sprite sprite = null, Color? color = null, Vector2? size = null, Material material = null)
        {
            // 创建一个新的 GameObject
            GameObject imageObject = new GameObject(name);

            imageObject.layer = LayerMask.NameToLayer("UI");

            // 添加 RectTransform 组件
            RectTransform rectTransform = imageObject.AddComponent<RectTransform>();

            // 设置其父级
            imageObject.transform.SetParent(parent);

            // 添加 Image 组件
            Image image = imageObject.AddComponent<Image>();

            // 设置一些默认属性
            image.sprite = sprite; // 设定默认 Sprite
            image.color = color ?? Color.white; // 设定默认颜色
            rectTransform.sizeDelta = size ?? new Vector2(100, 100); // 设定默认大小
            image.material = material;

            // 确保它在层级的最上面
            imageObject.transform.SetAsLastSibling();

            return imageObject;
        }
    }
}