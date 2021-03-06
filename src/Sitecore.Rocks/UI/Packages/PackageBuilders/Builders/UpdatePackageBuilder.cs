﻿// © 2015-2016 Sitecore Corporation A/S. All rights reserved.

using System;
using System.IO;
using System.Net;
using System.Windows;
using Sitecore.Rocks.Diagnostics;
using Sitecore.Rocks.Extensibility.Composition;

namespace Sitecore.Rocks.UI.Packages.PackageBuilders.Builders
{
    [Export(typeof(IPackageBuilder), Priority = 2000)]
    public class UpdatePackageBuilder : BasePackageBuilder
    {
        public UpdatePackageBuilder()
        {
            Name = "Update Package";
            RequestTypeName = "Packages.BuildUpdatePackage";
        }

        protected override void ProcessCompleted(string url, string targetFileName, Action<string> completed)
        {
            Debug.ArgumentNotNull(completed, nameof(completed));
            Debug.ArgumentNotNull(targetFileName, nameof(targetFileName));
            Debug.ArgumentNotNull(url, nameof(url));

            targetFileName = Path.ChangeExtension(targetFileName, "update");

            var client = new WebClient();
            try
            {
                client.DownloadFile(url, targetFileName);
            }
            catch (WebException ex)
            {
                if (AppHost.MessageBox(string.Format("Failed to download the package file: {0}\n\nDo you want to report this error?\n\n{1}\n{2}", url, ex.Message, ex.StackTrace), "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    AppHost.Shell.HandleException(ex);
                }

                completed(string.Empty);
                return;
            }

            completed(targetFileName);
        }
    }
}
