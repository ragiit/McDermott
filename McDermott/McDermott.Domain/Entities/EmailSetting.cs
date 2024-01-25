namespace McDermott.Domain.Entities
{
    public partial class EmailSetting : BaseAuditableEntity
    {
        [StringLength(300)]
        public string Name { get; set; }
        public int Sequence { get; set; }
        public bool Smpt_Debug { get; set; }
        public Encripts Smtp_Encryption { get; set; }
        [StringLength(200)]
        public string Smtp_Host { get; set; }
        public string Smtp_Pass { get; set; }
        public int Smtp_Port { get; set; }
        [StringLength(200)]
        public string Smtp_User { get; set; }



        public enum Encripts
        {
            none, TLS, SSL
        }
    }
}
