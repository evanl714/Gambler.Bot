using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using KryGamesBot.Ava.Classes;
using KryGamesBot.Ava.ViewModels.Common;
using KryGamesBot.Ava.ViewModels.Strategies;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Web;
using TextMateSharp.Grammars;

namespace KryGamesBot.Ava.Views.Games.Strategies
{
    public partial class ProgrammerModeView : ReactiveUserControl<ProgrammerModeViewModel>
    {
        public ProgrammerModeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            { 
                ViewModel.SaveFileInteraction.RegisterHandler(SaveFile);
                ViewModel.OpenFileInteraction.RegisterHandler(OpenFile);
            });

        }

        async Task SaveFile(InteractionContext<FilePickerSaveOptions, string?> interaction)
        {
            await IOHelper.SaveFile(interaction, TopLevel.GetTopLevel(this));
        }

        async Task OpenFile(InteractionContext<FilePickerOpenOptions, string?> interaction)
        {
            await IOHelper.OpenFile(interaction, TopLevel.GetTopLevel(this));
        }
    }
}
