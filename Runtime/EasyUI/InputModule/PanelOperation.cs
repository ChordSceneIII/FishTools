using System.Collections;
using FishTools;
using UnityEngine;

public class PanelOperation : BaseSingletonMono<PanelOperation>
{
    [Label("输入延迟")] public float delay = 0f;

    public void Open(GameObject gameObject)
    {
        StartCoroutine(Open_(gameObject));
    }

    public void Close(GameObject gameObject)
    {
        StartCoroutine(Close_(gameObject));
    }

    public void Repeat(GameObject gameObject)
    {
        StartCoroutine(Repeat_(gameObject));
    }

    private IEnumerator Open_(GameObject gameObject)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);
    }

    private IEnumerator Close_(GameObject gameObject)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (gameObject.activeSelf == true)
            gameObject.SetActive(false);
    }

    private IEnumerator Repeat_(GameObject gameObject)
    {
        yield return new WaitForSecondsRealtime(delay);
        gameObject.SetActive(!gameObject.activeSelf);
    }
}