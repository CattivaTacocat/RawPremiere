using System;
using Godot;

namespace RawPremiere.Components;

public partial class StringFormatComp : Node
{
    #region 属性
    [Notify,Export] public string Format { get => _format.Get(); set => _format.Set(value); }
    #endregion
}