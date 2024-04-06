using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
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
            var topLevel = TopLevel.GetTopLevel(this);
            var storage = topLevel.StorageProvider;
            var options = interaction.Input;
            var dirs = await storage.SaveFilePickerAsync(options);
            var dir = dirs.Path.AbsolutePath;
            dir = HttpUtility.UrlDecode(dir);
            interaction.SetOutput(dir);
        }

        async Task OpenFile(InteractionContext<FilePickerOpenOptions, string?> interaction)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var storage = topLevel.StorageProvider;
            var options = interaction.Input;
            var dirs = await storage.OpenFilePickerAsync(options);
            var dir = dirs.FirstOrDefault().Path.AbsolutePath;
            dir = HttpUtility.UrlDecode(dir);
            interaction.SetOutput(dir);
        }

    }
}
