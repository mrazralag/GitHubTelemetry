using GitHubTelemetry.Services;

namespace GitHubTelemetry
{
    /// <summary>
    /// Startup entry point 
    /// </summary>
    public class GitHubTelemetry
    {
        private GitHubService gitHubService;
        private ExcelService excelService;
        private AzureBlobService blobService;

        /// <summary>
        /// Dependency injected constructor with necessary services
        /// </summary>
        /// <param name="injectedGitHubService"></param>
        /// <param name="injectedExcelService"></param>
        public GitHubTelemetry(GitHubService injectedGitHubService, ExcelService injectedExcelService, AzureBlobService injectedBlobService)
        {
            gitHubService = injectedGitHubService;
            excelService = injectedExcelService;
            blobService = injectedBlobService;
        }

        public void Run()
        {
            // Capture and export github data
            string fileName = excelService.ExportPullRequestData(gitHubService.GetMergedPullRequests());

            // Upload it to blob storage
            blobService.UploadBlob(fileName);
        }
    }
}
