using Microsoft.WindowsAPICodePack.Dialogs;

namespace ImageToLockscreen.Ui.Controls
{
    internal sealed class FolderBrowserDialog
    {
        private static readonly bool IsLegacy;
        private delegate DialogResult ShowOpenFolderDialog();
        private ShowOpenFolderDialog _showOpenFolderDialog;
        private CommonOpenFileDialog _commonOpenFileDialog = null;
        private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog = null;
        private string _title = null;

        static FolderBrowserDialog()
        {
            FolderBrowserDialog.IsLegacy = !CommonFileDialog.IsPlatformSupported;
        }

        public FolderBrowserDialog()
        {
            this.SetCompatibleFolderDialog();
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                if (FolderBrowserDialog.IsLegacy && this._folderBrowserDialog != null)
                    this._folderBrowserDialog.Description = value;
                else if (this._commonOpenFileDialog != null)
                    this._commonOpenFileDialog.Title = value;
            }
        }
        public string SelectedPath { get; private set; }
        public DialogResult DialogResult { get; private set; }

        public DialogResult ShowDialog()
        {
            return this._showOpenFolderDialog();
        }

        private void SetCompatibleFolderDialog()
        {
            if (FolderBrowserDialog.IsLegacy)
            {
                this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                this._folderBrowserDialog.ShowNewFolderButton = true;
                this._folderBrowserDialog.Description = this.Title;
                this._showOpenFolderDialog = OpenFolderDialogLegacy;
            }
            else
            {
                this._commonOpenFileDialog = new CommonOpenFileDialog();
                this._commonOpenFileDialog.IsFolderPicker = true;
                this._commonOpenFileDialog.Title = this.Title;
                this._commonOpenFileDialog.Multiselect = false;
                this._showOpenFolderDialog = OpenFolderDialog;
            }
        }
        private DialogResult OpenFolderDialog()
        {
            if (this._commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.SelectedPath = this._commonOpenFileDialog.FileName;
                return DialogResult.Ok;
            }
            return DialogResult.Cancel;
        }
        private DialogResult OpenFolderDialogLegacy()
        {
            if (this._folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.SelectedPath = this._folderBrowserDialog.SelectedPath;
                return DialogResult.Ok;
            }
            return DialogResult.Cancel;
        }
    }

    public enum DialogResult
    {
        Cancel,
        Ok,
    }
}
