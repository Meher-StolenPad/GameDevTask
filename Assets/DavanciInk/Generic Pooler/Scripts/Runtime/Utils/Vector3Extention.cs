using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace DaVanciInk.GenericPooler
{
    public static class Vector3Extention
    {
        public static Vector3 Random(this Vector3 myVector, Vector3 min, Vector3 max)
        {
            myVector = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
            return myVector;
        }
        public static List<T> TryGetComponents<T>(this List<GameObject> gameObjects) where T : Component
        {
            List<T> components = new List<T>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                T component = gameObjects[i].GetComponent<T>();

                if (component != null)
                {
                    components.Add(component);
                }
            }

            return components;
        }
        public static RectBounds GetBounds(this Rect rect, float3 playerPosition)
        {
            float2 centerOffset = new float2(rect.center.x, rect.center.y);

            var bottomLeft = playerPosition + new float3(centerOffset.x - rect.width * 0.5f, 0f, centerOffset.y - rect.height * 0.5f);
            var topRight = playerPosition + new float3(centerOffset.x + rect.width * 0.5f, 0f, centerOffset.y + rect.height * 0.5f);

            RectBounds rectBounds = new RectBounds
            {
                topRight = topRight,
                bottomLeft = bottomLeft
            };
            return rectBounds;
        }

        public static Vector3 SetY(this Vector3 myVector, float y)
        {
            myVector = new Vector3(myVector.x, y, myVector.z);
            return myVector;
        }
    }
    public struct RectBounds
    {
        public float3 topRight;
        public float3 bottomLeft;
    }
}