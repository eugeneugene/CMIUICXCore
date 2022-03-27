using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CMIUICXCore.Code
{
    /// <summary>
    /// Defines the <see cref="VersionHelper" />.
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// Gets the Version.
        /// </summary>
        public static string Version
        {
            get
            {
                var ass = Assembly.GetEntryAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(ass.Location);

                StringBuilder version = new();
                version.AppendLine(VersionLineInt(fvi));
                if (!string.IsNullOrWhiteSpace(fvi.Comments))
                    version.AppendLine(fvi.Comments);
                if (!string.IsNullOrWhiteSpace(fvi.LegalCopyright))
                    version.AppendLine(fvi.LegalCopyright);
                return version.ToString();
            }
        }

        /// <summary>
        /// Gets the VersionLine.
        /// </summary>
        public static string VersionLine
        {
            get
            {
                var ass = Assembly.GetEntryAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(ass.Location);

                return VersionLineInt(fvi);
            }
        }

        /// <summary>
        /// Gets the VersionLines.
        /// </summary>
        public static IEnumerable<string> VersionLines
        {
            get
            {
                var ass = Assembly.GetEntryAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(ass.Location);

                var lines = new List<string> { VersionLineInt(fvi), };
                if (!string.IsNullOrWhiteSpace(fvi.Comments))
                    lines.Add(fvi.Comments);
                if (!string.IsNullOrWhiteSpace(fvi.LegalCopyright))
                    lines.Add(fvi.LegalCopyright);
                return lines;
            }
        }

        /// <summary>
        /// The BuildDate.
        /// </summary>
        /// <param name="fvi">The fvi<see cref="FileVersionInfo"/>.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        private static DateTime BuildDate(FileVersionInfo fvi)
        {
            DateTime build = new(2000, 1, 1);
            build = build.AddDays(fvi.FileBuildPart);
            build = build.AddSeconds(fvi.FilePrivatePart * 2.0);
            return build;
        }

        /// <summary>
        /// The VersionLineInt.
        /// </summary>
        /// <param name="fvi">The fvi<see cref="FileVersionInfo"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string VersionLineInt(FileVersionInfo fvi) => $"{fvi.ProductName} {fvi.ProductVersion}, {fvi.FileVersion} ({BuildDate(fvi):G})";
    }
}
