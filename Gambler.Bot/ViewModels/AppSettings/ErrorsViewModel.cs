using Gambler.Bot.Common.Enums;
using Gambler.Bot.Helpers;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Gambler.Bot.Classes.PersonalSettings;

namespace Gambler.Bot.ViewModels.AppSettings
{
    public class ErrorsViewModel : ViewModelBase
    {

        public Dictionary<ErrorType, ErrorSetting> Errors { get; set; } = new Dictionary<ErrorType, ErrorSetting>();
        
        public ObservableCollection<ErrorActions> Actions { get; set; } = new ObservableCollection<ErrorActions>();

        private ErrorActions invalidBet;

        public ErrorActions InvalidBet
        {
            get { return invalidBet; }
            set { invalidBet = value; this.RaisePropertyChanged(); Errors[ErrorType.InvalidBet].Action = value; }
        }

        private ErrorActions balanceTooLow;

        public ErrorActions BalanceTooLow
        {
            get { return balanceTooLow; }
            set { balanceTooLow = value; this.RaisePropertyChanged(); Errors[ErrorType.BalanceTooLow].Action = value; }
        }

        private ErrorActions betTooLow;

        public ErrorActions BetTooLow
        {
            get { return betTooLow; }
            set { betTooLow = value; this.RaisePropertyChanged(); Errors[ErrorType.BetTooLow].Action = value; }
        }

        private ErrorActions resetSeed;

        public ErrorActions ResetSeed
        {
            get { return resetSeed; }
            set { resetSeed = value; this.RaisePropertyChanged(); Errors[ErrorType.ResetSeed].Action = value; }
        }

        private ErrorActions withdrawal;

        public ErrorActions Withdrawal
        {
            get { return withdrawal; }
            set { withdrawal = value; this.RaisePropertyChanged(); Errors[ErrorType.Withdrawal].Action = value; }
        }

        private ErrorActions tip;

        public ErrorActions Tip
        {
            get { return tip; }
            set { tip = value; this.RaisePropertyChanged(); Errors[ErrorType.Tip].Action = value; }
        }

        private ErrorActions notImplemented;

        public ErrorActions NotImplemented
        {
            get { return notImplemented; }
            set { notImplemented = value; this.RaisePropertyChanged(); Errors[ErrorType.NotImplemented].Action = value; }
        }

        private ErrorActions other;

        public ErrorActions Other
        {
            get { return other; }
            set { other = value; this.RaisePropertyChanged(); Errors[ErrorType.Other].Action = value; }
        }

        private ErrorActions betMisMatch;

        public ErrorActions BetMisMatch
        {
            get { return betMisMatch; }
            set { betMisMatch = value; this.RaisePropertyChanged(); Errors[ErrorType.BetMismatch].Action = value; }
        }

        private ErrorActions unkown;

        public ErrorActions Unkown
        {
            get { return unkown; }
            set { unkown = value; this.RaisePropertyChanged(); Errors[ErrorType.Unknown].Action = value; }
        }
        private ErrorActions bank;

        public ErrorActions Bank
        {
            get { return unkown; }
            set { unkown = value; this.RaisePropertyChanged(); Errors[ErrorType.Bank].Action = value; }
        }

        public ErrorsViewModel(ILogger logger) : base(logger)
        {
            foreach (ErrorActions x in Enum.GetValues(typeof(ErrorActions)))
            {
                Actions.Add(x);
            }
        }
        public void AddItem(ErrorSetting error)
        {
            Errors.Add(error.Type,error);            
            switch (error.Type)
            {
                case Bot.Common.Enums.ErrorType.InvalidBet:
                    InvalidBet = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.BalanceTooLow:
                    BalanceTooLow = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.BetTooLow:
                    BetTooLow = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.ResetSeed:
                    ResetSeed = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.Withdrawal:
                    Withdrawal = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.Tip:
                    Tip = error.Action;
                    break;
                        case Bot.Common.Enums.ErrorType.NotImplemented:
                    NotImplemented = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.Other:
                    Other = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.BetMismatch:
                    BetMisMatch = error.Action;
                    break;
                    case Bot.Common.Enums.ErrorType.Unknown:
                    Unkown = error.Action;
                    break;
                case Bot.Common.Enums.ErrorType.Bank:
                    Bank = error.Action;
                    break;

            }
        }

        internal void Clear()
        {
            Errors?.Clear();
        }
    }
}
