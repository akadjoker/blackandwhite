using UnityEngine;

public static class MathLib
{
    public static float GetPitch(this Vector3 v)
    {
        float len = Mathf.Sqrt((v.x * v.x) + (v.z * v.z));    // Length on xz plane.
        return (-Mathf.Atan2(v.y, len));
    }

    public static float GetYaw(this Vector3 v)
    {
        return (Mathf.Atan2(v.x, v.z));
    }
    public static void RotateX(ref Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float ty = v.y;
        float tz = v.z;
        v.y = (cos * ty) - (sin * tz);
        v.z = (cos * tz) + (sin * ty);
    }

    public static void RotateY(ref Vector3 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) + (sin * tz);
        v.z = (cos * tz) - (sin * tx);
    }

    public static Vector2 RotateZ( Vector2 v, float angle)
    {
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);
        float tx = v.x;
        float ty = v.y;
        Vector2 result = new Vector2();
        result.x = (cos * tx) - (sin * ty);
        result.y = (cos * ty) + (sin * tx);
        return result;
    }
    public static byte LinearInterpolate(byte a, byte b, double t)
    {
        return (byte)(a * (1 - t) + b * t);
    }
    public static float LinearInterpolate(float a, float b, double t)
    {
        return (float)(a * (1 - t) + b * t);
    }
    public static Vector2 LinearInterpolate(Vector2 a, Vector2 b, double t)
    {
        return new Vector2(LinearInterpolate(a.x, b.x, t), LinearInterpolate(a.y, b.y, t));
    }
    public static Color LinearInterpolate(Color a, Color b, double t)
    {
        return new Color(LinearInterpolate(a.r, b.r, t), LinearInterpolate(a.g, b.g, t), LinearInterpolate(a.b, b.b, t), LinearInterpolate(a.a, b.a, t));
    }
 
}