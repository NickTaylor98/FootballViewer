namespace FootballApp
{
    class Match
    {
        public string FirstTeam { get; set; }
        public string SecondTeam { get; set; }
        public string GoalsFirstTeam { get; set; }
        public string GoalsSecondTeam { get; set; }
        public string TimeOfRunning { get; set; }
        public override string ToString() => TimeOfRunning + "     " + FirstTeam + " - " + SecondTeam + " " + GoalsFirstTeam + ":" + GoalsSecondTeam;
    }
}
