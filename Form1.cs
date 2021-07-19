using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_Bet
{
    public partial class Form1 : Form
    {

        public static List<League> leagueList = new List<League>();

        public League selectedLeague;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            leagueList.Add(new League("Ligue 1", "France", "2020-2021", "https://www.football-data.co.uk/mmz4281/2021/F1.csv"));
            leagueList[0].lastSeasons.Add(new League("Ligue 1", "France", "2019-2020", "http://www.football-data.co.uk/mmz4281/1920/F1.csv"));
            leagueList[0].lastSeasons.Add(new League("Ligue 1", "France", "2018-2019", "http://www.football-data.co.uk/mmz4281/1819/F1.csv"));
            leagueList[0].lastSeasons.Add(new League("Ligue 1", "France", "2017-2018", "http://www.football-data.co.uk/mmz4281/1718/F1.csv"));
            leagueList[0].lastSeasons.Add(new League("Ligue 1", "France", "2016-2017", "http://www.football-data.co.uk/mmz4281/1617/F1.csv"));
            leagueList[0].lastSeasons.Add(new League("Ligue 1", "France", "2015-2016", "http://www.football-data.co.uk/mmz4281/1516/F1.csv"));

            leagueList.Add(new League("LaLiga", "Spain", "2020-2021", "https://www.football-data.co.uk/mmz4281/2021/SP1.csv"));
            leagueList[1].lastSeasons.Add(new League("LaLiga", "Spain", "2019-2020", "http://www.football-data.co.uk/mmz4281/1920/SP1.csv"));
            leagueList[1].lastSeasons.Add(new League("LaLiga", "Spain", "2018-2019", "http://www.football-data.co.uk/mmz4281/1819/SP1.csv"));
            leagueList[1].lastSeasons.Add(new League("LaLiga", "Spain", "2017-2018", "http://www.football-data.co.uk/mmz4281/1718/SP1.csv"));
            leagueList[1].lastSeasons.Add(new League("LaLiga", "Spain", "2016-2017", "http://www.football-data.co.uk/mmz4281/1617/SP1.csv"));
            leagueList[1].lastSeasons.Add(new League("LaLiga", "Spain", "2015-2016", "http://www.football-data.co.uk/mmz4281/1516/SP1.csv"));

            leagueList.Add(new League("Bundesliga", "Germany", "2020-2021", "https://www.football-data.co.uk/mmz4281/2021/D1.csv"));
            leagueList[2].lastSeasons.Add(new League("Bundesliga", "Germany", "2019-2020", "http://www.football-data.co.uk/mmz4281/1920/D1.csv"));
            leagueList[2].lastSeasons.Add(new League("Bundesliga", "Germany", "2018-2019", "http://www.football-data.co.uk/mmz4281/1819/D1.csv"));
            leagueList[2].lastSeasons.Add(new League("Bundesliga", "Germany", "2017-2018", "http://www.football-data.co.uk/mmz4281/1718/D1.csv"));
            leagueList[2].lastSeasons.Add(new League("Bundesliga", "Germany", "2016-2017", "http://www.football-data.co.uk/mmz4281/1617/D1.csv"));
            leagueList[2].lastSeasons.Add(new League("Bundesliga", "Germany", "2015-2016", "http://www.football-data.co.uk/mmz4281/1516/D1.csv"));

            leagueList.Add(new League("Premier League", "England", "2020-2021", "https://www.football-data.co.uk/mmz4281/2021/E0.csv"));
            leagueList[3].lastSeasons.Add(new League("Premier League", "England", "2019-2020", "http://www.football-data.co.uk/mmz4281/1920/E0.csv"));
            leagueList[3].lastSeasons.Add(new League("Premier League", "England", "2018-2019", "http://www.football-data.co.uk/mmz4281/1819/E0.csv"));
            leagueList[3].lastSeasons.Add(new League("Premier League", "England", "2017-2018", "http://www.football-data.co.uk/mmz4281/1718/E0.csv"));
            leagueList[3].lastSeasons.Add(new League("Premier League", "England", "2016-2017", "http://www.football-data.co.uk/mmz4281/1617/E0.csv"));
            leagueList[3].lastSeasons.Add(new League("Premier League", "England", "2015-2016", "http://www.football-data.co.uk/mmz4281/1516/E0.csv"));

            leagueList.Add(new League("Serie A", "Italy", "2020-2021", "https://www.football-data.co.uk/mmz4281/2021/I1.csv"));
            leagueList[4].lastSeasons.Add(new League("Serie A", "Italy", "2019-2020", "http://www.football-data.co.uk/mmz4281/1920/I1.csv"));
            leagueList[4].lastSeasons.Add(new League("Serie A", "Italy", "2018-2019", "http://www.football-data.co.uk/mmz4281/1819/I1.csv"));
            leagueList[4].lastSeasons.Add(new League("Serie A", "Italy", "2017-2018", "http://www.football-data.co.uk/mmz4281/1718/I1.csv"));
            leagueList[4].lastSeasons.Add(new League("Serie A", "Italy", "2016-2017", "http://www.football-data.co.uk/mmz4281/1617/I1.csv"));
            leagueList[4].lastSeasons.Add(new League("Serie A", "Italy", "2015-2016", "http://www.football-data.co.uk/mmz4281/1516/I1.csv"));

            foreach (League league in leagueList)
            {
                if (!statisticsLeagueCB.Items.Contains(league.Name))
                {
                    statisticsLeagueCB.Items.Add(league.Name);
                }

                if (!predictionLeagueCB.Items.Contains(league.Name))
                {
                    predictionLeagueCB.Items.Add(league.Name);
                }

                if (!resultsLeagueCB.Items.Contains(league.Name))
                {
                    resultsLeagueCB.Items.Add(league.Name);
                }
            }
        }

        #region STATISTICS

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            League league = leagueList.Where(p => p.Name == statisticsLeagueCB.Text).First();

            statisticsSeasonCB.Items.Add(league.Season);
            foreach (League season in league.lastSeasons)
            {
                if (!statisticsSeasonCB.Items.Contains(season.Season))
                {
                    statisticsSeasonCB.Items.Add(season.Season);
                }
            }
        }

        private void metroComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (statisticsLeagueCB.SelectedIndex != -1)
            {

                League league = leagueList.Where(p => p.Name == statisticsLeagueCB.Text).First();

                if (league.Season == statisticsSeasonCB.Text)
                {
                    selectedLeague = league;
                    RH.Text = "Home: " + league.homeW.ToString();
                    RD.Text = "Draw: " + league.draws.ToString();
                    RA.Text = "Away: " + league.awayW.ToString();

                    THG.Text = "Total Home Goals: " + league.homeGoals.ToString();
                    TAG.Text = "Total Away Goals: " + league.awayGoals.ToString();
                    HGPM.Text = "Per Match " + Math.Round(league.homeGoalsPerMatch, 2).ToString();
                    AGPM.Text = "Per Match " + Math.Round(league.awayGoalsPerMatch, 2).ToString();

                    O15.Text = "+1.5: " + RoundPercentage(league, league.Over15).ToString();
                    O25.Text = "+2.5: " + RoundPercentage(league, league.Over25).ToString();
                    O35.Text = "+3.5: " + RoundPercentage(league, league.Over35).ToString();
                    O45.Text = "+4.5: " + RoundPercentage(league, league.Over45).ToString();
                    O55.Text = "+5.5: " + RoundPercentage(league, league.Over55).ToString();



                    BTTSYES.Text = "Yes: " + RoundPercentage(league, league.BTTSYes).ToString();
                    BTTSNO.Text = "No: " + RoundPercentage(league, league.BTTSNo).ToString();
                    statisticsLeagueLV.Items.Clear();
                    int i = 0;
                    foreach (string team in league.Table.Keys)
                    {
                        i++;
                        ListViewItem item = statisticsLeagueLV.Items.Add(i.ToString());
                        item.SubItems.Add(team);
                        item.SubItems.Add(league.Table[team].ToString());
                    }
                }
                else
                {
                    foreach (League season in league.lastSeasons)
                    {
                        if (season.Season == statisticsSeasonCB.Text)
                        {
                            selectedLeague = season;
                            RH.Text = "Home: " + RoundPercentage(season, season.homeW).ToString();
                            RD.Text = "Draw: " + RoundPercentage(season, season.draws).ToString();
                            RA.Text = "Away: " + RoundPercentage(season, season.awayW).ToString();

                            THG.Text = "Total Home Goals: " + season.homeGoals.ToString();
                            TAG.Text = "Total Away Goals: " + season.awayGoals.ToString();
                            HGPM.Text = "Per Match " + Math.Round(season.homeGoalsPerMatch, 2).ToString();
                            AGPM.Text = "Per Match " + Math.Round(season.awayGoalsPerMatch, 2).ToString();

                            O15.Text = "+1.5: " + RoundPercentage(season, season.Over15).ToString();
                            O25.Text = "+2.5: " + RoundPercentage(season, season.Over25).ToString();
                            O35.Text = "+3.5: " + RoundPercentage(season, season.Over35).ToString();
                            O45.Text = "+4.5: " + RoundPercentage(season, season.Over45).ToString();
                            O55.Text = "+5.5: " + RoundPercentage(season, season.Over55).ToString();


                            BTTSYES.Text = "Yes: " + RoundPercentage(season, season.BTTSYes).ToString();
                            BTTSNO.Text = "No: " + RoundPercentage(season, season.BTTSNo).ToString();

                            statisticsLeagueLV.Items.Clear();
                            int i = 0;
                            foreach (string team in season.Table.Keys)
                            {
                                i++;
                                ListViewItem item = statisticsLeagueLV.Items.Add(i.ToString());
                                item.SubItems.Add(team);
                                item.SubItems.Add(season.Table[team].ToString());
                            }

                        }
                    }
                }

            }
        }

        public double RoundPercentage(League league, double number)
        {
            return Math.Round((number / league.playedMatch) * 100, 2);
        }

        public void AddItem(string text1, string text2)
        {
            ListViewItem item = new ListViewItem();
            item.Text = text1;
            item.SubItems.Add(text2);

            statisticsTeamLV.Items.Add(item);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (statisticsLeagueLV.SelectedItems[0].Index != -1)
                {
                    Team team = selectedLeague.Teams.Where(p => p.Name == statisticsLeagueLV.SelectedItems[0].SubItems[1].Text).First();
                    metroLabel3.Text = team.Name;

                    statisticsTeamLV.Items.Clear();

                    AddItem("Home Wins %", Math.Round((team.HomeW / team.homePlayedMatch * 100), 2).ToString());
                    AddItem("Home Draws %", Math.Round((team.HomeD / team.homePlayedMatch * 100), 2).ToString());
                    AddItem("Home Losts %", Math.Round((team.HomeL / team.homePlayedMatch * 100), 2).ToString());

                    AddItem("away Wins %", Math.Round((team.AwayW / team.awayPlayedMatch * 100), 2).ToString());
                    AddItem("away Draws %", Math.Round((team.AwayD / team.awayPlayedMatch * 100), 2).ToString());
                    AddItem("away Losts %", Math.Round((team.AwayL / team.awayPlayedMatch * 100), 2).ToString());

                    AddItem("Home Average Scored Goals", Math.Round(team.HomeScoredGoalsAverage, 2).ToString());
                    AddItem("Home Average Conceded Goals", Math.Round(team.HomeConcededGoalsAverage, 2).ToString());
                    AddItem("Away Average Scored Goals", Math.Round(team.AwayScoredGoalsAverage, 2).ToString());
                    AddItem("Away Average Conceded Goals", Math.Round(team.AwayConcededGoalsAverage, 2).ToString());

                    AddItem("Home BTTS %", Math.Round((team.HomeBTTSYes / team.homePlayedMatch * 100), 2).ToString());
                    AddItem("Away BTTS %", Math.Round((team.AwayBTTSYes / team.awayPlayedMatch * 100), 2).ToString());

                    AddItem("Home +1.5 %", Math.Round(team.HOver15 / team.homePlayedMatch * 100, 2).ToString());
                    AddItem("Home +2.5 %", Math.Round(team.HOver25 / team.homePlayedMatch * 100, 2).ToString());
                    AddItem("Home +3.5 %", Math.Round(team.HOver35 / team.homePlayedMatch * 100, 2).ToString());
                    AddItem("Home +4.5 %", Math.Round(team.HOver45 / team.homePlayedMatch * 100, 2).ToString());
                    AddItem("Home +5.5 %", Math.Round(team.HOver55 / team.homePlayedMatch * 100, 2).ToString());

                    AddItem("Away +1.5 %", Math.Round(team.AOver15 / team.awayPlayedMatch * 100, 2).ToString());
                    AddItem("Away +2.5 %", Math.Round(team.AOver25 / team.awayPlayedMatch * 100, 2).ToString());
                    AddItem("Away +3.5 %", Math.Round(team.AOver35 / team.awayPlayedMatch * 100, 2).ToString());
                    AddItem("Away +4.5 %", Math.Round(team.AOver45 / team.awayPlayedMatch * 100, 2).ToString());
                    AddItem("Away +5.5 %", Math.Round(team.AOver55 / team.awayPlayedMatch * 100, 2).ToString());

                }
            }
            catch (Exception)
            {


            }
        }

        #endregion

        #region PREDICTION



        #endregion

        private void predictionLeagueCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            League league = leagueList.Where(p => p.Name == predictionLeagueCB.Text).First();

            homeTeamCB.Items.Clear();
            awayTeamCB.Items.Clear();

            foreach (Team team in league.Teams)
            {
                if (!homeTeamCB.Items.Contains(team.Name))
                {
                    homeTeamCB.Items.Add(team.Name);
                }

                if (!awayTeamCB.Items.Contains(team.Name))
                {
                    awayTeamCB.Items.Add(team.Name);
                }
            }
        }

        private void homeTeamCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            League league = leagueList.Where(p => p.Name == predictionLeagueCB.Text).First();
            Team Home = league.Teams.Where(p => p.Name == homeTeamCB.Text).First();
            Team Away = league.Teams.Where(p => p.Name == awayTeamCB.Text).First();

            if (homeTeamCB.Text != "" && homeTeamCB.Text != awayTeamCB.Text && awayTeamCB.Text != "")
            {
               
                Prediction prediction = new Prediction(Home, Away, league, DateTime.Now);

                homePB.Value = Convert.ToInt32(prediction.Home);
                drawPB.Value = Convert.ToInt32(prediction.Draw);
                awayPB.Value = Convert.ToInt32(prediction.Away);

                bttsYesPB.Value = Convert.ToInt32(prediction.BTTS);
                bttsNoPB.Value = Convert.ToInt32(prediction.NoBTTS);

                O15PB.Value = Convert.ToInt32(prediction.Over15);
                O25PB.Value = Convert.ToInt32(prediction.Over25);
                O35PB.Value = Convert.ToInt32(prediction.Over35);
                O45PB.Value = Convert.ToInt32(prediction.Over45);
                O55PB.Value = Convert.ToInt32(prediction.Over55);
            }
        }

        private void metroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            trustValue.Text = metroTrackBar1.Value.ToString() + "%";
        }

        private void resultsLeagueCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            resultsSeasonCB.Items.Clear();

            League league = leagueList.Where(p => p.Name == resultsLeagueCB.Text).First();

            resultsSeasonCB.Items.Add(league.Season);

            foreach (League season in league.lastSeasons)
            {
                if (!resultsSeasonCB.Items.Contains(season.Season))
                {
                    resultsSeasonCB.Items.Add(season.Season);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double good = 0;
            double bad = 0;

            double percentage = 0;


            if(resultsSeasonCB.SelectedIndex != -1 && resultsLeagueCB.SelectedIndex != -1 && resultsBetCB.SelectedIndex != -1)
            {
                resultsLV.Items.Clear();

                League league = leagueList.Where(p => p.Name == resultsLeagueCB.Text).First();

                if(league.Season != resultsSeasonCB.Text)
                {
                    foreach(League season in league.lastSeasons)
                    {
                        if(season.Season == resultsSeasonCB.Text)
                        {
                            league = season;
                        }
                    }
                }

                foreach(Foo text in league.asList)
                {
                    Team Home = league.Teams.Where(p => p.Name == text.HomeTeam).First();
                    Team Away = league.Teams.Where(p => p.Name == text.AwayTeam).First();

                    DateTime date;

                    if (DateTime.TryParseExact(text.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {

                    }
                    else
                    {
                        if (DateTime.TryParseExact(text.Date, "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {

                        }
                    }

                    Prediction prediction = new Prediction(Home, Away, league, date);

                    switch (resultsBetCB.Text)
                    {
                        case "1X2":

                            if(prediction.Home > metroTrackBar1.Value && prediction.Home > prediction.Away && prediction.Home > prediction.Draw)
                            {
                                if(text.FTHG > text.FTAG) //HOME TEAM WIN
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.LimeGreen;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("1");
                                    lvi.SubItems.Add(prediction.Home.ToString());
                                    resultsLV.Items.Add(lvi);
                                    good++;
                                }
                                else if(text.FTHG < text.FTAG)
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("1");
                                    lvi.SubItems.Add(prediction.Home.ToString());
                                    resultsLV.Items.Add(lvi);
                                    bad++;
                                }
                                else
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("1");
                                    lvi.SubItems.Add(prediction.Home.ToString());
                                    resultsLV.Items.Add(lvi);
                                    bad++;
                                }
                            }else if(prediction.Away > metroTrackBar1.Value && prediction.Away > prediction.Home && prediction.Away > prediction.Draw)
                            {
                                if (text.FTHG < text.FTAG) //AWAY TEAM WIN
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.LimeGreen;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("2");
                                    lvi.SubItems.Add(prediction.Away.ToString());
                                    resultsLV.Items.Add(lvi);
                                    good++;
                                }
                                else if (text.FTHG > text.FTAG)
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("2");
                                    lvi.SubItems.Add(prediction.Away.ToString());
                                    resultsLV.Items.Add(lvi);
                                    bad++;
                                }
                                else
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("2");
                                    lvi.SubItems.Add(prediction.Home.ToString());
                                    resultsLV.Items.Add(lvi);
                                    bad++;
                                }
                            }
                            else if(prediction.Draw > metroTrackBar1.Value && prediction.Draw > prediction.Home && prediction.Draw > prediction.Away)
                            {
                                if (text.FTHG < text.FTAG) //AWAY TEAM WIN
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("X");
                                    lvi.SubItems.Add(prediction.Away.ToString());
                                    resultsLV.Items.Add(lvi);
                                    good++;
                                }
                                else if (text.FTHG > text.FTAG)
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("X");
                                    lvi.SubItems.Add(prediction.Away.ToString());
                                    resultsLV.Items.Add(lvi);
                                    bad++;
                                }
                                else
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.LimeGreen;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("X");
                                    lvi.SubItems.Add(prediction.Home.ToString());
                                    resultsLV.Items.Add(lvi);
                                    bad++;
                                }
                            }
                            

                            break;

                        case "BTTS YES":
                            if (prediction.BTTS > metroTrackBar1.Value)
                            {
                                if(text.FTHG > 0 && text.FTAG > 0)
                                {
                                    good++;

                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.LimeGreen;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString()); 
                                    lvi.SubItems.Add("BTTS");
                                    lvi.SubItems.Add(prediction.BTTS.ToString());
                                    resultsLV.Items.Add(lvi);
                                }
                                else
                                {
                                    bad++;

                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("BTTS");
                                    lvi.SubItems.Add(prediction.BTTS.ToString());
                                    resultsLV.Items.Add(lvi);
                                }
                            }/*else if(prediction.NoBTTS > metroTrackBar1.Value && prediction.NoBTTS > prediction.BTTS)
                            {
                                if (text.FTHG > 0 && text.FTAG > 0)
                                {
                                    bad++;

                                    ListViewItem lvi = new ListViewItem();  
                                    lvi.ForeColor = Color.Red;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("BTTS NO");
                                    lvi.SubItems.Add(prediction.BTTS.ToString());
                                    resultsLV.Items.Add(lvi);
                                }
                                else
                                {
                                    good++;

                                    ListViewItem lvi = new ListViewItem();
                                    lvi.ForeColor = Color.LimeGreen;
                                    lvi.Text = Home.Name + " - " + Away.Name;
                                    lvi.SubItems.Add(text.FTHG.ToString() + "-" + text.FTAG.ToString());
                                    lvi.SubItems.Add("BTTS NO");
                                    lvi.SubItems.Add(prediction.BTTS.ToString());
                                    resultsLV.Items.Add(lvi);
                                }
                            }*/
                            break;

                            
                    }
                }

                goodCount.Text = good.ToString();
                badCount.Text = bad.ToString();

                percentage = good / (good + bad) * 100;
                percentage = Math.Round(percentage, 2);

                goodPercentage.Text = percentage.ToString();
            }
        }
    }
}
