namespace McDermott.Telemedic.Model
{
    public class TelemedicResult
    {
        public Metadata Metadata { get; set; }
        public Data Data { get; set; }
    }

    public class Metadata
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class Data
    {
        public List<User> User { get; set; }
        public List<Docter> Docter { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string setNameFamily { get; set; }
        public DateTime? Register { get; set; } = DateTime.Now;
        // Tambahkan properti lain yang diperlukan
    }

    public class Docter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string service { get; set; }
    }
}
