using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using KryGamesBot.Ava.Classes;
using KryGamesBot.Ava.ViewModels.Common;
using ReactiveUI;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.Views.Common
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
