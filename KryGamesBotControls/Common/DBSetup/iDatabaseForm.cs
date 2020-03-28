using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBotControls.Common.DBSetup
{
    public interface iDatabaseForm
    {
        string ConnectionString();
        string Provider();
        bool Validate();
    }
}
