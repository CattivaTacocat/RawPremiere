using Godot;
using System;
using System.Linq;

namespace DeadDog.Ordexp
{
    public static class EnumExpends
    {
        #region 决策
        /// <summary>
        /// 能否赋值
        /// </summary>
        /// <param name="origin">原值</param>
        /// <param name="value">新值</param>
        /// <param name="canEmpty">新值能否为所谓的空情况</param>
        /// <returns>不能赋值则为true，反之为false</returns>
        public static bool CantAssignValue<T>(this T origin, T value, bool canEmpty = false) where T : Enum
        {
            if (!canEmpty && value is null) return true;
            if (Enum.IsDefined(typeof(T), value))
                return (origin.Equals(value));
            GD.PushWarning($"\"{nameof(value)}\"不是枚举\"{nameof(T)}\"的合法值");
            return true;
        }
        #endregion
        #region 处理
        /// <summary>
        /// 枚举值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="hasDebug">是否有Debug辅助枚举值</param>
        /// <returns>枚举值字符串数组</returns>
        /// <exception cref="ArgumentException">类型错误</exception>
        public static string[] EnumValues(this Type enumType,bool hasDebug = true)
        {
            ArgumentNullException.ThrowIfNull(enumType);
            var values = enumType.IsEnum 
                ? Enum.GetNames(enumType) 
                : throw new ArgumentException($"传入的类型{nameof(enumType)}不是枚举类型！");
            if (hasDebug) return values;
            var list = values.ToList();
            list.RemoveAll(x =>
                string.Compare(x, "Unknown", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(x, "Count", StringComparison.OrdinalIgnoreCase) == 0);
            values = list.ToArray();
            return values;
        }
        #endregion
    }
}
