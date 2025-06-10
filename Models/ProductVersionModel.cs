using System;

namespace TLGames.Models
{
    internal class ProductVersionModel
    {
        public int ProductVersionId { get; private set; }
        public int ProductId { get; private set; }
        public string ExecutablePath { get; private set; }
        public string DownloadURL { get; private set; }
        public DateTime UploadDate { get; private set; }
        public string Version { get; private set; }
        public string LaunchArgs { get; private set; }
        public decimal SizeMB { get; private set; }
        public string UpdateDescription { get; private set; }

        public ProductVersionModel() { }

        public ProductVersionModel(int productVersionId, int productId, string executablePath, string downloadURL,
                                   DateTime uploadDate, string version, string launchArgs, decimal sizeMB, string updateDescription)
        {
            ProductVersionId = productVersionId;
            ProductId = productId;
            ExecutablePath = executablePath;
            DownloadURL = downloadURL;
            UploadDate = uploadDate;
            Version = version;
            LaunchArgs = launchArgs;
            SizeMB = sizeMB;
            UpdateDescription = updateDescription;
        }

        public void SetProductVersionId(int id) { ProductVersionId = id; }
        public void SetProductId(int id) { ProductId = id; }
        public void SetExecutablePath(string path) { ExecutablePath = path; }
        public void SetDownloadURL(string url) { DownloadURL = url; }
        public void SetUploadDate(DateTime date) { UploadDate = date; }
        public void SetVersion(string version) { Version = version; }
        public void SetLaunchArgs(string args) { LaunchArgs = args; }
        public void SetSizeMB(decimal size) { SizeMB = size; }
        public void SetUpdateDescription(string description) { UpdateDescription = description; }
    }
}
