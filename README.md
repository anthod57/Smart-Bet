Smart Bet is a C# software made to predict soccer results on a match between two specified teams. Other than winning team, the software can predict: goals (Over or Under a specific number), and if the both team are going to score.

The software scraps 5 last seasons datas of: Ligue 1, Serie A, Premier League, Bundesliga and Liga from https://www.football-data.co.uk/. Datas comes in CSV files, which contains all season matches of all teams.
Once the csv files downloaded, the program use CSV Helper to read all lines from the CSV file. From all read matches, the program gather statistics (such as: number of home or away scored goals of a specific team, or percentage of away win).

Predictions are given as a percentage of chance they have of happening.

Once done, you get access to:

- Prediction: You can choose two differents teams from a league, and the program will calculate prediction, mainly using poisson distribution and percentages from gathered datas.

![alt text](https://i.imgur.com/59WIgcY.png)

- Statistics: You can navigate between 5 leagues and view datas: total scored goals, % of away wins, % of matches where 2 or more goals were scored. You can also see individual statistics of all teams.

![alt text](https://i.imgur.com/356Q8bl.png)

- Results: The program makes a prediction for all matches of the selected season (hundreds of matches) so you are able to see at what percentages of chance you get the most of good predictions.
           Ex: A prediction tell you that there is 67% of chance that both team are going to scores.
           On the results, you see that for the 2020-2021 season, the program did 88% of good predictions for 67% chance. So you know that it's a good ratio.
           
![alt text](https://i.imgur.com/EddFl0I.png)
