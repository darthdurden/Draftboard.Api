using CsvHelper.Configuration.Attributes;

namespace Draftboard.Api.Csv.Records
{
    public class PlayerRecord
    {
        [Name("ID")]
        public string Id { get; set; }
        public string Player { get; set; }
        public string Team { get; set; }
        public string Position { get; set; }
        public int RkOv { get; set; }
        public string Status { get; set; }
        public int Age { get; set; }
        public string Opponent { get; set; }
        public float Salary { get; set; }
        public string Contract { get; set; }
        public float Score { get; set; }
        [Name("%D")]
        public string PercentDrafted { get; set; }
        public string ADP { get; set; }
        [Name("Ros %")]
        public string RosteredPercentage { get; set; }
    }
}
