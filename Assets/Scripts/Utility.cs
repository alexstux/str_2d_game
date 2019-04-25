using System;

using UnityEngine;

public static class Vector2Extension {
    public static float Cos(this Vector2 vector1, Vector2 vector2) {
        return
            vector1.Scalar(vector2)
            / (vector1.magnitude * vector2.magnitude);
    }

    public static float Sin(this Vector2 vector1, Vector2 vector2) {
        return
            (vector1.x * vector2.y - vector1.y * vector2.x)
            / (vector1.magnitude * vector2.magnitude);
    }

    public static float Tan(this Vector2 vector1, Vector2 vector2) {
        return
            vector1.Sin(vector2) / vector1.Cos(vector2); 
    }

    public static float Scalar(this Vector2 vector1, Vector2 vector2) {
        return
            vector1.x * vector2.x + vector1.y * vector2.y;
    }

    public static Vector2 GetOrt(this Vector2 vector) {
        // Gramm Schmidt
        Vector2 tempVector = Vector2.right;
        if (vector.Sin(tempVector) == 0) {
            tempVector = Vector2.up;
        }
        
        float alpha =
            -vector.Scalar(tempVector)
            / vector.sqrMagnitude;

        return (tempVector + alpha * vector).normalized;
    }
}

public static class PlayerProperties {
    private static float aScalar { get; } = 2.99792458f / 2;
    private static Vector2 aPrivate;
    public static float time { get; set; } = 0f;
    public static Vector2 r { get; set; } = Vector2.zero;
    public static Vector2 v { get; set; } = Vector2.zero;
    public static Vector2 a {
        get {
            return aPrivate;
        }
        set {
            if (value == Vector2.zero) {
                aPrivate = Vector2.zero;
            } else {
                aPrivate = value.normalized * aScalar;
            }
        }
    }
}

public class Pair<T1, T2> {
    public T1 first { get; set; }
    public T2 second { get; set; }

    public Pair(T1 first, T2 second) {
        this.first = first;
        this.second = second;
    }
    
    public Pair() {
        this.first = default;
        this.second = default;
    }
}

public static class Formulas {
    //public static UInt64 c { get; } = 299792458;
    public static float c { get; } = 29.9792458f / 2;

    public static Pair<float, float> Scale(Pair<float, float> scale) {
        float scaleX = K(new Vector2(PlayerProperties.v.x, 0f))
            * scale.first,
            scaleY = K(new Vector2(0f, PlayerProperties.v.y))
            * scale.second;

        return new Pair<float, float>(scaleX, scaleY);
    }
    public static float K(Vector2 v) {
        return (float)Math.Sqrt(1 - v.sqrMagnitude / (c * c));
    }

    public static Vector2 W0(Vector2 v0) {
        return v0 / K(v0);
    }

    public static Vector2 V(float deltaTime) {
        if (PlayerProperties.a == Vector2.zero) {
            return Vector2.zero;
        }

        Vector2 w0 = W0(PlayerProperties.v);
        return
            (w0 + PlayerProperties.a * deltaTime)
            / (float)Math.Sqrt(
                    1 + (w0 + PlayerProperties.a * deltaTime).sqrMagnitude / (c * c)
                );
    }

    public static Vector2 R(float deltaTime) {
        if (PlayerProperties.a == Vector2.zero) {
            return PlayerProperties.r;
        }

        Vector2 w0 = W0(PlayerProperties.v);
        return
            PlayerProperties.r
            + ((PlayerProperties.a * c) / PlayerProperties.a.sqrMagnitude)
            * (float)(Math.Sqrt(c * c + ((w0 + PlayerProperties.a * deltaTime).sqrMagnitude))
            - Math.Sqrt(c * c + w0.sqrMagnitude))
            + ((w0 * PlayerProperties.a.sqrMagnitude 
            - PlayerProperties.a * (PlayerProperties.a.Scalar(w0)))
            / PlayerProperties.a.sqrMagnitude) 
            * (RealityTime(deltaTime) - PlayerProperties.time);
    }
    
    public static float RealityTime(float deltaTime) {
        if (PlayerProperties.a == Vector2.zero) {
            return PlayerProperties.time + deltaTime;
        }

        Vector2 w0 = W0(PlayerProperties.v);
        return
            PlayerProperties.time
            + c / PlayerProperties.a.magnitude
            * (float) Math.Log(
                    (Math.Sqrt(c * c + ((w0 + PlayerProperties.a * deltaTime).sqrMagnitude))
                    + PlayerProperties.a.magnitude * deltaTime
                    + w0.Scalar(PlayerProperties.a) / PlayerProperties.a.magnitude)
                    / (Math.Sqrt(c * c + w0.sqrMagnitude) 
                    + w0.Scalar(PlayerProperties.a) / PlayerProperties.a.magnitude)
                );
    }
}
