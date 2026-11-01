using Shiny.FaceIntelligence.Maui;
using SkiaSharp;
using Xunit;

namespace Shiny.FaceIntelligence.Tests
{
    public class FrameImageConverterTests
    {
        // CameraFrame's contract is clockwise rotation, followed by a horizontal
        // mirror in upright space. Unique pixel labels catch orientation errors
        // that dimension-only tests and symmetric face images cannot detect.
        [Theory]
        [InlineData(0, false, "ABC", "DEF", null)]
        [InlineData(0, true, "CBA", "FED", null)]
        [InlineData(90, false, "DA", "EB", "FC")]
        [InlineData(90, true, "AD", "BE", "CF")]
        [InlineData(180, false, "FED", "CBA", null)]
        [InlineData(180, true, "DEF", "ABC", null)]
        [InlineData(270, false, "CF", "BE", "AD")]
        [InlineData(270, true, "FC", "EB", "DA")]
        public void Orient_RotatesThenMirrorsInUprightSpace(
            int rotation, bool mirrored, string first, string second, string? third)
        {
            var source = new SKBitmap(3, 2);
            var inputRows = new[] { "ABC", "DEF" };
            for (var y = 0; y < source.Height; y++)
                for (var x = 0; x < source.Width; x++)
                    source.SetPixel(x, y, Pixel(inputRows[y][x]));

            // Orient owns the source and may return it unchanged.
            using var result = FrameImageConverter.Orient(source, rotation, mirrored);
            var expectedRows = third is null ? [first, second] : new[] { first, second, third };
            Assert.Equal(expectedRows.Length, result.Height);
            Assert.Equal(first.Length, result.Width);
            for (var y = 0; y < result.Height; y++)
                for (var x = 0; x < result.Width; x++)
                    Assert.Equal(Pixel(expectedRows[y][x]), result.GetPixel(x, y));
        }

        private static SKColor Pixel(char label) => new((byte)label, 20, 40, 255);

        [Theory]
        [InlineData(1280, 720, 0, 720, 405)]
        [InlineData(720, 1280, 90, 720, 405)]
        [InlineData(320, 240, 0, 320, 240)]
        public void UprightLimit_RespectsWidthAspectAndNeverUpscales(int w, int h, int rotation, int expectedW, int expectedH)
        {
            var source = new SKBitmap(w, h);
            using var result = FrameImageConverter.LimitWidth(FrameImageConverter.Orient(source, rotation, true), 720);
            Assert.Equal(expectedW, result.Width);
            Assert.Equal(expectedH, result.Height);
        }
    }
}

// The linked production file's native entry point is not under test here. Only its shared
// bitmap operations run; Android YUV conversion still requires the platform verification.
namespace Shiny.Controls.Camera { public sealed class CameraFrame { public SkiaSharp.SKBitmap? Bitmap { get; init; } } }
namespace Shiny.FaceIntelligence.Maui
{
    sealed partial class FrameImageConverter
    {
        public partial SKBitmap? ToUpright(Shiny.Controls.Camera.CameraFrame frame, int maxWidth) => frame.Bitmap?.Copy();
    }
}
