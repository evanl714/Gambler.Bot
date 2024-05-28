using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextMateSharp.Grammars;

namespace Gambler.Bot.Behaviours
{
    public class NumberFormatBehaviour : Behavior<SelectableTextBlock>
    {
        private SelectableTextBlock _textEditor = null;
        
        public static readonly StyledProperty<string> MaskProperty =
            AvaloniaProperty.Register<EditorBindingBehavior, string>(nameof(Mask));
        public static readonly StyledProperty<object> TextProperty =
           AvaloniaProperty.Register<EditorBindingBehavior, object>(nameof(Text));



        public string Mask
        {
            get => GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }
        public object Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is SelectableTextBlock textEditor)
            {
                //_textEditor.PropertyChanged += TextChanged;

                _textEditor = textEditor;
                

                //this.GetObservable(MaskProperty).Subscribe(TextPropertyChanged);
                this.GetObservable(TextProperty).Subscribe(TextPropertyChanged);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (_textEditor != null)
            {
                
            }
        }

        private void TextChanged(object sender, AvaloniaPropertyChangedEventArgs eventArgs)
        {
            
        }

        private void TextPropertyChanged(object text)
        {
            
            if (_textEditor != null )
            {
                if (text == null)
                {
                    _textEditor.Text = "";
                    return;
                }
                if (Mask==null)
                {
                    _textEditor.Text = text.ToString();
                    return;
                }

                if (text is decimal d)
                {
                    _textEditor.Text = d.ToString(Mask);
                }
                else if (text is int i)
                {
                    _textEditor.Text = i.ToString(Mask);
                }
                else if (text is DateTime dt)
                {
                    _textEditor.Text = dt.ToString(Mask);
                }
                else if (text is TimeSpan ts)
                {
                    _textEditor.Text = ts.ToString(Mask);
                }
                else
                {
                    _textEditor.Text = text.ToString();
                }
            }
        }       
    }
}
