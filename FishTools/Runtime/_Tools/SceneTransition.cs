using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Events;

namespace FishTools
{
    /// <summary>
    /// 场景切换过渡器
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class SceneTransition : MonoBehaviour
    {
        [Header("过渡设置")]
        [Label("加载界面")] public GameObject loadingScreen;
        [Label("进度条")] public Slider progressBar;
        [Label("进度文本")] public TextMeshProUGUI progressText;
        [Label("过渡动画")] public Animation anim;
        private Canvas _canvas;
        public Canvas canvas=>FishUtility.LazyGet(this, ref _canvas);
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 更新进度条和文本
        /// </summary>
        private void UpdateProgressUI(float progress)
        {
            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            if (progressText != null)
            {
                progressText.text = $"Loading... {progress * 100:F0}%";
            }
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            // 显示加载界面
            loadingScreen?.SetActive(true);

            // 异步加载场景
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // 更新进度条
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 将进度映射到 0 到 1

                // 更新进度条和文本
                UpdateProgressUI(progress);

                // 检查加载是否完成
                if (progress >= 0.9f)
                {
                    // 允许场景激活
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }

            // 加载完后隐藏加载界面
            loadingScreen?.SetActive(false);


            // 播放过渡动画
            if (anim != null)
            {
                anim.Play();
                yield return new WaitForSeconds(anim.clip.length); // 等待动画播放完成
            }

            // 销毁自身
            Destroy(gameObject);

        }
    }

}