using System;
using System.Collections.Generic;
using DeadDog.Ordexp;
using DeadDog.RawPremiere.Standards;
using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Codes.Factories;

public static class ShapeFactory
{
    #region 字段
    private static Dictionary<ShapeEnum, List<Vector2>> ShapeTypeDictionary = new()
    {
        {ShapeEnum.Triangle, CreateRegularShape(3)},
        {ShapeEnum.Rectangle, CreateRegularShape(4)},
        {ShapeEnum.Pentagon, CreateRegularShape(5)},
        {ShapeEnum.Hexagon, CreateRegularShape(6)},
        {ShapeEnum.Octagon, CreateRegularShape(8)},
        {ShapeEnum.Circle, CreateRegularShape(GlobalUnit.CIRCLE_SIDES)},
        {ShapeEnum.Lozenge, CreateLozengeShape()},
        {ShapeEnum.Cross, CreateCrossShape()},
        {ShapeEnum.Arrow, CreateArrowShape()},
        {ShapeEnum.Mucro, CreateAngleShape()},
        {ShapeEnum.Drop, CreateDropShape()},
        {ShapeEnum.Diamond, CreateDiamondShape()},
        {ShapeEnum.Star4, CreateStarShape(4)},
        {ShapeEnum.Star5, CreateStarShape(5)},
        {ShapeEnum.Star6, CreateStarShape(6)},
        {ShapeEnum.Heart, CreateHeartShape()},
        {ShapeEnum.Unknown, []},
    };
    #endregion
    #region 操作
    public static List<Vector2> CreateShape(ShapeEnum shapeType) =>
        ShapeTypeDictionary.TryGetValue(shapeType, out var shape) 
            ? shape : ShapeTypeDictionary[ShapeEnum.Unknown];
    #endregion
    #region 处理
    private static List<Vector2> CreateRegularShape(int sides)
    {
        if (sides is < 3 or > 128)
        {
            return ShapeTypeDictionary[ShapeEnum.Unknown];
        }
        var shapePoints = new List<Vector2>();
        if (sides % 4 == 0)
        {
            var startAngle = MathF.PI / sides;
            var increment = startAngle * 2;
            var radius = GlobalUnit.UNIT_HALF_LENGTH / MathF.Cos(startAngle);
            for (var i = 0; i < sides; i++)
            {
                var angle = startAngle + i * increment;
                shapePoints.Add(MathExpends.RadToVector2(angle, radius));
            }
        }
        else
        {
            const float startAngle = -MathF.PI / 2;
            const float radius = GlobalUnit.UNIT_HALF_LENGTH;
            var increment = MathF.Tau / sides;
            for (var i = 0; i < sides; i++)
            {
                var angle = startAngle + i * increment;
                shapePoints.Add(MathExpends.RadToVector2(angle, radius));
            }
        }
        return shapePoints;
    }
    
    private static List<Vector2> CreateLozengeShape() =>
    [
        Vector2.Up * GlobalUnit.UNIT_HALF_LENGTH,
        Vector2.Left * GlobalUnit.UNIT_HALF_LENGTH,
        Vector2.Down * GlobalUnit.UNIT_HALF_LENGTH,
        Vector2.Right * GlobalUnit.UNIT_HALF_LENGTH
    ];
    
    private static List<Vector2> CreateCrossShape()
    {
        const float high = GlobalUnit.UNIT_HALF_LENGTH;
        const float low = high / 5;
        return
        [
            new(-high, low), new(-high, -low), new(-low, -low), new(-low, -high),
            new(low, -high), new(low, -low), new(high, -low), new(high, low),
            new(low, low), new(low, high), new(-low, high), new(-low, low)
        ];
    }
    
    private static List<Vector2> CreateArrowShape() =>
    [
        new(0, -20), new(-13, -7), new(-13, 2),
        new(-4, -7), new(-4, 20), new(4, 20),
        new(4, -7), new(13, 2), new(13, -7)
    ];
    
    private static List<Vector2> CreateAngleShape() =>
    [
        new(0, -20), new(-13, -7), new(-13, 2),
        new(0, -11), new(13, 2), new(13, -7)
    ];
    
    private static List<Vector2> CreateDropShape()
    {
        var shapePoints = new List<Vector2>();
        const int sides = GlobalUnit.CIRCLE_SIDES / 4 * 3;
        const float startAngle = MathF.PI / GlobalUnit.CIRCLE_SIDES;
        const float increment = startAngle * 2;
        var radius = GlobalUnit.UNIT_HALF_LENGTH / MathF.Cos(startAngle);
        for (var i = 0; i < sides; i++)
        {
            var angle = startAngle + i * increment;
            shapePoints.Add(MathExpends.RadToVector2(angle, radius));
        }
        shapePoints.Add(new(GlobalUnit.UNIT_HALF_LENGTH, -GlobalUnit.UNIT_HALF_LENGTH));
        return shapePoints;
    }
    
    private static List<Vector2> CreateDiamondShape() => 
        [new(0, 20), new(-20, -6), new(-11, -20), new(11, -20), new(20, -6)];
    
    private static List<Vector2> CreateStarShape(int points)
    {
        if (points < 3 || points > 128)
        {
            return ShapeTypeDictionary[ShapeEnum.Unknown];
        }
        List<Vector2> shapePoints = [];
        const float startAngle = -MathF.PI / 2;
        var increment = MathF.PI / points;
        var innerLength = GlobalUnit.UNIT_HALF_LENGTH * MathF.Sin(MathF.PI / 6)
                          / MathF.Sin(increment + MathF.PI / 6);
        for (var i = 0; i < points; i++)
        {
            var outerAngle = startAngle + i * 2 * increment;
            var innerAngle = outerAngle + increment;
            shapePoints.Add(MathExpends.RadToVector2(outerAngle, GlobalUnit.UNIT_HALF_LENGTH));
            shapePoints.Add(MathExpends.RadToVector2(innerAngle, innerLength));
        }
        return shapePoints;
    }

    private static List<Vector2> CreateHeartShape()
    {
        var shapePoints = new List<Vector2>();
        const float dt = MathF.Tau / GlobalUnit.CIRCLE_SIDES;
        for (int i = 0; i < GlobalUnit.CIRCLE_SIDES; i++)
        {
            var t = i * dt;
            var tmp = MathF.Sin(t) * MathF.Sin(t) * MathF.Sin(t);
            var x = GlobalUnit.UNIT_HALF_LENGTH * tmp;
            var y = 20 * MathF.Cos(t) - 5 * MathF.Cos(2 * t) -
                    2.5f * MathF.Cos(3 * t) - 0.5f * MathF.Cos(4 * t);
            shapePoints.Add(new Vector2(x, -3.5f - y));
        }
        return shapePoints;
    }
    #endregion
}