#include "MillicastScreenCapturerActor.h"

#include "Components/MillicastScreenCapturerComponent.h"

AMillicastScreenCapturerActor::AMillicastScreenCapturerActor(const FObjectInitializer& ObjectInitializer) : Super(ObjectInitializer)
{
	ScreenCapturer = ObjectInitializer.CreateDefaultSubobject<UMillicastScreenCapturerComponent>(
		this,
		TEXT("MillicastScreenCapturerComponent")
	);
	// ScreenCapturer->AttachToComponent(RootComponent, FAttachmentTransformRules::KeepRelativeTransform);
}
