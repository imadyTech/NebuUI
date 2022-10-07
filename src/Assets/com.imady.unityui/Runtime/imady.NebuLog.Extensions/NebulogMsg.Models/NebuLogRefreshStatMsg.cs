
namespace imady.NebuUI
{
    public class NebuLogRefreshStatMsg : imady.NebuEvent.NebuMessageBase, INebuLogRequest
    {
        public string StatId { get; set; }
        public string StatValue { get; set; }

    }
}