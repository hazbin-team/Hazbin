using System.Reflection;
using HarmonyLib;
using LiteNetLib.Utils;

namespace Hazbin.Core.Extensions;

public static class ReflectionExtensions
{
    public static void InvokeStaticMethod(this Type type, string methodName, object[] param)
    {
        type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod)?.Invoke(null, param);
    }

    public static void InvokeStaticEvent(this Type type, string eventName, object[] param)
    {
        MulticastDelegate multicastDelegate = (MulticastDelegate) type.GetField(eventName, AccessTools.all)!.GetValue(null);
        if ((object) multicastDelegate == null)
            return;
        foreach (Delegate invocation in multicastDelegate.GetInvocationList())
            invocation.Method.Invoke(invocation.Target, param);
    }

    public static void CopyProperties(this object target, object source)
    {
        Type type = target.GetType();
        if (type != source.GetType())
            throw new InvalidTypeException("Target and source type mismatch!");
        foreach (PropertyInfo property in type.GetProperties())
            type.GetProperty(property.Name)?.SetValue(target, property.GetValue(source, null), null);
    }
        
    public static TField GetFieldValue<TField>(this object instance, string fieldName)
    {
        if (instance == null || string.IsNullOrEmpty(fieldName))
            throw new InvalidOperationException("One of arguments is null");
                
        Type type = instance.GetType();
                
        FieldInfo? fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (fieldInfo == null)
            throw new MissingFieldException($"Field '{fieldName}' does not exist in type '{type.FullName}'.");

        object value = fieldInfo.GetValue(instance);

        if (value is TField value1)
            return value1;
                
        throw new InvalidCastException($"Value of field {fieldName} cannot be casted to gived type");
    }
            
    public static T GetStaticFieldValue<T>(this Type type, string fieldName)
    {
        if (type == null || string.IsNullOrEmpty(fieldName))
            throw new InvalidOperationException("One of arguments is null");
                
        FieldInfo? fieldInfo = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (fieldInfo == null)
            throw new MissingFieldException($"Field '{fieldName}' does not exist in type '{type.FullName}'.");

        object value = fieldInfo.GetValue(null);

        if (value is T value1)
            return value1;
                
        throw new InvalidCastException($"Value of field {fieldName} cannot be casted to gived type");
    }
            
    public static void SetFieldValue(this object instance, string fieldName, object newValue)
    {
        if (instance == null || string.IsNullOrEmpty(fieldName))
            throw new InvalidOperationException("One of arguments is null");
                
        Type type = instance.GetType();
                
        FieldInfo? fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (fieldInfo == null)
            throw new MissingFieldException($"Field '{fieldName}' does not exist in type '{type.FullName}'.");

        fieldInfo.SetValue(instance, newValue);
    }
            
    public static void SetStaticFieldValue(this Type type, string fieldName, object newValue)
    {
        if (type == null || string.IsNullOrEmpty(fieldName))
            throw new InvalidOperationException("One of arguments is null");
                
        FieldInfo? fieldInfo = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (fieldInfo == null)
            throw new MissingFieldException($"Field '{fieldName}' does not exist in type '{type.FullName}'.");

        fieldInfo.SetValue(null, newValue);
    }
        
    public static IEnumerable<TBase> GetAllOfBase<TBase>(this Assembly assembly)
    {
        HashSet<TBase> bases = new();
        
        IEnumerable<Type> targetTypes = assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && typeof(TBase).IsAssignableFrom(x));
        
        foreach (Type type in targetTypes)
        {
            if (Activator.CreateInstance(type) is TBase instance)
            {
                bases.Add(instance);
            }
        }
        
        return bases;
    }
    
    public static string GetLongFuncName(Type type, string methodName) {
        const BindingFlags methodFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        MethodInfo method = type.GetMethod(methodName, methodFlags) ?? throw new InvalidOperationException($"[ReflectionExtensions.GetLongFuncName] {type.FullName}.{methodName} could not be found.");
        return GetLongFuncName(type, method);
    }
    
    public static string GetLongFuncName(Type type, MethodInfo method) {
        return $"{method.ReturnType.FullName} {type.FullName}::{method.Name}({string.Join(",", method.GetParameters().Select(x => x.ParameterType.FullName))})";
    }
}