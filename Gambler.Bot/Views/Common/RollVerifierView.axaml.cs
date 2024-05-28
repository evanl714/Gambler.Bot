using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Gambler.Bot.Classes;
using Gambler.Bot.ViewModels.Common;
using ReactiveUI;
using System.Threading.Tasks;

namespace Gambler.Bot.Views.Common
{
    public partial class RollVerifierView : ReactiveUserControl<RollVerifierViewModel>
    {
        public RollVerifierView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                ViewModel.SaveFileInteraction.RegisterHandler(SaveFile);
            });
        }
        async Task SaveFile(InteractionContext<FilePickerSaveOptions, string?> interaction)
        {
            await IOHelper.SaveFile(interaction, TopLevel.GetTopLevel(this));
        }
    }
}
