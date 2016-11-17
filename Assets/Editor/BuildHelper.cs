using UnityEditor;
using System.Collections;

// Libraries for Zip
using Ionic.Zip;

public class BuildHelperEditor {

	private static string[] DELETE_FILES_LIST = new string[]{"*.pdb"};


	static System.Collections.Generic.List<string> GetAllSelectedScenes()
	{
		System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
		foreach(var s in  EditorBuildSettings.scenes)
		{
			if(s.enabled)
			{
				list.Add(s.path);
			}
		}

		return list;
	}

	static string GetExtension( BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.StandaloneWindows : return ".exe";
		case BuildTarget.StandaloneWindows64 : return ".exe";
		case BuildTarget.Android : return ".apk";
		case BuildTarget.StandaloneOSXIntel : return ".app";
		case BuildTarget.StandaloneOSXIntel64 : return ".app";
		case BuildTarget.StandaloneOSXUniversal : return ".app";
		case BuildTarget.StandaloneLinux : return ".x86";
		case BuildTarget.StandaloneLinux64 : return ".x86_64";
		case BuildTarget.StandaloneLinuxUniversal : return "";
		}
		return "";
	}

	[MenuItem ("Build/Build/Zip Builds",true,-10)]
	public static bool ToogleZipValidate()
	{
		Menu.SetChecked("Build/Build/Zip Builds",EditorPrefs.GetBool("BuildHelperEditor_ZIP",false));
		return true;
	}
	[MenuItem ("Build/Build/Zip Builds",false,-10)]
	public static void ToogleZip()
	{
		EditorPrefs.SetBool("BuildHelperEditor_ZIP", !EditorPrefs.GetBool("BuildHelperEditor_ZIP",false));
	}


	[MenuItem ("Build/Build/Windows 32_64bits and OSXIntel64")]
	public static void Build_Win32_Win64_OSX()
	{
		string path = EditorUtility.SaveFolderPanel("Build Path for OSX64, Windows 32 and 64bits",EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget),"");
		if(string.IsNullOrEmpty(path))
		{
			return;
		}
		string path32 = path +"/Win_32bits";
		string path64 = path +"/Win_64bits";
		string pathOSXIntel64 = path +"/OSX_64bits";

		BuildHelper(path32,BuildTarget.StandaloneWindows);

		BuildHelper(path64,BuildTarget.StandaloneWindows64);

		BuildHelper(pathOSXIntel64,BuildTarget.StandaloneOSXIntel64);

		if(EditorPrefs.GetBool("BuildHelperEditor_ZIP",false))
		{
			string zipPath = path+"/_ZIP/";

			ZipFiles(path32,zipPath,PlayerSettings.productName+"_Win32");
			ZipFiles(path64,zipPath,PlayerSettings.productName+"_Win64");
			ZipFiles(pathOSXIntel64,zipPath,PlayerSettings.productName+"_OSX64");
		}

	}


	[MenuItem ("Build/Build/Windows 32_64bits, OSXIntel64, Linux32")]
	public static void Build_Win32_Win64_OSX_Linux32()
	{
		string path = EditorUtility.SaveFolderPanel("Build Path for OSX64, linux32, Windows 32 and 64bits",EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget),"");
		if(string.IsNullOrEmpty(path))
		{
			return;
		}
		string path32 = path +"/Win_32bits";
		string path64 = path +"/Win_64bits";
		string pathOSXIntel64 = path +"/OSX_64bits";
		string pathLinux32 = path +"/Linux32";

		BuildHelper(path32,BuildTarget.StandaloneWindows);

		BuildHelper(path64,BuildTarget.StandaloneWindows64);

		BuildHelper(pathOSXIntel64,BuildTarget.StandaloneOSXIntel64);

		BuildHelper(pathLinux32,BuildTarget.StandaloneLinux);

		if(EditorPrefs.GetBool("BuildHelperEditor_ZIP",false))
		{
			string zipPath = path+"/_ZIP/";

			ZipFiles(path32,zipPath,PlayerSettings.productName+"_Win32");
			ZipFiles(path64,zipPath,PlayerSettings.productName+"_Win64");
			ZipFiles(pathOSXIntel64,zipPath,PlayerSettings.productName+"_OSX64");
			ZipFiles(pathLinux32,zipPath,PlayerSettings.productName+"_Linux32");
		}
	}

	private static void BuildHelper(string path, BuildTarget target)
	{
		if(!System.IO.Directory.Exists(path))
			System.IO.Directory.CreateDirectory(path);
		
		string[] levels =GetAllSelectedScenes().ToArray();
		BuildPipeline.BuildPlayer( levels, path + "/" + PlayerSettings.productName +GetExtension(target) ,target, BuildOptions.None);

		foreach(string filter in DELETE_FILES_LIST)
		{
			var files = new System.IO.DirectoryInfo(path).GetFiles(filter);
			foreach(var f in files)
			{
				//Debug.Log("Deleting file after build : "+f.FullName);
				f.Delete();
			}
		}
	}



	private static string compressingFileName = "";
	private static void ZipFiles(string buidPath, string folderDestination, string fileName)
	{

		// Create "_ZIP" Directory if not exist
		if (!System.IO.Directory.Exists(folderDestination)) {
			System.IO.Directory.CreateDirectory(folderDestination);
			//Debug.LogFormat("The directory was created successfully at {0}.", Directory.GetCreationTime(folderDestination));
		}

		// Check if Build exist
		if (System.IO.Directory.Exists(buidPath)) {

			compressingFileName = fileName;

			EditorUtility.DisplayProgressBar("Compressing Files: " + compressingFileName, "Compressing Files", 0f);

			ZipFile zip;
			zip = new ZipFile();

			//zip.Comment = "Bowling Game";
			zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Default;
			zip.SaveProgress += SaveProgress;
			zip.AddDirectory(buidPath);
			zip.Save(folderDestination + "/" + fileName + ".zip");

			EditorUtility.ClearProgressBar();

		} else {
			UnityEngine.Debug.Log(buidPath + " - Don't have Build");
		}
	}

	private static float progressTotal = 0;
	private static void SaveProgress(object sender, SaveProgressEventArgs e)
	{
		string info = "Compressing Files: ";
		if (e.EventType == ZipProgressEventType.Saving_Started) {
			//Debug.Log("Begin Saving: " + e.ArchiveName);
		} else if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry) {
			progressTotal = (float)e.EntriesSaved / (float)e.EntriesTotal;
			info += (e.EntriesSaved + 1) + "/" + e.EntriesTotal;

		} else if (e.EventType == ZipProgressEventType.Saving_EntryBytesRead) {
			info += " " + e.CurrentEntry.FileName + "(";
			info += (e.BytesTransferred * 100) + "/" + e.TotalBytesToTransfer;
			info += ")";
			//progressBar1.Value = (int)((e.BytesTransferred * 100) / e.TotalBytesToTransfer);
		} else if (e.EventType == ZipProgressEventType.Saving_Completed) {
			progressTotal = 1;
		}

		if (EditorUtility.DisplayCancelableProgressBar("Compressing Files: " + compressingFileName, info, progressTotal)) {
			e.Cancel = true;
		}
	}
}
