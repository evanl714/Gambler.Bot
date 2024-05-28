using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gambler.Bot.Classes
{
    internal class IOHelper
    {
        public static async Task SaveFile(InteractionContext<FilePickerSaveOptions, string?> interaction, TopLevel topLevel)
        {
            
            var storage = topLevel.StorageProvider;
            var options = interaction.Input;
            var dirs = await storage.SaveFilePickerAsync(options);
            var dir = dirs?.Path.AbsolutePath;
            dir = HttpUtility.UrlDecode(dir);
            interaction.SetOutput(dir);
        }

        public static async Task OpenFile(InteractionContext<FilePickerOpenOptions, string?> interaction, TopLevel topLevel)
        {
            
            var storage = topLevel.StorageProvider;
            var options = interaction.Input;
            var dirs = await storage.OpenFilePickerAsync(options);
            var dir = dirs.FirstOrDefault()?.Path.AbsolutePath;
            dir = HttpUtility.UrlDecode(dir);
            interaction.SetOutput(dir);
        }
    }
}
