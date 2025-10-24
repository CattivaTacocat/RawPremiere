using System;
using System.Collections.Generic;
using Godot;

namespace DeadDog.Nodeo.Structures
{
    /// <summary>
    /// 节点池（来自Nodeo库）
    /// </summary>
    public class NodePool<T> where T : Node
    {
        #region 属性
        /// <summary>
        /// 池中剩余节点数
        /// </summary>
        public int Count => _pool.Count;

        /// <summary>
        /// 池目前的容量大小
        /// </summary>
        public int Size => _currentSize;

        #endregion
        #region 辅助字段
        private readonly Func<T> _nodeFactory;

        private readonly Queue<T> _pool = new();
        private readonly HashSet<T> _loanedNodes = new();

        private readonly int _maxSize;
        private int _currentSize;
        private readonly int _initSize;

        private bool _allCleaned;
        #endregion
        #region 事件信号
        private readonly Action<T> _onTaken;
        private readonly Action<T> _onReturned;
        #endregion
        #region 生命周期
        #region 创建
        private NodePool(Func<T> factory,
            int initSize = 10,
            int maxSize = int.MaxValue,
            Action<T> onTaken = null,
            Action<T> onReturned = null)
        {
            _nodeFactory = factory;
            _maxSize = maxSize;
            _currentSize = initSize;
            _initSize = initSize;
            _onTaken = onTaken;
            _onReturned = onReturned;
            InitPool();
        }

        /// <summary>
        /// 从场景中创建
        /// </summary>
        /// <param name="scene">打包场景</param>
        /// <param name="initSize">初始容量</param>
        /// <param name="maxSize">最大容量</param>
        /// <param name="onTaken">拿取的时候干什么</param>
        /// <param name="onReturned">放回的时候干什么</param>
        /// <returns>节点池</returns>
        public static NodePool<T> CreateFromScene(
            PackedScene scene,
            Action<T> onTaken = null,
            Action<T> onReturned = null,
            int initSize = 10,
            int maxSize = int.MaxValue
            )
        {
            T CreateNode() => scene.Instantiate<T>();
            return new NodePool<T>(CreateNode, initSize, maxSize, onTaken, onReturned);
        }

        /// <summary>
        /// 从节点副本中创建
        /// </summary>
        /// <param name="dupNode">节点副本，该节点必须是未销毁过的！</param>
        /// <param name="initSize">初始容量</param>
        /// <param name="maxSize">最大容量</param>
        /// <param name="onTaken">拿取的时候干什么</param>
        /// <param name="onReturned">放回的时候干什么</param>
        /// <returns>节点池</returns>
        public static NodePool<T> CreateFromDup(
            T dupNode,
            Action<T> onTaken = null,
            Action<T> onReturned = null,
            int initSize = 10,
            int maxSize = int.MaxValue
            )
        {
            return new NodePool<T>(CreateNode, initSize, maxSize, onTaken, onReturned);

            T CreateNode()
            {
                var dup = dupNode.Duplicate() as T;
                return dup;
            }
        }

        private void InitPool()
        {
            for (int i = 0; i < _currentSize; i++)
            {
                var node = _nodeFactory();
                _pool.Enqueue(node);
            }
            _allCleaned = false;
        }
        #endregion
        #endregion
        #region 操作
        /// <summary>
        /// 拿取
        /// </summary>
        /// <returns>节点对象</returns>
        public T Take()
        {
            if (_allCleaned) InitPool();
            T node;
            if (_pool.Count > 0)
            {
                node = _pool.Dequeue();
            }
            else if (_currentSize < _maxSize)
            {
                node = _nodeFactory();
                _currentSize++;
            }
            else
                throw ErrForTakeOfExcess();

            _loanedNodes.Add(node);
            _onTaken?.Invoke(node);

            return node;
        }

        /// <summary>
        /// 放回
        /// </summary>
        /// <param name="node">放回对象</param>
        public void Return(T node)
        {
            if (node is null) return;

            if (WrnForReturnOfNoLend(node)) return;

            _onReturned?.Invoke(node);

            if (node.GetParent() is not null)
                node.GetParent()?.RemoveChild(node);

            _loanedNodes.Remove(node);
            _pool.Enqueue(node);
        }

        /// <summary>
        /// 拿取一些
        /// </summary>
        /// <param name="count">拿取数量</param>
        /// <returns>节点对象数组</returns>
        public T[] Take(int count)
        {
            var nodes = new T[count];
            for (int i = 0; i < count; i++)
            {
                nodes[i] = Take();
            }

            return nodes;
        }
        
        /// <summary>
        /// 放回一些
        /// </summary>
        /// <param name="nodes">节点对象</param>
        public void Return(params T[] nodes)
        { 
            var count = nodes.Length;
            for (int i = 0; i < count; i++)
            {
                Return(nodes[i]);
            }
        }

        /// <summary>
        /// 扩容
        /// </summary>
        /// <param name="count">扩大容量数量</param>
        public void Expand(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_currentSize >= _maxSize)
                    break;

                var node = _nodeFactory();
                _pool.Enqueue(node);
                _currentSize++;
            }
        }

        /// <summary>
        /// 清除所有
        /// 注意！慎用！此方法会将池内和池外的所有节点销毁掉！并且让池容量回到初始状态！
        /// </summary>
        public void ClearAll(Action<T> onBeforeClear = null)
        {
            if (_allCleaned) return;
            ClearPool();
            foreach (var node in _loanedNodes)
            {
                onBeforeClear?.Invoke(node);
                node.QueueFree();
            }
            _loanedNodes.Clear();
            _currentSize = _initSize;
            _allCleaned = true;
        }

        /// <summary>
        /// 清理池
        /// 清理池内即还未借出的节点
        /// </summary>
        public void ClearPool()
        {
            while (_pool.Count > 0)
            {
                var node = _pool.Dequeue();
                node.QueueFree();
            }
        }
        #endregion
        #region 异常处理
        private static InvalidOperationException ErrForTakeOfExcess()
        {
            var funName = nameof(Take);
            var clsName = nameof(NodePool<T>);
            var msg = $"{funName}:\"{clsName}\"已经到达最大容量了，无法继续分配更多节点，请优化复用";
            return new InvalidOperationException(msg);
        }

        private bool WrnForReturnOfNoLend(T node)
        {
            if (_loanedNodes.Contains(node)) return false;
            GD.PushWarning($"{nameof(Return)}:尝试归还一个未借出的节点\"{node}\"");
            return true;
        }
        #endregion
    }
}
