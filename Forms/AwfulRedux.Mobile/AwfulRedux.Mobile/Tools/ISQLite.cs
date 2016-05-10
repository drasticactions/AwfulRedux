using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Interop;

namespace AwfulRedux.Mobile.Tools
{
    public interface ISQLite
    {
        ISQLitePlatform GetPlatform();

        string GetPath(string file);
    }
}
