using CarMarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text.Json;

namespace CarMarket
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public int Age { get; set; }
        public int Balance { get; set; } = 50000;

        public static List<User> Users { get; set; } = new List<User>();

        public List<Car> PurchasedCars { get; set; } = new List<Car>();

        public override string ToString()
        {
            return $"{Name} {Password} {Role} {Balance} {Age}";
        }
        private static readonly string UserFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\users.json";

      
        public static void SaveUsersInfo()
        {
          
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Users, options);
            File.WriteAllText(UserFilePath, json);
        }
        public static void LoadUsersFile()
        {



            if (File.Exists(UserFilePath))
            {
                string json = File.ReadAllText(UserFilePath);
                Users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
        }
        
        public void CheckBalance()
        {
            Console.WriteLine($"Your balance is: {Balance} $");
        }


        public static void Register(string name, string password, string role, int age)
        {
            while (true)
            {
                try
                {
                    if (age <= 18)
                    {
                        throw new Exception("Exception: You are under 18.");
                    }

                    Users.Add(new User { Name = name, Password = password, Role = role, Age = age });
                    SaveUsersInfo();
                    Console.WriteLine("Registered successfully.");
                    break; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please try again.");
                    Console.Write("Enter Age: ");
                    age = int.Parse(Console.ReadLine());
                }
            }
        }

       

        public User Login(string name, string password)
        {
            while (true)
            {
                try
                {
                    var user = Users.FirstOrDefault(x => x.Name == name && x.Password == password);
                    if (user == null)
                    {
                        throw new CarException("Incorrect username or password.");
                    }

                    Console.WriteLine(user.ToString());
                 
                    Console.WriteLine($"Logged in as {user.Role}");
                    return user; 
                }
                catch (CarException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Please try again.");
                    Console.Write("Enter Name: ");
                    name = Console.ReadLine();
                    Console.Write("Enter Password: ");
                    password = Console.ReadLine();
                }
            }
        }


        public void BuyCar(Car car)
        {
            if (Balance < car.Price)
            {
                throw new CarException($"you dont have enough money for {car.Model}. You need atleast {car.Price - Balance} broke boy");
            }

            Balance -= car.Price;
            PurchasedCars.Add(car);
            Console.WriteLine($"You have bought the car {car.Model} for {car.Price}. Your balance = {Balance}.");
            SaveUsersInfo();
        }

        public void SellCar(Car car)
        {
            if (!PurchasedCars.Contains(car))
            {
                throw new CarException($"Exception: you dont own {car.Model}");
            }

            Balance += car.Price;
            PurchasedCars.Remove(car);
            Console.WriteLine($"You have sold the car {car.Model} for {car.Price}. Your new balance is {Balance}.");
            SaveUsersInfo();
        }

        public void DisplayPurchasedCars()
        {
            if (PurchasedCars.Count == 0)
            {
                Console.WriteLine("You have not purchased any cars.");
            }
            else
            {
                Console.WriteLine("Purchased Cars:");
                foreach (var car in PurchasedCars)
                {
                    Console.WriteLine($"Car Model: {car.Model}, Price: {car.Price}, Year: {car.Year}, Manufacturer: {car.Manufacturer}");
                }
            }
        }

        public static void RegistrationMenu()
        {
            LoadUsersFile();
            Car.LoadCarsFile();
            User loggedInUser = null;

            while (true)
            {
                Console.WriteLine("Select an option: ");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                string option = Console.ReadLine();

                try
                {
                    switch (option)
                    {
                        case "1":
                            Console.Write("Enter Name: ");
                            string name = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            string password = Console.ReadLine();
                            Console.Write("Enter Role (admin/user): ");
                            string role = Console.ReadLine();
                            Console.Write("Enter Age: ");
                            int age = int.Parse(Console.ReadLine());

                            Register(name, password, role, age);
                            loggedInUser = new User().Login(name, password);

                            if (loggedInUser.Role == "admin")
                            {
                                AdminMenu(loggedInUser);
                            }
                            else if (loggedInUser.Role == "user")
                            {
                                UserMenu(loggedInUser);
                            }
                            break;

                        case "2":
                            Console.Write("Enter Name: ");
                            name = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            password = Console.ReadLine();

                            loggedInUser = new User().Login(name, password);

                            if (loggedInUser.Role == "admin")
                            {
                                AdminMenu(loggedInUser);
                            }
                            else if (loggedInUser.Role == "user")
                            {
                                UserMenu(loggedInUser);
                            }
                            break;

                        case "3":
                            Console.WriteLine("Bye Bye");
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}. Please try again.");
                }
            }
        }


        public static void AdminMenu(User loggedInUser)
        {
            while (true)
            {
                Console.WriteLine("Admin Options: ");
                Console.WriteLine("1: Add Car");
                Console.WriteLine("2: Remove Car");
                Console.WriteLine("3: Display All Cars");
                Console.WriteLine("4: Update Car");
                Console.WriteLine("5: Log Out");

                string adminOption = Console.ReadLine();

                switch (adminOption)
                {
                    case "1":
                        Car.AddCar();
                        break;

                    case "2":

                        Console.WriteLine("Enter the car ID to remove: ");
                        int removeCarId = int.Parse(Console.ReadLine());
                        Car.RemoveCar(removeCarId);
                        break;

                    case "3":
                        Car.DisplayAllCars();
                        break;

                    case "4":
                        Console.WriteLine("Enter the car ID to update the price: ");
                        int carId = int.Parse(Console.ReadLine());
                        Car car = Car.GetCarById(carId);

                        if (car != null)
                        {
                            Console.WriteLine("Enter the new price: ");
                            int newPrice = int.Parse(Console.ReadLine());
                            car.Price = newPrice;
                            Car.SaveCarsInfo();
                            Console.WriteLine($"The new price of {car.Model} is {newPrice}");
                        }
                        else
                        {
                            Console.WriteLine("Car not found.");
                        }
                        break
                            ;
                    case "5":
                        Console.WriteLine("Bye bye");
                        return;

                    default:
                        Console.WriteLine("Invalid");
                        break;
                }
            }
        }

        public static void UserMenu(User loggedInUser)
        {
            Car carManager = new Car();
            while (true)
            {
                Console.WriteLine("User Options: ");
                Console.WriteLine("1: Buy Car");
                Console.WriteLine("2: My Car");
                Console.WriteLine("3: Display All Cars");
                Console.WriteLine("4: Check Balance");
                Console.WriteLine("5: Sell My Car");
                Console.WriteLine("6: Sort Price By Ascending");
                Console.WriteLine("7: Sort Price By Descending");
                Console.WriteLine("8: Log Out");

                string userOption = Console.ReadLine();

                switch (userOption)
                {
                    case "1":
                        Car.DisplayAllCars();
                        Console.WriteLine("Enter the car ID to buy: ");
                        int carId = int.Parse(Console.ReadLine());

                        Car carToBuy = Car.GetCarById(carId);

                        if (carToBuy != null)
                        {
                            loggedInUser.BuyCar(carToBuy);
                        }
                        else
                        {
                            Console.WriteLine("Car not found.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Your Cars: ");
                        loggedInUser.DisplayPurchasedCars();
                        break;

                    case "3":
                        Car.DisplayAllCars();
                        break;

                    case "4":
                        loggedInUser.CheckBalance();
                        break;

                    case "5":
                        Console.WriteLine("Your Cars: ");
                        loggedInUser.DisplayPurchasedCars();
                        Console.WriteLine("Enter the car ID to sell: ");
                        int carId2 = int.Parse(Console.ReadLine());
                        Car carToSell = Car.GetCarById(carId2);
                        if (carToSell != null)
                        {
                            loggedInUser.SellCar(carToSell);
                        }
                        else
                        {
                            Console.WriteLine("Car not found.");
                        }
                        break;

                    case "6":
                        carManager.SortByAsc();
                        break;

                    case "7":
                        carManager.SortByDesc();
                        break;

                    case "8":
                        Console.WriteLine("Logging out...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}