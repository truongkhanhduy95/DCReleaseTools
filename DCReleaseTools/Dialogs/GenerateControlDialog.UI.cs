using System;
using MonoDevelop.Core;
using Xwt;

namespace DCReleaseTools.Dialogs
{
    public partial class GenerateControlDialog : Dialog
    {
        ComboBox filesComboBox;
        DialogButton okButton;

        void Build()
        {
            Title = GettextCatalog.GetString("Generate control wrapper");
            Width = 410;
            Resizable = false;

            var mainVBox = new VBox();
            Content = mainVBox;

            var label = new Label();
            label.Text = GettextCatalog.GetString("Select resource");

            mainVBox.PackStart(label);

            filesComboBox = new ComboBox();
            mainVBox.PackStart(filesComboBox);

            okButton = new DialogButton(GettextCatalog.GetString("OK"), Command.Ok);
            Buttons.Add(okButton);

            okButton.Clicked += OkButton_Clicked;
        }
    }
}
