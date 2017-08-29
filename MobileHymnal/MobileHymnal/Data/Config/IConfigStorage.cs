using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileHymnal.Data.Config
{
    public interface IConfigStorage
    {
        ISettings GetSettings();
    }
}
