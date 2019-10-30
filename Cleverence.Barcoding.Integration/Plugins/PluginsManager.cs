using System.Collections.Generic;
using Android.Content;
using Android.Content.PM;

using System;

namespace Cleverence.Warehouse.Compact.Plugins
{
    public interface IPluginConnection : IServiceConnection
    {
        bool IsConnected { get; }
    }

    public abstract class PluginManagerBase
    {
        public static string PluginsAssetsDir = "PluginsIntegration";
        public static string[] NotFoundPlugins()
        {
            return notFound.ToArray();
        }
        protected static Dictionary<string, string> plugins = new Dictionary<string, string>();
        protected static List<string> notFound = new List<string>();

        public static List<Tuple<string, string, Version>> FindPlugins(IEnumerable<string> names)
        {
            var ret = new List<Tuple<string, string, Version>>(5);

            var packageManager = CommonContext.PackageManager;

            foreach (var item in names)
            {
                //var intent = new Intent(item);

                var list = packageManager.GetInstalledApplications(PackageInfoFlags.Services);

                if (list != null)
                {
                    foreach (var item2 in list)
                    {
                        if (item2.PackageName == item)
                        {
                            var info = packageManager.GetPackageInfo(item, PackageInfoFlags.Services);


                            var version = Version.Parse(info.VersionName);

                            ret.Add(new Tuple<string, string, Version>(item, info.ApplicationInfo.NonLocalizedLabel.ToString(), version));

                            break;
                        }
                    }
                }
            }

            return ret;
        }

        public static Context CommonContext
        {
            get;
            set;
        }

        public static bool FindPlugins(string pluginName)
        {
            var packageManager = CommonContext.PackageManager;
            var intent = new Intent(pluginName);
            var list = packageManager.QueryIntentServices(intent, PackageInfoFlags.Services);

            bool bres = false;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    bres = true;
                    var serviceInfo = item.ServiceInfo;
                    if (!plugins.ContainsKey(serviceInfo.Name))
                        plugins.Add(serviceInfo.Name, serviceInfo.ApplicationInfo.PackageName);
                }
            }
            else
            {
                bres = FindServiceInAllPackages(pluginName);
            }
            return bres;
        }

        private static bool FindServiceInAllPackages(string pluginName)
        {
            var pkgs = CommonContext.PackageManager.GetInstalledPackages(PackageInfoFlags.Services);
            var result = new List<ResolveInfo>();
            bool bres = false;
            foreach (var pkg in pkgs)
            {
                if (pkg.Services == null)
                    continue;
                foreach (var svc in pkg.Services)
                {
                    if (svc.Name == pluginName)
                    {
                        bres = true;
                        if (!plugins.ContainsKey(svc.Name))
                            plugins.Add(svc.Name, svc.ApplicationInfo.PackageName);
                    }
                }
            }
            return bres;
        }

#if !TESTCASHREGISTER
        /*public static bool IsPluginFileInMainApp(string name)
        {
            var list = AndroidOps.GetAssetNamesInDir(PluginsAssetsDir);

            if (list.Length > 0)
            {
                var fileName = name + ".apk";

                foreach (var item in list)
                {
                    if (item.EndsWith(fileName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }*/
#endif

        private static readonly string _urlPluginTemplate = @"http://cleverence.ru/downloads/plugins/{0}";

        /*public static void DeployFromUrl(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName))
                return;

            string address = string.Format(_urlPluginTemplate, fileName);
#if !TESTCASHREGISTER
            AndroidOps.DeployFromUrl(FormStack.OwnerForm, address, fileName);
#endif

        }*/
    }

    public abstract class PluginManagerBase<T> : PluginManagerBase
        where T : class, IPluginConnection
    {
        protected static Dictionary<string, T> startedPlugins = new Dictionary<string, T>();

        protected T GetRemoteService(string pluginName, string pluginIntentName)
        {
            FindPlugins(pluginIntentName);

            if (plugins.ContainsKey(pluginName))
            {
                if (notFound.Contains(pluginName))
                {
                    notFound.Remove(pluginName);
                }

                if (!startedPlugins.ContainsKey(pluginName))
                {
                    var name1 = new ComponentName(plugins[pluginName], pluginName);
                    var i = new Intent();
                    i.SetComponent(name1);

                    var myServiceConnection = CreatePluginConnection();
                    StartService(i);

                    if (BindService(i, myServiceConnection, Bind.AutoCreate))
                    {
                        startedPlugins.Add(pluginName, myServiceConnection);
                        OnReturnCreatePluginConnection(myServiceConnection);
                        return myServiceConnection;
                    }

                    return null;
                }
                else
                {
                    return startedPlugins[pluginName];
                }
            }

            if (!notFound.Contains(pluginName))
            {
                notFound.Add(pluginName);
            }

            return null;
        }

        protected abstract T CreatePluginConnection();
        protected virtual void OnReturnCreatePluginConnection(T pluginConnection)
        { }

        public static void RemoveService(string name)
        {
            if (startedPlugins.ContainsKey(name))
            {
                var service = startedPlugins[name];

                if (service.IsConnected)
                {
                    UnbindService(service);
                    StopServiceInt(name);
                }

                startedPlugins.Remove(name);
            }
        }

        public static void StopService(string pluginName)
        {
            if (!plugins.ContainsKey(pluginName))
                FindPlugins(pluginName);
            StopServiceInt(pluginName);
        }

        private static void StopServiceInt(string pluginName)
        {
            if (!plugins.ContainsKey(pluginName))
                return;
            var name1 = new ComponentName(plugins[pluginName], pluginName);
            var i = new Intent();
            i.SetComponent(name1);
            StopService(i);
        }

        private static void StartService(Intent i)
        {
#if TESTCASHREGISTER
            Common.Context.StartService(i);
#else
            CommonContext.StartService(i);
#endif
        }

        private static bool BindService(Intent i, IServiceConnection connection, Bind b)
        {
#if TESTCASHREGISTER
            return Common.Context.BindService(i, connection, b);
#else
            return CommonContext.BindService(i, connection, b);
#endif
        }

        private static void UnbindService(IServiceConnection connection)
        {
#if TESTCASHREGISTER
            Common.Context.UnbindService(connection);
#else
            CommonContext.UnbindService(connection);
#endif
        }

        private static void StopService(Intent i)
        {
#if TESTCASHREGISTER
            Common.Context.StopService(i);
#else
            CommonContext.StopService(i);
#endif
        }
    }

#if !TESTCASHREGISTER
    public class BarcodeDevicePluginsManager : PluginManagerBase<PluginBarcodeScannerConnection>
    {
        private static string barcodePluginIntentName = "com.cleverence.intent.barcodeDevicePlugin";

        protected override PluginBarcodeScannerConnection CreatePluginConnection()
        {
            return new PluginBarcodeScannerConnection();
        }

        public PluginBarcodeScannerConnection GetRemoteBarcodeScannerService(string name)
        {
            return this.GetRemoteService(name, barcodePluginIntentName);
        }

        //private static void FindPlugins()
        //{
        //    var packageManager = Common.Context.PackageManager;
        //    var intent = new Intent(barcodePluginIntentName);
        //    var list = packageManager.QueryIntentServices(intent, PackageInfoFlags.Services);

        //    if (list.Count > 0)
        //    {
        //        foreach (var item in list)
        //        {
        //            var serviceInfo = item.ServiceInfo;

        //            if (!plugins.ContainsKey(serviceInfo.Name))
        //            {
        //                plugins.Add(serviceInfo.Name, serviceInfo.ApplicationInfo.PackageName);
        //            }
        //        }
        //    }
        //}

        //private static Dictionary<string, PluginBarcodeScannerConnection> startedPlugins = new Dictionary<string, PluginBarcodeScannerConnection>();

        //public static PluginBarcodeScannerConnection GetRemoteBarcodeScannerService(string name)
        //{
        //    FindPlugins(barcodePluginIntentName);

        //    if (plugins.ContainsKey(name))
        //    {
        //        if (notFound.Contains(name))
        //        {
        //            notFound.Remove(name);
        //        }

        //        if (!startedPlugins.ContainsKey(name))
        //        {
        //            var name1 = new ComponentName(plugins[name], name);
        //            var i = new Intent();
        //            i.SetComponent(name1);

        //            var myServiceConnection = new PluginBarcodeScannerConnection();

        //            FormStack.ApplicationContext.StartService(i);

        //            if (FormStack.ApplicationContext.BindService(i, myServiceConnection, Bind.AutoCreate))
        //            {
        //                startedPlugins.Add(name, myServiceConnection);

        //                return myServiceConnection;
        //            }

        //            return null;
        //        }
        //        else
        //        {
        //            return startedPlugins[name];
        //        }
        //    }

        //    if (!notFound.Contains(name))
        //    {
        //        notFound.Add(name);
        //    }

        //    return null;
        //}

        //public static string PluginsAssetsDir = "PluginsIntegration";

        //public static bool IsPluginFileInMainApp(string name)
        //{
        //    var list = AndroidOps.GetAssetNamesInDir(PluginsAssetsDir);

        //    if (list.Length > 0)
        //    {
        //        var fileName = name + ".apk";

        //        foreach (var item in list)
        //        {
        //            if (item.EndsWith(fileName))
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        //public static void RemoveRemoteBarcodeScannerService(string name)
        //{
        //    if (startedPlugins.ContainsKey(name))
        //    {
        //        var service = startedPlugins[name];

        //        if (service.IsConnected)
        //        {
        //            FormStack.ApplicationContext.UnbindService(service);

        //            var name1 = new ComponentName(plugins[name], name);
        //            var i = new Intent();
        //            i.SetComponent(name1);

        //            FormStack.ApplicationContext.StopService(i);

        //            //var name1 = new ComponentName(plugins[name], name);
        //            ////var i = new Intent();
        //            //i.SetComponent(name1);

        //            //Common.Context.StopService(i);
        //        }

        //        startedPlugins.Remove(name);
        //    }
        //}
    }
#endif
}