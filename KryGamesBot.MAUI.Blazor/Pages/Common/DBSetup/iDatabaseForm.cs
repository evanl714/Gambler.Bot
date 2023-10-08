using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBot.MAUI.Blazor.Pages.Common.DBSetup
{
    public interface iDatabaseForm
    {
        string ConnectionString();
        string Provider();
        bool Validate();
    }
}
