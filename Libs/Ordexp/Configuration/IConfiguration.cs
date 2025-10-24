namespace DeadDog.Ordexp.Configuration;

/// <summary>
/// 被该接口实现的类型，必须写一个默认构造器定义初始化默认属性
/// </summary>
/// <typeparam name="T">Dto数据</typeparam>
public interface IConfiguration
{
    /// <summary>
    /// 从文件路径中加载配置
    /// </summary>
    /// <param name="path">文件路径</param>
    void Load(string path);
    /// <summary>
    /// 将配置保存到文件路径
    /// </summary>
    /// <param name="path">文件路径</param>
    void Save(string path);
}

public interface IConfiguration<T> : IConfiguration where T : class
{
    /// <summary>
    /// 当前储存配置
    /// </summary>
    T Config { get; }
    /// <summary>
    /// 默认配置
    /// </summary>
    T Default { get; }
    /// <summary>
    /// 手动注入配置
    /// </summary>
    /// <param name="dto">配置数据</param>
    void Inject(T dto);
}