// Copyright Dolby.io 2023. All Rights Reserved.

#pragma once

#include "WebRTCInc.h"
#if WITH_AVENCODER
#include "AVEncoderContext.h"
#endif
/** Video Source adapter to create webrtc video frame from a Texture 2D and push it into webrtc pipelines */
namespace Millicast::Publisher
{
	class FTexture2DVideoSourceAdapter : public rtc::AdaptedVideoTrackSource
	{
	public:
		void OnFrameReady(const FTexture2DRHIRef& FrameBuffer);

		// rtc::AdaptedVideoTrackSource
		webrtc::MediaSourceInterface::SourceState state() const override { return webrtc::MediaSourceInterface::kLive; }
		absl::optional<bool> needs_denoising() const override { return false; }
		bool is_screencast() const override { return false; }
		bool remote() const override { return false; }
		// ~rtc::AdaptedVideoTrackSource

		void SetSimulcast(bool InSimulcast) { Simulcast = InSimulcast; }
		
	private:
		bool AdaptVideoFrame(int64 TimestampUs, FIntPoint Resolution);
		void TryInitializeCaptureContexts(const FTexture2DRHIRef& FrameBuffer);
#if WITH_AVENCODER

		TArray<TUniquePtr<FAVEncoderContext>> CaptureContexts;
#endif
		bool Simulcast = false;
	};
}
