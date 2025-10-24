using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeadDog.Ordexp.Configuration;

/// <summary>
/// 配置管理器，使用前一定要先初始化一遍！
/// </summary>
public static class ConfigurationManager
{
    private static readonly Dictionary<Type,IConfiguration> _configs = new();

    /// <summary>
    /// 初始化配置管理器
    /// </summary>
    public static void Init()
    {
        var assembly = Assembly.GetCallingAssembly();
        ScanAndRegisterConfigurations(assembly);
    }

    /// <summary>
    /// 提供一个从程序集扫描配置的初始化方法
    /// </summary>
    /// <param name="assembly">程序集</param>
    public static void InitFromAssembly(Assembly assembly)
    {
        ScanAndRegisterConfigurations(assembly);
    }

    private static void ScanAndRegisterConfigurations(Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<ConfigurationAttribute>() is not null);
        foreach (var type in types)
        {
            try
            {
                if (typeof(IConfiguration).IsAssignableFrom(type))
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor is null)
                    {
                        throw new InvalidOperationException($"配置类{type.FullName}没有无参构造函数");
                    }

                    var instance = (IConfiguration)constructor.Invoke(null);
                    _configs[type] = instance;
                }
                else
                {
                    throw new InvalidOperationException(
                        $"类{type.FullName}被标记为配置类，但未实现IConfiguration接口"
                    );
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"初始化配置类{type.FullName}失败：{e.Message}", e);
            }
        }
    }

    public static IConfiguration GetConfiguration(Type configType)
    {
        return _configs.TryGetValue(configType, out var config) ? 
            config : throw new KeyNotFoundException($"未找到类型为{configType}的配置");
    }
    
    public static T GetConfiguration<T>() where T : class
    {
        return (T)GetConfiguration(typeof(T));
    }
}