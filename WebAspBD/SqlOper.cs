using System.Data.SqlClient;
namespace WebAspBD
{
    public class Sql_Operation
    {
        public SqlCommand command { get; set; }
        public User user { get; set; }
        private int UserEmailID { get; set; }
        private int UserPassID { get; set; }
        private int UserTokenID { get; set; }
        private int UserRoleID { get; set; }
        private SqlDataReader reader { get; set; }

        public Sql_Operation(User user)
        {
            this.user = user;
            Connection_DataBase.Connect();


        }
        private bool ExistLogin(string login)
        {
            if (Connection_DataBase.isConnect)
            {

                using (command = new SqlCommand($"SELECT * FROM [User] WHERE [Login]='{user.Login}'", Connection_DataBase.conn))
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        return false;
                    }

                }


            }
            return false;
        }
        private bool ExistEmail(string email)
        {
            if (Connection_DataBase.isConnect)
            {

                using (command = new SqlCommand($"SELECT * FROM [User_Mail] WHERE [Email]='{user.Email}'", Connection_DataBase.conn))
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        return false;
                    }
                }


            }
            return false;
        }
        private bool ExistPass(string pass)
        {
            if (Connection_DataBase.isConnect)
            {

                using (command = new SqlCommand($"SELECT * FROM [User_Password] WHERE [Hash]='{user.Pass}'", Connection_DataBase.conn))
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        return false;
                    }

                }


            }
            return false;
        }
        public bool AddUser(User user)
        {
            if (!ExistEmail(user.Email) && !ExistLogin(user.Login) && !ExistPass(user.Pass))
            {
                if (Connection_DataBase.isConnect)
                {
                    if (SaveMail() && SavePass() && SaveToken())
                    {
                        if (TakeIDItems())
                        {
                            var dt = user.BirthDay.ToString("yyyy-MM-dd");
                            using (command = new SqlCommand($"INSERT INTO [User]VALUES('{user.Login}','{user.BirthDay.ToString("yyyy-MM-dd")}','{user.dateRegistr.ToString("yyyy-MM-dd H:m:s")}',{UserEmailID},{UserPassID},{UserTokenID},{UserRoleID})", Connection_DataBase.conn))
                            {
                                reader = command.ExecuteReader();
                                if (reader.Read())
                                {
                                    reader.Close();
                                    return true;
                                }
                                else
                                {
                                    reader.Close();
                                    return false;
                                }

                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        private bool TakeIDItems()
        {
            bool Check = false;
            using (command = new SqlCommand($"SELECT ID FROM [User_Password]WHERE[HASH]='{user.Pass}'", Connection_DataBase.conn))
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserPassID = (int)reader["ID"];
                }
                reader.Close();

                command = new SqlCommand($"SELECT ID FROM [User_Mail]WHERE[Email]='{user.Email}'", Connection_DataBase.conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserEmailID = (int)reader["ID"];
                }
                reader.Close();

                command = new SqlCommand($"SELECT ID FROM [User_Token]WHERE[Token]='{user.Token}'", Connection_DataBase.conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserTokenID = (int)reader["ID"];
                }
                reader.Close();
                UserRoleID = 1;
            }
            if (UserEmailID > 0 && UserPassID > 0 && UserRoleID > 0 && UserTokenID > 0)
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            return Check;
        }
        private bool SaveMail()
        {

            using (command = new SqlCommand($"INSERT INTO [User_Mail]VALUES('{user.Email}')", Connection_DataBase.conn))
            {
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception)
                {

                    return false;
                }
            }

        }
        private bool SavePass()
        {
            using (command = new SqlCommand($"INSERT INTO [User_Password]VALUES('{user.Pass}')", Connection_DataBase.conn))
            {
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception)
                {

                    return false;
                }

            }


        }
        private bool SaveToken()
        {
            user.GenToken();
            using (command = new SqlCommand($"INSERT INTO [User_Token]VALUES('{user.Token}')", Connection_DataBase.conn))
            {
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (System.Exception)
                {

                    return false;
                }

            }
        }
        public bool Login()
        {

            if (FindLoginDB() && FindPassDB())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool FindPassDB()
        {
            using (command = new SqlCommand($"SELECT [HASH] FROM [User_Password]WHERE [HASH]='{user.Pass}'", Connection_DataBase.conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
        private bool FindLoginDB()
        {
            using (command = new SqlCommand($"SELECT [Login] FROM [User]WHERE [Login]='{user.Login}'", Connection_DataBase.conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                    return false;
                }

            }
        }
    }
}