// HSLuv rev4 library for Unity.
// SPDX-FileCopyrightText: Copyright (c) Takayuki Matsuoka
// SPDX-License-Identifier: MIT

using System;
using System.Text;
using UnityEngine;

namespace Hsluv {
    public static class Hsluv {
        //
        // Standard HSLuv functions (sRGB)
        //
        public static Vector3 LchToRgb(Vector3 v)   => LchToSrgb(v);
        public static Vector3 RgbToLch(Vector3 v)   => SrgbToLch(v);
        public static Vector3 HsluvToRgb(Vector3 v) => HsluvToSrgb(v);
        public static Vector3 RgbToHsluv(Vector3 v) => SrgbToHsluv(v);
        public static Vector3 HpluvToRgb(Vector3 v) => HpluvToSrgb(v);
        public static Vector3 RgbToHpluv(Vector3 v) => SrgbToHpluv(v);
        public static Vector3 LuvToRgb(Vector3 v)   => LuvToSrgb(v);
        public static Vector3 RgbToLuv(Vector3 v)   => SrgbToLuv(v);

        //
        //  Color conversion
        //

        // Linear RGB color conversion
        public static Vector3 LchToLrgb(Vector3 v)   => Impl.XyzToLrgb(Impl.LuvToXyz(Impl.LchToLuv(v)));
        public static Vector3 LrgbToLch(Vector3 v)   => Impl.LuvToLch(Impl.XyzToLuv(Impl.LrgbToXyz(v)));
        public static Vector3 HsluvToLrgb(Vector3 v) => LchToLrgb(Impl.HsluvToLch(v));
        public static Vector3 LrgbToHsluv(Vector3 v) => Impl.LchToHsluv(LrgbToLch(v));
        public static Vector3 HpluvToLrgb(Vector3 v) => LchToLrgb(Impl.HpluvToLch(v));
        public static Vector3 LrgbToHpluv(Vector3 v) => Impl.LchToHpluv(LrgbToLch(v));
        public static Vector3 LuvToLrgb(Vector3 v)   => Impl.XyzToLrgb(Impl.LuvToXyz(v));
        public static Vector3 LrgbToLuv(Vector3 v)   => Impl.XyzToLuv(Impl.LrgbToXyz(v));
        public static Vector3 XyzToLrgb(Vector3 v)   => Impl.XyzToLrgb(v);
        public static Vector3 LrgbToXyz(Vector3 v)   => Impl.LrgbToXyz(v);

        // sRGB (Gamma RGB) color conversion
        public static Vector3 LchToSrgb(Vector3 v)   => Impl.LrgbToSrgb(LchToLrgb(v));
        public static Vector3 HsluvToSrgb(Vector3 v) => Impl.LrgbToSrgb(HsluvToLrgb(v));
        public static Vector3 HpluvToSrgb(Vector3 v) => Impl.LrgbToSrgb(HpluvToLrgb(v));
        public static Vector3 LuvToSrgb(Vector3 v)   => Impl.LrgbToSrgb(LuvToLrgb(v));
        public static Vector3 SrgbToLch(Vector3 v)   => LrgbToLch(Impl.SrgbToLrgb(v));
        public static Vector3 SrgbToHsluv(Vector3 v) => LrgbToHsluv(Impl.SrgbToLrgb(v));
        public static Vector3 SrgbToHpluv(Vector3 v) => LrgbToHpluv(Impl.SrgbToLrgb(v));
        public static Vector3 SrgbToLuv(Vector3 v)   => LrgbToLuv(Impl.SrgbToLrgb(v));
        public static Vector3 XyzToSrgb(Vector3 v)   => Impl.LrgbToSrgb(Impl.XyzToLrgb(v));
        public static Vector3 SrgbToXyz(Vector3 v)   => Impl.LrgbToXyz(Impl.SrgbToLrgb(v));

        // HSL Component interpolation
        public static float InterpolateH(float a, float b, float t) => Impl.NormalizeH(a + Impl.NormalizeDh(b - a) * t);
        public static float InterpolateS(float a, float b, float t) => Mathf.Lerp(a, b, t);
        public static float InterpolateL(float a, float b, float t) => Mathf.Lerp(a, b, t);

        // Color space conversion
        public static Vector3 HpluvToLch(Vector3 v) => Impl.HpluvToLch(v);
        public static Vector3 HsluvToLch(Vector3 v) => Impl.HsluvToLch(v);
        public static Vector3 LchToLuv(Vector3 v)   => Impl.LchToLuv(v);
        public static Vector3 LuvToXyz(Vector3 v)   => Impl.LuvToXyz(v);
        public static Vector3 XyzToLuv(Vector3 v)   => Impl.XyzToLuv(v);
        public static Vector3 LuvToLch(Vector3 v)   => Impl.LuvToLch(v);
        public static Vector3 LchToHsluv(Vector3 v) => Impl.LchToHsluv(v);
        public static Vector3 LchToHpluv(Vector3 v) => Impl.LchToHpluv(v);

        // Hex conversion
        public static string  RgbToHex(Vector3 v)  => SrgbToHex(v);
        public static Vector3 HexToRgb(string s)   => HexToSrgb(s);
        public static string  SrgbToHex(Vector3 v) => Impl.SrgbToHex(v);
        public static Vector3 HexToSrgb(string s)  => Impl.HexToSrgb(s);
        public static string  LrgbToHex(Vector3 v) => Impl.SrgbToHex(Impl.LrgbToSrgb(v));
        public static Vector3 HexToLrgb(string s)  => Impl.SrgbToLrgb(Impl.HexToSrgb(s));
    }

    //
    // Color manipulation in HSLuv color space
    //
    public static class HsluvColorSpace {
        //
        // Color interpolation (HSLuv)
        //

        // Interpolate HSLuv colors in HSLuv color space.
        public static Vector3 LerpHsluv(Vector3 hsluvA, Vector3 hsluvB, float t)
        {
            t = Mathf.Clamp01(t);
            float h = Hsluv.InterpolateH(hsluvA.x, hsluvB.x, t);
            float s = Hsluv.InterpolateS(hsluvA.y, hsluvB.y, t);
            float l = Hsluv.InterpolateL(hsluvA.z, hsluvB.z, t);
            return new Vector3(h, s, l);
        }

        // Interpolate linear RGB colors in HSLuv color space.
        public static Vector3 LerpLinearRgb(Vector3 lrgbA, Vector3 lrgbB, float t)
        {
            Vector3 a = Hsluv.LrgbToHsluv(lrgbA);
            Vector3 b = Hsluv.LrgbToHsluv(lrgbB);
            Vector3 p = LerpHsluv(a, b, t);
            Vector3 o = Hsluv.HsluvToLrgb(p);
            return o;
        }

        // Interpolate linear RGB colors in HSLuv color space.
        public static Color LerpLinearRgb(Color lrgbA, Color lrgbB, float t)
        {
            Vector3 a = new Vector3(lrgbA.r, lrgbA.g, lrgbA.b);
            Vector3 b = new Vector3(lrgbB.r, lrgbB.g, lrgbB.b);
            Vector3 o = LerpLinearRgb(a, b, t);
            float   w = Mathf.Lerp(lrgbA.a, lrgbB.a, t);
            return new Color(o.x, o.y, o.z, w);
        }

        // Interpolate sRGB (gamma) colors in HSLuv color space.
        public static Color LerpSrgb(Color srgbA, Color srgbB, float t)
        {
            Vector3 a = Impl.SrgbToLrgb(new Vector3(srgbA.r, srgbA.g, srgbA.b));
            Vector3 b = Impl.SrgbToLrgb(new Vector3(srgbB.r, srgbB.g, srgbB.b));
            Vector3 p = LerpLinearRgb(a, b, t);
            Vector3 o = Impl.LrgbToSrgb(p);
            float   w = Mathf.Lerp(srgbA.a, srgbB.a, t);
            return new Color(o.x, o.y, o.z, w);
        }

        //
        // Hue rotation (HSLuv)
        //

        // Rotate hue of HSLuv color in HSLuv color space.
        public static Vector3 RotateHsluvHue(Vector3 hsluv, float deltaHueInDegrees)
        {
            float h = Impl.NormalizeH(hsluv.x + deltaHueInDegrees);
            float s = hsluv.y;
            float l = hsluv.z;
            return new Vector3(h, s, l);
        }

        // Rotate hue of linear RGB color in HSLuv color space.
        public static Vector3 RotateLinearRgbHue(Vector3 lrgb, float deltaHueInDegrees)
        {
            Vector3 a = Hsluv.LrgbToHsluv(lrgb);
            Vector3 p = RotateHsluvHue(a, deltaHueInDegrees);
            return Hsluv.HsluvToLrgb(p);
        }

        // Rotate hue of linear RGB color in HSLuv color space.
        public static Color RotateLinearRgbHue(Color lrgb, float deltaHueInDegrees)
        {
            Vector3 a = new Vector3(lrgb.r, lrgb.g, lrgb.b);
            Vector3 o = RotateLinearRgbHue(a, deltaHueInDegrees);
            return new Color(r: o.x, g: o.y, b: o.z, a: lrgb.a);
        }

        // Rotate hue of linear sRGB (gamma) color in HSLuv color space.
        public static Color RotateSrgbHue(Color srgb, float deltaHueInDegrees)
        {
            Vector3 a = Impl.SrgbToLrgb(new Vector3(srgb.r, srgb.g, srgb.b));
            Vector3 p = RotateLinearRgbHue(a, deltaHueInDegrees);
            Vector3 o = Impl.LrgbToSrgb(p);
            return new Color(r: o.x, g: o.y, b: o.z, a: srgb.a);
        }
    }

    //
    // Color manipulation in HPLuv color space
    //
    public static class HpluvColorSpace {
        //
        // Color interpolation (HPLuv)
        //

        // Interpolate HPLuv colors in HPLuv color space.
        public static Vector3 LerpHpluv(Vector3 a, Vector3 b, float t)
        {
            t = Mathf.Clamp01(t);
            float h = Hsluv.InterpolateH(a.x, b.x, t);
            float s = Hsluv.InterpolateS(a.y, b.y, t);
            float l = Hsluv.InterpolateL(a.z, b.z, t);
            return new Vector3(h, s, l);
        }

        // Interpolate linear RGB colors in HPLuv color space.
        public static Vector3 LerpLinearRgb(Vector3 c, Vector3 d, float t)
        {
            Vector3 a = Hsluv.LrgbToHpluv(c);
            Vector3 b = Hsluv.LrgbToHpluv(d);
            Vector3 p = LerpHpluv(a, b, t);
            Vector3 o = Hsluv.HpluvToLrgb(p);
            return o;
        }

        // Interpolate linear RGB colors in HPLuv color space.
        public static Color LerpLinearRgb(Color c, Color d, float t)
        {
            Vector3 a = new Vector3(c.r, c.g, c.b);
            Vector3 b = new Vector3(d.r, d.g, d.b);
            Vector3 o = LerpLinearRgb(a, b, t);
            float   w = Mathf.Lerp(c.a, d.a, t);
            return new Color(r: o.x, g: o.y, b: o.z, a: w);
        }

        // Interpolate sRGB (gamma) colors in HPLuv color space.
        public static Color LerpSrgb(Color c, Color d, float t)
        {
            Vector3 a = Impl.SrgbToLrgb(new Vector3(c.r, c.g, c.b));
            Vector3 b = Impl.SrgbToLrgb(new Vector3(d.r, d.g, d.b));
            Vector3 p = LerpLinearRgb(a, b, t);
            Vector3 o = Impl.LrgbToSrgb(p);
            float   w = Mathf.Lerp(c.a, d.a, t);
            return new Color(r: o.x, g: o.y, b: o.z, a: w);
        }

        //
        // Hue rotation (HPLuv)
        //

        // Rotate hue of HPLuv color in HPLuv color space.
        public static Vector3 RotateHpluvHue(Vector3 a, float deltaHueInDegrees)
        {
            float h = Impl.NormalizeH(a.x + deltaHueInDegrees);
            float s = a.y;
            float l = a.z;
            return new Vector3(h, s, l);
        }

        // Rotate hue of linear RGB color in HPLuv color space.
        public static Vector3 RotateLinearRgbHue(Vector3 c, float deltaHueInDegrees)
        {
            Vector3 a = Hsluv.LrgbToHpluv(c);
            Vector3 p = RotateHpluvHue(a, deltaHueInDegrees);
            return Hsluv.HpluvToLrgb(p);
        }

        // Rotate hue of linear RGB color in HPLuv color space.
        public static Color RotateLinearRgbHue(Color c, float deltaHueInDegrees)
        {
            Vector3 a = new Vector3(c.r, c.g, c.b);
            Vector3 o = RotateLinearRgbHue(a, deltaHueInDegrees);
            return new Color(o.x, o.y, o.z, c.a);
        }

        // Rotate hue of linear sRGB (gamma) color in HPLuv color space.
        public static Color RotateSrgbHue(Color c, float deltaHueInDegrees)
        {
            Vector3 a = Impl.SrgbToLrgb(new Vector3(c.r, c.g, c.b));
            Vector3 p = RotateLinearRgbHue(a, deltaHueInDegrees);
            Vector3 o = Impl.LrgbToSrgb(p);
            return new Color(o.x, o.y, o.z, c.a);
        }
    }

    internal static class Impl {
        private const float PI      = 3.14159265358979323846f;
        private const float Epsilon = 0.00885645167903563082f;   // pow(6/29,3)
        private const float Kappa   = 112.91203703703703703704f; // pow(29/6,3)
        private const float Eps     = 0.0001220703125f;          // pow(2,-13)

        private static float Radians(float degree)           => degree * (PI     / 180.0f);
        private static float Degrees(float radian)           => radian * (180.0f / PI);
        private static float Pow3(float x)                   => x      * x * x;
        private static float Dot(Vector3 a, Vector3 b)       => Vector3.Dot(a, b);
        private static bool  TooSmall(float v)               => v < Eps;
        private static bool  TooSmallOrAlmost100(float v)    => TooSmall(v) || (v > 100.0f - Eps);
        private static float Min3(float x, float y, float z) => Mathf.Min(x, Mathf.Min(y, z));

        internal static float NormalizeH(float h)
        {
            h = h % 360.0f;
            if (h < 0.0f) {
                h += 360.0f;
            }
            return h;
        }

        internal static float NormalizeDh(float dh)
        {
            dh = dh % 360.0f;
            if (dh < -180.0f) {
                dh += 360.0f;
            } else if (dh >= 180.0f) {
                dh -= 360.0f;
            }
            return dh;
        }

        // For a given lightness, return a list of 6 lines in slope-intercept
        // form that represent the bounds in CIELUV, stepping over which will
        // push a value out of the RGB gamut
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L53
        private static void CalcBounds(
            float l,
            out float x0X, out float x0Y, out float x0Z,
            out float y0X, out float y0Y, out float y0Z,
            out float x1X, out float x1Y, out float x1Z,
            out float y1X, out float y1Y, out float y1Z
        )
        {
            // CIE XYZ(D65) -> Linear RGB transformation 3x3 matrix
            // https://en.wikipedia.org/wiki/SRGB#Specification_of_the_transformation
            var m0 = new Vector3(3.2409699419045214f,   -1.5373831775700935f,  -0.49861076029300328f);
            var m1 = new Vector3(-0.96924363628087983f, 1.8759675015077207f,   0.041555057407175613f);
            var m2 = new Vector3(0.055630079696993609f, -0.20397695888897657f, 1.0569715142428786f);

            const float kd = 1.0f / 126452.0f;

            var k = new Vector3(731718.0f, 769860.0f, 838422.0f);

            float bounds1XKx = (m0.x * 3.0f - m0.z) * (94839.0f * kd);
            float bounds1XKy = (m1.x * 3.0f - m1.z) * (94839.0f * kd);
            float bounds1XKz = (m2.x * 3.0f - m2.z) * (94839.0f * kd);

            float bounds1XLx = (m0.z * 5.0f - m0.y);
            float bounds1XLy = (m1.z * 5.0f - m1.y);
            float bounds1Xlz = (m2.z * 5.0f - m2.y);

            x0X = bounds1XKx / bounds1XLx;
            x0Y = bounds1XKy / bounds1XLy;
            x0Z = bounds1XKz / bounds1Xlz;

            float b1YAx = Dot(m0, k) * kd;
            float b1YAy = Dot(m1, k) * kd;
            float b1YAz = Dot(m2, k) * kd;
            float b1Yc  = (k.y * kd);

            float b0YKx = b1YAx / bounds1XLx;
            float b0YKy = b1YAy / bounds1XLy;
            float b0YKz = b1YAz / bounds1Xlz;

            // pow3(116) = pow(116,3) = 1560896
            float sub1 = Pow3(l + 16.0f) * (1.0f / Pow3(116.0f));
            float sub2 = (sub1 > Epsilon)
                ? sub1
                : (l * (1.0f / Kappa));

            x1X = bounds1XKx * sub2 / (bounds1XLx * sub2 + 1.0f);
            x1Y = bounds1XKy * sub2 / (bounds1XLy * sub2 + 1.0f);
            x1Z = bounds1XKz * sub2 / (bounds1Xlz * sub2 + 1.0f);

            y0X = b0YKx * l;
            y0Y = b0YKy * l;
            y0Z = b0YKz * l;

            y1X = l * (b1YAx * sub2 - b1Yc) / (bounds1XLx * sub2 + 1.0f);
            y1Y = l * (b1YAy * sub2 - b1Yc) / (bounds1XLy * sub2 + 1.0f);
            y1Z = l * (b1YAz * sub2 - b1Yc) / (bounds1Xlz * sub2 + 1.0f);
        }

        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L84
        private static float MaxSafeChromaForL(float l)
        {
            CalcBounds(
                l,
                out float x0X,
                out float x0Y,
                out float x0Z,
                out float y0X,
                out float y0Y,
                out float y0Z,
                out float x1X,
                out float x1Y,
                out float x1Z,
                out float y1X,
                out float y1Y,
                out float y1Z
            );
            IntersectLineLine(
                x0X,
                x0Y,
                x0Z,
                y0X,
                y0Y,
                y0Z,
                -1.0f / x0X,
                -1.0f / x0Y,
                -1.0f / x0Z,
                out float xs0X,
                out float xs0Y,
                out float xs0Z
            );
            IntersectLineLine(
                x1X,
                x1Y,
                x1Z,
                y1X,
                y1Y,
                y1Z,
                -1.0f / x1X,
                -1.0f / x1Y,
                -1.0f / x1Z,
                out float xs1X,
                out float xs1Y,
                out float xs1Z
            );
            DistanceFromPoleSq(
                xs0X,
                xs0Y,
                xs0Z,
                y0X + xs0X * x0X,
                y0Y + xs0Y * x0Y,
                y0Z + xs0Z * x0Z,
                out float lenSq0X,
                out float lenSq0Y,
                out float lenSq0Z
            );
            DistanceFromPoleSq(
                xs1X,
                xs1Y,
                xs1Z,
                y1X + xs1X * x1X,
                y1Y + xs1Y * x1Y,
                y1Z + xs1Z * x1Z,
                out float lenSq1X,
                out float lenSq1Y,
                out float lenSq1Z
            );
            float minDistSq = Mathf.Min(Min3(lenSq0X, lenSq0Y, lenSq0Z), Min3(lenSq1X, lenSq1Y, lenSq1Z));
            return Mathf.Sqrt(minDistSq);
        }

        private static void IntersectLineLine(
            float l1Xx, float l1XY, float l1XZ,
            float l1Yx, float l1Yy, float l1YZ,
            float l2Xx, float l2XY, float l2XZ,
            out float x, out float y, out float z
        )
        {
            // note: l2y is always (0, 0, 0).
            x = l1Yx / (l2Xx - l1Xx);
            y = l1Yy / (l2XY - l1XY);
            z = l1YZ / (l2XZ - l1XZ);
        }

        private static void DistanceFromPoleSq(
            float ax, float ay, float az,
            float bx, float by, float bz,
            out float x, out float y, out float z
        )
        {
            x = ax * ax + bx * bx;
            y = ay * ay + by * by;
            z = az * az + bz * bz;
        }

        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L100
        private static float MaxChromaForLh(float l, float h)
        {
            CalcBounds(
                l,
                out float x0X,
                out float x0Y,
                out float x0Z,
                out float y0X,
                out float y0Y,
                out float y0Z,
                out float x1X,
                out float x1Y,
                out float x1Z,
                out float y1X,
                out float y1Y,
                out float y1Z
            );
            float hRad = Radians(degree: h);
            float s    = Mathf.Sin(hRad);
            float c    = -Mathf.Cos(hRad);
            LengthOfRayUntilIntersect(s, c, x0X, x0Y, x0Z, y0X, y0Y, y0Z, out float l0, out float l1, out float l2);
            LengthOfRayUntilIntersect(s, c, x1X, x1Y, x1Z, y1X, y1Y, y1Z, out float r0, out float r1, out float r2);
            return Mathf.Min(Min3(l0, l1, l2), Min3(r0, r1, r2));
        }

        private static void LengthOfRayUntilIntersect(float s, float c, float xx, float xy, float xz, float yx, float yy, float yz,
                                                      out float lx, out float ly, out float lz)
        {
            lx = yx / (s + c * xx);
            ly = yy / (s + c * xy);
            lz = yz / (s + c * xz);
            if (lx < 0.0f) {
                lx = 1000.0f;
            }

            if (ly < 0.0f) {
                ly = 1000.0f;
            }

            if (lz < 0.0f) {
                lz = 1000.0f;
            }
        }

        //
        // Color space conversion functions
        //

        // LinearRGB -> sRGB
        internal static Vector3 LrgbToSrgb(Vector3 lrgb)
        {
            // LinearRGB -> sRGB conversion (component)
            // https://en.wikipedia.org/wiki/SRGB#Specification_of_the_transformation
            // https://github.com/hsluv/hsluv/blob/78b676629647f501e257fc5a4c135da6710c0c6c/math/cie.mac#L72
            return new Vector3(LinearToSrgb(lrgb.x), LinearToSrgb(lrgb.y), LinearToSrgb(lrgb.z));
        }

        private static float LinearToSrgb(float c) =>
            (c <= 0.0031308f)
                ? (12.92f * c)
                : (1.055f * Mathf.Pow(c, 1.0f / 2.4f) - 0.055f);

        // sRGB -> LinearRGB
        internal static Vector3 SrgbToLrgb(Vector3 srgb)
        {
            return new Vector3(SrgbToLinear(srgb.x), SrgbToLinear(srgb.y), SrgbToLinear(srgb.z));
        }

        // sRGB -> LinearRGB conversion (component)
        // https://en.wikipedia.org/wiki/SRGB#The_reverse_transformation
        // https://github.com/hsluv/hsluv/blob/78b676629647f501e257fc5a4c135da6710c0c6c/math/cie.mac#L77
        private static float SrgbToLinear(float c)
        {
            if (c <= 0.04045f) {
                return c * (1.0f / 12.92f);
            }

            //    (C_srgb + a) / (1+a)
            // -> (C_srgb / (1+a))  +  (a / (1+a))
            // -> (C_srgb * A + B), A=1/(1+a), B=a/(1+a)
            const float ka = 0.055f;
            const float a  = 1.0f / (1.0f + ka); // 1 / (1+a)
            const float b  = ka   / (1.0f + ka); // a / (1+a)
            return Mathf.Pow(c * a + b, 2.4f);
        }

        // CIE XYZ(D65) -> Linear RGB
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L147
        // https://en.wikipedia.org/wiki/SRGB#Specification_of_the_transformation
        internal static Vector3 XyzToLrgb(Vector3 xyz)
        {
            var cieMx = new Vector3(3.2409699419045214f,   -1.5373831775700935f,  -0.49861076029300328f);
            var cieMy = new Vector3(-0.96924363628087983f, 1.8759675015077207f,   0.041555057407175613f);
            var cieMz = new Vector3(0.055630079696993609f, -0.20397695888897657f, 1.0569715142428786f);
            return new Vector3(
                Dot(xyz, cieMx),
                Dot(xyz, cieMy),
                Dot(xyz, cieMz)
            );
        }

        // Linear RGB -> CIE XYZ(D65)
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L160
        internal static Vector3 LrgbToXyz(Vector3 lrgb)
        {
            var cieImx = new Vector3(0.41239079926595948f,  0.35758433938387796f, 0.18048078840183429f);
            var cieImy = new Vector3(0.21263900587151036f,  0.71516867876775593f, 0.072192315360733715f);
            var cieImz = new Vector3(0.019330818715591851f, 0.11919477979462599f, 0.95053215224966058f);
            return new Vector3(
                Dot(cieImx, lrgb),
                Dot(cieImy, lrgb),
                Dot(cieImz, lrgb)
            );
        }

        // CIE XYZ -> CIE LUV
        // https://en.wikipedia.org/wiki/CIELUV#The_forward_transformation
        internal static Vector3 XyzToLuv(Vector3 xyz)
        {
            float x = xyz.x;
            float y = xyz.y;
            float l = YToL(y);
            if (TooSmall(l)) {
                return new Vector3(l, 0, 0);
            }

            float dv = 1.0f / Dot(xyz, new Vector3(1.0f, 15.0f, 3.0f));

            return new Vector3(
                l,
                l * (52.0f  * (x * dv) - 2.57179f),
                l * (117.0f * (y * dv) - 6.08816f)
            );
        }

        // Y_to_L(Y)
        // https://github.com/hsluv/hsluv/blob/78b676629647f501e257fc5a4c135da6710c0c6c/math/cie.mac#L56
        private static float YToL(float y) => (y <= 0.0088564516790356308f)
            ? (y * 903.2962962962963f)
            : (116.0f * Mathf.Pow(y, 1.0f / 3.0f) - 16.0f);

        // CIE LUV -> CIE XYZ
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L237
        // https://en.wikipedia.org/wiki/CIELUV#The_reverse_transformation
        internal static Vector3 LuvToXyz(Vector3 luv)
        {
            float l = luv.x;
            if (TooSmall(l)) {
                return new Vector3(0, 0, 0);
            }

            float u  = luv.y / (13.0f * l) + 0.19783000664283681f;
            float v  = luv.z / (13.0f * l) + 0.468319994938791f;
            float iV = 1.0f / v;

            float y = LToY(l);
            float x = 2.25f * u * y * iV;
            float z = (3.0f * iV - 5.0f) * y - (x * (1.0f / 3.0f));
            return new Vector3(x, y, z);
        }

        private static float LToY(float l) => (l <= 8.0)
            ? (l * (1.0f / 903.2962962962963f))
            : (Mathf.Pow((l + 16.0f) * (1.0f / 116.0f), 3.0f));

        // CIE LUV to CIE LCh_uv
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L260
        // https://en.wikipedia.org/wiki/CIELUV#Cylindrical_representation_(CIELCH)
        internal static Vector3 LuvToLch(Vector3 luv)
        {
            float l = luv.x;
            float u = luv.y;
            float v = luv.z;
            float c = Mathf.Sqrt(u * u + v * v);

            // Greys: disambiguate hue
            // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L269
            if (TooSmall(c)) {
                return new Vector3(l, c, 0);
            }

            float h = Degrees(radian: Mathf.Atan2(v, u)); // radians to degrees
            if (h < 0.0f) {
                h = 360.0f + h;
            }
            return new Vector3(l, c, h);
        }

        // CIE LCh_uv to CIE LUV
        internal static Vector3 LchToLuv(Vector3 lch)
        {
            float hRad = Radians(degree: lch.z); // degrees to radians
            return new Vector3(lch.x, Mathf.Cos(hRad) * lch.y, Mathf.Sin(hRad) * lch.y);
        }

        //
        //  HSLuv
        //

        // HSLuv -> CIE LCH
        internal static Vector3 HsluvToLch(Vector3 hsluv)
        {
            float h = hsluv.x, s = hsluv.y, l = hsluv.z;
            // White and black: disambiguate chroma
            // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L310
            float c = TooSmallOrAlmost100(l)
                ? 0
                : MaxChromaForLh(l, h) * s * 0.01f;
            if (TooSmall(s)) {
                h = 0;
            }
            return new Vector3(l, c, h);
        }

        // CIE LCH -> HSLuv
        internal static Vector3 LchToHsluv(Vector3 lch)
        {
            float l = lch.x, c = lch.y, h = lch.z;
            // White and black: disambiguate chroma
            // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L335
            float s = TooSmallOrAlmost100(l)
                ? 0
                : c * 100.0f / MaxChromaForLh(l, h);
            if (TooSmall(c)) {
                h = 0;
            }
            return new Vector3(h, s, l);
        }

        //
        //  HPLuv
        //

        // HPLuv -> CIE LCH
        internal static Vector3 HpluvToLch(Vector3 hpluv)
        {
            float h = hpluv.x;
            float s = hpluv.y;
            float l = hpluv.z;
            float c = TooSmallOrAlmost100(l)
                ? 0
                : MaxSafeChromaForL(l) * s * 0.01f;
            if (TooSmall(s)) {
                h = 0;
            }
            return new Vector3(l, c, h);
        }

        // CIE LCH -> HPLuv
        internal static Vector3 LchToHpluv(Vector3 lch)
        {
            float l = lch.x;
            float c = lch.y;
            float h = lch.z;
            float s = TooSmallOrAlmost100(l)
                ? 0
                : c * 100.0f / MaxSafeChromaForL(l);
            if (TooSmall(c)) {
                h = 0;
            }
            return new Vector3(h, s, l);
        }

        // Hex conversion
        internal static string SrgbToHex(Vector3 srgb)
        {
            var sb = new StringBuilder();
            int r  = (int)(srgb.x * 255.0f);
            int g  = (int)(srgb.y * 255.0f);
            int b  = (int)(srgb.z * 255.0f);
            sb.Append($"#{r:x2}{g:x2}{b:x2}");
            return sb.ToString();
        }

        // hex7 : "#RRGGBB"
        internal static Vector3 HexToSrgb(string hex7)
        {
            if (hex7 == null || hex7.Length != 7 || hex7[0] != '#') {
                return default;
            }
            var hex = new char[6];
            for (int i = 0; i < 6; i++) {
                hex[i] = hex7[i + 1];
            }
            int i32 = Convert.ToInt32(new string(hex), fromBase: 16);
            return new Vector3(
                ((i32 / 256 / 256) % 256) / 255.0f,
                ((i32 / 256)       % 256) / 255.0f,
                ((i32)             % 256) / 255.0f
            );
        }
    }
}
