using Norm;

namespace HashMasher.Model
{
    public class User
    {
        [MongoIdentifier]
        public ObjectId Id { get; set; }
        public string TwitterId { get; set; }
        public string Logon { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
        //public string RemberToken { get; set; }
        //public DateTime RememberTokenExpiresAt { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImage { get; set; }
    }
}