using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.ComponentModel;

namespace KryGamesBot.Ava.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected readonly ILogger _logger;
        public ViewModelBase(ILogger logger)
        {
            _logger = logger;
        }
    }
}