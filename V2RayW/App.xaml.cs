using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace V2RayW
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            (MainWindow as V2RayW.MainWindow).QuitV2RayW(sender, null);
        }
    }

}
