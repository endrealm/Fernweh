using System.IO.Compression;
using ModBuildTool.Lib;
using ModBuildTool.Lib.File;

var directory = args.Length >= 1 ? args[0] : ".\\";

var buildTool = new BuildTool(directory);
buildTool.Load();
buildTool.Process();
buildTool.Export();