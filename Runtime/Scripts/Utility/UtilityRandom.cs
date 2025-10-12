using System.Collections.Generic;
using UnityEngine;

namespace Chonker.Core.Utility {
    public static class UtilityRandom {
        public static void Shuffle<T>(T[] array) {
            int n = array.Length;
            while (n > 1) {
                n--;
                int k = UnityEngine.Random.Range(0, array.Length);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }
        
        public static void Shuffle<T>(List<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = UnityEngine.Random.Range(0, list.Count);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static T PickRandomElement<T>(List<T> list) {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static T PickRandomElement<T>(T[] array) {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        
        public static int PickRandomIndex<T>(List<T> list) {
            return UnityEngine.Random.Range(0, list.Count);
        }
        
        public static int PickRandomIndex<T>(T[] array) {
            return UnityEngine.Random.Range(0, array.Length);
        }
    }
}