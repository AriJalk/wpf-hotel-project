using HotelProject.Model.BaseClasses;
using HotelProject.Model.DbClasses;
using HotelProject.Model.Helpers;
using System.Collections.Generic;


namespace HotelProject.ViewModel.Helpers
{
    static public class DbManagementMethods
    {
        public enum UserTypes
        {
            Customer,
            Worker,
            Manager
        };

        static public void CreateTables()
        {
            SqlDatabaseHelper.CreateTable<Customer>();
            SqlDatabaseHelper.CreateTable<Floor>();
            SqlDatabaseHelper.CreateTable<Room>();
            SqlDatabaseHelper.CreateTable<RoomType>();
            SqlDatabaseHelper.CreateTable<RoomReservation>();
            SqlDatabaseHelper.CreateTable<Service>();
            SqlDatabaseHelper.CreateTable<ServiceGroup>();
            SqlDatabaseHelper.CreateTable<Transaction>();
            SqlDatabaseHelper.CreateTable<TransactionPart>();
        }

        static public void DeleteAllTables()
        {
            SqlDatabaseHelper.DeleteTable<Customer>();
            Customer.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<Floor>();
            Floor.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<Room>();
            Room.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<RoomType>();
            RoomType.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<RoomReservation>();
            RoomReservation.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<Service>();
            Service.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<ServiceGroup>();
            ServiceGroup.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<Transaction>();
            Transaction.SetIdCount(0);
            SqlDatabaseHelper.DeleteTable<TransactionPart>();
            TransactionPart.SetIdCount(0);
        }
        static public void SetInitialData()
        {
            List<Floor> floors = new List<Floor>();
            for (int i = 0; i < 10; i++)
            {
                floors.Add(new Floor(i + 1));
                SqlDatabaseHelper.Insert(floors[i]);
            }
            RoomType regular = new RoomType(RoomTypesEnum.Regular,0);
            RoomType luxury = new RoomType(RoomTypesEnum.Luxury,500);
            RoomType penthouse = new RoomType(RoomTypesEnum.Penthouse, 1000);
            UserType worker = new UserType(UserTypesEnum.Worker);
            UserType manager = new UserType(UserTypesEnum.Manager);
            ServiceGroup sg1 = new ServiceGroup("Lodging");
            ServiceGroup sg2 = new ServiceGroup("Room Service");
            SqlDatabaseHelper.Insert(new Service("Weekday Half Pension Night", sg1, 400));
            SqlDatabaseHelper.Insert(new Service("Weekday Full Pension Night", sg1, 600));
            SqlDatabaseHelper.Insert(new Service("Weekend Half Pension Night", sg1, 600));
            SqlDatabaseHelper.Insert(new Service("Weekend Full Pension Night", sg1, 800));
            SqlDatabaseHelper.Insert(new Service("Food Delivery", sg2, 50));
            List<Room> rooms = new List<Room>();
            //Generate rooms (heaviest operation)
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j += 2)
                {
                    Room room1 = new Room((i + 1) * 100 + j + 1, regular, floors[i], 2, 1);
                    Room room2 = new Room((i + 1) * 100 + j + 2, luxury, floors[i], 4, 2);
                    if (i == 9)
                    {
                        room1.RoomType = penthouse;
                        room2.RoomType = penthouse;
                    }
                    SqlDatabaseHelper.Insert(room1);
                    SqlDatabaseHelper.Insert(room2);
                    rooms.Add(room1);
                    rooms.Add(room2);
                }
            }
            Customer.SetIdCount(0);
            Customer cus1=new Customer(new Person("Customer1", "Family1", "05000001","3019001"));
            SqlDatabaseHelper.Insert(cus1);
            SqlDatabaseHelper.Insert(new Customer(new Person("Customer2", "Family2", "05000002","0001")));
            SqlDatabaseHelper.Insert(new Customer(new Person("Customer3", "Family3", "052674", "000155")));
            User.SetIdCount(0);
            User ari = new User(new Person("Ari", "Jalk", "05267","301900601"), "arij", "pass", PasswordHelper.GetRandomSalt(), manager);
            SqlDatabaseHelper.Insert(regular);
            SqlDatabaseHelper.Insert(luxury);
            SqlDatabaseHelper.Insert(penthouse);
            SqlDatabaseHelper.Insert(worker);
            SqlDatabaseHelper.Insert(manager);
            SqlDatabaseHelper.Insert(sg1);
            SqlDatabaseHelper.Insert(sg2);
            SqlDatabaseHelper.Insert(ari);
        }
    }
}