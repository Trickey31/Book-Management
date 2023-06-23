namespace QLThuVien.Models
{
    public class CurrentUser
    {
        private static QlthuVienLtwebContext db;
        public static User user;
        public static void initSession(string username)
        {
            db = new QlthuVienLtwebContext();
            user = db.Users.SingleOrDefault(x => x.Username == username);
        }
    }
}
