using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApp2
{
    public class Player
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public Position Position { get; set; }
    }

    public enum Position
    {
        Goalkeeper,
        Defender,
        Midfielder,
        Forward
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox3.Clear();
            List<Player> Players = new List<Player>();
            string a = InputTxt.Text;
            string[] players = a.Split('\n');
            foreach (string player in players)
            {
                string[] Characteristics = player.Split();
                Player Player = new Player
                {
                    Name = Characteristics[0],
                    Rating = int.Parse(Characteristics[2])
                };

                switch (Characteristics[1])
                {
                    case "Goalkeeper":
                        Player.Position = Position.Goalkeeper;
                        break;
                    case "Defender":
                        Player.Position = Position.Defender;
                        break;
                    case "Midfielder":
                        Player.Position = Position.Midfielder;
                        break;
                    case "Forward":
                        Player.Position = Position.Forward;
                        break;
                }
                Players.Add(Player);
            }
            

            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (Players.Count % 2 == 1)
            {
                Players.Sort(delegate (Player us1, Player us2)
                { return us1.Rating.CompareTo(us2.Rating); });
            }
            int combination = 0;

            List<Player> firstTeam = new List<Player>();
            List<Player> secondTeam = new List<Player>();
            foreach (Player player in Players)
            {
                secondTeam.Add(player);
            }
            List<List<Player>> result = new List<List<Player>>();

            int goalkeeperCount = Players.Where(player => player.Position == Position.Goalkeeper).Count();
            int defenderCount = Players.Where(player => player.Position == Position.Defender).Count();
            int midfielderCount = Players.Where(player => player.Position == Position.Midfielder).Count();
            int forwardCount = Players.Where(player => player.Position == Position.Forward).Count();

            int allRatingSum = Players.Sum(player => player.Rating);
            int minSoccer = int.MaxValue;
            int minBalans = int.MaxValue;



            int bestRaiting = allRatingSum % 2;
            bool isEnd = false;

            int firstTeamCount = Players.Count / 2;

            for (int i = 0; i < Math.Pow(2, firstTeamCount); i++)
            {
                int firstTeamGoalCount = 0;
                int firstTeamDefenCount = 0;
                int firstTeamMidfenCount = 0;
                int firstTeamForvCount = 0;
                int firstTeamRating = 0;

                int teamCount = 0;
                for (int j = 0; j < firstTeamCount; j++)
                {
                    if ((i & (1 << j)) > 0)
                    {
                        Player player = Players[j];
                        firstTeamGoalCount += player.Position == Position.Goalkeeper ? 1 : 0;
                        firstTeamDefenCount += player.Position == Position.Defender ? 1 : 0;
                        firstTeamMidfenCount += player.Position == Position.Midfielder ? 1 : 0;
                        firstTeamForvCount += player.Position == Position.Forward ? 1 : 0;
                        firstTeamRating += player.Rating;
                        teamCount++;
                    }
                }
                int teamCountMore = firstTeamCount - teamCount;
                int secondTeamCount = Players.Count - firstTeamCount;
                int[] arr = new int[teamCountMore + 1];
                for (int j = 1; j <= teamCountMore; j++)
                {
                    arr[j] = j;
                }
                int p = teamCountMore;
                while (p >= 1 && teamCountMore < secondTeamCount)
                {
                    int firstTeamGoalCountMore = firstTeamGoalCount;
                    int firstTeamDefenCountMore = firstTeamDefenCount;
                    int firstTeamMidfenCountMore = firstTeamMidfenCount;
                    int firstTeamForvCountMore = firstTeamForvCount;
                    int firstTeamRatingMore = firstTeamRating;

                    for (int j = 1; j <= teamCountMore; j++)
                    {
                        int c = (firstTeamCount - 1) + arr[j];
                        Player player = Players[c];
                        firstTeamGoalCountMore += player.Position == Position.Goalkeeper ? 1 : 0;
                        firstTeamDefenCountMore += player.Position == Position.Defender ? 1 : 0;
                        firstTeamMidfenCountMore += player.Position == Position.Midfielder ? 1 : 0;
                        firstTeamForvCountMore += player.Position == Position.Forward ? 1 : 0;
                        firstTeamRatingMore += player.Rating;
                    }

                    int ratingDiff = Math.Abs(allRatingSum - 2 * firstTeamRatingMore);

                    int goalBalans = Math.Abs(goalkeeperCount - 2 * firstTeamGoalCountMore);
                    int deffBalans = Math.Abs(defenderCount - 2 * firstTeamDefenCountMore);
                    int midfenBalans = Math.Abs(midfielderCount - 2 * firstTeamMidfenCountMore);
                    int forvBalans = Math.Abs(forwardCount - 2 * firstTeamForvCountMore);

                    int balans = goalBalans + deffBalans + midfenBalans + forvBalans;
                    int soccer = ratingDiff;

                    if (goalBalans > 1 || deffBalans > 1 || midfenBalans > 1 || forvBalans > 1)
                    {
                        soccer = ratingDiff * 10;
                        if (ratingDiff == 0)
                        {
                            soccer = 100;
                        }
                    }

                    combination++;

                    if ((soccer < minSoccer) || ((soccer == minSoccer) && (balans < minBalans)))
                    {
                        minSoccer = soccer;
                        minBalans = balans;

                        firstTeam.Clear();

                        for (int j = 0; j < firstTeamCount; j++)
                        {
                            if ((i & (1 << j)) > 0)
                            {
                                firstTeam.Add(Players[j]);
                            }

                        }
                        for (int j = 1; j <= teamCountMore; j++)
                        {
                            int c = (firstTeamCount - 1) + arr[j];

                            firstTeam.Add(Players[c]);


                        }

                        if ((soccer == bestRaiting) || (soccer == 0))
                        {
                            isEnd = true;
                            break;
                        }
                    }
                    if (arr[teamCountMore] == secondTeamCount)
                    {
                        p = p - 1;
                    }
                    else
                    {
                        p = teamCountMore;
                    }

                    if (p >= 1)
                    {
                        for (int j = teamCountMore; j >= p; j--)
                        {
                            arr[j] = arr[p] + j - p + 1;
                        }
                    }
                    if (sw.ElapsedMilliseconds > 5000)
                    {
                        isEnd = true;
                        break;
                    }
                }

                if (isEnd)
                {
                    break;
                }
            }

            foreach (Player player in firstTeam)
            {
                secondTeam.Remove(player);
            }

            for (int i = 0; i < firstTeam.Count; i++)
            {
                textBox1.Text += firstTeam[i].Name + " ";
                textBox1.Text += firstTeam[i].Position + " ";
                textBox1.Text += firstTeam[i].Rating + Environment.NewLine;
            }

            for (int i = 0; i < secondTeam.Count; i++)
            {
                textBox3.Text += secondTeam[i].Name + " ";
                textBox3.Text += secondTeam[i].Position+ " ";
                textBox3.Text += secondTeam[i].Rating + Environment.NewLine;

            }
            label1.Text = Convert.ToString( sw.ElapsedMilliseconds);
            label2.Text = Convert.ToString(minSoccer);
        }
    }
}
