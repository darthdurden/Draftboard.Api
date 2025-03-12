using CsvHelper.Configuration.Attributes;

namespace Draftboard.Api.Csv.Records
{
    public class KeeperRecord
    {
        public string ID { get; set; }
        [Name("Status")]
        public string FantasyTeam { get; set; }
        public float Salary { get; set; }
        public string Contract { get; set; }
    }
}
