using Godot;
using System;
using System.IO;

namespace DeadDog.Ordexp
{
    /// <summary>
    /// 字符串拓展（来自OrdinaryExpansion库）
    /// </summary>
    public static class StringExpends
    {
        #region 决策
        /// <summary>
        /// 能否赋值
        /// </summary>
        /// <param name="origin">原值</param>
        /// <param name="value">新值</param>
        /// <param name="canEmpty">新值能否为所谓的空情况</param>
        /// <returns>不能赋值则为true，反之为false</returns>
        public static bool CantAssignValue(this string origin, string value, bool canEmpty = false)
        {
            if (canEmpty) return origin.Equals(value);
            return string.IsNullOrEmpty(value) || origin.Equals(value);
        }

        public static bool IsValidFilePath(this string path)
        {
            if (path.IsValidFileName()) return true;
            try
            {
                Path.GetFullPath(path);
                return true;
            }
            catch
            {
                GD.PushWarning($"文件路径\"{path}\"不合法");
                return false;
            }
        }

        public static bool IsValidUri(this string uri)
        {
            if (string.IsNullOrEmpty(uri)) return false;
            return Uri.TryCreate(uri, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp ||
                       uriResult.Scheme == Uri.UriSchemeHttps ||
                       uriResult.Scheme == Uri.UriSchemeFtp);
        }
        #endregion
        #region 操作
        /// <summary>
        /// 限制字符串
        /// 将字符串限制在指定长度内，超出部分截断
        /// </summary>
        /// <param name="origin">原字符串</param>
        /// <param name="length">限制长度</param>
        /// <returns>截取结果</returns>
        public static string RestrictString(this string origin, int length)
        {
            if (origin is null) return null;
            if (length <= 0) return string.Empty;
            return origin.Length >= length ? origin[..length] : origin;
        }
        
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="code">十六进制颜色代码或颜色单词</param>
        /// <returns>颜色</returns>
        public static Color Color(this string code) =>
            Godot.Color.HtmlIsValid(code) ? 
                Godot.Color.FromHtml(code) : 
                Godot.Color.FromString(code,Colors.White);

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="filePath">文件路径或虚拟路径</param>
        /// <returns>绝对路径</returns>
        public static string GetAbsolutePath(this string filePath)
        {
            try
            {
                return ProjectSettings.GlobalizePath(filePath);
            }
            catch
            {
                return Path.GetFullPath(filePath);
            }
        }
        #endregion
    }
}
