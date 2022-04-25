# HSLuv-Unity

This is a C# implementation of [HSLuv rev.4](http://www.hsluv.org/) for Unity.

Note that there's an official repository of generic C# implementation.  https://github.com/hsluv/hsluv-csharp


## Using HSLuv-Unity

Put `Assets/Hsluv/Runtime/Hsluv.cs` into your project.  It contains the following [standard HSLuv functions](https://www.hsluv.org/implementations/).

```C#
namespace Hsluv {
    public static class Hsluv {
        public static Vector3 HsluvToRgb(Vector3 v);
        public static Vector3 RgbToHsluv(Vector3 v);
        public static Vector3 HpluvToRgb(Vector3 v);
        public static Vector3 RgbToHpluv(Vector3 v);
    }
}
```


See `Assets/Hsluv/Samples~/` for details of other utility functions.
