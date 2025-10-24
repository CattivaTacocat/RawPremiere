using Godot;

namespace DeadDog.Nodeo.Tools;

public static class PackageSceneTool
{
    #region 操作
    public static PackedScene GetScene(string path) => ResourceLoader.Load<PackedScene>(path);

    public static Node GetInstance(string path) => 
        GetScene(path).Instantiate();

    public static T GetInstance<T>(string path) where T : Node =>
         GetScene(path).Instantiate<T>();
    #endregion
}