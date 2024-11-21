namespace McDermott.Domain.Entities
{
    public class BpjsWebServiceTemporary : BaseAuditableEntity
    {
        public string MenuName { get; set; } = string.Empty; // Example General Consultant Service
        public long? MenuId { get; set; } // Example GC with Id = 1
        public DateTime Date { get; set; } = DateTime.Now;
        public string Url { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;
        public DateTime? LastAttempt { get; set; }
        public EnumBpjsWebServiceTemporary Status { get; set; } = EnumBpjsWebServiceTemporary.Success;
    }
}