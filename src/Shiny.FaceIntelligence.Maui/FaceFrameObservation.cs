using Microsoft.Maui.Graphics;

namespace Shiny.FaceIntelligence.Maui;

public enum FaceFrameQuality { Unknown, Usable, TooDark, TooBright, Blurry }

/// <summary>Immutable frame identity assigned before conversion, detection or asynchronous recognition.</summary>
public sealed record FaceFrameStamp(Guid SessionId, long FrameId, long Timestamp, DateTime CapturedAtUtc);
public sealed record FaceFrameObservation(FaceFrameStamp Stamp, int FaceCount, RectF? Bounds,
    bool InsideScanWindow, float? Confidence, float? Brightness, float? Sharpness,
    FaceFrameQuality Quality, byte[]? ImageData, int ImageWidth, int ImageHeight, string? Error = null);

public interface IFaceRecognitionAnalyzer
{
    Guid SessionId { get; }
    event EventHandler<FaceFrameObservation>? FrameObserved;
    event EventHandler<FaceRecognizedEventArgs>? FaceRecognized;
    void BeginSession();
    Task<bool> StopSessionAsync(TimeSpan timeout);
}
