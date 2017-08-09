using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using HtmlAgilityPack;

namespace FootballApp
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private HtmlNode CentralNode;
        private HtmlNode YesterdayNode;
        private bool today;
        public Main(string name)
        {
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.Title = "Пользователь: " + name;
            Today.ItemsSource = ParseToday();
            Yesterday.ItemsSource = ParseYesterday();
        }

        private void SetCentralNode()
        {
            var HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml(GetHtml("https://www.sport-express.ru/ajax/translations-block/?dateinterval=today&sportname=football"));
            CentralNode = HtmlDoc.DocumentNode;
        }

        private void SetYesterdayNode()
        {
            var HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml(GetHtml("https://www.sport-express.ru/ajax/translations-block/?dateinterval=yesterday&sportname=football"));
            YesterdayNode = HtmlDoc.DocumentNode;
        }

        private string[] ParseYesterday()
        {
            List<string> rc = new List<string>();
            SetYesterdayNode();
            var nodes = YesterdayNode.Descendants("div").Where(d => d.Attributes.Contains("class") &&
                                                                  d.Attributes["class"].Value.Contains("heading")).ToArray();
            for (int i = 0; i < nodes.Length; i++) rc.Add(nodes[i].InnerHtml);
            return rc.ToArray();
        }

        private string[] ParseToday()
        {
            List<string> rc = new List<string>();
            SetCentralNode();
            var nodes = CentralNode.Descendants("div").Where(d =>d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("heading")).ToArray();
            for (int i = 0; i < nodes.Length; i++) rc.Add(nodes[i].InnerHtml);
            return rc.ToArray();
        }
        //*[@id="translation_part_football"]/div[1]/div[2]/div/a
        private void Today_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Match> rc = new List<Match>();
            List<string> mat = new List<string>();
            SetCentralNode();
            today = true;
            int l = Today.SelectedIndex + 1;
            HtmlNode HelpNode = CentralNode.SelectSingleNode("//*[@id=\"translation_part_football\"]/div[" + l +
                                                             "]/div/div/a");
            var nodesleft = HelpNode.SelectNodes("/div[2]/div");
            var nodesright = HelpNode.SelectNodes("/div[4]/div");
            var leftGoals = HelpNode.SelectNodes("/div[3]/div/div[1]");
            var rightGoals = HelpNode.SelectNodes("/div[3]/div/div[3]");
            var times = HelpNode.SelectNodes("/div[1]/div/div[1]");


            for (int i = 0; i < nodesleft.Count; i++)
            {
                rc.Add(new Match()
                {
                    FirstTeam = nodesleft[i].InnerHtml.Trim(),
                    SecondTeam = nodesright[i].InnerHtml.Trim(),
                    GoalsFirstTeam = leftGoals[i].InnerHtml.Trim(),
                    GoalsSecondTeam = rightGoals[i].InnerHtml.Trim(),
                    TimeOfRunning = times[i].InnerHtml
                });
                mat.Add(rc[i].ToString());
            }
            Matches.Visibility = Visibility.Visible;
            Matches.ItemsSource = mat;
        }
        private int FindMatchIndex() => Matches.SelectedIndex + 2;

        private string GetLink()
        {
            string help = "";
            HtmlNode node = null;
            if (today)
                help = CentralNode.SelectSingleNode("//*[@id=\"translation_part_football\"]/div[" + (Today.SelectedIndex + 1) +
                                             "]/div[" + FindMatchIndex() + "]/div/a").GetAttributeValue("href", null);
            else
                help = YesterdayNode.SelectSingleNode("//*[@id=\"translation_part_football\"]/div[" + (Yesterday.SelectedIndex + 1) +
                                                    "]/div[" + FindMatchIndex() + "]/div/a").GetAttributeValue("href", null);
            Regex regex = new Regex(@"\d+");
            return regex.Match(help).Value;
        }


        #region Alternative Get Html from URL
        public string GetHtml(String urlAddress)
        {
            string rc = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null) readStream = new StreamReader(receiveStream);
                else readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                rc = data;
            }
            return rc;
        }
        #endregion
        #region Settings of Information

        private void SetStatus(HtmlNode node)
        {
            Status.Content = node.SelectSingleNode("/match").GetAttributeValue("statusName", null);
        }
        private void SetNamesOfCommands(HtmlNode node)
        {
            Squad1.Content = node.
                SelectSingleNode("/match/homecommand").GetAttributeValue("name", null);
            Squad2.Content = node.
                SelectSingleNode("/match/guestcommand").GetAttributeValue("name", null);
        }

        private void SetSquadsOfCommands(HtmlNode node)
        {
            BestSquad.Content = "Основной состав";
            AnotherPlayers.Content = "Запасные";
            List<string> squad1 = new List<string>(),
                squad2 = new List<string>(),
                anotherSquad1 = new List<string>(),
                anotherSquad2 = new List<string>();
            var FootballersFromFirstTeam = node.SelectNodes("/match/homecommand/players");
            var FootballersFromSecondTeam = node.SelectNodes("/match/guestcommand/players");
            HtmlNode info = null;
            for (int i = 0; i < FootballersFromFirstTeam.Count; i++)
            {
                info = FootballersFromFirstTeam[i].ChildNodes["info"];
                if (info != null)
                {
                    if (info.GetAttributeValue("orderChange", "").Equals("0"))
                        squad1.Add(FootballersFromFirstTeam[i].GetAttributeValue("name", null));
                    else anotherSquad1.Add(FootballersFromFirstTeam[i].GetAttributeValue("name", null));
                }
            }
            for (int i = 0; i < FootballersFromSecondTeam.Count; i++)
            {
                info = FootballersFromSecondTeam[i].ChildNodes["info"];
                if (info != null)
                {
                    if (info.GetAttributeValue("orderChange", "").Equals("0"))
                        squad2.Add(FootballersFromSecondTeam[i].GetAttributeValue("name", null));
                    else anotherSquad2.Add(FootballersFromSecondTeam[i].GetAttributeValue("name", null));
                }
            }
            FirstSquad.Visibility = Visibility.Visible;
            FirstSquad.ItemsSource = squad1;
            SecondSquad.Visibility = Visibility.Visible;
            SecondSquad.ItemsSource = squad2;
            AnotherFirstSquad.Visibility = Visibility.Visible;
            AnotherFirstSquad.ItemsSource = anotherSquad1;
            AnotherSecondSquad.Visibility = Visibility.Visible;
            AnotherSecondSquad.ItemsSource = anotherSquad2;
        }

        private void SetTrainersOfCommands(HtmlNode node)
        {
            Trainer1.Content = Trainer2.Content = "";
            var trainer1 = node.SelectSingleNode("/match/homecommand/trainer");
            var trainer2 = node.SelectSingleNode("/match/guestcommand/trainer");
            if (trainer1 != null)
                Trainer1.Content = trainer1.GetAttributeValue("name", "");
            if (trainer2 != null)
                Trainer2.Content = trainer2.GetAttributeValue("name", "");
        }

        private void SetGoalsOfCommands(HtmlNode node)
        {
            string firstTeamGoals = "", secondTeamGoals = "", help;
            var AllGoals = node.SelectNodes("/match/events");
            for (int i = 0; i < AllGoals.Count; i++)
            {
                var Goal = AllGoals[i];
                if (Goal.GetAttributeValue("type", "").Equals("goal"))
                {
                    help = "";
                    help += Goal.GetAttributeValue("fullMinute", "");
                    help += " " + Goal.ChildNodes["player"].GetAttributeValue("shortname", "");
                    if (Goal.GetAttributeValue("kind", "").Equals("penalty")) help += "(п)";
                    if (Goal.ChildNodes["info"].GetAttributeValue("scoredFor", "").Equals("home"))
                        firstTeamGoals += help + "\n";
                    else secondTeamGoals += help + "\n";
                }
            }
            FirstGoals.Content = firstTeamGoals;
            SecondGoals.Content = secondTeamGoals;
            Goals.Content = node.SelectSingleNode("/match").GetAttributeValue("score", null);
        }
        #endregion
        private void Matches_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Matches.SelectedItem == null)
            {
                Squad1.Content = Squad2.Content = Goals.Content = Trainer1.Content =
                    Trainer2.Content = FirstGoals.Content = SecondGoals.Content = BestSquad.Content =
                    Status.Content = AnotherPlayers.Content = "";
                FirstSquad.ItemsSource = SecondSquad.ItemsSource = AnotherFirstSquad.ItemsSource = AnotherSecondSquad.ItemsSource = null;
                return;
            }
            string url = "https://www.sport-express.ru/services/match/football/" + GetLink() + "/online/se/?xml=1";
            var HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml(GetHtml(url));
            var RootNode = HtmlDoc.DocumentNode;
            SetStatus(RootNode);
            SetNamesOfCommands(RootNode);
            SetTrainersOfCommands(RootNode);
            SetGoalsOfCommands(RootNode);
            SetSquadsOfCommands(RootNode);
        }

        private void Yesterday_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> matches = new List<string>();
            today = false;
            int l = Yesterday.SelectedIndex + 1;
            var FirstTeams = YesterdayNode.SelectNodes("//*[@id=\"translation_part_football\"]/div[" + l + "]/div/div/a/div[2]/div");
            var SecondTeams = YesterdayNode.SelectNodes("//*[@id=\"translation_part_football\"]/div[" + l + "]/div/div/a/div[4]/div");
            var FirstGoals = YesterdayNode.SelectNodes("//*[@id=\"translation_part_football\"]/div[" + l + "]/div/div/a/div[3]/div/div[1]");
            var SecondGoals = YesterdayNode.SelectNodes("//*[@id=\"translation_part_football\"]/div[" + l + "]/div/div/a/div[3]/div/div[3]");
            var Times = YesterdayNode.SelectNodes("//*[@id=\"translation_part_football\"]/div[" + l + "]/div/div/a/div[1]/div/div[1]");
            for (int i = 0; i < FirstTeams.Count; i++)
                matches.Add(new Match()
                {
                    FirstTeam = FirstTeams[i].InnerHtml.Trim(),
                    GoalsFirstTeam = FirstGoals[i].InnerHtml.Trim(),
                    GoalsSecondTeam = SecondGoals[i].InnerHtml.Trim(),
                    SecondTeam = SecondTeams[i].InnerHtml.Trim(),
                    TimeOfRunning = Times[i].InnerHtml.Trim()
                }.ToString());
            Matches.Visibility = Visibility.Visible;
            Matches.ItemsSource = matches;
        }
    }
}
