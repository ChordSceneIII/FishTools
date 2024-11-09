using System.Collections;
using UnityEngine;
using TMPro;
namespace FishTools.TextManager
{
    //文本播放器
    public class TextPlayer : MonoBehaviour
    {
        [Label("节点索引"), SerializeField] private int index = 0; //当前读取位置
        public int Index { get => index; }
        [Label("间隔时间 ")] public float duration = 0.25f;
        [Label("对话"), SerializeField] private TextMeshProUGUI textMesh;
        [Label("角色名"), SerializeField] private TextMeshProUGUI nameMesh;


        public TextMeshProUGUI TextMesh
        {
            get
            {
                if (textMesh == null)
                {
                    Debug.LogWarning("未设置文本组件");
                }
                return textMesh;
            }
        }
        public TextMeshProUGUI NameMesh
        {
            get
            {
                if (nameMesh == null)
                {
                    Debug.LogWarning("未设置角色名组件");
                }
                return nameMesh;
            }
        }
        private Coroutine playCoroutine;
        private Coroutine autoPlayCoroutine;
        public Branch currentBranch;//当前分支
        private bool isPlaying = false;
        public bool IsPlaying { get => isPlaying; }

        //加载分支
        public void LoadBranch(string branchName, int index = 0)
        {
            Branch branch = BranchSave.LoadBranch(branchName);
            this.index = index;
            this.currentBranch = branch;
        }

        //分支跳转
        public void JumpBranch(string branchName)
        {
            Branch branch = BranchSave.LoadBranch(branchName);
            this.index = 0;
            this.currentBranch = branch;
        }

        //保存当前分支
        public void SaveCurBranch(string branchName)
        {
            BranchSave.SaveBranch(currentBranch, branchName);
        }

        public void AutoPlay()
        {
            if (currentBranch.nodeList.Count > 0)
            {
                StopAutoPlay();
                autoPlayCoroutine = StartCoroutine(AutoPlayCoroutine());
            }
        }

        public void StopAutoPlay()
        {
            if (autoPlayCoroutine != null)
            {
                StopCoroutine(autoPlayCoroutine);
                autoPlayCoroutine = null;
            }
        }

        public void Skip()
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
            }
            TextMesh.text = currentBranch.nodeList[index].text;
        }

        public void Next()
        {
            if (index < currentBranch.nodeList.Count - 1)
            {
                index++;
                PlayText();
            }
        }

        public void Last()
        {
            if (index > 0)
            {
                if (index >= currentBranch.nodeList.Count)
                {
                    StopAutoPlay();
                    index = currentBranch.nodeList.Count - 2;
                }
                else
                {
                    index--;
                }
                PlayText();
            }
        }

        public void PlayText()
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }
            if (index >= currentBranch.nodeList.Count)
            {
                return;
            }
            playCoroutine = StartCoroutine(PrintTextNode(currentBranch.nodeList[index]));
        }

        //默认播放形式
        private IEnumerator PrintTextNode(TextNode textnode)
        {
            isPlaying = true;
            // 执行文本节点的事件
            textnode.ActionEvent_Callback();
            NameMesh.text = textnode.name;

            //逐字动画
            for (int i = 0; i < textnode.text.Length; i++)
            {
                yield return new WaitForSeconds(duration); // 逐字动画的延迟
                TextMesh.text = textnode.text.Substring(0, i + 1);
            }

            yield return new WaitForSeconds(duration);
            isPlaying = false;
        }

        // 自动播放协程
        private IEnumerator AutoPlayCoroutine()
        {
            while (true)
            {

                //等待打字机动画结束
                yield return new WaitUntil(() => !isPlaying);

                if (playCoroutine != null && index <= currentBranch.nodeList.Count)
                {
                    index++;
                }

                PlayText();

            }
        }


#if UNITY_EDITOR //测试数据
        [ContextMenu("设置测试数据")]
        public void TestSet()
        {
            index = 0;
            currentBranch = new Branch();
            currentBranch.nodeList.Clear();

            // 使用第一条形式添加节点
            currentBranch.nodeList.Add(new TextNode("A", "今天吃什么？"));
            currentBranch.nodeList.Add(new TextNode("B", "不知道，你呢？"));
            currentBranch.nodeList.Add(new TextNode("A", "我想吃火锅。"));
            currentBranch.nodeList.Add(new TextNode("B", "火锅不错，去哪里？"));
            currentBranch.nodeList.Add(new TextNode("A", "就去那家新开的吧。"));
            currentBranch.nodeList.Add(new TextNode("B", "好的，走吧。"));
            currentBranch.nodeList.Add(new TextNode("A", "出发！"));

            Debug.Log("已更改内容为测试数据");
        }
#endif

    }

}