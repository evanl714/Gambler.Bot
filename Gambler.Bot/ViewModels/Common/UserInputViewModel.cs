

using Gambler.Bot.Strategies.Strategies.Abstractions;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.Common
{
    public class UserInputViewModel:ViewModelBase
    {
        private ReadEventArgs args;

        public ReadEventArgs Args
        {
            get { return args; }
            set { args = value; }
        }
        public Interaction<Unit?, Unit?> CloseDialogInteraction { get; internal set; }
        public UserInputViewModel(ILogger logger):base(logger)
        {
            CloseDialogInteraction = new Interaction<Unit?, Unit?>();
        }

        public async Task Ok()
        {
            if (Args.DataType==0)
                Args.Result = true;

            //close window inderaction
            await CloseDialogInteraction.Handle(Unit.Default);
        }

        public async Task Cancel()
        {
            if (Args.DataType == 0)
                Args.Result = false;
            else
                Args.Result = null;
            //close window inderaction
            await CloseDialogInteraction.Handle(Unit.Default);
        }
    }
}
