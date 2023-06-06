using HotelProject.Model.BaseClasses;
using HotelProject.Model.Helpers;
using HotelProject.Model.Interfaces;
using HotelProject.ViewModel.Helpers;
using System.Collections.Generic;
using System.Diagnostics;

namespace HotelProject.Model.DbClasses
{
    /// <summary>
    /// Represents a user that can use the program
    /// </summary>
    public class User : Person, IIncremented
    {

        public static new Dictionary<string, string> Fields { get; set; }

        private static int _idcount = 0;

        public override int IdCount
        {
            get
            {
                return _idcount;
            }
            set
            {
                _idcount = value;
            }
        }

        private int _userid;
        //DB Property
        //Primary Key
        public int UserId
        {
            get { return _userid; }
            set
            {
                _userid = value;
                if (_userid > IdCount)
                    IdCount = _userid;
            }
        }

        private string _login;
        /// <summary>
        /// DB Property
        /// Login Name
        /// </summary>
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
            }
        }


        private string _hasheddpassword;

        /// <summary>
        /// Db Property
        /// </summary>
        public string HashedPassword
        {
            get { return _hasheddpassword; }
            set { _hasheddpassword = value; }
        }

        private string _passwordSalt;
        /// <summary>
        /// DB Property
        /// </summary>
        public string PasswordSalt
        {
            get { return _passwordSalt; }
            set { _passwordSalt = value; }
        }


        private int _usertypeid;
        /// <summary>
        /// DB Property
        /// </summary>
        public int UserTypeId
        {
            get { return _usertypeid; }
            set
            {
                _usertypeid = value;
            }
        }


        private UserType _usertype;
        /// <summary>
        /// The usertype of user
        /// </summary>
        public UserType UserType
        {
            get
            {
                return _usertype;
            }
            set
            {
                if (value != null)
                {
                    _usertype = value;
                    UserTypeId = _usertype.GetPrimaryKey();
                }
            }
        }

        private List<Transaction> _transactions;
        /// <summary>
        /// All transaction made by the user
        /// </summary>
        public List<Transaction> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }

        private bool _is_checked;
        public bool Is_Checked
        {
            get
            {
                return _is_checked;
            }
            set
            {
                _is_checked = value;
                Debug.WriteLine("Is Checked - " + _is_checked);
            }
        }

        public User(Person person, string login, string hashedpassword, string passwordSalt, UserType userType) : base(person.FName, person.LName, person.PhoneNumber, person.IdNumber)
        {
            Login = login;
            HashedPassword = hashedpassword;
            PasswordSalt = passwordSalt;
            UserType = userType;
            UserId = IdCount + 1;
        }

        public User(Person person) : base(person.FName, person.LName, person.PhoneNumber, person.IdNumber)
        {
            Is_Checked = false;
            UserId = IdCount + 1;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="user"></param>
        public User(User user) : base(user.FName, user.LName, user.PhoneNumber, user.IdNumber)
        {
            Login = user.Login;
            HashedPassword = user.HashedPassword;
            PasswordSalt = user.PasswordSalt;
            UserType = user.UserType;
            _userid = user.UserId;
            CreatedTime = user.CreatedTime;
        }

        public User()
        {
            _userid = IdCount + 1;
            PasswordSalt = PasswordHelper.GetRandomSalt();
            HashedPassword = PasswordHelper.HashPassword("12345",PasswordSalt);
        }

        static User()
        {
            Fields = new Dictionary<string, string>(Person.Fields)
            {
                { "UserId", "INT NOT NULL UNIQUE" },
                { "Login","VARCHAR(50) NOT NULL UNIQUE" },
                { "HashedPassword", "VARCHAR(255) NOT NULL" },
                { "UserTypeId","INT NOT NULL" },
                { "PasswordSalt", "VARCHAR(255) NOT NULL" }
            };
        }

        public static int GetIdCount()
        {
            return _idcount;
        }

        public static void SetIdCount(int value)
        {
            _idcount = value;
        }
        public override int GetPrimaryKey()
        {
            return UserId;
        }

        public override string GetPrimaryKeyType()
        {
            return "UserId";
        }

        public override void SetPrimaryKey(int value)
        {
            UserId = value;
        }

        public override List<string> GetTableTemplate()
        {
            List<string> template = base.GetTableTemplate();
            template.Add(TableFieldFormat(GetPrimaryKeyType(), Fields[GetPrimaryKeyType()] + " PRIMARY KEY"));
            template.Add(TableFieldFormat("Login", Fields["Login"]));
            template.Add(TableFieldFormat("HashedPassword", Fields["HashedPassword"]));  
            if (UserType == null)
                UserType = new UserType();
            template.Add(TableFieldFormat("UserTypeId", UserType.Fields["UserTypeId"]));
            template.Add(TableFieldFormat("PasswordSalt", Fields["PasswordSalt"]));
            return template;
        }

        public override List<TableData> GetValues()
        {
            List<TableData> values = base.GetValues();
            values.Add(new TableData(GetPrimaryKey().ToString(), GetPrimaryKeyType()));
            values.Add(new TableData(Login, "Login"));
            values.Add(new TableData(HashedPassword, "HashedPassword"));
            values.Add(new TableData(PasswordSalt, "PasswordSalt"));
            values.Add(new TableData(UserType.UserTypeId.ToString(), "UserTypeId"));
            return values;
        }

        public override Dictionary<string, string> GetFields()
        {
            return Fields;
        }


        public override List<string> GenerateErrors()
        {
            List<string> errors = base.GenerateErrors();
            if (string.IsNullOrEmpty(Login))
                errors.Add("Login");
            if (UserType == null)
                errors.Add("User Type");
            return errors;
        }

        public void SetTempId(int id)
        {
            _userid = id;
        }

        public override void SetInDb()
        {
            if (!string.IsNullOrEmpty(FName))
                IsInDb = true;
        }
    }
}
