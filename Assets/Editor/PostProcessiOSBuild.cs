using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Text.RegularExpressions;

public class PostProcessiOSBuild {
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {
        if (target != BuildTarget.iOS) return;
        
        var projPath = Path.Combine(pathToBuiltProject, "Unity-iPhone.xcodeproj/project.pbxproj");
        if (File.Exists(projPath)) {
            var text = File.ReadAllText(projPath);
            
            text = Regex.Replace(text,
                @"IPHONEOS_DEPLOYMENT_TARGET = [0-9.]+;",
                "IPHONEOS_DEPLOYMENT_TARGET = 13.0;");

            File.WriteAllText(projPath, text);
            UnityEngine.Debug.Log("✅ Fixed iOS deployment target to 13.0 in project.pbxproj");
        }
        
        var storyboardPath = Path.Combine(pathToBuiltProject, "LaunchScreen-iPhone.storyboard");
        if (File.Exists(storyboardPath)) {
            var text = File.ReadAllText(storyboardPath);
            
            text = Regex.Replace(text,
                @"toolsVersion=""[0-9.]+"" systemVersion=""[0-9.]+""",
                @"toolsVersion=""0.0"" systemVersion=""0.0""");

            text = Regex.Replace(text,
                @"targetRuntime=""iOS[0-9.]+""",
                @"targetRuntime=""iOS""");

            File.WriteAllText(storyboardPath, text);
            UnityEngine.Debug.Log("✅ Cleaned LaunchScreen.storyboard from hardcoded iOS SDK version");
        }
    }
}