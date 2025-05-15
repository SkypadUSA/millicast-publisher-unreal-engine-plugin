// Copyright Dolby.io 2023. All Rights Reserved.

using System.IO;

namespace UnrealBuildTool.Rules
{
    using System;
    using System.IO;

    public class MillicastPublisher: ModuleRules
	{
		public MillicastPublisher(ReadOnlyTargetRules Target) : base(Target)
		{
			PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

            bool bUseMillicastWebRTC = true;

            try
            {
                String modulePath = GetModuleDirectory("MillicastWebRTC"); // This throw an exception if the module does not exists
                bUseMillicastWebRTC = !modulePath.Equals("");
            }
            catch (Exception)
            {
                bUseMillicastWebRTC = false;
            }

            if (bUseMillicastWebRTC)
            {
                Console.WriteLine("The plugin will link against MillicastWebRTC");
                PublicDependencyModuleNames.AddRange(new string[] { "MillicastWebRTC" });
                PublicDefinitions.Add("WITH_MILLICAST_WEBRTC=1");
            }
            else
            {
                Console.WriteLine("The plugin will link against UnrealEngine WebRTC module");
                PublicDependencyModuleNames.AddRange(new string[] { "WebRTC" });
                PublicDefinitions.Add("WEBRTC_VERSION=96");
            }

            DynamicallyLoadedModuleNames.AddRange(
			new string[] {
				"Media",
			});

			PublicDependencyModuleNames.AddRange(
				new string[] {
					"AudioCaptureCore",
					"Core",
					"CoreUObject",
					"Engine",
					"MediaAssets",
					"OpenSSL",
					"RenderCore",
					"TimeManagement"
                });

			PrivateDependencyModuleNames.AddRange(
				new string[] {
					"AudioPlatformConfiguration",
					"AudioMixer",
					"CinematicCamera",
					"HeadMountedDisplay",
					"HTTP",
					"InputCore",
					"Json",
					"libOpus",
					"MediaUtils",
					"MediaIOCore",
					"Projects",
					"Slate",
					"SlateCore",
					"Renderer",
					"RHI",
					"SSL",
					"WebSockets",
					"HeadMountedDisplay",
					"CinematicCamera",
					"InputCore",
					"libOpus",
					"AudioPlatformConfiguration"
				});

			if(Target.Platform.ToString() == "Android" || Target.Platform.ToString() == "IOS" || Target.Platform.ToString() == "Mac")
			{	
				PublicDefinitions.Add("WITH_AVENCODER=0");
			}
			else 
			{
				PublicDefinitions.Add("WITH_AVENCODER=1");
				// required for casting UE4 BackBuffer to Vulkan Texture2D for NvEnc
				PrivateDependencyModuleNames.AddRange(new string[] { "AVEncoder", "CUDA", "VulkanRHI", "nvEncode" });
				AddEngineThirdPartyPrivateStaticDependencies(Target, "NVAftermath");
			}

            if (Target.Platform.ToString() == "Win64" || bUseMillicastWebRTC)
            {
                PublicDefinitions.Add("WITH_SCREENCAPTURER=1");
            }
            else
            {
                PublicDefinitions.Add("WITH_SCREENCAPTURER=0");
            }

            if (ReadOnlyBuildVersion.Current.MajorVersion >= 5)
			{
				PrivateDependencyModuleNames.AddRange(new string[] {
					"RHICore"
				});
			}
			else
			{
				CppStandard = CppStandardVersion.Cpp17;
			}

			PrivateIncludePathModuleNames.AddRange(
				new string[] {
					"Media",
				});

			var EngineDir = Path.GetFullPath(Target.RelativeEnginePath);

			AddEngineThirdPartyPrivateStaticDependencies(Target, "Vulkan");
			
			PrivateIncludePathModuleNames.Add("VulkanRHI");
			PrivateIncludePaths.Add(Path.Combine(EngineDir, "Source/Runtime/VulkanRHI/Private"));
			AddEngineThirdPartyPrivateStaticDependencies(Target, "Vulkan");

			if (Target.IsInPlatformGroup(UnrealPlatformGroup.Windows))
			{
				PublicDependencyModuleNames.Add("D3D11RHI");
				PublicDependencyModuleNames.Add("D3D12RHI");
				PrivateIncludePaths.AddRange(
					new string[]{
						Path.Combine(EngineDir, "Source/Runtime/D3D12RHI/Private"),
						Path.Combine(EngineDir, "Source/Runtime/D3D12RHI/Private/Windows")
					});

#if UE_5_2_OR_LATER
		        AddEngineThirdPartyPrivateStaticDependencies(Target, "DX11", "DX12");
				PrivateIncludePaths.AddRange(new string[] {
					"MillicastPublisher/Private",
                    Path.Combine(Path.GetFullPath(Target.RelativeEnginePath), "Source/ThirdParty/WebRTC/4664/Include/third_party/libyuv/include"), // for libyuv headers
				});
#else
                AddEngineThirdPartyPrivateStaticDependencies(Target, "DX12");
				PublicSystemLibraries.AddRange(new string[] {
					"DXGI.lib",
					"d3d11.lib",
				});

                PrivateIncludePaths.AddRange(new string[] {
					"MillicastPublisher/Private",
                    Path.Combine(Path.GetFullPath(Target.RelativeEnginePath), "Source/ThirdParty/WebRTC/4147/Include/third_party/libyuv/include"), // for libyuv headers
				});
#endif

                PrivateIncludePaths.Add(Path.Combine(EngineDir, "Source/Runtime/VulkanRHI/Private/Windows"));
			}
			else if (Target.IsInPlatformGroup(UnrealPlatformGroup.Linux))
			{
				PrivateIncludePaths.Add(Path.Combine(EngineDir, "Source/Runtime/VulkanRHI/Private/Linux"));
			}

#if UE_5_2_OR_LATER
		    PublicDefinitions.Add("WEBRTC_VERSION=96");
		    PublicDefinitions.Add("INTEL_EXTENSIONS=0");
#endif

#if UE_5_3_OR_LATER
            PublicDefinitions.Add("WITH_NVAPI=0");
#endif
        }
    }
}
