using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBot.Ava.Classes
{
    public interface iDatabaseForm
    {
        string ConnectionString();
        string Provider();
        bool Validate();
    }
}
