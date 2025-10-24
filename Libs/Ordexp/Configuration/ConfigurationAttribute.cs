using System;

namespace DeadDog.Ordexp.Configuration;

[AttributeUsage(AttributeTargets.Class,Inherited = false,AllowMultiple = false)]
public class ConfigurationAttribute : Attribute
{
    public ConfigurationAttribute()
    {
        
    }
}