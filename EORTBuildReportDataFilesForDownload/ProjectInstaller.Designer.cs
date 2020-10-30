namespace EORTBuildReportDataFilesForDownload
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.EORTBuildReportDataFilesForDownload = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            // 
            // EORTBuildReportDataFilesForDownload
            // 
            this.EORTBuildReportDataFilesForDownload.DelayedAutoStart = true;
            this.EORTBuildReportDataFilesForDownload.Description = "Builds the Raw Data Reports and zips them up for download.";
            this.EORTBuildReportDataFilesForDownload.DisplayName = "Exec. Overview Report Tool Build Report Data Files for Download Service";
            this.EORTBuildReportDataFilesForDownload.ServiceName = "EORTBuildReportDataFilesForDownload";
            this.EORTBuildReportDataFilesForDownload.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.EORTBuildReportDataFilesForDownload});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller EORTBuildReportDataFilesForDownload;
    }
}