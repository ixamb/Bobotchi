using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Core.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            if (list is null || list.Count == 0)
                throw new ArgumentNullException(nameof(list));
            return list[new Random().Next(list.Count)];
        }

        public static T Pop<T>(this List<T> list, T element)
        {
            var index = list.IndexOf(element);
            if (index < 0)
                return default;
            
            var value = list[index];
            list.RemoveAt(index);
            return value;

        }
        
        public static T PopRandom<T>(this List<T> list, Random rng)
        {
            var index = rng.Next(list.Count);
            var value = list[index];
            list.RemoveAt(index);
            return value;
        }
        
        public static List<T> DeepCopy<T>(this List<T> list, Func<T, T> cloneFunc = null)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            return cloneFunc != null ? list.Select(cloneFunc).ToList() : new List<T>(list);
        }

        public static void DestroyAndClear<T>(this List<T> list) where T : Object
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));

            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                    continue;
                if (list[i] is Component component)
                    Object.Destroy(component.gameObject);
                else
                    Object.Destroy(list[i]);
            }
    
            list.Clear();
        }
    }
}