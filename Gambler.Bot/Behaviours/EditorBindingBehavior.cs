using Avalonia;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using Gambler.Bot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextMateSharp.Grammars;

namespace Gambler.Bot.Behaviours
{
    public class EditorBindingBehavior : Behavior<TextEditor>
    {
        private TextEditor _textEditor = null;
        private TextMate.Installation _textMateInstallation = null;
        private RegistryOptions _registryOptions = null;

        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<EditorBindingBehavior, string>(nameof(Text));

        public static readonly StyledProperty<string> LanguageProperty =
            AvaloniaProperty.Register<EditorBindingBehavior, string>(nameof(Language));

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Language
        {
            get => GetValue(LanguageProperty);
            set => SetValue(LanguageProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is TextEditor textEditor)
            {

                
                _textEditor = textEditor;
                _registryOptions = new RegistryOptions(ThemeName.DarkPlus);
                //Initial setup of TextMate.
                _textMateInstallation = _textEditor.InstallTextMate(_registryOptions);
                if (UISettings.Settings?.DarkMode??true)
                    _textMateInstallation?.SetTheme(_registryOptions.LoadTheme(ThemeName.Dark));
                else
                    _textMateInstallation?.SetTheme(_registryOptions.LoadTheme(ThemeName.Light));
                _textEditor.TextChanged += TextChanged;
                
                this.GetObservable(TextProperty).Subscribe(TextPropertyChanged);
                this.GetObservable(LanguageProperty).Subscribe(LanguagePropertyChanged);
                UISettings.Settings.PropertyChanged += Settings_PropertyChanged;
            }
        }

        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UISettings.DarkMode))
            {
                if (UISettings.Settings?.DarkMode ?? true)
                    _textMateInstallation?.SetTheme(_registryOptions.LoadTheme(ThemeName.Dark));
                else
                    _textMateInstallation?.SetTheme(_registryOptions.LoadTheme(ThemeName.Light));
                
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_textEditor != null)
            {
                _textEditor.TextChanged -= TextChanged;
            }
            UISettings.Settings.PropertyChanged -= Settings_PropertyChanged;
        }

        private void TextChanged(object sender, EventArgs eventArgs)
        {
            if (_textEditor != null && _textEditor.Document != null)
            {
                Text = _textEditor.Document.Text;
            }
        }

        private void TextPropertyChanged(string text)
        {
            if (_textEditor != null && _textEditor.Document != null && text != null)
            {
                var caretOffset = _textEditor.CaretOffset;
                _textEditor.Document.Text = text;
                _textEditor.CaretOffset = caretOffset;
            }
        }

        private void LanguagePropertyChanged(string text)
        {
            if (_textEditor != null && text != null)
            {
                _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension($".{text}").Id));
            }
        }
    }
}
