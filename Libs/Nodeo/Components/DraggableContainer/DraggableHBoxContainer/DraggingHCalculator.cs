using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace DeadDog.Nodeo.Components.DraggableContainer;

public class DraggingHCalculator
{
    #region 辅助字段
    private DraggableHBoxContainer _container;
    private int _lastInsertIdx;
    private List<Rect2> _childrenRect = [];
    #endregion
    #region 创建
    public DraggingHCalculator(DraggableHBoxContainer container)
    {
        _container = container;
        _container.OnStartDragging += RespondStartDragging;
    }
    #endregion
    #region 响应
    private void RespondStartDragging()
    {
        if (_container.GetChildren().Count <= 0) return;
        CalcChildrenX();
    }
    #endregion
    #region 操作
    /// <summary>
    /// 获取插入位置
    /// </summary>
    /// <param name="originPos">原坐标</param>
    /// <param name="currentPos">现坐标</param>
    /// <returns>插入索引，如果无法插入则返回-1</returns>
    public int GetInsertIndex(Vector2 originPos,Vector2 currentPos)
    {
        if (Vector2.Inf.Equals(originPos)) return -1;
        var originIdx = CalcPosLoadedIndex(originPos.X);
        var currentIdx = CalcPosLoadedIndex(currentPos.X);
        if (originIdx > currentIdx) return currentIdx == -1 ? 0 : currentIdx;
        var length = _childrenRect.Count;
        if (originIdx < currentIdx) return currentIdx == -1 ? length - 1 : currentIdx + 1;
        return -1;
    }

    public Vector2 GetInsertLinePos(int index)
    {
        if (index < 0 || index > _childrenRect.Count) return Vector2.Inf;
        if (index == 0)
            return new Vector2(
                _childrenRect[0].Position.X - _container.GetThemeConstant("separation") / 2,
                _childrenRect[0].Position.Y
            );
        if (index == _childrenRect.Count)
        {
            return new Vector2
            (
                _childrenRect[^1].Position.X + _childrenRect[^1].Size.X
                + _container.GetThemeConstant("separation") / 2,
                _childrenRect[^1].Position.Y
            );
        }

        var left = _childrenRect[index - 1];
        var right = _childrenRect[index];
        var l = left.Position.X + left.Size.X;
        return new Vector2(
            l + (right.Position.X - l) / 2,
            left.Position.Y > right.Position.Y ? left.Position.Y : right.Position.Y
        );
    }
    #endregion
    #region 处理
    private void CalcChildrenX()
    {
        var children = _container.GetChildren();
        var length = children.Count;
        _childrenRect.Clear();
        for (var i = 0; i < length; i++)
        {
            var child = children[i];
            if (child is not Control ctrl) return;
            var rect = ctrl.GetGlobalRect();
            _childrenRect.Add(rect);
        }
    }

    private int CalcPosLoadedIndex(float x)
    {
        var first = _childrenRect[0];
        var last = _childrenRect[^1];
        if (first.Position.X > x || last.Position.X + last.Size.X < x) return -1;
        var sep = _container.GetThemeConstant("separation");
        var tmp = _childrenRect.FindIndex(0,
            r => r.Position.X - sep <= x && r.Position.X + r.Size.X + sep >= x);
        _lastInsertIdx = tmp == -1 ? _lastInsertIdx : tmp;
        return _lastInsertIdx;
    }
    #endregion
}