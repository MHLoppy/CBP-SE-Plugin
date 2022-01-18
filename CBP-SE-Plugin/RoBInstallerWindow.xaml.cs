/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System;
using System.IO;
using System.Windows;
using System.Xml;

namespace CBP_SE_Plugin
{
    public partial class RoBInstallerWindow : Window
    {
        private readonly string SoundXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sound.xml");
        private static readonly string workshopRoB = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..", @"workshop\content\287450\2287791153\Rise of Babel"));

        private XmlDocument doc = new XmlDocument();

        public RoBInstallerWindow()
        {
            InitializeComponent();
            DataContext = this;

            CleanFile();
        }

        // 1) check if RoB is >>already<< installed in the sound.xml file - we want to avoid unnecessarily installing over an existing RoB install
        // --> do this by checking if taunt 201 exists - in a default sound.xml file this won't exist, but in all RoB languages it will
        // 2) create 201-999 with a silence file, no id, volume 255, and <string> content from pre-compiled RoB list

        // could actually support multiple languages without --too-- much effort I think - I already have all the translations on hand
        // update: turns out most of the "translations" provided with RoB don't actually include translations lol

        private void CleanFile()
        {
            try
            {
                //pre-load the file as XML to make the search faster (can focus single node instead of whole file - potentially a big saving for RoB users with a comparatively large file)
                //a little janky but still better than doing a full-text search
                doc.Load(SoundXML);
                XmlNode intros = doc.SelectSingleNode("ROOT/TRACKS/INTROS");

                //pre-treat to remove bad chars found in default sound.xml file
                if (intros.InnerText.Contains("--"))
                {
                    string text = File.ReadAllText(SoundXML);
                    text = text.Replace("/>--", "/>");
                    File.WriteAllText(SoundXML, text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cleaning XML file: " + ex);
            }
        }

        private void Check201(string languageChoice)//this evolved from -only- checking to both checking -and- copying because it made code layout easier (but not e.g. too complex)
        {
            try
            {
                doc.Load(SoundXML);

                XmlNode taunt201 = doc.SelectSingleNode("ROOT/TAUNTS/TAUNT[201]");
                if (taunt201 != null)
                {
                    if (MessageBox.Show("It looks like custom taunts (such as RoB) may already be installed. Continue with RoB installation anyway?", "Possible conflict", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        MessageBox.Show("No action taken.");
                        return;
                    }
                }

                //else continue: copy modified sound.xml to new location
                File.Copy(Path.Combine(workshopRoB, languageChoice, "sound.xml"), SoundXML, true);

                MessageBox.Show("Rise of Babel taunts (text only) - " + languageChoice + " were installed successfully");

                Close();
                new MusicTracksSelectionWindow().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:\n\n" + ex);
            }
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            Check201("English");
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new MusicTracksSelectionWindow().Show();
        }
    }
}
