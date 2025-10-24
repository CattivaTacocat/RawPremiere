#nullable enable
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Newtonsoft.Json;

namespace DeadDog.RecallPast.Libs.Nodeo.Input;

public static class NodeoInputParser
{
    #region 操作
    public static NodeoInputDto? ParseFromInputEvent(InputEvent @event) =>
        ParseInputType(@event) switch
        {
            NodeoInputTypeEnum.Keyboard => CreateDtoFromKey(@event as InputEventKey),
            NodeoInputTypeEnum.MouseButton => CreateDtoFromMouseButton(@event as InputEventMouseButton),
            NodeoInputTypeEnum.JoypadButton => CreateDtoFromJoypadButton(@event as InputEventJoypadButton),
            NodeoInputTypeEnum.JoypadMotion => CreateDtoFromJoypadMotion(@event as InputEventJoypadMotion),
            _ => null
        };

    private static NodeoInputTypeEnum ParseInputType(InputEvent @event) =>
        @event switch
        {
            InputEventKey => NodeoInputTypeEnum.Keyboard,
            InputEventMouseButton => NodeoInputTypeEnum.MouseButton,
            InputEventMouseMotion => NodeoInputTypeEnum.MouseMotion,
            InputEventJoypadButton => NodeoInputTypeEnum.JoypadButton,
            InputEventJoypadMotion => NodeoInputTypeEnum.JoypadMotion,
            InputEventScreenTouch => NodeoInputTypeEnum.GestureTouch,
            InputEventScreenDrag => NodeoInputTypeEnum.GestureDrag,
            InputEventMagnifyGesture => NodeoInputTypeEnum.GestureZoom,
            InputEventMidi => NodeoInputTypeEnum.MIDI,
            _ => NodeoInputTypeEnum.Unknown
        };
    
    public static InputEvent? ParseFromNodeoInputDto(NodeoInputDto dto) =>
        dto.InputType switch
        {
            NodeoInputTypeEnum.Keyboard => CreateEventKeyFromDto(dto),
            NodeoInputTypeEnum.MouseButton => CreateEventMouseButtonFromDto(dto),
            NodeoInputTypeEnum.JoypadButton => CreateEventJoypadButtonFromDto(dto),
            NodeoInputTypeEnum.JoypadMotion => CreateEventJoypadMotionFromDto(dto),
            _ => null
        };

    public static Dictionary<string, InputEvent[]> ParseMapDtoToDic(NodeoInputMapDto map)
    {
        var dic = new Dictionary<string, InputEvent[]> ();
        foreach (var (key, value) in map.InputMap) 
            dic.Add(key,value.Select(ParseFromNodeoInputDto).OfType<InputEvent>().ToArray());
        return dic;
    }

    public static NodeoInputMapDto ParseMapDicToDto(Dictionary<string, InputEvent[]> dic)
    {
        var map = new NodeoInputMapDto();
        foreach (var (key, value) in dic)
            map.InputMap.Add(key,value.Select(ParseFromInputEvent).OfType<NodeoInputDto>().ToArray());
        return map;
    }
    
    /// <summary>
    /// 从Json文件中读取
    /// </summary>
    /// <param name="jsonPath">文件的绝对路径</param>
    /// <returns>动作-事件 键值对数组</returns>
    public static Dictionary<string,InputEvent[]> Read(string jsonPath)
    {
        var jsonString = File.ReadAllText(jsonPath);
        var map = JsonConvert.DeserializeObject<NodeoInputMapDto>(jsonString);
        return map is not null ? ParseMapDtoToDic(map) : new Dictionary<string, InputEvent[]>();
    }
    
    /// <summary>
    /// 写入Json文件
    /// </summary>
    /// <param name="jsonPath">写入路径</param>
    /// <param name="map">映射数据</param>
    public static void Write(string jsonPath, NodeoInputDto map)
    {
        var js = JsonSerializer.Create();
        var jsonString = JsonConvert.SerializeObject(map);
        File.WriteAllText(jsonPath,jsonString);
    }
    #endregion
    #region 数据
    private static NodeoInputDto? CreateDtoFromKey(InputEventKey? key)
    {
        if (key is null) return null;
        return new()
        {
            InputType = NodeoInputTypeEnum.Keyboard,
            InputIndex = (long)key.Keycode
        };
    }

    private static NodeoInputDto? CreateDtoFromMouseButton(InputEventMouseButton? button)
    {
        if (button is null) return null;
        return new()
        {
            InputType = NodeoInputTypeEnum.MouseButton,
            InputIndex = (long)button.ButtonIndex
        };
    }

    private static NodeoInputDto? CreateDtoFromJoypadButton(InputEventJoypadButton? button)
    {
        if (button is null) return null;
        return new()
        {
            InputType = NodeoInputTypeEnum.JoypadButton,
            InputIndex = (long)button.ButtonIndex
        };
    }

    private static NodeoInputDto? CreateDtoFromJoypadMotion(InputEventJoypadMotion? motion)
    {
        if (motion is null) return null;
        return new()
        {
            InputType = NodeoInputTypeEnum.JoypadMotion,
            InputIndex = (long)motion.Axis,
            AxisValue = motion.AxisValue
        };
    }

    private static InputEventKey? CreateEventKeyFromDto(NodeoInputDto? dto)
    {
        if (dto is null) return null;
        return new()
        {
            Keycode = (Key)dto.InputIndex,
            Pressed = true
        };
    }
    
    private static InputEventMouseButton? CreateEventMouseButtonFromDto(NodeoInputDto? dto)
    {
        if (dto is null) return null;
        return new()
        {
            ButtonIndex = (MouseButton)dto.InputIndex,
            Pressed = true
        };
    }
    
    private static InputEventJoypadButton? CreateEventJoypadButtonFromDto(NodeoInputDto? dto)
    {
        if (dto is null) return null;
        return new()
        {
            ButtonIndex = (JoyButton)dto.InputIndex,
            Pressed = true
        };
    }
    
    private static InputEventJoypadMotion? CreateEventJoypadMotionFromDto(NodeoInputDto? dto)
    {
        if (dto?.AxisValue is null) return null;
            return new()
            {
                Axis = (JoyAxis)dto.InputIndex,
                AxisValue = (float)dto.AxisValue
            };
    }
    #endregion
}