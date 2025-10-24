using System;
using Godot;

namespace DeadDog.Nodeo.Tools;

public static class Texture2DTool
{
    #region 操作
    public static Texture2D GetTexture(string path)
    {
        var texture = ResourceLoader.Load<Texture2D>(path);
        return texture;
    }

    public static string GetPath(Texture2D texture)
    {
        return texture is null ? string.Empty : texture.ResourcePath;
    }

    public static Texture2D[] GetTextures(params string[] paths)
    {
        var length = paths.Length;
        var textures = new Texture2D[length];
        for (int i = 0; i < length; i++)
        {
            textures[i] = GetTexture(paths[i]);
        }
        return textures;
    }

    public static string[] GetPaths(params Texture2D[] textures)
    {
        var length = textures.Length;
        var paths = new string[length];
        for (int i = 0; i < length; i++)
        {
            paths[i] = GetPath(textures[i]);
        }
        return paths;
    }
    
    public static DpiTexture GetSvgTexture(string path)
    {
        var texture = new DpiTexture();
        var source = FileAccess.GetFileAsString(path);
        texture.SetSource(source);
        return texture;
    }

    public static DpiTexture GetSvgTexture(this Texture2D texture)
    {
        var path = texture.ResourcePath;
        return GetSvgTexture(path);
    }

    public static Texture2DTypeEnum ParseTextureType(Texture2D texture) =>
        texture switch
        {
            ViewportTexture => Texture2DTypeEnum.Viewport,
            CanvasTexture => Texture2DTypeEnum.Canvas,
            CompressedTexture2D => Texture2DTypeEnum.Compressed,
            PortableCompressedTexture2D => Texture2DTypeEnum.PortableCompressed,
            ImageTexture => Texture2DTypeEnum.Image,
            AtlasTexture => Texture2DTypeEnum.Atlas,
            MeshTexture => Texture2DTypeEnum.Mesh,
            CurveTexture => Texture2DTypeEnum.Curve,
            CurveXyzTexture => Texture2DTypeEnum.CurveXYZ,
            GradientTexture1D => Texture2DTypeEnum.Gradient1D,
            GradientTexture2D => Texture2DTypeEnum.Gradient2D,
            CameraTexture => Texture2DTypeEnum.Camera,
            ExternalTexture => Texture2DTypeEnum.External,
            PlaceholderTexture2D => Texture2DTypeEnum.Placeholder,
            DpiTexture => Texture2DTypeEnum.DPI,
            Texture2Drd => Texture2DTypeEnum.Texture2DRD,
            NoiseTexture2D => Texture2DTypeEnum.Noise,
            _ => Texture2DTypeEnum.Unknown
        };
    #endregion
}

public enum Texture2DTypeEnum
{
    Unknown,
    Viewport,
    Canvas,
    Compressed,
    PortableCompressed,
    Image,
    Atlas,
    Mesh,
    Curve,
    CurveXYZ,
    Gradient1D,
    Gradient2D,
    Camera,
    External,
    Placeholder,
    DPI,
    Texture2DRD,
    Noise,
    Count
}