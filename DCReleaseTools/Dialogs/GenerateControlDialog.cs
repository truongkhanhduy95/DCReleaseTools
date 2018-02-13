using System;
namespace DCReleaseTools.Dialogs
{
    public partial class GenerateControlDialog : Gtk.Dialog
    {
        private string _resourceFile;

        public GenerateControlDialog()
        {
            this.Build();
            this.Title = "Enter resource file name";


            AddEvents();
        }

        private void AddEvents()
        {
            btnCancel.Clicked += BtnCancel_Clicked; 
            btnOK.Clicked += BtnOK_Clicked;
        }

        void BtnOK_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResourceName.Text))
            {
                _resourceFile = txtResourceName.Text;
                Console.WriteLine(_resourceFile);
            }

            this.Destroy();
        }

        void BtnCancel_Clicked(object sender, EventArgs e)
        {
            this.Destroy();
        }
    }
}
