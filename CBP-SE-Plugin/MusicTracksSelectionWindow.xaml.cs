/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace CBP_SE_Plugin
{
    public partial class MusicTracksSelectionWindow : Window
    {
        private readonly string SoundXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sound.xml");
        private XmlDocument doc = new XmlDocument();

        List<string> victoryTracks = new List<string>();
        List<string> defeatTracks = new List<string>();
        List<string> economicTracks = new List<string>();

        // empty list that is temporarily populated with current stuff
        private List<string> currentList = new List<string>();

        // initially populate with parsed tracks (from sound.xml) that are on known tracks list, user can add/remove from there
        private readonly List<string> knownAll = new List<string>()
                {
                    @".\sounds\tracks\ArcDeTriomphe.wav",
                    @".\sounds\tracks\AcrossTheBog.wav",
                    @".\sounds\tracks\Allerton.wav",
                    @".\sounds\tracks\Attack.wav",
                    @".\sounds\tracks\BattleAtWitchCreek.wav",
                    @".\sounds\tracks\Bengal.wav",
                    @".\sounds\tracks\Brazil.wav",
                    @".\sounds\tracks\DarkForest.wav",
                    @".\sounds\tracks\DesertWind.wav",
                    @".\sounds\tracks\Eire.wav",
                    @".\sounds\tracks\Galleons.wav",
                    @".\sounds\tracks\Gobi.wav",
                    @".\sounds\tracks\HalfMoon.wav",
                    @".\sounds\tracks\Hearth.wav",
                    @".\sounds\tracks\HighStrung.wav",
                    @".\sounds\tracks\Indochine.wav",
                    @".\sounds\tracks\Misfire.wav",
                    @".\sounds\tracks\MistAtDawn.wav",
                    @".\sounds\tracks\Morocco.wav",
                    @".\sounds\tracks\Osaka.wav",
                    @".\sounds\tracks\OverTheDam.wav",
                    @".\sounds\tracks\PeacePipe.wav",
                    @".\sounds\tracks\Revolver.wav",
                    @".\sounds\tracks\Rockets.wav",
                    @".\sounds\tracks\SacrificeToTheSun.wav",
                    @".\sounds\tracks\Santiago.wav",
                    @".\sounds\tracks\SimpleSong.wav",
                    @".\sounds\tracks\SriLanka.wav",
                    @".\sounds\tracks\TheHague(ruffmix2).wav",
                    @".\sounds\tracks\TheRussian.wav",
                    @".\sounds\tracks\ThunderBird.wav",
                    @".\sounds\tracks\ThunderBird.wav",
                    @".\sounds\tracks\Tribes.wav",
                    @".\sounds\tracks\Waterloo.wav",
                    @".\sounds\tracks\WilliamWallace.wav",
                    @".\sounds\tracks\WingAndAPrayer.wav"
                };

        private readonly List<string> knownFullIntros = new List<string>()
                {
                    @".\sounds\tracks\AcrossTheBog.wav",
                    @".\sounds\tracks\Allerton.wav",
                    @".\sounds\tracks\Attack.wav",
                    @".\sounds\tracks\BattleAtWitchCreek.wav",
                    @".\sounds\tracks\Bengal.wav",
                    @".\sounds\tracks\Brazil.wav",
                    @".\sounds\tracks\DarkForest.wav",
                    @".\sounds\tracks\DesertWind.wav",
                    @".\sounds\tracks\Eire.wav",
                    @".\sounds\tracks\Galleons.wav",
                    @".\sounds\tracks\Gobi.wav",
                    @".\sounds\tracks\HalfMoon.wav",
                    @".\sounds\tracks\Hearth.wav",
                    @".\sounds\tracks\HighStrung.wav",
                    @".\sounds\tracks\Indochine.wav",
                    @".\sounds\tracks\Misfire.wav",
                    @".\sounds\tracks\MistAtDawn.wav",
                    @".\sounds\tracks\Morocco.wav",
                    @".\sounds\tracks\Osaka.wav",
                    @".\sounds\tracks\OverTheDam.wav",
                    @".\sounds\tracks\PeacePipe.wav",
                    @".\sounds\tracks\Revolver.wav",
                    @".\sounds\tracks\Rockets.wav",
                    @".\sounds\tracks\SacrificeToTheSun.wav",
                    @".\sounds\tracks\Santiago.wav",
                    @".\sounds\tracks\SimpleSong.wav",
                    @".\sounds\tracks\SriLanka.wav",
                    @".\sounds\tracks\TheHague(ruffmix2).wav",
                    @".\sounds\tracks\TheRussian.wav",
                    @".\sounds\tracks\ThunderBird.wav",
                    @".\sounds\tracks\ThunderBird.wav",
                    @".\sounds\tracks\Tribes.wav",
                    @".\sounds\tracks\WilliamWallace.wav",
                    @".\sounds\tracks\WingAndAPrayer.wav"
                };

        private readonly List<string> knownIntrosVanilla = new List<string>()
                {
                    @".\sounds\tracks\Attack.wav",
                    @".\sounds\tracks\Osaka.wav",
                    @".\sounds\tracks\Revolver.wav",
                    @".\sounds\tracks\Tribes.wav",
                    @".\sounds\tracks\WilliamWallace.wav"
                };

        private readonly List<string> knownIntrosTaP = new List<string>()
                {
                    @".\sounds\tracks\Bengal.wav",
                    @".\sounds\tracks\Misfire.wav",
                    @".\sounds\tracks\OverTheDam.wav",
                    @".\sounds\tracks\PeacePipe.wav",
                    @".\sounds\tracks\Rockets.wav",
                    @".\sounds\tracks\TheHague(ruffmix2).wav",
                    @".\sounds\tracks\ThunderBird.wav"
                };

        private readonly List<string> knownIntrosCombined = new List<string>()
                {
                    @".\sounds\tracks\Attack.wav",
                    @".\sounds\tracks\Osaka.wav",
                    @".\sounds\tracks\Revolver.wav",
                    @".\sounds\tracks\Tribes.wav",
                    @".\sounds\tracks\WilliamWallace.wav",
                    @".\sounds\tracks\Bengal.wav",
                    @".\sounds\tracks\Misfire.wav",
                    @".\sounds\tracks\OverTheDam.wav",
                    @".\sounds\tracks\PeacePipe.wav",
                    @".\sounds\tracks\Rockets.wav",
                    @".\sounds\tracks\TheHague(ruffmix2).wav",
                    @".\sounds\tracks\ThunderBird.wav"
                };

        private readonly List<string> knownWin = new List<string>()
                {
                    @".\sounds\tracks\AcrossTheBog.wav"
                };

        private readonly List<string> knownLose = new List<string>()
                {
                    @".\sounds\tracks\Waterloo.wav",
                };

        private readonly List<string> knownVictory = new List<string>()
                {
                    @".\sounds\tracks\Attack.wav",
                    @".\sounds\tracks\Galleons.wav",
                    @".\sounds\tracks\HighStrung.wav",
                    @".\sounds\tracks\Revolver.wav",
                    @".\sounds\tracks\TheRussian.wav",
                    @".\sounds\tracks\WilliamWallace.wav"
                };

        private readonly List<string> knownDefeatVanilla = new List<string>()
                {
                    @".\sounds\tracks\Allerton.wav",
                    @".\sounds\tracks\BattleAtWitchCreek.wav",
                    @".\sounds\tracks\DesertWind.wav",
                    @".\sounds\tracks\MistAtDawn.wav",
                    @".\sounds\tracks\Osaka.wav",
                    @".\sounds\tracks\Tribes.wav"
                };

        private readonly List<string> knownDefeatTaP = new List<string>()
                {
                    @".\sounds\tracks\Allerton.wav",
                    @".\sounds\tracks\BattleAtWitchCreek.wav",
                    @".\sounds\tracks\DesertWind.wav",
                    @".\sounds\tracks\Misfire.wav",
                    @".\sounds\tracks\MistAtDawn.wav",
                    @".\sounds\tracks\Osaka.wav",
                    @".\sounds\tracks\Tribes.wav"
                };

        private readonly List<string> knownEconomicVanilla = new List<string>()
                {
                    @".\sounds\tracks\AcrossTheBog.wav",
                    @".\sounds\tracks\Brazil.wav",
                    @".\sounds\tracks\DarkForest.wav",
                    @".\sounds\tracks\Eire.wav",
                    @".\sounds\tracks\Gobi.wav",
                    @".\sounds\tracks\Hearth.wav",
                    @".\sounds\tracks\Indochine.wav",
                    @".\sounds\tracks\Morocco.wav",
                    @".\sounds\tracks\Santiago.wav",
                    @".\sounds\tracks\SimpleSong.wav",
                    @".\sounds\tracks\SriLanka.wav",
                    @".\sounds\tracks\WingAndAPrayer.wav"
                };

        private readonly List<string> knownEconomicTaP = new List<string>()
                {
                    @".\sounds\tracks\AcrossTheBog.wav",
                    @".\sounds\tracks\Brazil.wav",
                    @".\sounds\tracks\DarkForest.wav",
                    @".\sounds\tracks\Eire.wav",
                    @".\sounds\tracks\Gobi.wav",
                    @".\sounds\tracks\Hearth.wav",
                    @".\sounds\tracks\Indochine.wav",
                    @".\sounds\tracks\Morocco.wav",
                    @".\sounds\tracks\Santiago.wav",
                    @".\sounds\tracks\SimpleSong.wav",
                    @".\sounds\tracks\SriLanka.wav",
                    @".\sounds\tracks\WingAndAPrayer.wav",
                    @".\sounds\tracks\SacrificeToTheSun.wav",
                    @".\sounds\tracks\PeacePipe.wav",
                    @".\sounds\tracks\Rockets.wav",
                    @".\sounds\tracks\Bengal.wav",
                    @".\sounds\tracks\TheHague(ruffmix2).wav",
                    @".\sounds\tracks\ThunderBird.wav",
                    @".\sounds\tracks\OverTheDam.wav",
                    @".\sounds\tracks\HalfMoon.wav"
                };

        public MusicTracksSelectionWindow()
        {
            InitializeComponent();
            DataContext = this;

            CleanFile();
            HighlightSelection();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /*private void OptWindowExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }*/

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

        private void HighlightSelection()
        {
            // reset weights
            MenuOldButton.FontWeight = FontWeights.Normal;
            MenuNewButton.FontWeight = FontWeights.Normal;
            MenuCombinedButton.FontWeight = FontWeights.Normal;
            LosingOldButton.FontWeight = FontWeights.Normal;
            LosingNewButton.FontWeight = FontWeights.Normal;
            EconomicOldButton.FontWeight = FontWeights.Normal;
            EconomicNewButton.FontWeight = FontWeights.Normal;

            List<string> currentMenu = new List<string>();
            List<string> currentLosing = new List<string>();
            List<string> currentEconomic = new List<string>();

            doc.Load(SoundXML);

            // get current menu tracks, then compare if it matches any of the presets
            XmlNode trackList = doc.SelectSingleNode(@"ROOT/TRACKS/INTROS");
            foreach (XmlNode track in trackList.ChildNodes)
            {
                XmlAttribute trackFile = track.Attributes["file"];
                currentMenu.Add(trackFile.Value);
            }
            if (Enumerable.SequenceEqual(currentMenu.OrderBy(e => e), knownIntrosVanilla.OrderBy(e => e))) //https://www.techiedelight.com/compare-two-lists-for-equality-csharp/
            {
                MenuOldButton.FontWeight = FontWeights.Bold;
            }
            else if (Enumerable.SequenceEqual(currentMenu.OrderBy(e => e), knownIntrosTaP.OrderBy(e => e)))
            {
                MenuNewButton.FontWeight = FontWeights.Bold;
            }
            else if (Enumerable.SequenceEqual(currentMenu.OrderBy(e => e), knownIntrosCombined.OrderBy(e => e)))
            {
                MenuCombinedButton.FontWeight = FontWeights.Bold;
            }

            // get current battle defeat tracks, then compare if it matches any of the presets
            trackList = doc.SelectSingleNode(@"ROOT/TRACKS/TRIBE/AGE/MOOD[2]");
            foreach (XmlNode track in trackList.ChildNodes)
            {
                XmlAttribute trackFile = track.Attributes["file"];
                currentLosing.Add(trackFile.Value);
            }
            if (Enumerable.SequenceEqual(currentLosing.OrderBy(e => e), knownDefeatVanilla.OrderBy(e => e)))
            {
                LosingOldButton.FontWeight = FontWeights.Bold;
            }
            else if (Enumerable.SequenceEqual(currentLosing.OrderBy(e => e), knownDefeatTaP.OrderBy(e => e)))
            {
                LosingNewButton.FontWeight = FontWeights.Bold;
            }

            // get current battle victory tracks, then compare if it matches any of the presets
            trackList = doc.SelectSingleNode(@"ROOT/TRACKS/TRIBE/AGE/MOOD[3]");
            foreach (XmlNode track in trackList.ChildNodes)
            {
                XmlAttribute trackFile = track.Attributes["file"];
                currentEconomic.Add(trackFile.Value);
            }
            if (Enumerable.SequenceEqual(currentEconomic.OrderBy(e => e), knownEconomicVanilla.OrderBy(e => e)))
            {
                EconomicOldButton.FontWeight = FontWeights.Bold;
            }
            else if (Enumerable.SequenceEqual(currentEconomic.OrderBy(e => e), knownEconomicTaP.OrderBy(e => e)))
            {
                EconomicNewButton.FontWeight = FontWeights.Bold;
            }
        }

        private void ModifyXML(string categoryPath, List<string> categoryChoice)
        {
            try
            {
                List<string> currentTracks = new List<string>();

                doc.Load(SoundXML);
                XmlNode trackList = doc.SelectSingleNode(categoryPath);

                // populate the list of current intro/menu tracks
                foreach (XmlNode track in trackList.ChildNodes)
                {
                    XmlAttribute trackFile = track.Attributes["file"];
                    currentTracks.Add(trackFile.Value);
                }

                string oldTracks = String.Join("\n", currentTracks);
                string newTracks = String.Join("\n", categoryChoice);

                if (MessageBox.Show("Please confirm that you want the following change(s) to the track list:\n"
                    + "Current tracks:\n" + oldTracks
                    + "\n\n"
                    + "New tracks:\n" + newTracks, "Confirm track selection", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // wipe existing track list for this category
                    trackList.RemoveAll();

                    // restore the mood description if applicable
                    if (categoryChoice == knownDefeatVanilla || categoryChoice == knownDefeatTaP)
                    {
                        XmlNode battleDefeat = doc.SelectSingleNode("ROOT/TRACKS/TRIBE/AGE/MOOD[2]");
                        XmlAttribute mood = doc.CreateAttribute("mood");
                        mood.Value = "BATTLE_DEFEAT";
                        battleDefeat.Attributes.Append(mood);
                    }
                    if (categoryChoice == knownEconomicVanilla || categoryChoice == knownEconomicTaP)
                    {
                        XmlNode battleDefeat = doc.SelectSingleNode("ROOT/TRACKS/TRIBE/AGE/MOOD[3]");
                        XmlAttribute mood = doc.CreateAttribute("mood");
                        mood.Value = "ECONOMIC";
                        battleDefeat.Attributes.Append(mood);
                    }

                    // replace with new track list
                    foreach (string trackName in categoryChoice)
                    {
                        XmlElement trackNew = doc.CreateElement("SOUND");
                        trackList.AppendChild(trackNew);

                        XmlAttribute file = doc.CreateAttribute("file");
                        file.Value = trackName;
                        trackNew.Attributes.Append(file);
                    }

                    doc.Save(SoundXML);
                    HighlightSelection();
                    MessageBox.Show("Sound.xml file updated successfully.");
                }
                else
                {
                    MessageBox.Show("No action taken.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error modifying sound.xml: " + ex);
            }
        }

        private void MenuOldButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyXML("ROOT/TRACKS/INTROS", knownIntrosVanilla);
        }

        private void MenuNewButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyXML("ROOT/TRACKS/INTROS", knownIntrosTaP);
        }

        private void MenuCombinedButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyXML("ROOT/TRACKS/INTROS", knownIntrosCombined);
        }

        private void LosingOldButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyXML("ROOT/TRACKS/TRIBE/AGE/MOOD[2]", knownDefeatVanilla);
        }

        private void LosingNewButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyXML("ROOT/TRACKS/TRIBE/AGE/MOOD[2]", knownDefeatTaP);
        }

        private void EconomicOldButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyXML("ROOT/TRACKS/TRIBE/AGE/MOOD[3]", knownEconomicVanilla);
        }

        private void EconomicNewButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyXML("ROOT/TRACKS/TRIBE/AGE/MOOD[3]", knownEconomicTaP);
        }
    }
}
