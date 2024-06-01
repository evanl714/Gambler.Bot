using Avalonia.Collections;
using Gambler.Bot.Core.Games;
using Gambler.Bot.Core.Storage;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.Common
{
    public  class BetHistoryViewModel: ViewModelBase
    {
        private DataGridCollectionView bets;

        public DataGridCollectionView Bets
        {
            get { return bets; }
            private set { bets = value; this.RaisePropertyChanged(); }
        }

        private BotContext context;

        public BotContext Context
        {
            get { return context; }
            set { context = value; LoadData(); this.RaisePropertyChanged(); }
        }

        private string site;

        public string Site
        {
            get { return site; }
            set { site = value; LoadData(); this.RaisePropertyChanged(); }
        }

        private int currentPage;

        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; this.RaisePropertyChanged(); }
        }


        private void LoadData()
        {
            if (Context== null)
                return;
            try
            {
                DataGridCollectionView tmpBets=null;
                switch (Game)
                {
                    case Core.Games.Games.Crash:
                        tmpBets = new DataGridCollectionView(Context.CrashBets.Where(x => x.Site == Site));
                        break;
                    case Core.Games.Games.Dice:
                        tmpBets = new DataGridCollectionView(Context.DiceBets.Where(x => x.Site == Site));
                        break;
                    case Core.Games.Games.Roulette:
                        tmpBets = new DataGridCollectionView(Context.RouletteBets.Where(x => x.Site == Site));
                        break;
                    case Core.Games.Games.Plinko:
                        tmpBets = new DataGridCollectionView(Context.PlinkoBets.Where(x => x.Site == Site));
                        break;
                }
                tmpBets.PageSize = PageSize;
                Bets = tmpBets;
                
                
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error loading data");
            }
        }

        private Gambler.Bot.Core.Games.Games game = Core.Games.Games.Dice;

        public Gambler.Bot.Core.Games.Games Game
        {
            get { return game; }
            set { game = value; LoadData(); this.RaisePropertyChanged();  }
        }
        private int pageSize=20;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; Bets.PageSize = pageSize; this.RaisePropertyChanged(nameof(TotalPages)) ; this.RaisePropertyChanged(); }
        }

        private int totalPages;

        public int TotalPages
        {
            get { return (Bets?.TotalItemCount??0)/PageSize; }
            
        }


        public BetHistoryViewModel(ILogger logger):base(logger)
        {
            
        }

        public void FirstPage()
        {
            if (Bets.MoveToFirstPage())
            {
                CurrentPage = Bets.PageIndex;
            }
        }
        public void NextPage()
        {
            if (Bets.MoveToNextPage())
            {
                CurrentPage = Bets.PageIndex;
            }
        }
        public void PreviousPage()
        {
            if (Bets.MoveToPreviousPage())
            {
                CurrentPage = Bets.PageIndex;
            }
        }
        public void LastPage()
        {
            if (Bets.MoveToLastPage())
            {
                CurrentPage = Bets.PageIndex;
            }
        }
    }
}
