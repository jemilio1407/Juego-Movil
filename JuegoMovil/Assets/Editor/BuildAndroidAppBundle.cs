using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildAndroidAppBundle
{
    [MenuItem("Build/Build Hoover Hero Android AAB")]
    public static void Build()
    {
        const string output = "Builds/HooverHero-PlayStore-v2.aab";
        Directory.CreateDirectory("Builds");

        EditorUserBuildSettings.buildAppBundle = true;
        var options = new BuildPlayerOptions
        {
            scenes = new[]
            {
                "Assets/Scenes/MainMenu.unity",
                "Assets/Scenes/Gameplay.unity"
            },
            locationPathName = output,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        Debug.Log("Starting signed Android App Bundle build: " + output);
        BuildReport report = BuildPipeline.BuildPlayer(options);
        Debug.Log("Android AAB build result: " + report.summary.result + "; errors: " + report.summary.totalErrors);
        if (report.summary.result == BuildResult.Succeeded && File.Exists(output))
        {
            Debug.Log("AAB_READY=" + Path.GetFullPath(output) + "; bytes=" + new FileInfo(output).Length);
            Debug.Log("Hoover Hero Android AAB build completed successfully.");
        }
        else
        {
            Debug.LogError("Android AAB build failed. See the build report and Editor.log.");
        }
    }
}
