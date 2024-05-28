using System;
using System.Collections.Generic;
using System.Text;

namespace Gambler.Bot.Classes
{
    public interface iDatabaseForm
    {
        string ConnectionString();
        string Provider();
        bool Validate();
    }
}
