using UnityEngine;
using FishTools;
/// <summary>
/// 定制化鼠标场景交互器
///
/// 带一个静态方法获取Ray指向的物体(射线检测参数需要使用配置文件ScenUIConfig配置)
/// </summary>

namespace SceneUITool
{
    public class SceneUIManager : BaseSingletonMono<SceneUIManager>
    {
        [SerializeField]
        private SceneUIConfig config;//配置文件
        public SceneUIConfig Config
        {
            get
            {
                if (config == null)
                {
                    config = Resources.Load<SceneUIConfig>("SceneUIConfig");

                    if (config == null)
                        Debug.LogError("未找到ScenUI的配置资源");

                    return config;
                }
                else
                {
                    return config;
                }
            }
        }

        #region MonoLife
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            // 获取鼠标位置的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GameObject targetObj = GetRayPointObject(ray);

            ButtonHandler(targetObj);
            DragHandler(targetObj);
        }
        #endregion

        #region ButtonHandler 按钮控制器逻辑
        ButtonHandler lastBtnHandler = null;
        ButtonHandler lastClickBtnHandler = null;
        void ButtonHandler(GameObject obj)
        {
            //初始化当前指向游戏对象
            ButtonHandler handler = null;

            if (obj != null)
            {
                handler = obj.GetComponent<ButtonHandler>();
            }

            //点击空白处处理
            if (Input.GetMouseButtonDown(0))
            {
                if (handler != null)
                {
                    if (lastClickBtnHandler != null && lastClickBtnHandler != handler)
                    {
                        lastClickBtnHandler.ClickOutsideEvent();
                    }
                    lastClickBtnHandler = handler;
                }
                if (handler == null && lastClickBtnHandler != null)
                {
                    lastClickBtnHandler.ClickOutsideEvent();
                }
            }

            //鼠标悬停在目标上
            if (handler != null)
            {
                //如果鼠标从一个目标直接移动到另一个目标而没有移动到空白处
                if (lastBtnHandler != handler)
                {
                    //执行上一个目标的回调
                    if (lastBtnHandler != null && lastBtnHandler.isHighLight == false)
                    {
                        //对离开的上一个目标进行操作
                        lastBtnHandler.isHighLight = true;
                        lastBtnHandler.Recover();
                    }
                    //记录当前目标为上一个目标
                    lastBtnHandler = handler;
                }

                //高亮，限制执行一次(不用isHighLight限制则一直执行)
                if (handler.isHighLight == true)
                {
                    handler.isHighLight = false;
                    handler.HighLight();
                }

                if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
                {
                    handler.ClickDownEvent();
                    handler.Press();
                }
                else if (Input.GetMouseButtonUp(0)) // 检测鼠标左键松开
                {
                    handler.ClickUpEvent();
                    handler.Recover();
                }
                else if (Input.GetMouseButton(0)) // 检测鼠标左键按住
                {
                    //持续回调事件
                    handler.ClickEvent();
                }
            }

            //如果鼠标移动到空白处
            if (handler == null)
            {
                //上一个离开的目标操作
                if (lastBtnHandler != null && lastBtnHandler.isHighLight == false)
                {
                    /// 可选
                    /// lastBtnHandler.ClickUpEvent()
                    /// 移动至空白处也执行松开事件（不加这句就是只有在对象下松开才执行）
                    lastBtnHandler.isHighLight = true;
                    lastBtnHandler.Recover();
                }
            }

            //处理悬停事件
            if (handler != null)
            {
                handler.HoverInsideEvent();
            }
            if ((handler == null && lastBtnHandler != null) || (handler != lastBtnHandler))
            {
                lastBtnHandler.HoverOutsideEvent();
            }

        }

        #endregion

        #region DragHandler 拖拽控制器逻辑
        DragHandler thisDragHandler = null;
        void DragHandler(GameObject obj)
        {
            DragHandler handler = null;
            if (obj != null)
            {
                handler = obj.GetComponent<DragHandler>();
            }

            if (Input.GetMouseButtonDown(0) && handler != null)
            {
                thisDragHandler = handler.GetComponent<DragHandler>();
            }

            if (thisDragHandler != null)
            {
                thisDragHandler.Drag();
            }

            if (Input.GetMouseButtonUp(0))
            {
                thisDragHandler = null;
            }
        }
        #endregion


        #region GetHandler 通过Ray获取UI控制器
        public static GameObject GetRayPointObject(Ray ray)
        {
            // 尝试进行2D射线检测
            GameObject hitObject = TryHandleRaycast2D(ray);
            if (hitObject != null)
            {
                return hitObject;
            }

            // 尝试进行3D射线检测
            hitObject = TryHandleRaycast3D(ray);
            if (hitObject != null)
            {
                return hitObject;
            }
            return null;
        }

        public static GameObject TryHandleRaycast3D(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Instance.Config.distance3D, Instance.Config.layerMask3D))
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        public static GameObject TryHandleRaycast2D(Ray ray)
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Instance.Config.distance2D, Instance.Config.layerMask2D);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            return null;
        }
        #endregion
    }
}
