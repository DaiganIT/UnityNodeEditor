public interface INodeFactory
{
    BaseNode CreateNode(string nodeType, BaseGraphView graphView);
}