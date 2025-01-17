using UnityEngine;

namespace FishTools
{
    [CreateAssetMenu(fileName = "SceneTransitionObject", menuName = "FishTools/SceneTransitionObject")]
    public class SceneTransitionObject : ScriptableObject
    {
        public SceneTransition transitionPrefab;
        public void LoadScene(string sceneName)
        {
            try
            {
                if (transitionPrefab != null)
                {
                    Time.timeScale = 1;

                    var _trans = Instantiate(transitionPrefab);

                    _trans?.LoadScene(sceneName);
                }
            }
            catch (System.Exception e)
            {
                // 捕获并记录异常
                Debug.LogError($" 加载场景失败 : {e.Message}");
            }
        }

    }
}