# HSLuv-Unity

This is a C# implementation of [HSLuv rev.4](http://www.hsluv.org/) for Unity 2021.3+.

Note that there's an official repository of generic C# implementation.  https://github.com/hsluv/hsluv-csharp


## Using HSLuv-Unity

1. Add "Mathematics" package to your project.
2. Put `Assets/Hsluv/Runtime/Hsluv.cs` into your project.

It contains the following [standard HSLuv functions](https://www.hsluv.org/implementations/).

```C#
namespace Hsluv {
    public static class Hsluv {
        public static float3 HsluvToRgb(float3 v);
        public static float3 RgbToHsluv(float3 v);
        public static float3 HpluvToRgb(float3 v);
        public static float3 RgbToHpluv(float3 v);

        public static Vector3 HsluvToRgb(Vector3 v);
        public static Vector3 RgbToHsluv(Vector3 v);
        public static Vector3 HpluvToRgb(Vector3 v);
        public static Vector3 RgbToHpluv(Vector3 v);
    }
}
```

See `Assets/Hsluv/Samples~/` for details of other utility functions.
