using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit
{
    public static class TestFileProvider
    {
        public static string ReadText(string name)
        {
            var resourceName = $"{typeof(TestFileProvider).Namespace}.TestFiles.{name}";
            var assembly = Assembly.GetAssembly(typeof(TestFileProvider));
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"No resource found with name \"{resourceName}\"");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
