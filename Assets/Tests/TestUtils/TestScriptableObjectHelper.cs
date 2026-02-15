using System;
using System.Reflection;
using UnityEngine;

namespace Tests.TestUtils
{
    public static class TestScriptableObjectHelper
    {
        public static T CreateAndSet<T>(Action<T> configure) where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance<T>();
            configure(instance);
            return instance;
        }
    
        public static void SetPrivateField(object obj, string fieldName, object value)
        {
            var type = obj.GetType();
            FieldInfo field = null;
    
            while (type != null && field == null)
            {
                field = type.GetField(fieldName, 
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        
                if (field == null)
                {
                    field = type.GetField($"<{fieldName}>k__BackingField", 
                        BindingFlags.NonPublic | BindingFlags.Instance);
                }
        
                type = type.BaseType;
            }
    
            if (field == null)
                throw new Exception($"Field '{fieldName}' not found in {obj.GetType().Name} or its base classes");
    
            field.SetValue(obj, value);
        }
    }
}