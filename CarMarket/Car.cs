using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace CarMarket
{
    public class Car
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public int Id { get; set; }
        public override string ToString()
        {
            return $"Manufacturer: {Manufacturer}; Model: {Model}; Year:{Year}; Price: {Price}$; Id:{Id}";
        }

        private static List<Car> cars = new List<Car>()
        {
            new Car() { Manufacturer = "Bmw", Model = "M4", Price = 45000, Id = 101, Year = 2020 },
            new Car() { Manufacturer = "Honda", Model = "Civic", Price = 22000, Id = 102, Year = 2021 },
            new Car() { Manufacturer = "Mercedes", Model = "E63", Price = 63000, Id = 103, Year = 2019 },
            new Car() { Manufacturer = "Chevrolet",     Model = "Malibu", Price = 23000, Id = 104, Year = 2022 },
            new Car() { Manufacturer = "Nissan", Model = "Sentra", Price = 19500, Id = 105, Year = 2018 },
            new Car() { Manufacturer = "Bmw", Model = "X5", Price = 20500, Id = 106, Year = 2021 },
            new Car() { Manufacturer = "Hyundai",   Model = "Elantra", Price = 19800, Id = 107, Year = 2020 },
            new Car() { Manufacturer = "Kia", Model = "Forte", Price = 20000, Id = 108, Year = 2022 },
            new Car() { Manufacturer = "Volkswagen", Model = "Jetta", Price = 21500, Id = 109, Year = 2019 },
            new Car() { Manufacturer = "Subaru", Model = "Impreza", Price = 22500, Id = 110, Year = 2021 }
        };
       

        public static void SaveCarsInfo()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\cars.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(cars, options);
            File.WriteAllText(path, json);
        }

        public static void LoadCarsFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\cars.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                cars = JsonSerializer.Deserialize<List<Car>>(json) ?? new List<Car>();
            }
        }
        public void WriteFile(string content)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\cars.txt";
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(content);
                }
            }
        }

        public void ReadFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\cars.txt";
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        Console.WriteLine(sr.ReadToEnd());
                    }
                }
            }
        }
            public void SortByAsc()
        {
            var sortedCars = cars.OrderBy(c => c.Price).ToList();
            Console.WriteLine("Cars sorted by price (ascending):");
            foreach (var car in sortedCars)
            {
                Console.WriteLine(car);
            }
        }
        public void SortByDesc()
        {
            var sortedCars = cars.OrderByDescending(c => c.Price).ToList();
            Console.WriteLine("Cars sorted by price (ascending):");
            foreach (var car in sortedCars)
            {
                Console.WriteLine(car);
            }
        }

   
        public static void AddCar()
        {
            Car newCar = new Car();

            Console.WriteLine("Enter Manufacturer:");
            newCar.Manufacturer = Console.ReadLine();  
            Console.WriteLine("Manufacturer added.");

            Console.WriteLine("Enter ID:");
            newCar.Id = int.Parse(Console.ReadLine());  
            Console.WriteLine("ID added.");

            Console.WriteLine("Enter Model:");
            newCar.Model = Console.ReadLine();  
            Console.WriteLine("Model added.");

            Console.WriteLine("Enter Price:");
            newCar.Price = int.Parse(Console.ReadLine());  
            Console.WriteLine("Price added.");

            Console.WriteLine("Enter Year of Release:");
            newCar.Year = int.Parse(Console.ReadLine());  
            Console.WriteLine("Year of release added.");

       
            cars.Add(newCar);
            Console.WriteLine("Car successfully added to the collection.");
             SaveCarsInfo();
        }
        public static void RemoveCar(int id)
        {
            var car = cars.FirstOrDefault(x => x.Id == id);
            if (car != null)
            {
                cars.Remove(car);
                Console.WriteLine("Car removed successfully.");
                SaveCarsInfo();
            }
            else
            {
                Console.WriteLine("Car not found.");
            }
            
        }

        public static void DisplayAllCars()
        {
            if (cars.Count > 0)
            {
                Console.WriteLine("Available cars:");
                foreach (var car in cars)
                {
                    Console.WriteLine(car);
                }
            }
            else
            {
                Console.WriteLine("No cars available.");
            }
        }
        public static Car GetCarById(int id)
        {
            return cars.FirstOrDefault(car => car.Id == id);
        }
      
    }
}
