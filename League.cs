using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Linq;
using System.Text.RegularExpressions;

namespace Smart_Bet
{
    public class League
    {
        public string Name;
        public string Country;
        public string Season;
        public List<League> lastSeasons = new List<League>();
        public List<Team> Teams = new List<Team>();
        public Dictionary<string, double> Table = new Dictionary<string, double>();

        public List<Match> matches = new List<Match>();

        public string URL;

        public double playedMatch;

        public double homeW;
        public double draws;
        public double awayW;

        public double homeGoals;
        public double awayGoals;

        public double homeGoalsPerMatch;
        public double awayGoalsPerMatch;

        public double BTTSYes;
        public double BTTSNo;

        public double Over15;
        public double Over25;
        public double Over35;
        public double Over45;
        public double Over55;

        public double Under15;
        public double Under25;
        public double Under35;
        public double Under45;
        public double Under55;

        IEnumerable<Foo> records;

        List<Foo> recordsList;

        public List<Foo> asList = new List<Foo>();

        public League(string name, string country, string season, string url)
        {
            Name = name;
            Country = country;
            Season = season;

           using (var client = new WebClient())
            {
                if (!Directory.Exists(Application.StartupPath + @"\Leagues"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\Leagues");

                }
                else
                {
                    if (!Directory.Exists(Application.StartupPath + @"\Leagues\" + name))
                    {
                        Directory.CreateDirectory(Application.StartupPath + @"\Leagues\" + name);
                    }
                }
                client.DownloadFile(url, Application.StartupPath + @"\Leagues\" + name + @"\" + season + ".csv");
            }

            using (var reader = new StreamReader(Application.StartupPath + @"\Leagues\" + name + @"\" + season + ".csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                
                recordsList = csv.GetRecords<Foo>().ToList();
                asList = recordsList;
                CSVRead();
            }

            

           /* using (var reader = new StreamReader(Application.StartupPath + @"\Leagues\" + name + @"\" + season + ".csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Foo>();              
                CSVRead();
            }*/



            homeGoalsPerMatch = (double)homeGoals / (double)playedMatch;
            awayGoalsPerMatch = (double)awayGoals / (double)playedMatch;
            RefreshRank();

            foreach (Team t in Teams)
            {
                t.RefreshStats();
            }

            

        }
        public void CSVRead()
        {
            string d = "a";

            try
            {
                foreach (var text in recordsList)
                {

                    if (text.FTHG.HasValue && text.FTAG.HasValue)
                    {
                        d = text.Date;
                        List<Team> matchHome = Teams.FindAll(x => x.Name == text.HomeTeam);
                        List<Team> matchAway = Teams.FindAll(x => x.Name == text.AwayTeam);

                        Match match = new Match();
                        match.HomeTeam = text.HomeTeam;
                        match.AwayTeam = text.AwayTeam;
                        match.HomeGoals = text.FTHG.GetValueOrDefault();
                        match.AwayGoals = text.FTAG.GetValueOrDefault();
                        match.Date = text.Date;
                        homeGoals += text.FTHG.GetValueOrDefault();
                        awayGoals += text.FTAG.GetValueOrDefault();
                        playedMatch++;
                        if (matchHome.Count<Team>() < 1)
                        {
                            Team team = new Team(text.HomeTeam);
                            Teams.Add(team);
                        }

                        if (matchAway.Count<Team>() < 1)
                        {
                            Team team = new Team(text.AwayTeam);
                            Teams.Add(team);
                        }


                        Teams.Where(p => p.Name == match.HomeTeam).First<Team>().HomeMatches.Insert(0, match);
                        Teams.Where(p => p.Name == match.AwayTeam).First<Team>().AwayMatches.Insert(0, match);
                        Teams.Where(p => p.Name == match.AwayTeam).First<Team>().lastMatches.Insert(0, match);
                        Teams.Where(p => p.Name == match.HomeTeam).First<Team>().lastMatches.Insert(0, match);
                        matches.Add(match);
                        homeGoalsPerMatch += text.FTHG.GetValueOrDefault();
                        awayGoalsPerMatch += text.FTAG.GetValueOrDefault();

                        // Home Win
                        if (text.FTHG > text.FTAG)
                        {
                            homeW++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HomeW++;
                                    t.playedMatch++;
                                    t.homePlayedMatch++;
                                    t.Points += 3;
                                    t.HomeScoredGoals += text.FTHG.GetValueOrDefault();
                                    t.HomeConcededGoals += text.FTAG.GetValueOrDefault();
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AwayL++;
                                    t.playedMatch++;
                                    t.awayPlayedMatch++;
                                    t.AwayScoredGoals += text.FTAG.GetValueOrDefault();
                                    t.AwayConcededGoals += text.FTHG.GetValueOrDefault();
                                }
                            }
                        }
                        ///////////////////////////////
                        // Draw
                        if (text.FTHG == text.FTAG)
                        {
                            draws++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HomeD++;
                                    t.homePlayedMatch++;
                                    t.playedMatch++;
                                    t.Points += 1;
                                    t.HomeScoredGoals += text.FTHG.GetValueOrDefault();
                                    t.HomeConcededGoals += text.FTAG.GetValueOrDefault();
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AwayD++;
                                    t.playedMatch++;
                                    t.awayPlayedMatch++;
                                    t.Points += 1;
                                    t.AwayScoredGoals += text.FTAG.GetValueOrDefault();
                                    t.AwayConcededGoals += text.FTHG.GetValueOrDefault();
                                }
                            }
                        }
                        ///////////////////////////////
                        // Away
                        if (text.FTHG < text.FTAG)
                        {
                            awayW++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.homePlayedMatch++;
                                    t.HomeL++;
                                    t.playedMatch++;
                                    t.HomeScoredGoals += text.FTHG.GetValueOrDefault();
                                    t.HomeConcededGoals += text.FTAG.GetValueOrDefault();
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.awayPlayedMatch++;
                                    t.Points += 3;
                                    t.playedMatch++;
                                    t.AwayW++;
                                    t.AwayScoredGoals += text.FTAG.GetValueOrDefault();
                                    t.AwayConcededGoals += text.FTHG.GetValueOrDefault();
                                }
                            }
                        }
                        ///////////////////////////////
                        // BTTS
                        if (text.FTHG > 0 && text.FTAG > 0)
                        {
                            BTTSYes++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HomeBTTSYes++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AwayBTTSYes++;
                                }
                            }
                        }
                        else
                        {
                            BTTSNo++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HomeBTTSNo++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AwayBTTSNo++;
                                }
                            }
                        }
                        ///////////////////////////////
                        int totalGoals = text.FTHG.GetValueOrDefault() + text.FTAG.GetValueOrDefault();
                        // Over 1.5
                        if (totalGoals > 1)
                        {
                            Over15++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HOver15++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AOver15++;
                                }
                            }
                        }
                        else
                        {
                            Under15++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HUnder15++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AUnder15++;
                                }
                            }
                        }
                        ///////////////////////////////
                        // Over 2.5
                        if (totalGoals > 2)
                        {
                            Over25++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HOver25++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AOver25++;
                                }
                            }
                        }
                        else
                        {
                            Under25++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HUnder25++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AUnder25++;
                                }
                            }
                        }
                        ///////////////////////////////
                        // Over 3.5
                        if (totalGoals > 3)
                        {
                            Over35++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HOver35++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AOver35++;
                                }
                            }
                        }
                        else
                        {
                            Under35++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HUnder35++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AUnder35++;
                                }
                            }
                        }
                        ///////////////////////////////
                        // Over 4.5
                        if (totalGoals > 4)
                        {
                            Over45++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HOver45++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AOver45++;
                                }
                            }
                        }
                        else
                        {
                            Under45++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HUnder45++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AUnder45++;
                                }
                            }
                        }
                        ///////////////////////////////
                        // Over 5.5
                        if (totalGoals > 5)
                        {
                            Over55++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HOver55++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AOver55++;
                                }
                            }

                        }
                        else
                        {
                            Under55++;
                            foreach (Team t in Teams)
                            {
                                if (t.Name == text.HomeTeam)
                                {
                                    t.HUnder55++;
                                }

                                if (t.Name == text.AwayTeam)
                                {
                                    t.AUnder55++;
                                }
                            }
                        }
                    }
                    
                }
            }
            catch (Exception exc)
            {

              
            }

           
        }

        public void RefreshRank()
        {
            Table.Clear();

            foreach (Team t in Teams)
            {
                Table.Add(t.Name, t.Points);
            }

            Table = Table.OrderByDescending(u => u.Value).ToDictionary(z => z.Key, y => y.Value);
        }
    }
}
