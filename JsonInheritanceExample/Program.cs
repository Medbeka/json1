using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        // === STEP 1: Create and write a manual JSON file ===
        var users = new List<User>
        {
            new User { Name = "Alice", Age = 25, City = "Vilnius" },
            new User { Name = "Bob", Age = 30, City = "Kaunas" }
        };
        File.WriteAllText("user.json", JsonConvert.SerializeObject(users, Formatting.Indented));
        Console.WriteLine(" user.json created.");

        // === STEP 2: Read XML ===
        File.WriteAllText("users.xml", @"<Users><User><Name>Eva</Name><Age>28</Age><City>Klaipėda</City></User></Users>");
        var xmlDoc = XDocument.Load("users.xml");
        foreach (var elem in xmlDoc.Descendants("User"))
        {
            Console.WriteLine($"XML User: {elem.Element("Name")?.Value}, {elem.Element("Age")?.Value}, {elem.Element("City")?.Value}");
        }

        // === STEP 3: Deserialize JSON to C# objects ===
        string json = File.ReadAllText("user.json");
        var userList = JsonConvert.DeserializeObject<List<User>>(json);
        Console.WriteLine("\n--- JSON Users ---");
        foreach (var user in userList)
        {
            Console.WriteLine($"{user.Name}, Age: {user.Age}, City: {user.City}");
        }

        // === STEP 4: Create specialized user types ===
        var mixedUsers = new List<User>
        {
            new Admin { Name = "John", Age = 35, City = "New York", Permissions = "Full" },
            new RegularUser { Name = "Sara", Age = 22, City = "Berlin", SubscriptionLevel = "Premium" }
        };

        // === STEP 5: Serialize user_types.json ===
        string typeJson = JsonConvert.SerializeObject(mixedUsers, Formatting.Indented,
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        File.WriteAllText("user_types.json", typeJson);
        Console.WriteLine("\n user_types.json created with inherited types.");

        // === STEP 6: Deserialize user_types.json and output ===
        var deserializedUsers = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("user_types.json"),
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

        Console.WriteLine("\n--- Deserialized user_types.json ---");
        foreach (var user in deserializedUsers)
        {
            if (user is Admin admin)
            {
                Console.WriteLine($"Admin: {admin.Name}, Permissions: {admin.Permissions}");
            }
            else if (user is RegularUser regular)
            {
                Console.WriteLine($"Regular User: {regular.Name}, Subscription: {regular.SubscriptionLevel}");
            }
        }
    }
}
