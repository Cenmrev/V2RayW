using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Web.Script.Serialization;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using V2RayW.Resources;

namespace V2RayW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int pacListMenuItemIndex = 11;
        private const int serverMenuListIndex = 9;
        private const int routingRuleSetMenuItemIndex = 10;


        private TaskbarIcon notifyIcon;


        public bool proxyState = false;
        public ProxyMode proxyMode = ProxyMode.manual;
        public int localPort = 1081;
        public int httpPort = 8001;
        public bool udpSupport = false;
        public bool shareOverLan = false;
        public bool useCusProfile= false;
        public bool useMultipleServer = false;
        public int selectedServerIndex = 0;
        private string selectedCusConfig = "";
        public int selectedRoutingSet = 0;
        public string selectedPacFileName = "pac.js";
        public string dnsString = "localhost";
        public List<Dictionary<string, object>> profiles = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> subsOutbounds = new List<Dictionary<string, object>>();

        public List<string> cusProfiles = new List<string>();
        public string logLevel = "none";
        public bool enableRestore = false;
        public List<string> subscriptions = new List<string>();
        public List<Dictionary<string, object>> routingRuleSets = new List<Dictionary<string, object>> { Utilities.ROUTING_GLOBAL, Utilities.ROUTING_DIRECT, Utilities.ROUTING_BYPASSCN_PRIVATE_APPLE };

        private FileSystemWatcher pacFileWatcher;

        public MainWindow()
        {
            Hide();
            InitializeComponent();
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            this.mainMenu = FindResource("TrayMenu") as ContextMenu;
            if(CheckFiles() == false)
            {
                Application.Current.Shutdown();
                return;
            }
            // read config
            ReadSettings();


            this.InitializeHttpServer();
            this.InitializeCoreProcess();

            pacFileWatcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory + @"pac\")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = selectedPacFileName
            };
            pacFileWatcher.Changed += (object source, FileSystemEventArgs fe) =>
            {
                Debug.WriteLine("detect pac content change change");
                if (proxyState == true && proxyMode == ProxyMode.pac)
                {
                    UpdateSystemProxy();
                }
            };
            pacFileWatcher.EnableRaisingEvents = true;

            configScanner.DoWork += ConfigScanner_DoWork;
            configScanner.RunWorkerCompleted += ConfigScanner_RunWorkerCompleted;
            configScanner.RunWorkerAsync();

            OverallChanged(this, null);
        }

        #region startup check
        private bool ClearLog()
        {
            if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"log\"))
            {
                try
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"log\");
                } catch
                {
                    MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + @"log\", Strings.messagedircreatefail);
                    return false;
                }
            }
            try
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"log\access.log").Close();
                File.Create(AppDomain.CurrentDomain.BaseDirectory + @"log\error.log").Close();
            } catch
            {
                MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory + "log\\access.log\n" + AppDomain.CurrentDomain.BaseDirectory + @"log\access.log", Strings.messagedircreatefail);
                return false;
            }
            return true;
        }

        BackgroundWorker coreDownloader;

        private void CoreDownloader_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DownloadCore();
                Dispatcher.Invoke(() =>
                {
                    notifyIcon.ShowBalloonTip("", Strings.messagedownloadsuccess, BalloonIcon.Info);
                    (mainMenu.Items[1] as MenuItem).IsEnabled = true;
                });
            }
            catch
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(Strings.messagefilecreatefail);
                    Application.Current.Shutdown();
                });
            }
        }

        void DownloadCore()
        {
            string coreDirectory = AppDomain.CurrentDomain.BaseDirectory + @"v2ray-core\";
            if (Directory.Exists(coreDirectory))
            {
                Directory.Delete(coreDirectory, true);
                Debug.WriteLine("core dir is deleted");
            }
            string downloadLink = $"https://raw.githubusercontent.com/v2ray/dist/master/v2ray-windows-{(Environment.Is64BitOperatingSystem ? "64" : "32")}.zip";
            WebClient client = new WebClient();
            client.DownloadFile(downloadLink, AppDomain.CurrentDomain.BaseDirectory + "v2ray.zip");
            Debug.WriteLine("core downloaded");
            ZipArchive archive = ZipFile.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "v2ray.zip");
            archive.ExtractToDirectory(AppDomain.CurrentDomain.BaseDirectory + @"v2ray-core\");
            Debug.WriteLine("core is extraced");
        }

        private bool CheckFiles()
        {
            #region pac, config, log dir 
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string folder in new string[] {@"pac\", @"config\", @"config\" }) {
                if (!Directory.Exists(currentDir + folder))
                {
                    try
                    {
                        Directory.CreateDirectory(currentDir + folder);
                    } catch
                    {
                        MessageBox.Show(currentDir + folder, Strings.messagedircreatefail);
                        return false;
                    }
            }
            }
            if(ClearLog() == false) {
                return false;
            }
            #endregion

            #region check core

            bool findMissingFile = false;
            string coreDirectory = AppDomain.CurrentDomain.BaseDirectory + @"v2ray-core\";
            foreach (string file in Utilities.necessaryFiles)
            {
                if(!File.Exists(coreDirectory + file))
                {
                    findMissingFile = true;
                    break;
                }
            }
            if(findMissingFile)
            {
                var result = MessageBox.Show(Strings.messagenocore, Strings.messagenocoretitle, MessageBoxButton.YesNoCancel);
                switch(result)
                {
                    case MessageBoxResult.Yes:
                        {
                            proxyState = false;
                            notifyIcon.ShowBalloonTip("", Strings.messagedownloading, BalloonIcon.Info);
                            coreDownloader = new BackgroundWorker();
                            coreDownloader.DoWork += CoreDownloader_DoWork;
                            coreDownloader.RunWorkerAsync();
                            return true;
                        }
                    case MessageBoxResult.No:
                        {
                            Process.Start(Strings.coreHelppage);
                            return false;
                        }
                    default:
                        {
                            return false;
                        }
                }

            } else
            {
                (mainMenu.Items[1] as MenuItem).IsEnabled = true;
                return true;
            }
            #endregion 
        }

        #endregion

        #region read/write settings
        private void WriteSettings()
        {
            var settingPath = AppDomain.CurrentDomain.BaseDirectory + "settings.json";
            var settings = new Dictionary<string, object>
            {
                {
                    "appStatus",
                    new Dictionary<string, object>
                    {
                        { "proxyState", proxyState },
                        { "proxyMode", proxyMode },
                        { "selectedServerIndex", selectedServerIndex },
                        { "selectedCusConfig", selectedCusConfig },
                        { "selectedRoutingSet", selectedRoutingSet },
                        { "useMultipleServer", useMultipleServer },
                        { "useCusProfile", useCusProfile }
                    }
                },
                { "selectedPacFileName", selectedPacFileName },
                { "logLevel", logLevel },
                { "localPort", localPort },
                { "httpPort", httpPort },
                { "udpSupport", udpSupport },
                { "shareOverLan", shareOverLan },
                { "dnsString", dnsString },
                { "enableRestore", enableRestore },
                { "profiles", profiles },
                { "subscriptions", subscriptions },
                { "routingRuleSets", routingRuleSets },
            };
            File.WriteAllText(settingPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

        private void ReadSettings()
        {
            var settingPath = AppDomain.CurrentDomain.BaseDirectory + "settings.json";
            if (!File.Exists(settingPath))
            {
                WriteSettings();
            } else
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                var settingString = File.ReadAllText(settingPath);

                dynamic settings = javaScriptSerializer.Deserialize<dynamic>(settingString);
                try
                {
                    proxyState = settings["appStatus"]["proxyState"];
                    proxyMode = (ProxyMode)settings["appStatus"]["proxyMode"];
                    selectedServerIndex = settings["appStatus"]["selectedServerIndex"];
                    selectedCusConfig = settings["appStatus"]["selectedCusConfig"]; 
                    selectedRoutingSet = settings["appStatus"]["selectedRoutingSet"];
                    useMultipleServer = settings["appStatus"]["useMultipleServer"];
                    useCusProfile = settings["appStatus"]["useCusProfile"];

                    selectedPacFileName = settings["selectedPacFileName"];
                    logLevel = settings["logLevel"];
                    localPort = (int)settings["localPort"];
                    httpPort = (int)settings["httpPort"];
                    udpSupport = settings["udpSupport"];
                    shareOverLan = settings["shareOverLan"];
                    dnsString = settings["dnsString"];
                    enableRestore = settings["enableRestore"];
                    
                    
                    foreach(dynamic profile in settings["profiles"])
                    {
                        try
                        {
                            if(Utilities.RESERVED_TAGS.FindIndex(x => x == profile["tag"] as string) == -1)
                            {
                                profiles.Add(profile as Dictionary<string, object>);
                            }
                        } catch { continue; }
                    }
                    foreach(string subscription in settings["subscriptions"])
                    {
                        subscriptions.Add(subscription);
                    }
                    routingRuleSets.Clear();
                    foreach(Dictionary<string, object> set in settings["routingRuleSets"])
                    {
                        routingRuleSets.Add(set);
                    }
                    Debug.WriteLine($"read {routingRuleSets.Count} rules");
                    if (routingRuleSets.Count == 0)
                    {
                        routingRuleSets = new List<Dictionary<string, object>> { Utilities.ROUTING_GLOBAL, Utilities.ROUTING_DIRECT, Utilities.ROUTING_BYPASSCN_PRIVATE_APPLE };
                        Debug.WriteLine("reset routing rules");
                    }
                } 
                catch
                {
                    notifyIcon.ShowBalloonTip("", Strings.messagereaddefaultserror, BalloonIcon.Info);
                }
            }
        }
       
        #endregion

        private ContextMenu mainMenu;

        #region mode and status management

        private void UpdateStatusAndModeMenus()
        {
            if (proxyState)
            {
                (mainMenu.Items[0] as MenuItem).Header = Strings.coreloaded;
                (mainMenu.Items[1] as MenuItem).Header = Strings.unloadcore;
            } else
            {
                (mainMenu.Items[0] as MenuItem).Header = Strings.coreunloaded;
                (mainMenu.Items[1] as MenuItem).Header = Strings.loadcore;
            }
            for (int i = 0; i < 3; i += 1)
            {
                (mainMenu.Items[i + 5] as MenuItem).IsChecked = (int)proxyMode == i;
            }
        }

        void ModeChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"mode changed by {sender} {e}");
            MenuItem senderItem = sender as MenuItem;
            int senderTag = Int32.Parse(senderItem.Tag as string);
            if(proxyState == true && proxyMode == ProxyMode.manual && senderTag != (int)ProxyMode.manual)
            {
                this.BackupSystemProxy();
            }
            if (proxyState == true && proxyMode != ProxyMode.manual && senderTag == (int)ProxyMode.manual)
            {
                if(enableRestore)
                {
                    RestoreSystemProxy();
                } else
                {
                    CancelSystemProxy();
                }
            }
            proxyMode = (ProxyMode)senderTag;
            this.UpdateStatusAndModeMenus();
            if(senderTag == (int)ProxyMode.pac)
            {
                this.UpdatePacMenuList();
            }
            if(proxyState == true)
            {
                this.UpdateSystemProxy();
            }
        }

        public void OverallChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("refresh all settings");
            bool previousStatus = proxyState;
            if (sender == this)
            {
                previousStatus = false;
            }
            else if (sender == mainMenu.Items[1] || sender == v2rayCoreWorker)
            {
                proxyState = !proxyState;
            }
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"pac\" + selectedPacFileName))
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"pac\" + selectedPacFileName, Properties.Resources.simplepac);
            }

            selectedServerIndex = Math.Min(profiles.Count + subsOutbounds.Count - 1, selectedServerIndex);
            if (profiles.Count + subsOutbounds.Count > 0)
            {
                selectedServerIndex = Math.Max(selectedServerIndex, 0);
            }
            if(!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"config\" + selectedCusConfig))
            {
                selectedCusConfig = "";
            }
            selectedRoutingSet = Math.Min(routingRuleSets.Count - 1, selectedRoutingSet);
            if (routingRuleSets.Count > 0)
            {
                selectedRoutingSet = Math.Max(selectedRoutingSet, 0);
            }

            if ((!useMultipleServer && selectedServerIndex == -1 && selectedCusConfig == "") ||
                (useMultipleServer && profiles.Count + subsOutbounds.Count < 1))
            {
                proxyState = false;
            } else if (!useMultipleServer && selectedCusConfig == "")
            {
                useCusProfile = false;
            } else if (!useMultipleServer && selectedServerIndex == -1)
            {
                useCusProfile = true;
            }

            if (proxyMode != ProxyMode.manual)
            {
                if(previousStatus == false && proxyState == true)
                {
                    this.BackupSystemProxy();
                } else if (previousStatus == true && proxyState == false)
                {
                    if (enableRestore)
                    {
                        RestoreSystemProxy();
                    } else
                    {
                        CancelSystemProxy();
                    }
                }
            }

            this.CoreConfigChanged(this);

            if (proxyState == true)
            {
                this.UpdateSystemProxy();
            }
            else if (sender == mainMenu.Items[1])
            {
                this.UnloadV2ray();
            }
            this.UpdateStatusAndModeMenus();
            this.UpdatePacMenuList();
            if(sender.GetType().Equals(typeof(ConfigWindow)))
            {
                WriteSettings();
            }
        }
        #endregion

        #region system proxy management
        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/19517edf-8348-438a-a3da-5fbe7a46b61a/how-to-change-global-windows-proxy-using-c-net-with-immediate-effect?forum=csharpgeneral
        [DllImport("wininet.dll")]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        const int INTERNET_OPTION_REFRESH = 37;
        const int INTERNET_OPTION_PROXY_SETTINGS_CHANGED = 95;

        public void BackupSystemProxy()
        {
            ;
        }

        public void RestoreSystemProxy()
        {
            ;
        }

        
        Random paccounter = new Random(); // to force windows refresh pac files

        public void UpdateSystemProxy()
        {
            if (proxyState == false || proxyMode == ProxyMode.manual)
            {
                return;
            }
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            if (proxyMode == ProxyMode.pac)
            {
                registry.SetValue("ProxyEnable", 0);
                registry.SetValue("AutoConfigURL", $"http://127.0.0.1:18000/proxy.pac/{paccounter.Next()}", RegistryValueKind.String);
            } else if (proxyMode == ProxyMode.global)
            {
                registry.SetValue("ProxyEnable", 1);
                var proxyServer = $"http://127.0.0.1:{httpPort}";
                var proxyOverride = "<local>;localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;172.32.*;192.168.*";
                registry.SetValue("ProxyServer", proxyServer);
                registry.SetValue("ProxyOverride", proxyOverride);
                registry.DeleteValue("AutoConfigURL", false);
            }
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        void CancelSystemProxy()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 0);
            registry.DeleteValue("AutoConfigURL", false);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        #endregion
        

        #region pac management

        private void UpdatePacMenuList()
        {
            var pacMenuList = mainMenu.Items[pacListMenuItemIndex] as MenuItem;
            pacMenuList.Items.Clear();
            var pacDir = AppDomain.CurrentDomain.BaseDirectory + @"pac\";
            DirectoryInfo pacDirInfo = new DirectoryInfo(pacDir);
            var pacFiles = pacDirInfo.GetFiles("*.js", SearchOption.TopDirectoryOnly);
            int i = 0;
            foreach(var pacFile in pacFiles)
            {
                MenuItem menuItem = new MenuItem
                {
                    Header = "_" + pacFile.Name,
                    Tag = i,
                    IsCheckable = true,
                    IsChecked = pacFile.Name == selectedPacFileName
                };
                menuItem.Click += SwitchPac;
                pacMenuList.Items.Add(menuItem);
                i += 1;
            }
            pacMenuList.Items.Add(new Separator());
            pacMenuList.Items.Add(FindResource("editPacMenuItem"));
            pacMenuList.Items.Add(FindResource("resetPacButton"));
        }

        private void SwitchPac(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"pac switched by {sender} {e}");
            this.selectedPacFileName = ((sender as MenuItem).Header as string).Substring(1); // exclude leading _
            pacFileWatcher.Filter = selectedPacFileName;
            if (proxyState == true && proxyMode == ProxyMode.pac)
            {
                UpdateSystemProxy();
                UpdatePacMenuList();
            }
        }

        private void EditPacMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "/select, \"" + AppDomain.CurrentDomain.BaseDirectory + @"pac\" + selectedPacFileName + @"\");
        }

        #endregion

        #region server info management

        private const int useAllServerTag = -10;
        private const int useCusConfigTag = -11;

        private BackgroundWorker configScanner = new BackgroundWorker();

        private void ConfigScanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(() => { UpdateServerMenuList(); });
        }

        private void ConfigScanner_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("will scan configs");
            Process v2rayProcess = new Process();
            v2rayProcess.StartInfo.FileName = Utilities.corePath;

            DirectoryInfo configDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"config\");
            var cusConfigs = configDirectoryInfo.GetFiles("*.json", SearchOption.TopDirectoryOnly);
            cusProfiles.Clear();
            foreach (var cusConfig in cusConfigs)
            {
                Debug.WriteLine(v2rayProcess.StartInfo.FileName);
                v2rayProcess.StartInfo.Arguments = "-test -config " + "\"" + cusConfig.FullName + "\"";
                v2rayProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                try
                {
                    v2rayProcess.Start();
                    v2rayProcess.WaitForExit();
                    Debug.WriteLine("exit code is " + v2rayProcess.ExitCode.ToString());
                    if (v2rayProcess.ExitCode == 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            cusProfiles.Add(cusConfig.Name);
                        });
                    }
                }
                catch { };
            }
        }

        private void UpdateServerMenuList()
        {
            var serverMenuList = mainMenu.Items[serverMenuListIndex] as MenuItem;
            serverMenuList.Items.Clear();
            if (profiles.Count == 0 && cusProfiles.Count == 0 && subsOutbounds.Count == 0)
            {
                serverMenuList.Items.Add(Strings.messagenoserver);
                return;
            }
            int tagIndex = 0;
            foreach (Dictionary<string, object> outbound in this.profiles)
            {
                var newOutboundItem = new MenuItem
                {
                    Header = outbound["tag"],
                    Tag = tagIndex,
                    IsChecked = tagIndex == selectedServerIndex && !useMultipleServer && !useCusProfile
                };
                newOutboundItem.Click += SwitchServer;
                serverMenuList.Items.Add(newOutboundItem);
                tagIndex += 1;
            }
            foreach (Dictionary<string, object> outbound in this.subsOutbounds)
            {
                var newOutboundItem = new MenuItem
                {
                    Header = outbound["tag"],
                    Tag = tagIndex,
                    IsChecked = tagIndex == selectedServerIndex && !useMultipleServer && !useCusProfile
                };
                newOutboundItem.Click += SwitchServer;
                serverMenuList.Items.Add(newOutboundItem);
                tagIndex += 1;
            }
            if (profiles.Count + subsOutbounds.Count > 0)
            {
                serverMenuList.Items.Add(new Separator());
                var newItem = new MenuItem
                {
                    Header = Strings.useall,
                    Tag = useAllServerTag,
                    IsChecked = !useCusProfile && useMultipleServer
                };
                newItem.Click += SwitchServer;
                serverMenuList.Items.Add(newItem);
            }
            if (subscriptions.Count > 0)
            {
                serverMenuList.Items.Add(new Separator());
                serverMenuList.Items.Add(FindResource("updateSubscriptionMenuItem"));
            }
            if (cusProfiles.Count > 0)
            {
                serverMenuList.Items.Add(new Separator());
            }
            foreach (string cusConfigFileName in cusProfiles)
            {
                var newOutboundItem = new MenuItem
                {
                    Header = "_" + cusConfigFileName,
                    IsChecked = useCusProfile && cusConfigFileName == selectedCusConfig,
                    Tag = useCusConfigTag
                };
                newOutboundItem.Click += SwitchServer;
                serverMenuList.Items.Add(newOutboundItem);
            }
        }

        void SwitchServer(object sender, RoutedEventArgs e)
        {
            int outboundCount = profiles.Count + subsOutbounds.Count;
            int senderTag = (int)(sender as MenuItem).Tag;
            if (senderTag >= 0 && senderTag < outboundCount)
            {
                this.useMultipleServer = false;
                this.useCusProfile = false;
                this.selectedServerIndex = senderTag;
            } else if (senderTag == useCusConfigTag)
            {
                useMultipleServer = false;
                useCusProfile = true;
                selectedCusConfig = ((sender as MenuItem).Header as string).Substring(1);
            } else if (senderTag == useAllServerTag)
            {
                useMultipleServer = true;
                useCusProfile = false;
            }
            Debug.WriteLine("switch server");
            this.CoreConfigChanged(sender);
        }


        private void UpdateSubscription(object sender, RoutedEventArgs e)
        {

        }
        #endregion


        #region simple http server

        private static HttpListener listener = new HttpListener();
        static BackgroundWorker httpServerWorker = new BackgroundWorker();

        private void InitializeHttpServer()
        {
            listener.Prefixes.Add("http://127.0.0.1:18000/proxy.pac/");
            listener.Prefixes.Add("http://127.0.0.1:18000/config.json/");
            listener.Start();
            httpServerWorker.WorkerSupportsCancellation = true;
            httpServerWorker.DoWork += new DoWorkEventHandler(HttpServerWorker_DoWork);
            httpServerWorker.RunWorkerAsync();
        }

        private void HttpServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                Debug.WriteLine("get request "+request.Url+DateTime.Now.ToString());
                byte[] respondBytes = new byte[0];
                HttpListenerResponse response = context.Response;
                if (request.Url.AbsolutePath == "/config.json")
                {
                    respondBytes = v2rayJsonConfig;
                    response.ContentType = "application/json; charset=utf-8";
                } else if (request.Url.AbsolutePath.StartsWith("/proxy.pac"))
                {
                    Debug.WriteLine("pac file is requested");
                    // https://support.microsoft.com/en-us/help/4025058/windows-10-does-not-read-a-pac-file-referenced-by-a-file-protocol
                    response.ContentType = "application/x-ns-proxy-autoconfig";
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"pac\" + selectedPacFileName))
                    {
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"pac\" + selectedPacFileName, Properties.Resources.simplepac);
                    }
                    respondBytes = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"pac\" + selectedPacFileName);
                }
                // Obtain a response object.
                System.IO.Stream output = response.OutputStream;
                output.Write(respondBytes, 0, respondBytes.Length);
                output.Close();
            }
        }

        #endregion


        #region core load/unload management

        private static BackgroundWorker v2rayCoreWorker = new BackgroundWorker();
        private Process v2rayProcess;
        private Semaphore coreWorkerSemaphore = new Semaphore(0, 1);
        private bool coreKilledByMe = false;

        private void InitializeCoreProcess()
        {
            v2rayProcess = new System.Diagnostics.Process();
            v2rayProcess.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"v2ray-core\v2ray.exe";
            Debug.WriteLine(v2rayProcess.StartInfo.FileName);
            v2rayProcess.StartInfo.Arguments = @"-config http://127.0.0.1:18000/config.json";
#if DEBUG
            v2rayProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
#else
            v2rayProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
#endif
            v2rayCoreWorker.DoWork += V2rayCoreWorker_DoWork;
            v2rayCoreWorker.RunWorkerAsync();
        }

        private void V2rayCoreWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                coreWorkerSemaphore.WaitOne();
                v2rayProcess.Start();
                coreKilledByMe = false;
                v2rayProcess.WaitForExit();
                if (!coreKilledByMe)
                {
                    this.Dispatcher.Invoke(() => 
                    {
                        this.OverallChanged(v2rayCoreWorker, null);
                        notifyIcon.ShowBalloonTip("", Strings.messagecorequit, BalloonIcon.Warning);
                    });
                }
            }
        }

        public void UnloadV2ray()
        {
            try
            {
                coreKilledByMe = true;
                v2rayProcess.Kill();
            }
            catch {
            };
        }

        void ToggleCore()
        {
            this.UnloadV2ray();
            coreWorkerSemaphore.Release(1);
        }
#endregion

#region core config management
        byte[] v2rayJsonConfig = new byte[0];

        void CoreConfigChanged(object sender)
        {
            Debug.WriteLine($"{sender} calls config change");
            if (proxyState == true)
            {
                if(!useMultipleServer && useCusProfile)
                {
                    v2rayJsonConfig = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"config\" + selectedCusConfig);
                } else
                {
                    v2rayJsonConfig = this.GenerateConfigFile();
                }
                this.ToggleCore();
            }
            this.UpdateServerMenuList();
            this.UpdateRuleSetMenuList();
        }

        byte[] GenerateConfigFile()
        {
            Dictionary<string, object> fullConfig = Utilities.configTemplate;
            fullConfig["log"] = new Dictionary<string, string>
            {
#if DEBUG
                {"loglevel", "debug" }
#else
                { "error",AppDomain.CurrentDomain.BaseDirectory + @"log\error.log" },
                { "access", AppDomain.CurrentDomain.BaseDirectory + @"log\access.log"},
                {"loglevel", logLevel }
#endif
            };
            var dnsList = dnsString.Split(',');
            if(dnsList.Count() > 0)
            {
                fullConfig["dns"] = new Dictionary<string, string[]>
                {
                    { "servers", dnsList }
                };
            }
            fullConfig["inbounds"] = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "port", localPort },
                    { "listen", shareOverLan ? @"0.0.0.0" : @"127.0.0.1" },
                    {
                        "settings",
                        new Dictionary<string, object> { {"udp", udpSupport} }
                    },
                    { "protocol", "socks" }
                },
                new Dictionary<string, object>
                {
                    { "port", httpPort },
                    { "listen", shareOverLan ? @"0.0.0.0" : @"127.0.0.1" },
                    { "protocol", "http" }
                }
            };

            List<Dictionary<string, object>> allOutbounds = new List<Dictionary<string, object>>(profiles);
            allOutbounds.AddRange(subsOutbounds);

            Dictionary<string, object> outboundsForConfig = new Dictionary<string, object>();
            Dictionary<string, object> uniqueTagOutbounds = new Dictionary<string, object>();

            foreach(Dictionary<string, object> outbound in allOutbounds)
            {
                uniqueTagOutbounds[outbound[@"tag"].ToString()] = outbound;
            }
            var uniqueTagsExceptDirectDecline = new List<object>(uniqueTagOutbounds.Keys);
            uniqueTagOutbounds["direct"] = Utilities.OUTBOUND_DIRECT;
            uniqueTagOutbounds["decline"] = Utilities.OUTBOUND_DECLINE;

            fullConfig["routing"] = Utilities.DeepClone(routingRuleSets[selectedRoutingSet]);
            var currentRules = (fullConfig["routing"] as Dictionary<string, object>)["rules"] as IList<object>;
            foreach (Dictionary<string, object> aRule in currentRules)
            {
                if (aRule.ContainsKey("outboundTag") && aRule["outboundTag"].ToString() == "main")
                {
                    if (!useMultipleServer)
                    {
                        aRule["outboundTag"] = allOutbounds[selectedServerIndex][@"tag"].ToString();
                    } else
                    {
                        aRule.Remove("outboundTag");
                        aRule["balancerTag"] = "balance";
                    }
                }
            }
            bool useBalance = false;
            foreach (Dictionary<string, object> aRule in currentRules)
            {
                if(aRule.ContainsKey("balancerTag") && !aRule.ContainsKey("outboundTag"))
                {
                    useBalance = true;
                    break;
                } else
                {
                    if(uniqueTagOutbounds.ContainsKey(aRule["outboundTag"].ToString()))
                    {
                        outboundsForConfig[aRule["outboundTag"].ToString()] = uniqueTagOutbounds[aRule["outboundTag"].ToString()];
                    }
                }
            }
            if(useBalance)
            {
                (fullConfig["routing"] as Dictionary<string, object>).Add("balancers", new List<object>
                {
                    new Dictionary<string, object>
                    {
                        { "tag", "balance" },
                        { "selector", uniqueTagsExceptDirectDecline }
                    }
                }
                );
                fullConfig["outbounds"] = uniqueTagOutbounds.Values;
            } else
            {
                fullConfig["outbounds"] = outboundsForConfig.Values;
            }
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fullConfig, Formatting.Indented));
        }
#endregion

#region routingRules 
        void UpdateRuleSetMenuList()
        {
            Debug.WriteLine($"rules count = {routingRuleSets.Count}");
            MenuItem ruleSetMenuItem = mainMenu.Items[routingRuleSetMenuItemIndex] as MenuItem;
            ruleSetMenuItem.Items.Clear();
            int i = 0;
            foreach(Dictionary<string, object> rule in routingRuleSets)
            {
                MenuItem menuItem = new MenuItem
                {
                    Tag = i,
                    Header = "_" + rule["name"],
                    IsCheckable = true,
                    IsChecked = i == selectedRoutingSet,
                };
                Debug.WriteLine(menuItem.Tag.ToString());
                menuItem.Click += SwitchRoutingRuleSet;
                ruleSetMenuItem.Items.Add(menuItem);
                i += 1;
            }
        }

        void SwitchRoutingRuleSet(object sender, RoutedEventArgs e)
        {
            selectedRoutingSet = (int)(sender as MenuItem).Tag;
            Debug.WriteLine("switch routing rule set");
            this.CoreConfigChanged(sender);
        }

#endregion


#region other main menu items

        public void QuitV2RayW(object sender, RoutedEventArgs e)
        {
            notifyIcon.Icon = null;
            this.UnloadV2ray();
            if(proxyState && proxyMode != ProxyMode.manual)
            {
                if (enableRestore)
                {
                    RestoreSystemProxy();
                } else
                {
                    CancelSystemProxy();
                }
            }
            this.WriteSettings();
            pacFileWatcher.EnableRaisingEvents = false;
            Application.Current.Shutdown();
        }

        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            Process.Start(V2RayW.Resources.Strings.V2RayHomePage);
        }

        private void ShowConfigWindow(object sender, RoutedEventArgs e)
        {
            ConfigWindow configWindow = new ConfigWindow
            {
                mainWindow = this
            };
            configWindow.InitializeData();
            configWindow.Show();
        }

        private void ViewCurrentConfig(object sender, RoutedEventArgs e)
        {
            Process.Start("http://127.0.0.1:18000/config.json");
        }

#endregion

        private void ShowLogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"log\");
        }

        private void ResetPacButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"pac\pac.js", Properties.Resources.simplepac);
            UpdatePacMenuList();
        }
    }
}
