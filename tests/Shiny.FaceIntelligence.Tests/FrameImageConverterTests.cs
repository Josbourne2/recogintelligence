using Shiny.FaceIntelligence.Maui;
using SkiaSharp;
using Xunit;

namespace Shiny.FaceIntelligence.Tests
{
    public class FrameImageConverterTests
    {
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
namespace Shiny.Controls.Camera { public sealed class CameraFrame; }
namespace Shiny.FaceIntelligence.Maui
{
    sealed partial class FrameImageConverter
    {
        public partial SKBitmap? ToUpright(Shiny.Controls.Camera.CameraFrame frame, int maxWidth) => null;
    }
}
