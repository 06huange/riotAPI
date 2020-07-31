using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LoL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string apiKey = "?api_key=" + "RGAPI-7f6c051a-e6d5-452d-b9f6-414835055785";
            string playerName = textBox1.Text;
            HttpClient riot = new HttpClient();
            riot.BaseAddress = new Uri("https://na1.api.riotgames.com/");

            //Basic Summoner Info -----------------------------------------------------------------------------------
            string summoner = riot.GetStringAsync("lol/summoner/v4/summoners/by-name/" + playerName + apiKey).Result;
            Summoner Data = JsonConvert.DeserializeObject<Summoner>(summoner);
            Name.Text = "Summoner Name: " + Data.name;
            Level.Text = "Level: " + Data.summonerLevel.ToString();

            //Champion Mastery -----------------------------------------------------------------------------------
            string mastery = riot.GetStringAsync("lol/champion-mastery/v4/champion-masteries/by-summoner/" + Data.id + apiKey).Result;
            List<ChampionMastery> masteryData = JsonConvert.DeserializeObject<List<ChampionMastery>>(mastery);
            long mostPlayedID = masteryData.ElementAt(0).championId;

            //Champion Info -----------------------------------------------------------------------------------
            HttpClient ddragon = new HttpClient();
            string champion = ddragon.GetStringAsync("https://raw.githubusercontent.com/ngryman/lol-champions/master/champions.json").Result;
            List<Champion> championData = JsonConvert.DeserializeObject<List<Champion>>(champion);
            int mostPlayedKey = championData.FindIndex(item => item.key == mostPlayedID);
            string mostPlayedName = championData.ElementAt(mostPlayedKey).name;

            //Summoner Rank -----------------------------------------------------------------------------------
            string RankUri = riot.GetStringAsync("lol/league/v4/entries/by-summoner/" + Data.id + apiKey).Result;
            List<Rank> RankData = JsonConvert.DeserializeObject<List<Rank>>(RankUri);
            try
            {
                Tier.Text = "Tier: " + RankData.ElementAt(1).tier + " " + RankData.ElementAt(1).rank;
            }
            catch
            {
                Tier.Text = "Tier: Unranked";
            }

            dataGridView1.Show();
            dataGridView1.DataSource = RankData;

            //Match History ---------------------------------------------------------------------------------
            string matchHistory = riot.GetStringAsync("lol/match/v4/matchlists/by-account/BGkqQ8xY9A4BPYgF0k31CUKXUmz46hXFVtuDGpDsceS0E6U?endIndex=10&beginIndex=0" + apiKey).Result;
            List<MatchHistory> matchHistoryData = JsonConvert.DeserializeObject<List<MatchHistory>>(matchHistory);
            MatchHistoryGrid.Show();
            MatchHistoryGrid.DataSource = matchHistoryData;

            //Background ------------------------------------------------------------------------------------
            //Bitmap background = new Bitmap("C:/Users/Eagle/Desktop/Miscellaneous/c4351d0fd7f7898aa02be141eea80772_wallpaper-wolf-howling-moon-silhouette-minimal-4k-creative-_3840-2160.jpeg");
            //Bitmap nBackground = new Bitmap(background, new Size(this.Width, this.Height));
            //Form1.ActiveForm.BackgroundImage = nBackground;
            pictureBox2.Hide();

            //static background
            var request = WebRequest.Create("http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + mostPlayedName + "_0" + ".jpg");
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                Form1.ActiveForm.BackgroundImage = Bitmap.FromStream(stream);
            }

            //cycling background
            //bool tryAgain = true;
            //CancellationTokenSource source = new CancellationTokenSource();

            //var t = Task.Run(async delegate
            //{
            //    await Task.Delay(TimeSpan.FromSeconds(1.5), source.Token);
            //});
            //while (tryAgain)
            //{
            //    try
            //    {
            //        int x = 0;
            //        while (true)
            //        {
            //            var request = WebRequest.Create("http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + mostPlayedName + "_" + x + ".jpg");
            //            using (var response = request.GetResponse())
            //            using (var stream = response.GetResponseStream())
            //            {
            //                Form1.ActiveForm.BackgroundImage = Bitmap.FromStream(stream);
            //            }
            //            using (WebClient webClient = new WebClient())
            //            {
            //                webClient.DownloadFile("http://ddragon.leagueoflegends.com/cdn/img/champion/splash/" + mostPlayedName + "_" + x + ".jpg", @"C:\Users\Eagle\Desktop\Project\LoL\Champions\" + mostPlayedName + "_" + x + ".png");
            //            }
            //            x++;
            //            t.Wait();
            //        }
            //    }
            //    catch
            //    {

            //    }
            //}
        }

        private void TextBoxKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //button1_Click(sender, System.Windows.Forms.MouseEventArgs e);
            }
        }

        private void Name_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void WinRate_Click(object sender, EventArgs e)
        {

        }
    }
}
