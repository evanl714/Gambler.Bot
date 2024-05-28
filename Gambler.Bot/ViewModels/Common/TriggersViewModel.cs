using AvaloniaEdit.Utils;
using Gambler.Bot.AutoBet.Helpers;
using Gambler.Bot.Core.Helpers;
using Gambler.Bot.Core.Sites;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.BC;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gambler.Bot.ViewModels.Common
{
    public class TriggersViewModel : ViewModelBase
    {
        public bool IsNumber(Type value)
        {
            return value == typeof(sbyte  )
                || value == typeof(byte   )
                || value == typeof(short  )
                || value == typeof(ushort )
                || value == typeof(int    )
                || value == typeof(uint   )
                || value == typeof(long   )
                || value == typeof(ulong  )
                || value == typeof(Int128 )
                || value == typeof(UInt128)
                || value == typeof(nint   )
                || value == typeof(nuint  )
                || value == typeof(Half   )
                || value == typeof(float  )
                || value == typeof(double )
                || value == typeof(decimal);
        }

        public TriggersViewModel(ILogger logger) : base(logger)
        {
            RemoveCommand = ReactiveCommand.Create(Remove);
            AddCommand = ReactiveCommand.Create(Add);
            LoadedCommand = ReactiveCommand.Create(Load);

        }
        ICommand LoadedCommand { get; }
        public void Load()
        {
            
            List<string> newproperties = new List<string>();
            var props = typeof(Gambler.Bot.AutoBet.Helpers.SessionStats).GetProperties().ToList();
            props.AddRange(typeof(SiteStats).GetProperties());
            foreach (System.Reflection.PropertyInfo x in props)
            {
                if (IsNumber(x.PropertyType))
                {
                    newproperties.Add($"{x.DeclaringType.Name}.{x.Name}");
                }
            }
            newproperties.Sort();
            Properties = newproperties;
            foreach (TriggerComparison x in Enum.GetValues(typeof(TriggerComparison)))
            {
                Comparisons.Add(x);
            }
            foreach (CompareAgainst x in Enum.GetValues(typeof(CompareAgainst)))
            {
                Againsts.Add(x);
            }
            if (Notifications)
            {
                Actions.Add(TriggerAction.Popup);
                Actions.Add(TriggerAction.Email);
                Actions.Add(TriggerAction.Alarm);
                Actions.Add(TriggerAction.Chime);
            }
            else
            {
                Actions.Add(TriggerAction.Stop);
                Actions.Add(TriggerAction.Reset);
                Actions.Add(TriggerAction.Withdraw);
                Actions.Add(TriggerAction.Tip);
                Actions.Add(TriggerAction.Invest);
                Actions.Add(TriggerAction.Bank);
                Actions.Add(TriggerAction.ResetSeed);
                Actions.Add(TriggerAction.Switch);
            }
        }

        private Trigger  selectedTrigger;

        public Trigger SelectedTrigger
        {
            get { return selectedTrigger; }
            set
            {
                if (selectedTrigger != null)
                    selectedTrigger.PropertyChanged -= SelectedTrigger_PropertyChanged;
                this.RaiseAndSetIfChanged(ref selectedTrigger, value);
                this.RaisePropertyChanged(nameof(ShowEditor));
                this.RaisePropertyChanged(nameof(Target));
                if (selectedTrigger!=null)
                    selectedTrigger.PropertyChanged += SelectedTrigger_PropertyChanged;
                refreshLayout();
            }
        }

        private void SelectedTrigger_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Trigger.TargetType) ||
                e.PropertyName == nameof(Trigger.ValueType) ||
                e.PropertyName == nameof(Trigger.Action)
                )
            {
                refreshLayout();
                
            }
        }

        void refreshLayout()
        {
            this.RaisePropertyChanged(nameof(ShowTValue));
            this.RaisePropertyChanged(nameof(ShowTPercentage));
            this.RaisePropertyChanged(nameof(ShowTPrperty));

            this.RaisePropertyChanged(nameof(ShowVValue));
            this.RaisePropertyChanged(nameof(ShowVPercentage));
            this.RaisePropertyChanged(nameof(ShowVPrperty));

            this.RaisePropertyChanged(nameof(ShowAmount));
            this.RaisePropertyChanged(nameof(ShowDestination));
        }

        public bool Notifications { get; set; }

        public List<string> Properties { get; set; } = new List<string>();

        public List<TriggerComparison> Comparisons { get; set; } = new List<TriggerComparison>();

        public List<CompareAgainst> Againsts { get; set; } = new List<CompareAgainst>();

        public List<TriggerAction> Actions { get; set; } = new List<TriggerAction>();

        List<TriggerAction> AmountActions
        {
            get;
            set;
        } = new List<TriggerAction>()
        {
            TriggerAction.Bank,
            TriggerAction.Invest,
            TriggerAction.Tip,
            TriggerAction.Withdraw
        };

        List<TriggerAction> DestinationActions
        {
            get;
            set;
        } = new List<TriggerAction>() { TriggerAction.Email, TriggerAction.Tip, TriggerAction.Withdraw };

        public decimal Target
        {
            get { return getdecimal(SelectedTrigger?.Target); }
            set 
            {
                if (SelectedTrigger != null)
                {
                    SelectedTrigger.Target = value.ToString("n8");
                }
                this.RaisePropertyChanged(); }
        }

        decimal getdecimal(string value)
        {
            if (value == null)
                return 0;
            if(decimal.TryParse(value, out decimal result))
            {
                return result;
            } else
            {
                return 0;
            }
        }

        private List<Trigger> triggers;

        public List<Trigger> Triggers
        {
            get { return triggers; }
            protected set
            {
                triggers = value;
                this.RaisePropertyChanged();
            }
        }

        public void SetTriggers(List<Trigger> triggers)
        {
            Load();
            Triggers = triggers;            
        }

        private bool showEditor;

        public bool ShowEditor { get { return SelectedTrigger != null; } }


        public ICommand RemoveCommand { get; }

        void Remove()
        {
            if (SelectedTrigger!=null)
            {
                Triggers.Remove(SelectedTrigger);
                SelectedTrigger = null;
                this.RaisePropertyChanged(nameof(Triggers));
            }
        }

        public ICommand AddCommand { get; }

        void Add()
        {   
            Trigger newTrigger = new Trigger();
            Triggers.Add(newTrigger);
            this.RaisePropertyChanged(nameof(Triggers));
            SelectedTrigger = newTrigger;
            
        }

        

        public bool ShowTValue
        {
            get { return SelectedTrigger?.TargetType == CompareAgainst.Value; }            
        }
        public bool ShowTPercentage
        {
            get { return SelectedTrigger?.TargetType == CompareAgainst.Percentage; }
        }
        public bool ShowTPrperty
        {
            get { return SelectedTrigger?.TargetType == CompareAgainst.Property; }
        }

        public bool ShowVValue
        {
            get { return SelectedTrigger?.ValueType == CompareAgainst.Value; }
        }
        public bool ShowVPercentage
        {
            get { return SelectedTrigger?.ValueType == CompareAgainst.Percentage; }
        }
        public bool ShowVPrperty
        {
            get { return SelectedTrigger?.ValueType == CompareAgainst.Property; }
        }

        public bool ShowAmount
        {
            get { return AmountActions.Contains(SelectedTrigger?.Action?? TriggerAction.Stop); }
        }
        public bool ShowDestination
        {
            get { return DestinationActions.Contains(SelectedTrigger?.Action ?? TriggerAction.Stop); }
        }
    }
}
