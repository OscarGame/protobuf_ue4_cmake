// Copyright 1998-2017 Epic Games, Inc. All Rights Reserved.

using System.IO;
using UnrealBuildTool;
using Tools.DotNETCommon;

public class MyProj : ModuleRules
{
    RulesAssembly r;

    private string ModulePath
    {
        get { return Path.GetDirectoryName(r.GetModuleFileName(this.GetType().Name).CanonicalName); }
    }

    private string ThirdPartyPath
    {
        get { return Path.GetFullPath(Path.Combine(ModulePath, "../../ThirdParty/")); }
    }

    private string ProtocBuildPath
    {
        get { return Path.GetFullPath(Path.Combine(ModulePath, "../../ThirdParty/Protobuf/Include/")); }
    }

    public MyProj(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore"});

        FileReference CheckProjectFile;
        UProjectInfo.TryGetProjectForTarget("MyProj", out CheckProjectFile);
        r = RulesCompiler.CreateProjectRulesAssembly(CheckProjectFile);

        LoadProtobuf(Target);
    }

    public bool LoadProtobuf(ReadOnlyTargetRules Target)
    {
        PublicIncludePaths.Add(ProtocBuildPath);

        //Library path
        string PlatformString = null;
        switch (Target.Platform)
        {
            case UnrealTargetPlatform.Win64:
                {
                    switch (Target.Configuration)
                    {
                        case UnrealTargetConfiguration.Debug:
                        case UnrealTargetConfiguration.DebugGame:
                            {
                                //UE4��Debugģʽ���ӵ�������ʱ����Ȼʹ�õ���Release����������û��ʹ��debug�汾lib
                                PlatformString = ".lib";
                                break;
                            }
                        case UnrealTargetConfiguration.Development:
                        case UnrealTargetConfiguration.Shipping:
                            {
                                PlatformString = ".lib";
                                break;
                            }
                    }

                    break;
                }
        }

        if (null != PlatformString)
        {
            string LibrariesPath = Path.Combine(ThirdPartyPath, "Protobuf", "Library", "libprotobuf-lite" + PlatformString);
            PublicAdditionalLibraries.Add(LibrariesPath);

            string MyLibPath = Path.Combine(ThirdPartyPath, "Protobuf", "Library", "libtest" + PlatformString);
            PublicAdditionalLibraries.Add(MyLibPath);

            System.Console.WriteLine("++++++++++++ Set Protobuf Libraries: " + LibrariesPath + "\r");
        }

        //Include path
        string IncludePath = Path.Combine(ThirdPartyPath, "Protobuf", "Include");
        PublicIncludePaths.Add(IncludePath);

        System.Console.WriteLine("++++++++++++ Set Protobuf Includes: " + IncludePath + "\r");

        return true;
    }
}
