using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        private HtmlNode TomorrowNode;
        public Main(string name)
        {
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.Title = "Пользователь: " + name;
            Today.ItemsSource = ParseToday();
            Yesterday.ItemsSource = ParseYesterday();
            Tomorrow.ItemsSource = ParseTomorrow();
        }

        private void SetNode(string day, ref HtmlNode node)
        {
            var HtmlDoc = new HtmlDocument();
            HtmlDoc.LoadHtml(GetHtml("https://www.sport-express.ru/ajax/translations-block/?dateinterval=" + day + "&sportname=football"));
            node = HtmlDoc.DocumentNode;
        }
        private void SetCentralNode()
        {
            SetNode("today", ref CentralNode);
        }

        private void SetTomorrowNode()
        {
            SetNode("tomorrow",ref TomorrowNode);
        }
        private void SetYesterdayNode()
        {
            SetNode("yesterday",ref YesterdayNode);
        }

        private string[] ParseYesterday()
        {
            SetYesterdayNode();
            return Parse(YesterdayNode);
        }

        private HtmlNode[] GetElementsByClassName(HtmlNode parent, string tagname, string classname)
            => parent.Descendants(tagname).Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(classname)).ToArray();

        private string[] ParseToday()
        {
            SetCentralNode();
            return Parse(CentralNode);
        }

        private string[] ParseTomorrow()
        {
            SetTomorrowNode();
            return Parse(TomorrowNode);
        }
        private string[] Parse(HtmlNode node)
        {
            List<string> rc = new List<string>();
            var nodes = GetElementsByClassName(node, "div", "heading");
            for (int i = 0; i < nodes.Length; i++) rc.Add(nodes[i].InnerHtml);
            return rc.ToArray();
        }

        private void Today_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCentralNode();
            SetMatches(CentralNode, Today);
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
            string help = "";
            for (int i = 0; i < FootballersFromFirstTeam.Count; i++)
            {
                info = FootballersFromFirstTeam[i].ChildNodes["info"];
                if (info != null)
                {
                    help = String.Format("{0, -5}{1}", info.GetAttributeValue("number", null),
                        FootballersFromFirstTeam[i].GetAttributeValue("name", null));
                    if (info.GetAttributeValue("orderChange", "").Equals("0"))
                        squad1.Add(help);
                    else anotherSquad1.Add(help);
                }
            }
            for (int i = 0; i < FootballersFromSecondTeam.Count; i++)
            {
                info = FootballersFromSecondTeam[i].ChildNodes["info"];
                if (info != null)
                {
                    help = String.Format("{0, -5}{1}", info.GetAttributeValue("number", null),
                        FootballersFromSecondTeam[i].GetAttributeValue("name", null));
                    if (info.GetAttributeValue("orderChange", "").Equals("0"))
                        squad2.Add(help);
                    else anotherSquad2.Add(help);
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
            Goals.Text = node.SelectSingleNode("/match").GetAttributeValue("score", null);
        }
        #endregion
        private void Matches_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Matches.SelectedItem == null)
            {
                Squad1.Content = Squad2.Content = Trainer1.Content =
                    Trainer2.Content = FirstGoals.Content = SecondGoals.Content = BestSquad.Content =
                    Status.Content = AnotherPlayers.Content = Goals.Text = "";
                FirstSquad.ItemsSource = SecondSquad.ItemsSource = AnotherFirstSquad.ItemsSource = AnotherSecondSquad.ItemsSource = null;
                return;
            }
            string url = "https://www.sport-express.ru/services/match/football/" + ((Match)Matches.SelectedItem).Link + "/online/se/?xml=1";
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
            SetYesterdayNode();
            SetMatches(YesterdayNode, Yesterday);
        }

        private void SetMatches(HtmlNode node, ListBox list)
        {
            List<Match> mat = new List<Match>();
            Regex regex = new Regex(@"\d+");
            var AllNodes = GetElementsByClassName(node, "div", "item is_");
            for (int i = 0; i < AllNodes.Length; i++)
            {
                if (AllNodes[i].ParentNode.ParentNode.SelectSingleNode("div[1]").InnerHtml.Equals((string)list.SelectedItem) ||
                    AllNodes[i].ParentNode.SelectSingleNode("div[1]").InnerHtml.Equals((string)list.SelectedItem))
                {
                    mat.Add(new Match()
                    {
                        FirstTeam = AllNodes[i].SelectSingleNode("a/div[2]/div").InnerHtml.Trim(),
                        SecondTeam = AllNodes[i].SelectSingleNode("a/div[4]/div").InnerHtml.Trim(),
                        GoalsFirstTeam = AllNodes[i].SelectSingleNode("a/div[3]/div/div[1]").InnerHtml.Trim(),
                        GoalsSecondTeam = AllNodes[i].SelectSingleNode("a/div[3]/div/div[3]").InnerHtml.Trim(),
                        TimeOfRunning = AllNodes[i].SelectSingleNode("a/div[1]/div/div[1]").InnerHtml,
                        Link = regex.Match(AllNodes[i].ChildNodes["a"].GetAttributeValue("href", "")).Value
                    });
                }
            }
            Matches.Visibility = Visibility.Visible;
            Matches.ItemsSource = mat;
        }

        private void Tomorrow_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetTomorrowNode();
            SetMatches(TomorrowNode, Tomorrow);
        }
    }
}
