using Avalonia;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextMateSharp.Grammars;

namespace KryGamesBot.Ava.Behaviours
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
                _textEditor.TextChanged += TextChanged;

                this.GetObservable(TextProperty).Subscribe(TextPropertyChanged);
                this.GetObservable(LanguageProperty).Subscribe(LanguagePropertyChanged);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_textEditor != null)
            {
                _textEditor.TextChanged -= TextChanged;
            }
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
