// HSLuv rev4 library for Unity.
// SPDX-FileCopyrightText: Copyright (c) Takayuki Matsuoka
// SPDX-License-Identifier: MIT

using Hsluv;
using UnityEngine;

public class HsluvTestInterpolators : MonoBehaviour {
    public Color                   colorLeft       = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    public Color                   colorRight      = new Color(0.5f, 0.0f, 1.0f, 1.0f);
    public Interpolator.ColorSpace leftColorSpace  = Interpolator.ColorSpace.Hsv;
    public Interpolator.ColorSpace rightColorSpace = Interpolator.ColorSpace.Hsluv;

    private const int       FontSize = 32;
    private       GUIStyle  _style;
    private       Texture2D _tex2dLeft;
    private       Texture2D _tex2dRight;

    private void OnEnable()
    {
        const int           width  = 128;
        const int           height = 1;
        const TextureFormat format = TextureFormat.RGBA32;
        _tex2dLeft = new Texture2D(width, height, format, false)
                     {
                         hideFlags = HideFlags.HideAndDontSave
                     };
        _tex2dRight = new Texture2D(width, height, format, false)
                      {
                          hideFlags = HideFlags.HideAndDontSave
                      };
    }

    private void OnDisable()
    {
        DestroyObject(ref _tex2dLeft);
        DestroyObject(ref _tex2dRight);
    }

    private static void DestroyObject(ref Texture2D tex)
    {
        if (tex == null) {
            return;
        }
        Object.Destroy(tex);
        tex = null;
    }

    private void UpdateTextures()
    {
        if ((Time.frameCount % 2) == 0) {
            UpdateTexture2D(_tex2dLeft, (float t) => Interpolator.Interpolate(leftColorSpace, colorLeft, colorRight, t));
        } else {
            UpdateTexture2D(_tex2dRight, (float t) => Interpolator.Interpolate(rightColorSpace, colorLeft, colorRight, t));
        }
    }

    private static void UpdateTexture2D(Texture2D tex2d, System.Func<float, Color> fun)
    {
        int   width  = tex2d.width;
        int   height = tex2d.height;
        float k      = 1.0f / width;
        for (var y = 0; y < height; ++y) {
            for (var x = 0; x < width; ++x) {
                tex2d.SetPixel(x, y, fun(x * k));
            }
        }
        tex2d.Apply();
    }

    private void DrawTextures()
    {
        const string testString = "## TEST STRING ABCD 0123 ##";

        int    x = 0, y = 0, w = 512, h = 512;
        GUI.DrawTextureWithTexCoords(new Rect(x, y, w, h), _tex2dLeft, new Rect(0.0f, 0.0f, 1.0f, 1.0f));
        PutStrings(x, y, w, h, leftColorSpace.ToString(), testString);

        x += w + 16;
        GUI.DrawTextureWithTexCoords(new Rect(x, y, w, h), _tex2dRight, new Rect(0.0f, 0.0f, 1.0f, 1.0f));
        PutStrings(x, y, w, h, rightColorSpace.ToString(), testString);
    }

    private void PutStrings(int x, int y, int w, int h, string header, string testString)
    {
        int py = y, ph = FontSize;
        GUI.backgroundColor = Color.clear;

        void Put(Color fgColor, string str)
        {
            GUI.contentColor = fgColor;
            GUI.Label(new Rect(x, py, w, h), str, _style);
            py += ph;
        }

        Put(new Color(255, 255, 255), header);
        py += h / 4 - ph * 3 / 2;
        for (int i = 0; i <= 7; ++i) {
            float r = ((i >> 0) & 1) * 255;
            float g = ((i >> 1) & 1) * 255;
            float b = ((i >> 2) & 1) * 255;
            Put(new Color(r, g, b), testString);
        }
        py = h - ph * 3 / 2;
        Put(new Color(0, 0, 0), header);
    }

    private void OnGUI()
    {
        if (_style == null || _style.fontSize != FontSize) {
            _style = new GUIStyle(GUI.skin.box)
                     {
                         normal =
                         {
                             background = Texture2D.whiteTexture,
                             textColor  = Color.white
                         },
                         fontSize = FontSize
                     };
        }
        UpdateTextures();
        DrawTextures();
    }

    public static class Interpolator {
        public enum ColorSpace {
            Srgb,
            LinearRgb,
            Hsv,
            Hsluv,
            Hpluv
        }

        public static Color Interpolate(ColorSpace colorSpace, Color srgb0, Color srgb1, float t)
        {
            switch (colorSpace) {
                case ColorSpace.Srgb:      return InterpolateInSrgb(srgb0, srgb1, t);
                case ColorSpace.LinearRgb: return InterpolateInLrgb(srgb0, srgb1, t);
                case ColorSpace.Hsv:       return InterpolateInHsv(srgb0, srgb1, t);
                case ColorSpace.Hsluv:     return InterpolateInHsluv(srgb0, srgb1, t);
                case ColorSpace.Hpluv:     return InterpolateInHpluv(srgb0, srgb1, t);
                default:                   return default;
            }
        }

        private static float Lerp(float a, float b, float t)
            => a + (b - a) * t;

        private static float LerpH(float a, float b, float t)
        {
            float dh = NormalizeDh(b - a);
            return NormalizeH(a + dh * t);
        }

        private static float Fmod360(float a) => a % 360.0f;

        private static float NormalizeH(float h)
        {
            h = Fmod360(h);
            if (h < 0.0f) {
                h += 360.0f;
            }
            return h;
        }

        private static float NormalizeDh(float dh)
        {
            dh = Fmod360(dh);
            if (dh < -180.0f) {
                dh += 360.0f;
            } else if (dh >= 180.0f) {
                dh -= 360.0f;
            }
            return dh;
        }

        // Interpolate sRGB colors in sRGB color space.  Returns sRGB color.
        private static Color InterpolateInSrgb(Color srgb0, Color srgb1, float t)
            => Color.Lerp(srgb0, srgb1, t);

        // Interpolate sRGB colors in linear RGB color space.  Returns sRGB color.
        private static Color InterpolateInLrgb(Color srgb0, Color srgb1, float t)
        {
            Color lrgb0 = srgb0.linear;
            Color lrgb1 = srgb1.linear;
            Color lrgbi = Color.Lerp(lrgb0, lrgb1, t);
            return lrgbi.gamma;
        }

        // Interpolate sRGB colors in HSV color space.  Returns sRGB color.
        private static Color InterpolateInHsv(Color srgb0, Color srgb1, float t)
        {
            Color.RGBToHSV(srgb0, out float h0, out float s0, out float v0);
            Color.RGBToHSV(srgb1, out float h1, out float s1, out float v1);
            h0 *= 360.0f;
            h1 *= 360.0f;
            float hi = LerpH(h0, h1, t) / 360.0f;
            float si = Lerp(s0, s1, t);
            float vi = Lerp(v0, v1, t);
            return Color.HSVToRGB(hi, si, vi);
        }

        // Interpolate sRGB colors in HSLuv color space.  Returns sRGB color.
        private static Color InterpolateInHsluv(Color srgb0, Color srgb1, float t)
            => HsluvColorSpace.LerpSrgb(srgb0, srgb1, t);

        // Interpolate sRGB colors in HPLuv color space.  Returns sRGB color.
        private static Color InterpolateInHpluv(Color srgb0, Color srgb1, float t)
            => HpluvColorSpace.LerpSrgb(srgb0, srgb1, t);
    }
}
