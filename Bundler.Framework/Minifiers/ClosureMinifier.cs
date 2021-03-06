using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Bundler.Framework.Minifiers
{
    public class ClosureMinifier: IJavaScriptCompressor
    {
        public static string Identifier
        {
            get { return "closure"; }
        }

        string IJavaScriptCompressor.Identifier
        {
            get { return Identifier; }
        }

        private string CompressFile(string file)
        {
            var a = Assembly.GetExecutingAssembly();
            string path = Path.GetDirectoryName(a.Location);
            string outFile = Path.GetTempPath() + Path.GetRandomFileName();
            try
            {                
                string arguments = "-jar \"{0}\\closure-compiler\\compiler.jar\" --js \"{1}\" --js_output_file \"{2}\"";
                arguments = String.Format(arguments, path, file, outFile);
                var startInfo = new ProcessStartInfo("java", arguments);
                startInfo.UseShellExecute = false;
                startInfo.Arguments = arguments;
                startInfo.RedirectStandardError = true;
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                var process = Process.Start(startInfo);
                Console.Error.Write(process.StandardError.ReadToEnd());                    
                process.WaitForExit();

                string output;
                using (var sr = new StreamReader(outFile))
                {
                    output = sr.ReadToEnd();
                }
                return output;
            }
            finally
            {
                File.Delete(outFile);
            }            
        }

        public string CompressContent(string content)
        {
            string inputFileName = Path.GetTempPath() + Path.GetRandomFileName();
            try
            {
                using (var sw = new StreamWriter(inputFileName))
                {
                    sw.Write(content);
                }
                return CompressFile(inputFileName);
            }
            finally
            {
                File.Delete(inputFileName);
            }
        }
    }
}