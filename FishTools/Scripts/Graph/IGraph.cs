namespace FishTools.FGraph
{
    public interface IGraph<T>
    {
        //获取顶点数
        int NodeCount { get; }
        //获取边的数目
        int EdgeCount { get; }
        void UpdateGraph(Node<T>[] nodes, int[,] matrix);
        void AddNode(Node<T> newNode);
        void DelNode(Node<T> node);
        void AddEdge(Node<T> start, Node<T> end, int weight);
        void DelEdge(Node<T> start, Node<T> end);
    }

}