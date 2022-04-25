// HSLuv rev4 library for Unity.
// SPDX-FileCopyrightText: Copyright (c) Takayuki Matsuoka
// SPDX-License-Identifier: MIT

using Hsluv;
using UnityEngine;

public class HsluvTestHueRotation : MonoBehaviour {
    public                Color bgColor  = new Color(0.7f, 0.0f, 0.0f, 1.0f);
    public                Color fgColor  = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    [Range(0, 1)]  public float speed    = 0.5f;
    [Range(8, 64)] public int   fontSize = 32;

    private float    _hueOffset = 0.0f;
    private GUIStyle _style;

    private void OnGUI()
    {
        _hueOffset += Mathf.Pow(speed, 2) * Time.deltaTime;

        if (_style == null || _style.fontSize != fontSize) {
            _style                   = new GUIStyle(GUI.skin.box);
            _style.normal.background = Texture2D.whiteTexture;
            _style.normal.textColor  = Color.white;
            _style.fontSize          = fontSize;
        }

        void Put(Color contentColor, Color backgroundColor, string s)
        {
            GUI.contentColor    = contentColor;
            GUI.backgroundColor = backgroundColor;
            GUILayout.Box(s, _style);
        }

        Put(Color.white, Color.black, "## Comparison of HSLuv, HPLuv and HSV Hue rotation ##");
        Put(Color.white, Color.black, "Background colors are rotated in different color space.");

        const int n = 12;
        for (int i = 0; i < n; i++) {
            float a = i / ((float)(n - 1));
            float t = a + _hueOffset;
            GUILayout.BeginHorizontal();
            GUILayout.Space(32.0f);
            {
                Color bg = HsluvColorSpace.RotateSrgbHue(srgb: bgColor, deltaHueInDegrees: t * 360.0f);
                Put(fgColor, bg, " HSLuv Hue Rotation ");
            }
            GUILayout.Space(32.0f);
            {
                Color bg = HpluvColorSpace.RotateSrgbHue(bgColor, deltaHueInDegrees: t * 360.0f);
                Put(fgColor, bg, " HPLuv Hue Rotation ");
            }
            GUILayout.Space(32.0f);
            {
                Color bg = HSV.RotateGammaColorHue(bgColor, deltaHue: t);
                Put(fgColor, bg, " HSV Hue Rotation ");
            }
            GUILayout.Space(32.0f);
            GUILayout.EndHorizontal();
        }

        Put(Color.white, Color.black, "Compare contrast of foreground and background colors.");
    }

    private static class HSV {
        public static Color RotateGammaColorHue(Color c, float deltaHue)
        {
            Color.RGBToHSV(c, out float h, out float s, out float v);
            h += deltaHue;
            h %= 1.0f;
            if (h < 0) {
                h += 1.0f;
            }
            Color d = Color.HSVToRGB(h, s, v);
            return new Color(d.r, d.g, d.b, c.a);
        }
    }
}
