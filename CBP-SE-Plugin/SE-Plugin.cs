/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System;
using System.IO;
using System.Windows;
using CBPSDK;

namespace CBP_SE_Plugin
{
    public class SE_Plugin : IPluginCBP
    {
        public string PluginTitle => "Sound.xml Editor (RoB + music tracks)";

        public string PluginVersion => "0.1.3";//Assembly.GetExecutingAssembly().GetName().Version.ToString() would mean not updating two fields each time, but surely would be comparatively slower?

        public string PluginAuthor => "MHLoppy";

        public bool CBPCompatible => true;

        public bool DefaultMultiplayerCompatible => true;

        public string PluginDescription => "A graphical interface (GUI) to edit parts of RoN's sound.xml file."
                                            + "\n\nMain features:"
                                            + "\n- Install Rise of Babel taunts (English, text only)"
                                            + "\n- Configure music tracks based on presets (including music bugfix for Rise of Babel installs)"
                                            + "\n\nUnloading this plugin will undo all of its changes. Reload the plugin to re-select choices."
                                            + "\n\nPlugin source code: https://github.com/MHLoppy/CBP-SE-Plugin";

        public bool IsSimpleMod => false;

        public string LoadResult { get; set; }

        private string soundOrig;
        private string SEFolder;
        private string loadedSE;

        private string MTPFolder;
        private string loadedMTP;
        private bool warnedMTP = false;

        public void DoSomething(string workshopModsPath, string localModsPath)
        {
            soundOrig = Path.GetFullPath(Path.Combine(localModsPath, @"..\", "Data", "sound.xml"));
            SEFolder = Path.GetFullPath(Path.Combine(localModsPath, @"..\", "CBP", "SE"));
            loadedSE = Path.Combine(SEFolder, "soundeditorplugin.txt");

            // only needed for the compatibility check / warning (can be removed when MT plugin is removed from CBP)
            MTPFolder = Path.GetFullPath(Path.Combine(localModsPath, @"..\", "CBP", "MTP"));
            loadedMTP = Path.Combine(MTPFolder, "musictracksplugin.txt");

            //if folder doesn't exist, make it
            if (!Directory.Exists(SEFolder))
            {
                try
                {
                    Directory.CreateDirectory(SEFolder);
                    LoadResult = (PluginTitle + " detected for first time. Doing first-time setup.");
                }
                catch (Exception ex)
                {
                    LoadResult = (PluginTitle + ": error writing first-time file:\n\n" + ex);
                }
            }
            else
            {
                LoadResult = (SEFolder + " already exists; no action taken.");
            }

            //if file doesn't exist, make one
            if (!File.Exists(loadedSE))
            {
                try
                {
                    File.WriteAllText(loadedSE, "0");
                    LoadResult = (PluginTitle + " completed first time setup successfully. Created file:\n" + loadedSE);
                    //MessageBox.Show(PluginTitle + ": Created file:\n" + loadedSE);//removed to reduce number of popups for first-time CBP users
                }
                catch (Exception ex)
                {
                    LoadResult = (PluginTitle + ": error writing first-time file:\n\n" + ex);
                }
            }
            else
            {
                LoadResult = (loadedSE + " already exists; no action taken.");
            }

            CheckIfLoaded();//this can be important to do here, otherwise the bool might be accessed without a value depending on how other stuff is set up
        }

        public bool CheckIfLoaded()
        {
            if (File.ReadAllText(loadedSE) != "0")
            {
                if (!LoadResult.Contains("is loaded"))
                {
                    LoadResult += "\n\n" + PluginTitle + " is loaded.";
                }
                return true;
            }
            else
            {
                if (!LoadResult.Contains("is not loaded"))
                {
                    LoadResult += "\n\n" + PluginTitle + " is not loaded.";
                }
                return false;
            }
        }

        public void LoadPlugin(string workshopModsPath, string localModsPath)
        {
            try
            {
                //ensures minimal compatibility with Music Tracks Selector plugin - this function will be disabled at the same time the MT plugin gets removed
                if (MTLoaded() && (warnedMTP == false))
                {
                    warnedMTP = true;

                    MessageBox.Show("It looks like you have the Music Tracks Selector plugin loaded. That plugin's functionality is being merged into a larger Sound.xml Editor plugin."
                        + "\n\nPlease unload the Music Tracks selector plugin, then use the new Sound Editor plugin to choose your music tracks instead. The old plugin will be removed soon.");
                }

                BackupSoundXML();
                new RoBInstallerWindow().Show();//RoB window then cycles to the Music Tracks selector window

                File.WriteAllText(loadedSE, "1");
                CheckIfLoaded();
                LoadResult = (PluginTitle + " was loaded.");
            }
            catch (Exception ex)
            {
                LoadResult = (PluginTitle + " had an error while loading: " + ex);
                MessageBox.Show("Error while loading:\n\n" + ex);
            }
        }

        public void UnloadPlugin(string workshopModsPath, string localModsPath)
        {
            try
            {
                RestoreSoundXML();

                File.WriteAllText(loadedSE, "0");
                CheckIfLoaded();
                LoadResult = (PluginTitle + ": Previous sound.xml file has been restored.");
                MessageBox.Show("Previous sound.xml file has been restored.");
            }
            catch (Exception ex)
            {
                LoadResult = (PluginTitle + " had an error while unloading: " + ex);
                MessageBox.Show("Error while unloading:\n\n" + ex);
            }
        }

        public void UpdatePlugin(string workshopModsPath, string localModsPath)
        {
            //not needed, so do nothing - plugin itself is kept updated by Steam
        }

        private void BackupSoundXML()
        {
            File.Copy(soundOrig, Path.Combine(SEFolder, "sound.xml"), true);
        }

        private void RestoreSoundXML()
        {
            File.Copy(Path.Combine(SEFolder, "sound.xml"), soundOrig, true);
        }

        private bool MTLoaded()
        {
            if ((File.Exists(loadedMTP)) && (File.ReadAllText(loadedMTP) == "1"))
                return true;
            else
                return false;
        }

        // note also that the RoB plugin was never publicly released, so no need to worry about its settings etc

        //decided not to bother fully implementing this - only takes users 5-10 seconds to do this in GUI, but would take far more than 500-1000 seconds to implement doing it automatically
        /*private void ImportSettings()//import settings from Music Tracks plugin
        {

        }*/
    }
}
