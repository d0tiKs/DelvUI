﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dalamud.Plugin;
using DelvUI.Config.Tree;
using DelvUI.Interface;
using ImGuiScene;

namespace DelvUI.Config {
    public class ConfigurationManager : IDisposable {
        private static ConfigurationManager _instance;

        public TextureWrap BannerImage;

        public string ConfigDirectory;

        public BaseNode ConfigBaseNode;

        public bool DrawConfigWindow;

        public ConfigurationManager(TextureWrap bannerImage, string configDirectory, BaseNode configBaseNode) {
            BannerImage = bannerImage;
            ConfigDirectory = configDirectory;
            ConfigBaseNode = configBaseNode;
            _instance = this;
            LoadConfigurations();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) {
            if (!disposing) {
                return;
            }

            _instance = null;
        }

        public static ConfigurationManager Initialize(DalamudPluginInterface pluginInterface) {
            BlackMageHudConfig blmConfig = new BlackMageHudConfig();

            return Initialize(pluginInterface, blmConfig);
        }

        public static ConfigurationManager Initialize(DalamudPluginInterface pluginInterface, params PluginConfigObject[] configObjects) {
            var node = new BaseNode();

            foreach (var configObject in configObjects) {
                node.GetOrAddConfig(configObject);
            }

            var banner = BuildBanner(pluginInterface);

            return new ConfigurationManager(banner, pluginInterface.GetPluginConfigDirectory(), node);
        }

        public static ConfigurationManager GetInstance() { return _instance; }

        private static TextureWrap BuildBanner(DalamudPluginInterface pluginInterface) {
            // var bannerImage = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Media", "Images", "banner_short_x150.png");
            //
            // if (File.Exists(bannerImage)) {
            //     try {
            //         return pluginInterface.UiBuilder.LoadImage(bannerImage);
            //     } catch (Exception ex) {
            //         PluginLog.Log($"Image failed to load. {bannerImage}");
            //         PluginLog.Log(ex.ToString());
            //     }
            // } else {
            //     PluginLog.Log($"Image doesn't exist. {bannerImage}");
            // }

            return null;
        }

        public void Draw() {
            if (DrawConfigWindow) {
                ConfigBaseNode.Draw();
            }
        }

        public void LoadConfigurations() { ConfigBaseNode.Load(ConfigDirectory); }

        public void SaveConfigurations() { ConfigBaseNode.Save(ConfigDirectory); }

        public PluginConfigObject GetConfiguration(PluginConfigObject configObject) { return ConfigBaseNode.GetOrAddConfig(configObject).ConfigObject; }
    }
}