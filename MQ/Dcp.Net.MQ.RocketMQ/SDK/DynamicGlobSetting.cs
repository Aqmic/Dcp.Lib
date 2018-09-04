using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dcp.Net.MQ.RocketMQ.DynamicLib
{
    public static class DynamicGlobSetting
    {
        internal static void InitDynamicLib()
        {
            Console.WriteLine("初始化非托管dll");
            Assembly assembly = Assembly.GetAssembly(typeof(DynamicGlobSetting));
            if (IntPtr.Size != 8)
            {
                string errorMsg = $"{assembly.FullName}只支持x64平台！";
                Console.WriteLine(errorMsg);
                throw new NotSupportedException(errorMsg);
            }
            PlatformID platformID = Environment.OSVersion.Platform;
            string libName = "";
            Console.WriteLine(platformID);
            switch (platformID)
            {
                case PlatformID.Unix: {
                        //linux
                        libName= "libonsclient4cpp.so";
                    };break;
                default: {
                        //windows
                        libName = "ONSClient4CPP.dll";
                    };break;
            }
           var libBytes=GetEmbeddedResourceByPathWithNamespace($"DynamicLib{Path.PathSeparator}{libName}");
           if (libBytes != null)
           {
                Console.WriteLine("写入文件");
                File.WriteAllBytes(libName,libBytes);
           }
        }
        internal static byte[] GetEmbeddedResourceByPathWithNamespace(string path)
        {
            path = path.Replace(Path.PathSeparator, '.');
            path = path.Replace('/', '.');
            path = path.Replace("//", ".");
            Assembly assembly = Assembly.GetAssembly(typeof(DynamicGlobSetting));
            string assemblyName = assembly.GetName().Name;
            path = string.Format("{0}.{1}", assemblyName, path);

            var testnames= Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach(var item in testnames)
            {
                Console.WriteLine(item);
            }
            System.IO.Stream strm = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            if (strm == null)
            {
                return null;
            }
            strm.Position = 0;
            byte[] cbyte = new byte[strm.Length];
            strm.Read(cbyte, 0, cbyte.Length);
            return cbyte;
        }

    }
}
