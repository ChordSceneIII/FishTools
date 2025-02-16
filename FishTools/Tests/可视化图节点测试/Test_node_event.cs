using FishTools;
using FishTools.Graph;
using UnityEngine;

public class Test_node_event : MonoBehaviour, IWalkedEvent
{
    [SerializeField, ReadOnly] private NodeUI _node;
    public NodeUI node => _node;

    [SerializeField, ReadOnly] private WalkSimulator _simulator = null;
    public WalkSimulator simulator
    {
        get => _simulator;
        set => _simulator = value;
    }

    [SerializeField] private ObserveField<bool> _isAvailable = new ObserveField<bool>(false);
    public bool isAvailable
    {
        get => _isAvailable.field;
        set => _isAvailable.field = value;
    }

    public Animation the_animation;

    private void Awake()
    {
        _node = GetComponent<NodeUI>();
        _isAvailable.AddListener(OnAvailableChanged);
    }

    public void OnStartWalk()
    {
        Debug.Log("节点事件触发");
    }

    public void OnAvailableChanged(bool value)
    {
        if (value)
        {
            the_animation.Play();
            Debug.Log($"{node.NID} 可用");
        }
        else
        {
            the_animation.Rewind();
            the_animation.Stop();
            Debug.Log($"{node.NID} 不可用");
        }
    }

    public void OnEndWalk()
    {
        Debug.Log("节点事件结束");
    }
}
