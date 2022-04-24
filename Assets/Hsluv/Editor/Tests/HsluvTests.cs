// HSLuv rev4 library for Unity.
// SPDX-FileCopyrightText: Copyright (c) Takayuki Matsuoka
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Hsluv {
    internal static class HsluvTest {
        [MenuItem("Help/HSLuv/Test")]
        private static void Test()
        {
            const string               snapshotPath    = "Assets/Hsluv/Editor/Tests/snapshots~/snapshot-rev4.json";
            string                     snapshotJson    = System.IO.File.ReadAllText(snapshotPath);
            IEnumerable<SnapshotEntry> snapshotEntries = DeserializeSnapshotEntries(snapshotJson);

            var       errorCount           = 0;
            var       successCount         = 0;
            var       testCount            = 0;
            const int expectedSuccessCount = 4096;
            const int expectedTestCount    = 4096;

            void LogInfo(string s)  => Debug.Log($"{s}");
            void LogError(string s) => Debug.LogError($"{s}");

            foreach (SnapshotEntry snapshot in snapshotEntries) {
                var        actualResult = new ActualResult(snapshot);
                TestResult testResult   = Compare(snapshot, actualResult);
                if (testResult.Ok) {
                    successCount += 1;
                } else {
                    errorCount += 1;
                    GenerateErrorText(LogError, snapshot, actualResult, testResult);
                }
                testCount += 1;
            }

            if (successCount == expectedSuccessCount && testCount == expectedTestCount) {
                LogInfo($"OK");
            } else {
                LogError($"NG : error={errorCount}, success={successCount}/{expectedSuccessCount}");
            }
        }

        private static void GenerateErrorText(
            Action<string> logError,
            SnapshotEntry snapshot,
            ActualResult actualResult,
            TestResult testResult)
        {
            var sb = new StringBuilder();
            sb.Append($"entry \"{snapshot.HexString}\"");
            if (!testResult.RgbFromHex) {
                sb.Append($"NG: rgbFromHex   : expected={snapshot.RGB}, actual={actualResult.RgbFromHex}\n");
            }
            if (!testResult.XyzFromRgb) {
                sb.Append($"NG: xyzFromRgb   : expected={snapshot.XYZ}, actual={actualResult.XyzFromRgb}\n");
            }
            if (!testResult.LuvFromXyz) {
                sb.Append($"NG: luvFromXyz   : expected={snapshot.Luv}, actual={actualResult.LuvFromXyz}n");
            }
            if (!testResult.LchFromLuv) {
                sb.Append($"NG: lchFromLuv   : expected={snapshot.Lch}, actual={actualResult.LchFromLuv}n");
            }
            if (!testResult.HsluvFromLch) {
                sb.Append($"NG: hsluvFromLch : expected={snapshot.Hsluv}, actual={actualResult.HsluvFromLch}n");
            }
            if (!testResult.HpluvFromLch) {
                sb.Append($"NG: hpluvFromLch : expected={snapshot.Hpluv}, actual={actualResult.HpluvFromLch}n");
            }
            if (!testResult.HsluvFromHex) {
                sb.Append($"NG: hsluvFromHex : expected={snapshot.Hsluv}, actual={actualResult.HsluvFromHex}n");
            }
            if (!testResult.HpluvFromHex) {
                sb.Append($"NG: hpluvFromHex : expected={snapshot.Hpluv}, actual={actualResult.HpluvFromHex}n");
            }
            if (!testResult.LchFromHsluv) {
                sb.Append($"NG: lchFromHsluv : expected={snapshot.Lch}, actual={actualResult.LchFromHsluv}n");
            }
            if (!testResult.LchFromHpluv) {
                sb.Append($"NG: lchFromHpluv : expected={snapshot.Lch}, actual={actualResult.LchFromHpluv}n");
            }
            if (!testResult.LuvFromLch) {
                sb.Append($"NG: luvFromLch   : expected={snapshot.Luv}, actual={actualResult.LuvFromLch}n");
            }
            if (!testResult.XyzFromLuv) {
                sb.Append($"NG: xyzFromLuv   : expected={snapshot.XYZ}, actual={actualResult.XyzFromLuv}n");
            }
            if (!testResult.RgbFromXyz) {
                sb.Append($"NG: rgbFromXyz   : expected={snapshot.RGB}, actual={actualResult.RgbFromXyz}n");
            }
            if (!testResult.HexFromRgb) {
                sb.Append($"NG: hexFromRgb   : expected={snapshot.HexString}, actual={actualResult.HexFromRgb}n");
            }
            if (!testResult.HexFromHsluv) {
                sb.Append($"NG: hexFromHsluv : expected={snapshot.HexString}, actual={actualResult.HexFromHsluv}n");
            }
            if (!testResult.HexFromHpluv) {
                sb.Append($"NG: hexFromHpluv : expected={snapshot.HexString}, actual={actualResult.HexFromHpluv}n");
            }
            logError(sb.ToString());
        }

        private static IEnumerable<SnapshotEntry> DeserializeSnapshotEntries(string snapshotJsonString)
            => MicroJson.Deserialize(snapshotJsonString) is Dictionary<string, object> snapshot
                ? snapshot.Select(kv => new SnapshotEntry(kv)).ToList()
                : default(IEnumerable<SnapshotEntry>);

        private struct SnapshotEntry {
            public readonly string  HexString; // "#RRGGBB" in sRGB color space
            public readonly Double3 Lch;
            public readonly Double3 Luv;
            public readonly Double3 RGB; // sRGB
            public readonly Double3 XYZ;
            public readonly Double3 Hpluv;
            public readonly Double3 Hsluv;

            public SnapshotEntry(KeyValuePair<string, object> kv)
            {
                if (kv.Value is Dictionary<string, object> values) {
                    Double3 GetDouble3(IList<object> o) => new Double3((double)o[0], (double)o[1], (double)o[2]);

                    HexString = kv.Key;
                    // new Double3(Hsluv.HexToSrgb(kv.Key));
                    Lch   = GetDouble3(values["lch"] as IList<object>);
                    Luv   = GetDouble3(values["luv"] as IList<object>);
                    RGB   = GetDouble3(values["rgb"] as IList<object>);
                    XYZ   = GetDouble3(values["xyz"] as IList<object>);
                    Hpluv = GetDouble3(values["hpluv"] as IList<object>);
                    Hsluv = GetDouble3(values["hsluv"] as IList<object>);
                } else {
                    HexString = default;
                    Lch       = default;
                    Luv       = default;
                    RGB       = default;
                    XYZ       = default;
                    Hpluv     = default;
                    Hsluv     = default;
                }
            }
        }

        private struct ActualResult {
            // forward functions
            public readonly Vector3 RgbFromHex;
            public readonly Vector3 XyzFromRgb;
            public readonly Vector3 LuvFromXyz;
            public readonly Vector3 LchFromLuv;
            public readonly Vector3 HsluvFromLch;
            public readonly Vector3 HpluvFromLch;
            public readonly Vector3 HsluvFromHex;
            public readonly Vector3 HpluvFromHex;

            // backward functions
            public readonly Vector3 LchFromHsluv;
            public readonly Vector3 LchFromHpluv;
            public readonly Vector3 LuvFromLch;
            public readonly Vector3 XyzFromLuv;
            public readonly Vector3 RgbFromXyz;
            public readonly string  HexFromRgb;
            public readonly string  HexFromHsluv;
            public readonly string  HexFromHpluv;

            public ActualResult(SnapshotEntry snapshot)
            {
                // forward functions
                RgbFromHex   = Hsluv.HexToSrgb(snapshot.HexString);
                XyzFromRgb   = Hsluv.SrgbToXyz(snapshot.RGB.ToVector3());
                LuvFromXyz   = Hsluv.XyzToLuv(snapshot.XYZ.ToVector3());
                LchFromLuv   = Hsluv.LuvToLch(snapshot.Luv.ToVector3());
                HsluvFromLch = Hsluv.LchToHsluv(snapshot.Lch.ToVector3());
                HpluvFromLch = Hsluv.LchToHpluv(snapshot.Lch.ToVector3());
                HsluvFromHex = Hsluv.SrgbToHsluv(RgbFromHex);
                HpluvFromHex = Hsluv.SrgbToHpluv(RgbFromHex);

                // backward functions
                LchFromHsluv = Hsluv.HsluvToLch(snapshot.Hsluv.ToVector3());
                LchFromHpluv = Hsluv.HpluvToLch(snapshot.Hpluv.ToVector3());
                LuvFromLch   = Hsluv.LchToLuv(snapshot.Lch.ToVector3());
                XyzFromLuv   = Hsluv.LuvToXyz(snapshot.Luv.ToVector3());
                RgbFromXyz   = Hsluv.XyzToSrgb(snapshot.XYZ.ToVector3());
                HexFromRgb   = Hsluv.RgbToHex(snapshot.RGB.ToVector3()); // #RRGGBB
                HexFromHsluv = Hsluv.RgbToHex(Hsluv.HsluvToSrgb(snapshot.Hsluv.ToVector3()));
                HexFromHpluv = Hsluv.RgbToHex(Hsluv.HpluvToSrgb(snapshot.Hpluv.ToVector3()));
            }
        }

        private struct TestResult {
            public bool Ok;
            public bool RgbFromHex;
            public bool XyzFromRgb;
            public bool LuvFromXyz;
            public bool LchFromLuv;
            public bool HsluvFromLch;
            public bool HpluvFromLch;
            public bool HsluvFromHex;
            public bool HpluvFromHex;
            public bool LchFromHsluv;
            public bool LchFromHpluv;
            public bool LuvFromLch;
            public bool XyzFromLuv;
            public bool RgbFromXyz;
            public bool HexFromRgb;
            public bool HexFromHsluv;
            public bool HexFromHpluv;
        }

        private static TestResult Compare(SnapshotEntry snapshot, ActualResult result)
        {
            bool AlmostEquals(Double3 s, Vector3 actual, float range)
                => AlmostEqualsDd(s.X, actual.x, range) &&
                   AlmostEqualsDd(s.Y, actual.y, range) &&
                   AlmostEqualsDd(s.Z, actual.z, range);

            bool AlmostEqualsString(string s, string actual)
            {
                Vector3 snapshotSrgb = Hsluv.HexToSrgb(s);
                Vector3 actualSrgb   = Hsluv.HexToSrgb(actual);
                return AlmostEqualsDd(snapshotSrgb.x, actualSrgb.x, 255.0f) &&
                       AlmostEqualsDd(snapshotSrgb.y, actualSrgb.y, 255.0f) &&
                       AlmostEqualsDd(snapshotSrgb.z, actualSrgb.z, 255.0f);
            }

            bool AlmostEqualsDd(double s, double actual, double range)
                => Mathf.Abs((float)(actual - s)) < (range / 4096.0f);

            TestResult r;
            bool       ok        = true;
            ok &= r.RgbFromHex   = AlmostEquals(snapshot.RGB,   result.RgbFromHex,   1.0f);
            ok &= r.XyzFromRgb   = AlmostEquals(snapshot.XYZ,   result.XyzFromRgb,   1.0f);
            ok &= r.LuvFromXyz   = AlmostEquals(snapshot.Luv,   result.LuvFromXyz,   100.0f);
            ok &= r.LchFromLuv   = AlmostEquals(snapshot.Lch,   result.LchFromLuv,   100.0f);
            ok &= r.HsluvFromLch = AlmostEquals(snapshot.Hsluv, result.HsluvFromLch, 100.0f);
            ok &= r.HpluvFromLch = AlmostEquals(snapshot.Hpluv, result.HpluvFromLch, 100.0f);
            ok &= r.HsluvFromHex = AlmostEquals(snapshot.Hsluv, result.HsluvFromHex, 100.0f);
            ok &= r.HpluvFromHex = AlmostEquals(snapshot.Hpluv, result.HpluvFromHex, 100.0f);
            ok &= r.LchFromHsluv = AlmostEquals(snapshot.Lch,   result.LchFromHsluv, 100.0f);
            ok &= r.LchFromHpluv = AlmostEquals(snapshot.Lch,   result.LchFromHpluv, 100.0f);
            ok &= r.LuvFromLch   = AlmostEquals(snapshot.Luv,   result.LuvFromLch,   100.0f);
            ok &= r.XyzFromLuv   = AlmostEquals(snapshot.XYZ,   result.XyzFromLuv,   1.0f);
            ok &= r.RgbFromXyz   = AlmostEquals(snapshot.RGB,   result.RgbFromXyz,   1.0f);
            ok &= r.HexFromRgb   = AlmostEqualsString(snapshot.HexString, result.HexFromRgb);
            ok &= r.HexFromHsluv = AlmostEqualsString(snapshot.HexString, result.HexFromHsluv);
            ok &= r.HexFromHpluv = AlmostEqualsString(snapshot.HexString, result.HexFromHpluv);

            r.Ok = ok;
            return r;
        }

        private readonly struct Double3 {
            public readonly double X;
            public readonly double Y;
            public readonly double Z;

            public Double3(double x, double y, double z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }

            public          Vector3 ToVector3() => new Vector3((float)X, (float)Y, (float)Z);
            public override string  ToString()  => $"({X:R},{Y:R},{Z:R})";
        }
    }
}
