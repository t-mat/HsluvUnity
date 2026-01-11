// HSLuv rev4 library for Unity.
// SPDX-FileCopyrightText: Copyright (c) Takayuki Matsuoka
// SPDX-License-Identifier: MIT

using System;
using System.Text;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;

namespace Hsluv {
    // ReSharper disable UnusedType.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable MemberCanBePrivate.Global
    public static class Hsluv {
        //
        // Standard HSLuv functions (sRGB, float3)
        //
        public static float3 LchToRgb(float3 v)   => LchToSrgb(v);
        public static float3 RgbToLch(float3 v)   => SrgbToLch(v);
        public static float3 HsluvToRgb(float3 v) => HsluvToSrgb(v);
        public static float3 RgbToHsluv(float3 v) => SrgbToHsluv(v);
        public static float3 HpluvToRgb(float3 v) => HpluvToSrgb(v);
        public static float3 RgbToHpluv(float3 v) => SrgbToHpluv(v);
        public static float3 LuvToRgb(float3 v)   => LuvToSrgb(v);
        public static float3 RgbToLuv(float3 v)   => SrgbToLuv(v);

        //
        // Standard HSLuv functions (sRGB, Vector3)
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

        // Linear RGB color conversion, float3
        public static float3 LchToLrgb(float3 v)   => Impl.XyzToLrgb(Impl.LuvToXyz(Impl.LchToLuv(v)));
        public static float3 LrgbToLch(float3 v)   => Impl.LuvToLch(Impl.XyzToLuv(Impl.LrgbToXyz(v)));
        public static float3 HsluvToLrgb(float3 v) => LchToLrgb(Impl.HsluvToLch(v));
        public static float3 LrgbToHsluv(float3 v) => Impl.LchToHsluv(LrgbToLch(v));
        public static float3 HpluvToLrgb(float3 v) => LchToLrgb(Impl.HpluvToLch(v));
        public static float3 LrgbToHpluv(float3 v) => Impl.LchToHpluv(LrgbToLch(v));
        public static float3 LuvToLrgb(float3 v)   => Impl.XyzToLrgb(Impl.LuvToXyz(v));
        public static float3 LrgbToLuv(float3 v)   => Impl.XyzToLuv(Impl.LrgbToXyz(v));
        public static float3 XyzToLrgb(float3 v)   => Impl.XyzToLrgb(v);
        public static float3 LrgbToXyz(float3 v)   => Impl.LrgbToXyz(v);

        // Linear RGB color conversion, Vector3
        public static Vector3 LchToLrgb(Vector3 v)   => LchToLrgb((float3)v);
        public static Vector3 LrgbToLch(Vector3 v)   => LrgbToLch((float3)v);
        public static Vector3 HsluvToLrgb(Vector3 v) => HsluvToLrgb((float3)v);
        public static Vector3 LrgbToHsluv(Vector3 v) => LrgbToHsluv((float3)v);
        public static Vector3 HpluvToLrgb(Vector3 v) => HpluvToLrgb((float3)v);
        public static Vector3 LrgbToHpluv(Vector3 v) => LrgbToHpluv((float3)v);
        public static Vector3 LuvToLrgb(Vector3 v)   => LuvToLrgb((float3)v);
        public static Vector3 LrgbToLuv(Vector3 v)   => LrgbToLuv((float3)v);
        public static Vector3 XyzToLrgb(Vector3 v)   => XyzToLrgb((float3)v);
        public static Vector3 LrgbToXyz(Vector3 v)   => LrgbToXyz((float3)v);

        // sRGB (Gamma RGB) color conversion, float3
        public static float3 LchToSrgb(float3 v)   => Impl.LrgbToSrgb(LchToLrgb(v));
        public static float3 HsluvToSrgb(float3 v) => Impl.LrgbToSrgb(HsluvToLrgb(v));
        public static float3 HpluvToSrgb(float3 v) => Impl.LrgbToSrgb(HpluvToLrgb(v));
        public static float3 LuvToSrgb(float3 v)   => Impl.LrgbToSrgb(LuvToLrgb(v));
        public static float3 SrgbToLch(float3 v)   => LrgbToLch(Impl.SrgbToLrgb(v));
        public static float3 SrgbToHsluv(float3 v) => LrgbToHsluv(Impl.SrgbToLrgb(v));
        public static float3 SrgbToHpluv(float3 v) => LrgbToHpluv(Impl.SrgbToLrgb(v));
        public static float3 SrgbToLuv(float3 v)   => LrgbToLuv(Impl.SrgbToLrgb(v));
        public static float3 XyzToSrgb(float3 v)   => Impl.LrgbToSrgb(Impl.XyzToLrgb(v));
        public static float3 SrgbToXyz(float3 v)   => Impl.LrgbToXyz(Impl.SrgbToLrgb(v));

        // sRGB (Gamma RGB) color conversion, Vector3
        public static Vector3 LchToSrgb(Vector3 v)   => LchToSrgb((float3)v);
        public static Vector3 HsluvToSrgb(Vector3 v) => HsluvToSrgb((float3)v);
        public static Vector3 HpluvToSrgb(Vector3 v) => HpluvToSrgb((float3)v);
        public static Vector3 LuvToSrgb(Vector3 v)   => LuvToSrgb((float3)v);
        public static Vector3 SrgbToLch(Vector3 v)   => SrgbToLch((float3)v);
        public static Vector3 SrgbToHsluv(Vector3 v) => SrgbToHsluv((float3)v);
        public static Vector3 SrgbToHpluv(Vector3 v) => SrgbToHpluv((float3)v);
        public static Vector3 SrgbToLuv(Vector3 v)   => SrgbToLuv((float3)v);
        public static Vector3 XyzToSrgb(Vector3 v)   => XyzToSrgb((float3)v);
        public static Vector3 SrgbToXyz(Vector3 v)   => SrgbToXyz((float3)v);

        //
        // HSL Component interpolation
        //
        public static float InterpolateH(float a, float b, float t) => Impl.InterpolateH(a, b, t);
        public static float InterpolateS(float a, float b, float t) => lerp(a, b, t);
        public static float InterpolateL(float a, float b, float t) => lerp(a, b, t);

        //
        // Color space conversion
        //

        // Color space conversion, float3
        public static float3 HpluvToLch(float3 v) => Impl.HpluvToLch(v);
        public static float3 HsluvToLch(float3 v) => Impl.HsluvToLch(v);
        public static float3 LchToLuv(float3 v)   => Impl.LchToLuv(v);
        public static float3 LuvToXyz(float3 v)   => Impl.LuvToXyz(v);
        public static float3 XyzToLuv(float3 v)   => Impl.XyzToLuv(v);
        public static float3 LuvToLch(float3 v)   => Impl.LuvToLch(v);
        public static float3 LchToHsluv(float3 v) => Impl.LchToHsluv(v);
        public static float3 LchToHpluv(float3 v) => Impl.LchToHpluv(v);

        // Color space conversion, Vector3
        public static Vector3 HpluvToLch(Vector3 v) => HpluvToLch((float3)v);
        public static Vector3 HsluvToLch(Vector3 v) => HsluvToLch((float3)v);
        public static Vector3 LchToLuv(Vector3 v)   => LchToLuv((float3)v);
        public static Vector3 LuvToXyz(Vector3 v)   => LuvToXyz((float3)v);
        public static Vector3 XyzToLuv(Vector3 v)   => XyzToLuv((float3)v);
        public static Vector3 LuvToLch(Vector3 v)   => LuvToLch((float3)v);
        public static Vector3 LchToHsluv(Vector3 v) => LchToHsluv((float3)v);
        public static Vector3 LchToHpluv(Vector3 v) => LchToHpluv((float3)v);

        //
        // Hex string utility
        //

        // Color to hex conversion (float3 -> #RRGGBB)
        public static string RgbToHex(float3 v)                => SrgbToHex(v);
        public static string SrgbToHex(float3 v)               => Impl.SrgbToHex(v);
        public static string LrgbToHex(float3 v)               => Impl.SrgbToHex(Impl.LrgbToSrgb(v));
        public static int    RgbToHex(Span<char> s, float3 v)  => SrgbToHex(s, v);
        public static int    SrgbToHex(Span<char> s, float3 v) => Impl.SrgbToHexSpan(s, v);
        public static int    LrgbToHex(Span<char> s, float3 v) => Impl.SrgbToHexSpan(s, Impl.LrgbToSrgb(v));

        // Hex to color conversion (#RRGGBB -> float3)
        public static float3 HexToRgbFloat3(string s)  => HexToSrgbFloat3(s);
        public static float3 HexToSrgbFloat3(string s) => Impl.HexToSrgb(s);
        public static float3 HexToLrgbFloat3(string s) => Impl.SrgbToLrgb(Impl.HexToSrgb(s));

        public static float3 HexToRgb(string s, float3 defaultValue)  => HexToSrgb(s, defaultValue);
        public static float3 HexToSrgb(string s, float3 defaultValue) => Impl.HexToSrgb(s, defaultValue);
        public static float3 HexToLrgb(string s, float3 defaultValue) => Impl.SrgbToLrgb(HexToSrgb(s, defaultValue));

        public static float3 HexToRgb(ReadOnlySpan<char> s, float3 defaultValue) =>
            HexToSrgb(s, defaultValue);

        public static float3 HexToSrgb(ReadOnlySpan<char> s, float3 defaultValue) =>
            Impl.HexToSrgb(s, defaultValue);

        public static float3 HexToLrgb(ReadOnlySpan<char> s, float3 defaultValue) =>
            Impl.SrgbToLrgb(Impl.HexToSrgb(s, defaultValue));

        // Color to hex conversion (Vector3 -> #RRGGBB)
        public static string RgbToHex(Vector3 v)                => SrgbToHex(v);
        public static string SrgbToHex(Vector3 v)               => SrgbToHex((float3)v);
        public static string LrgbToHex(Vector3 v)               => LrgbToHex((float3)v);
        public static int    RgbToHex(Span<char> s, Vector3 v)  => SrgbToHex(s, v);
        public static int    SrgbToHex(Span<char> s, Vector3 v) => SrgbToHex(s, (float3)v);
        public static int    LrgbToHex(Span<char> s, Vector3 v) => LrgbToHex(s, (float3)v);

        // Hex to color conversion (#RRGGBB -> Vector3)
        public static Vector3 HexToRgb(string s)                                    => HexToSrgb(s);
        public static Vector3 HexToSrgb(string s)                                   => HexToSrgbFloat3(s);
        public static Vector3 HexToLrgb(string s)                                   => HexToLrgbFloat3(s);
        public static Vector3 HexToRgb(ReadOnlySpan<char> s, Vector3 defaultValue)  => HexToSrgb(s, defaultValue);
        public static Vector3 HexToSrgb(ReadOnlySpan<char> s, Vector3 defaultValue) => HexToSrgb(s, (float3)defaultValue);
        public static Vector3 HexToLrgb(ReadOnlySpan<char> s, Vector3 defaultValue) => HexToLrgb(s, (float3)defaultValue);
    }

    //
    // Color manipulation in HSLuv color space
    //
    public static class HsluvColorSpace {
        //
        // Color interpolation (HSLuv)
        //

        // Interpolate HSLuv colors in HSLuv color space.
        public static Vector3 LerpHsluv(Vector3 hsluvA, Vector3 hsluvB, float t) =>
            LerpHsluv((float3)hsluvA, (float3)hsluvB, t);

        public static float3 LerpHsluv(float3 hsluvA, float3 hsluvB, float t)
        {
            t = clamp(t, 0f, 1f);
            float h = Hsluv.InterpolateH(hsluvA.x, hsluvB.x, t);
            float s = Hsluv.InterpolateS(hsluvA.y, hsluvB.y, t);
            float l = Hsluv.InterpolateL(hsluvA.z, hsluvB.z, t);
            return new float3(h, s, l);
        }

        // Interpolate linear RGB colors in HSLuv color space.
        public static Vector3 LerpLinearRgb(Vector3 lrgbA, Vector3 lrgbB, float t) =>
            LerpLinearRgb((float3)lrgbA, (float3)lrgbB, t);

        public static float3 LerpLinearRgb(float3 lrgbA, float3 lrgbB, float t)
        {
            float3 a = Hsluv.LrgbToHsluv(lrgbA);
            float3 b = Hsluv.LrgbToHsluv(lrgbB);
            float3 p = LerpHsluv(a, b, t);
            float3 o = Hsluv.HsluvToLrgb(p);
            return o;
        }

        // Interpolate linear RGB colors in HSLuv color space.
        public static Color LerpLinearRgb(Color lrgbA, Color lrgbB, float t) =>
            Impl.Float4ToColor(LerpLinearRgb(Impl.ColorToFloat4(lrgbA), Impl.ColorToFloat4(lrgbB), t));

        public static float4 LerpLinearRgb(float4 lrgbA, float4 lrgbB, float t)
        {
            float3 o = LerpLinearRgb(lrgbA.xyz, lrgbB.xyz, t);
            float  w = lerp(lrgbA.w, lrgbB.w, t);
            return new float4(o.xyz, w);
        }

        // Interpolate sRGB (gamma) colors in HSLuv color space.
        public static Color LerpSrgb(Color srgbA, Color srgbB, float t) =>
            Impl.Float4ToColor(LerpSrgb(Impl.ColorToFloat4(srgbA), Impl.ColorToFloat4(srgbB), t));

        public static float4 LerpSrgb(float4 srgbA, float4 srgbB, float t)
        {
            float3 a = Impl.SrgbToLrgb(srgbA.xyz);
            float3 b = Impl.SrgbToLrgb(srgbB.xyz);
            float3 p = LerpLinearRgb(a, b, t);
            float3 o = Impl.LrgbToSrgb(p);
            float  w = lerp(srgbA.w, srgbB.w, t);
            return new float4(o.xyz, w);
        }

        //
        // Hue rotation (HSLuv)
        //

        // Rotate hue of HSLuv color in HSLuv color space.
        public static Vector3 RotateHsluvHue(Vector3 hsluv, float deltaHueInDegrees) =>
            RotateHsluvHue((float3)hsluv, deltaHueInDegrees);

        public static float3 RotateHsluvHue(float3 hsluv, float deltaHueInDegrees)
        {
            float h = Impl.NormalizeH(hsluv.x + deltaHueInDegrees);
            float s = hsluv.y;
            float l = hsluv.z;
            return new float3(h, s, l);
        }

        // Rotate hue of linear RGB color in HSLuv color space.
        public static Vector3 RotateLinearRgbHue(Vector3 lrgb, float deltaHueInDegrees) =>
            RotateLinearRgbHue((float3)lrgb, deltaHueInDegrees);

        public static float3 RotateLinearRgbHue(float3 lrgb, float deltaHueInDegrees)
        {
            float3 a = Hsluv.LrgbToHsluv(lrgb);
            float3 p = RotateHsluvHue(a, deltaHueInDegrees);
            return Hsluv.HsluvToLrgb(p);
        }

        // Rotate hue of linear RGB color in HSLuv color space.
        public static Color RotateLinearRgbHue(Color lrgb, float deltaHueInDegrees) =>
            Impl.Float4ToColor(RotateLinearRgbHue(Impl.ColorToFloat4(lrgb), deltaHueInDegrees));

        public static float4 RotateLinearRgbHue(float4 lrgb, float deltaHueInDegrees) =>
            new(RotateLinearRgbHue(lrgb.xyz, deltaHueInDegrees), lrgb.w);

        // Rotate hue of linear sRGB (gamma) color in HSLuv color space.
        public static Color RotateSrgbHue(Color srgb, float deltaHueInDegrees) =>
            Impl.Float4ToColor(RotateSrgbHue(Impl.ColorToFloat4(srgb), deltaHueInDegrees));

        public static float4 RotateSrgbHue(float4 srgb, float deltaHueInDegrees)
        {
            float3 a = Impl.SrgbToLrgb(srgb.xyz);
            float3 p = RotateLinearRgbHue(a, deltaHueInDegrees);
            float3 o = Impl.LrgbToSrgb(p);
            return new float4(o.x, o.y, o.z, srgb.w);
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
        public static Vector3 LerpHpluv(Vector3 a, Vector3 b, float t) =>
            LerpHpluv((float3)a, (float3)b, t);

        public static float3 LerpHpluv(float3 a, float3 b, float t)
        {
            t = clamp(t, 0f, 1f);
            float h = Hsluv.InterpolateH(a.x, b.x, t);
            float s = Hsluv.InterpolateS(a.y, b.y, t);
            float l = Hsluv.InterpolateL(a.z, b.z, t);
            return new Vector3(h, s, l);
        }

        // Interpolate linear RGB colors in HPLuv color space.
        public static Vector3 LerpLinearRgb(Vector3 c, Vector3 d, float t) =>
            LerpLinearRgb((float3)c, (float3)d, t);

        public static float3 LerpLinearRgb(float3 c, float3 d, float t)
        {
            float3 a = Hsluv.LrgbToHpluv(c);
            float3 b = Hsluv.LrgbToHpluv(d);
            float3 p = LerpHpluv(a, b, t);
            float3 o = Hsluv.HpluvToLrgb(p);
            return o;
        }

        // Interpolate linear RGB colors in HPLuv color space.
        public static Color LerpLinearRgb(Color c, Color d, float t) =>
            Impl.Float4ToColor(LerpLinearRgb(Impl.ColorToFloat4(c), Impl.ColorToFloat4(d), t));

        public static float4 LerpLinearRgb(float4 c, float4 d, float t)
        {
            float3 o = LerpLinearRgb(c.xyz, d.xyz, t);
            float  w = lerp(c.w, d.w, t);
            return new float4(o.xyz, w);
        }

        // Interpolate sRGB (gamma) colors in HPLuv color space.
        public static Color LerpSrgb(Color c, Color d, float t) =>
            Impl.Float4ToColor(LerpSrgb(Impl.ColorToFloat4(c), Impl.ColorToFloat4(d), t));

        public static float4 LerpSrgb(float4 c, float4 d, float t)
        {
            float3 a = Impl.SrgbToLrgb(c.xyz);
            float3 b = Impl.SrgbToLrgb(d.xyz);
            float3 p = LerpLinearRgb(a, b, t);
            float3 o = Impl.LrgbToSrgb(p);
            float  w = lerp(c.w, d.w, t);
            return new float4(o.xyz, w);
        }

        //
        // Hue rotation (HPLuv)
        //

        // Rotate hue of HPLuv color in HPLuv color space.
        public static Vector3 RotateHpluvHue(Vector3 a, float deltaHueInDegrees) =>
            RotateHpluvHue((float3)a, deltaHueInDegrees);

        public static float3 RotateHpluvHue(float3 a, float deltaHueInDegrees)
        {
            float h = Impl.NormalizeH(a.x + deltaHueInDegrees);
            float s = a.y;
            float l = a.z;
            return new float3(h, s, l);
        }

        // Rotate hue of linear RGB color in HPLuv color space.
        public static Vector3 RotateLinearRgbHue(Vector3 c, float deltaHueInDegrees) =>
            RotateLinearRgbHue((float3)c, deltaHueInDegrees);

        public static float3 RotateLinearRgbHue(float3 c, float deltaHueInDegrees)
        {
            float3 a = Hsluv.LrgbToHpluv(c);
            float3 p = RotateHpluvHue(a, deltaHueInDegrees);
            return Hsluv.HpluvToLrgb(p);
        }

        // Rotate hue of linear RGB color in HPLuv color space.
        public static Color RotateLinearRgbHue(Color c, float deltaHueInDegrees) =>
            Impl.Float4ToColor(RotateLinearRgbHue(Impl.ColorToFloat4(c), deltaHueInDegrees));

        public static float4 RotateLinearRgbHue(float4 c, float deltaHueInDegrees) =>
            new(RotateLinearRgbHue(c.xyz, deltaHueInDegrees), c.w);

        // Rotate hue of linear sRGB (gamma) color in HPLuv color space.
        public static Color RotateSrgbHue(Color c, float deltaHueInDegrees) =>
            Impl.Float4ToColor(RotateSrgbHue(Impl.ColorToFloat4(c), deltaHueInDegrees));

        public static float4 RotateSrgbHue(float4 c, float deltaHueInDegrees)
        {
            float3 a = Impl.SrgbToLrgb(c.xyz);
            float3 p = RotateLinearRgbHue(a, deltaHueInDegrees);
            float3 o = Impl.LrgbToSrgb(p);
            return new float4(o.xyz, c.w);
        }
    }
    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore UnusedType.Global

    internal static class Impl {
        private const float PI      = 3.14159265358979323846f;
        private const float Epsilon = 0.00885645167903563082f;   // pow(6/29,3)
        private const float Kappa   = 112.91203703703703703704f; // pow(29/6,3)
        private const float Eps     = 0.0001220703125f;          // pow(2,-13)

        private static bool   TooSmall(float v)               => v < Eps;
        private static bool   TooSmallOrAlmost100(float v)    => TooSmall(v) || v > 100.0f - Eps;
        private static float  Min3(float x, float y, float z) => min(x, min(y, z));
        public static  float4 ColorToFloat4(Color c)          => new(c.r, c.g, c.b, c.a);
        public static  Color  Float4ToColor(float4 f)         => new(f.x, f.y, f.z, f.w);

        public static float InterpolateH(float a, float b, float t)
        {
            return NormalizeH(a + NormalizeDh(b - a) * t);

            static float NormalizeDh(float dh)
            {
                dh %= 360.0f;
                return dh switch
                {
                    < -180.0f => dh + 360.0f,
                    >= 180.0f => dh - 360.0f,
                    _ => dh
                };
            }
        }

        public static float NormalizeH(float h)
        {
            h %= 360.0f;
            if (h < 0.0f) {
                h += 360.0f;
            }
            return h;
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
            float3 m0 = new(3.2409699419045214f, -1.5373831775700935f, -0.49861076029300328f);
            float3 m1 = new(-0.96924363628087983f, 1.8759675015077207f, 0.041555057407175613f);
            float3 m2 = new(0.055630079696993609f, -0.20397695888897657f, 1.0569715142428786f);

            const float kd = 1.0f / 126452.0f;

            float3 k = new(731718.0f, 769860.0f, 838422.0f);

            float bounds1XKx = (m0.x * 3.0f - m0.z) * (94839.0f * kd);
            float bounds1XKy = (m1.x * 3.0f - m1.z) * (94839.0f * kd);
            float bounds1XKz = (m2.x * 3.0f - m2.z) * (94839.0f * kd);

            float bounds1XLx = m0.z * 5.0f - m0.y;
            float bounds1XLy = m1.z * 5.0f - m1.y;
            float bounds1Xlz = m2.z * 5.0f - m2.y;

            x0X = bounds1XKx / bounds1XLx;
            x0Y = bounds1XKy / bounds1XLy;
            x0Z = bounds1XKz / bounds1Xlz;

            float b1YAx = dot(m0, k) * kd;
            float b1YAy = dot(m1, k) * kd;
            float b1YAz = dot(m2, k) * kd;
            float b1Yc  = k.y        * kd;

            float b0YKx = b1YAx / bounds1XLx;
            float b0YKy = b1YAy / bounds1XLy;
            float b0YKz = b1YAz / bounds1Xlz;

            // pow3(116) = pow(116,3) = 1560896
            float sub1 = Pow3(l + 16.0f) * (1.0f / Pow3(116.0f));
            float sub2 = sub1 > Epsilon
                ? sub1
                : l * (1.0f / Kappa);

            x1X = bounds1XKx * sub2 / (bounds1XLx * sub2 + 1.0f);
            x1Y = bounds1XKy * sub2 / (bounds1XLy * sub2 + 1.0f);
            x1Z = bounds1XKz * sub2 / (bounds1Xlz * sub2 + 1.0f);

            y0X = b0YKx * l;
            y0Y = b0YKy * l;
            y0Z = b0YKz * l;

            y1X = l * (b1YAx * sub2 - b1Yc) / (bounds1XLx * sub2 + 1.0f);
            y1Y = l * (b1YAy * sub2 - b1Yc) / (bounds1XLy * sub2 + 1.0f);
            y1Z = l * (b1YAz * sub2 - b1Yc) / (bounds1Xlz * sub2 + 1.0f);

            return;

            static float Pow3(float x) => x * x * x;
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
            float minDistSq = min(Min3(lenSq0X, lenSq0Y, lenSq0Z), Min3(lenSq1X, lenSq1Y, lenSq1Z));
            return sqrt(minDistSq);

            static void IntersectLineLine(
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

            static void DistanceFromPoleSq(
                float ax, float ay, float az,
                float bx, float by, float bz,
                out float x, out float y, out float z
            )
            {
                x = ax * ax + bx * bx;
                y = ay * ay + by * by;
                z = az * az + bz * bz;
            }
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
            float hRad = radians(h);
            float s    = sin(hRad);
            float c    = -cos(hRad);
            LengthOfRayUntilIntersect(s, c, x0X, x0Y, x0Z, y0X, y0Y, y0Z, out float l0, out float l1, out float l2);
            LengthOfRayUntilIntersect(s, c, x1X, x1Y, x1Z, y1X, y1Y, y1Z, out float r0, out float r1, out float r2);
            return min(Min3(l0, l1, l2), Min3(r0, r1, r2));

            static void LengthOfRayUntilIntersect(
                float s, float c, float xx, float xy, float xz, float yx, float yy, float yz,
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
        }

        //
        // Color space conversion functions
        //

        // LinearRGB -> sRGB
        // https://en.wikipedia.org/wiki/SRGB#Specification_of_the_transformation
        // https://github.com/hsluv/hsluv/blob/78b676629647f501e257fc5a4c135da6710c0c6c/math/cie.mac#L72
        public static float3 LrgbToSrgb(float3 lrgb)
        {
            return new float3(LinearToSrgb(lrgb.x), LinearToSrgb(lrgb.y), LinearToSrgb(lrgb.z));

            static float LinearToSrgb(float c) =>
                c <= 0.0031308f
                    ? 12.92f * c
                    : 1.055f * pow(c, 1.0f / 2.4f) - 0.055f;
        }

        // sRGB -> LinearRGB
        public static float3 SrgbToLrgb(float3 srgb)
        {
            return new float3(SrgbToLinear(srgb.x), SrgbToLinear(srgb.y), SrgbToLinear(srgb.z));

            // sRGB -> LinearRGB conversion (component)
            // https://en.wikipedia.org/wiki/SRGB#The_reverse_transformation
            // https://github.com/hsluv/hsluv/blob/78b676629647f501e257fc5a4c135da6710c0c6c/math/cie.mac#L77
            static float SrgbToLinear(float c)
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
                return pow(c * a + b, 2.4f);
            }
        }

        // CIE XYZ(D65) -> Linear RGB
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L147
        // https://en.wikipedia.org/wiki/SRGB#Specification_of_the_transformation
        public static float3 XyzToLrgb(float3 xyz)
        {
            float3 cieMx = new(3.2409699419045214f, -1.5373831775700935f, -0.49861076029300328f);
            float3 cieMy = new(-0.96924363628087983f, 1.8759675015077207f, 0.041555057407175613f);
            float3 cieMz = new(0.055630079696993609f, -0.20397695888897657f, 1.0569715142428786f);
            return new float3(
                dot(xyz, cieMx),
                dot(xyz, cieMy),
                dot(xyz, cieMz)
            );
        }

        // Linear RGB -> CIE XYZ(D65)
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L160
        public static float3 LrgbToXyz(float3 lrgb)
        {
            float3 cieImx = new(0.41239079926595948f, 0.35758433938387796f, 0.18048078840183429f);
            float3 cieImy = new(0.21263900587151036f, 0.71516867876775593f, 0.072192315360733715f);
            float3 cieImz = new(0.019330818715591851f, 0.11919477979462599f, 0.95053215224966058f);
            return new float3(
                dot(cieImx, lrgb),
                dot(cieImy, lrgb),
                dot(cieImz, lrgb)
            );
        }

        // CIE XYZ -> CIE LUV
        // https://en.wikipedia.org/wiki/CIELUV#The_forward_transformation
        public static float3 XyzToLuv(float3 xyz)
        {
            float x = xyz.x;
            float y = xyz.y;
            float l = YToL(y);
            if (TooSmall(l)) {
                return new float3(l, 0, 0);
            }

            float dv = 1.0f / dot(xyz, new float3(1.0f, 15.0f, 3.0f));

            return new float3(
                l,
                l * (52.0f  * (x * dv) - 2.57179f),
                l * (117.0f * (y * dv) - 6.08816f)
            );

            // Y_to_L(Y)
            // https://github.com/hsluv/hsluv/blob/78b676629647f501e257fc5a4c135da6710c0c6c/math/cie.mac#L56
            static float YToL(float y) =>
                y <= 0.0088564516790356308f
                    ? y * 903.2962962962963f
                    : 116.0f * pow(y, 1.0f / 3.0f) - 16.0f;
        }

        // CIE LUV -> CIE XYZ
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L237
        // https://en.wikipedia.org/wiki/CIELUV#The_reverse_transformation
        public static float3 LuvToXyz(float3 luv)
        {
            float l = luv.x;
            if (TooSmall(l)) {
                return new float3(0, 0, 0);
            }

            float u  = luv.y / (13.0f * l) + 0.19783000664283681f;
            float v  = luv.z / (13.0f * l) + 0.468319994938791f;
            float iV = 1.0f / v;

            float y = LToY(l);
            float x = 2.25f * u * y * iV;
            float z = (3.0f * iV - 5.0f) * y - x * (1.0f / 3.0f);
            return new float3(x, y, z);

            static float LToY(float l) =>
                l <= 8.0
                    ? l * (1.0f / 903.2962962962963f)
                    : pow((l + 16.0f) * (1.0f / 116.0f), 3.0f);
        }

        // CIE LUV to CIE LCh_uv
        // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L260
        // https://en.wikipedia.org/wiki/CIELUV#Cylindrical_representation_(CIELCH)
        public static float3 LuvToLch(float3 luv)
        {
            float l = luv.x;
            float u = luv.y;
            float v = luv.z;
            float c = sqrt(u * u + v * v);

            // Greys: disambiguate hue
            // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L269
            if (TooSmall(c)) {
                return new float3(l, c, 0);
            }

            float h = Degrees(radian: atan2(y: v, x: u)); // radians to degrees
            if (h < 0.0f) {
                h = 360.0f + h;
            }
            return new float3(l, c, h);

            static float Degrees(float radian) => radian * (180.0f / PI);
        }

        // CIE LCh_uv to CIE LUV
        public static float3 LchToLuv(float3 lch)
        {
            float hRad = radians(lch.z); // degrees to radians
            return new float3(lch.x, cos(hRad) * lch.y, sin(hRad) * lch.y);
        }

        //
        //  HSLuv
        //

        // HSLuv -> CIE LCH
        public static float3 HsluvToLch(float3 hsluv)
        {
            float h = hsluv.x;
            float s = hsluv.y;
            float l = hsluv.z;
            // White and black: disambiguate chroma
            // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L310
            float c = TooSmallOrAlmost100(l)
                ? 0
                : MaxChromaForLh(l, h) * s * 0.01f;
            if (TooSmall(s)) {
                h = 0;
            }
            return new float3(l, c, h);
        }

        // CIE LCH -> HSLuv
        public static float3 LchToHsluv(float3 lch)
        {
            float l = lch.x;
            float c = lch.y;
            float h = lch.z;
            // White and black: disambiguate chroma
            // https://github.com/hsluv/hsluv/blob/e15d91f432d529cc426ccd1d57a3c3c593e01a05/haxe/src/hsluv/Hsluv.hx#L335
            float s = TooSmallOrAlmost100(l)
                ? 0
                : c * 100.0f / MaxChromaForLh(l, h);
            if (TooSmall(c)) {
                h = 0;
            }
            return new float3(h, s, l);
        }

        //
        //  HPLuv
        //

        // HPLuv -> CIE LCH
        public static float3 HpluvToLch(float3 hpluv)
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
            return new float3(l, c, h);
        }

        // CIE LCH -> HPLuv
        public static float3 LchToHpluv(float3 lch)
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
            return new float3(h, s, l);
        }

        //
        // Hex conversion
        //

        // Generate "#RRGGBB" string.
        public static string SrgbToHex(float3 srgb)
        {
            int r = (int)(srgb.x * 255.0f);
            int g = (int)(srgb.y * 255.0f);
            int b = (int)(srgb.z * 255.0f);

            StringBuilder sb = new();
            sb.Append($"#{r:x2}{g:x2}{b:x2}");
            return sb.ToString();
        }

        // Write "#RRGGBB" string to span.
        public static int SrgbToHexSpan(Span<char> span, float3 srgb)
        {
            int r = (int)(srgb.x * 255.0f);
            int g = (int)(srgb.y * 255.0f);
            int b = (int)(srgb.z * 255.0f);

            int i = 0;

            span[i++] = '#';
            span[i++] = ToHexChar(r / 16);
            span[i++] = ToHexChar(r);
            span[i++] = ToHexChar(g / 16);
            span[i++] = ToHexChar(g);
            span[i++] = ToHexChar(b / 16);
            span[i++] = ToHexChar(b);

            return i;

            static char ToHexChar(int i)
            {
                i %= 16;
                return i <= 9 ? (char)('0' + i) : (char)('A' + i);
            }
        }

        // Compute float3 color value from "#RRGGBB" string.
        public static float3 HexToSrgb(string hex7, float3 defaultValue) =>
            HexToSrgb(hex7.AsSpan(), defaultValue);

        public static float3 HexToSrgb(string hex7) =>
            HexToSrgb(hex7, new float3(0, 0, 0));

        // Compute float3 color value from "#RRGGBB" span.
        public static float3 HexToSrgb(ReadOnlySpan<char> hex7, float3 defaultValue)
        {
            if (hex7.Length < 7 || hex7[0] != '#') {
                return defaultValue;
            }

            int i  = 1;
            int rh = FromHexChar(hex7[i++]);
            int rl = FromHexChar(hex7[i++]);
            int gh = FromHexChar(hex7[i++]);
            int gl = FromHexChar(hex7[i++]);
            int bh = FromHexChar(hex7[i++]);
            int bl = FromHexChar(hex7[i]);
            if (rh == -1 || rl == -1 || gh == -1 || gl == -1 || bh == -1 || bl == -1) {
                return defaultValue;
            }

            int r = rh * 16 + rl;
            int g = gh * 16 + gl;
            int b = bh * 16 + bl;

            return new float3(r / 255.0f, g / 255.0f, b / 255.0f);

            static int FromHexChar(char c) =>
                c switch
                {
                    >= '0' and <= '9' => c - '0' + 0,
                    >= 'A' and <= 'F' => c - 'A' + 10,
                    >= 'a' and <= 'f' => c - 'a' + 10,
                    _ => -1
                };
        }
    }
}
