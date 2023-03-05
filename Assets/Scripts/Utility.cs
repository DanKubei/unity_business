using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float eulerConvert = Mathf.PI / 180;

    public static float GetAngleFromVectors(Vector2 vector1, Vector2 vector2)
    {
        return Mathf.Acos((vector1.x * vector2.x + vector1.y * vector2.y) / vector1.magnitude / vector2.magnitude);
    }

    public static float GetAngleFromVector(Vector2 vector)
    {
        return Mathf.Acos(vector.normalized.x) * Mathf.Sign(vector.y);
    }

    public static Vector2 RotateVector(Vector2 vector, float eulerAngel)
    {
        if (eulerAngel % 360 == 0)
        {
            return vector;
        }
        float angel = eulerConvert * eulerAngel;
        float x = vector.x * Mathf.Cos(angel) - vector.y * Mathf.Sin(angel);
        float y = vector.x * Mathf.Sin(angel) + vector.y * Mathf.Cos(angel);
        Vector2 newVector = new Vector2(x,y);
        return newVector.normalized;
    }

    public static Vector2 GetPerpendicularVector(Vector2 vector1, Vector2 vector2)
    {
        return (vector1.normalized + vector2.normalized).normalized;
    }

}
