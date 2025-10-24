using System;
using Godot;

namespace RecallPast.Libs.Nodeo;

public static class NodeExpends
{
    #region 操作
    public static void ClearChildren(this Node node)
    {
        if (node is null) return;
        var children = node.GetChildren();
        var length = children.Count;
        for (var i = 0; i < length; i++) children[i].QueueFree();
    }

    public static void AddChildren(this Node node, params Node[] children)
    {
        if (node is null) return;
        foreach (var child in children)
            if (child is not null) node.AddChild(child);
    }

    public static Node ReplaceChildNode(this Node parent,int idx ,Node newNode)
    {
        if (parent is null)
            throw new InvalidOperationException("没有父节点，无法进行替换操作");
        var oldNode = parent.GetChild(idx);
        if (oldNode is null) return null;
        if (oldNode.Equals(newNode)) return oldNode;
        parent.RemoveChild(oldNode);
        parent.AddChild(newNode);
        return oldNode;
    }
    #endregion
}