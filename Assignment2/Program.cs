using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    internal class Program 
    {
        static List<Item> items = new List<Item>();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n** Inventory Management Application **");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. Display Items");
                Console.WriteLine("3. Find Item by ID");
                Console.WriteLine("4. Update Item");
                Console.WriteLine("5. Delete Item");
                Console.WriteLine("6. Exit");

                Console.WriteLine("Enter your choice:");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddItem();
                        break;
                    case 2:
                        DisplayItems();
                        break;
                    case 3:
                        Console.WriteLine("Enter Item ID :");
                        int itemID = int.Parse(Console.ReadLine());
                        FindItemById(itemID);
                        break;
                    case 4:
                        UpdateItem();
                        break;

                    case 5:
                        DeleteItem();
                        break;
                    case 6:
                        Console.WriteLine("Exiting application...");
                        
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 6. ");
                        break;
                }
            }

        }
        static void AddItem()
        {
            Console.WriteLine("Enter Item ID :");
            int itemID= int.Parse( Console.ReadLine());

            Console.WriteLine("Enter Item Name:");
            string itemName = Console.ReadLine();

            Console.WriteLine("Enter Item Price :");
            double itemPrice= double.Parse( Console.ReadLine());

            Console.WriteLine("Enter Item Quantity :");
            int itemQuantity= int.Parse( Console.ReadLine());

            items.Add(new Item(itemName,itemID, itemPrice,itemQuantity));

            FindItemById(itemID);
            Console.WriteLine("Item added successfully!");
        }
        static void DisplayItems()
        {
            if (items.Count == 0)
            {
                Console.WriteLine("There are no items.");
                return;
            }

            Console.WriteLine("***** Items List *****");
            int index = 1;
            Console.WriteLine("{0,-8} {1,-10} {2,-20} {3,-10} {4,-10}", "Index", "Item ID", "Name","Price","Quantity");
            Console.WriteLine("-------  --------------  -------------------- -------------- --------------");
            foreach (Item item in items)
            {
                Console.WriteLine($"{index,-8} {item.itemID,-10} {item.itemName,-20} {item.itemPrice,-10} {item.itemQuantity,-10}");
                index++;
            }
            
        }
        static Item FindItemById(int itemID)
        {
            Item itemDetails = items.Find(item => item.itemID == itemID);
            if (itemDetails != null)
            {
                Console.WriteLine("***** Items List *****");
                Console.WriteLine("{0,-8} {1,-10} {2,-20} {3,-10} {4,-10}", "Index", "Item ID", "Name", "Price", "Quantity");
                Console.WriteLine("------  ---------- -------------------- ---------- ---------- ");
                Console.WriteLine($"{"1",-8} {itemDetails.itemID,-10} {itemDetails.itemName,-20} {itemDetails.itemPrice,-10} {itemDetails.itemQuantity,-10}");

                return itemDetails;
            }
            else
            {
                Console.WriteLine("Item with ID {0} not found.", itemID);
                return null;
            }
        }
        static void UpdateItem()
        {
            if (items.Count == 0)
            {
                Console.WriteLine("There are no items to update.");
                return;
            }

            DisplayItems();

            Console.WriteLine("Enter the item ID to update: ");
            int itemID;
            Item itemToUpdate = null;

            while (itemToUpdate == null) 
            {
                itemID = int.Parse(Console.ReadLine());
                itemToUpdate = FindItemById(itemID);

                if (itemToUpdate == null)
                {
                    Console.WriteLine("Invalid item ID. Please enter a valid ID: ");
                }
            }
            bool keepUpdating = true;
            while (keepUpdating)
            {
                Console.WriteLine("Enter what to change \n1. Name of Item \n2. ID of Item \n3. Price of Item \n4. Quantity of Item \n5. Done Updating ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Update Name of Item :");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newName))
                        {
                            itemToUpdate.itemName = newName;
                        }
                        Console.WriteLine("Name of Item updated successfully!");
                        break;
                    case 2:
                        Console.WriteLine("Update ID of Item :");
                        int newID = int.Parse(Console.ReadLine());
                        itemToUpdate.itemID = newID;
                        Console.WriteLine("ID of Item updated successfully!");
                        break;
                    case 3:
                        Console.WriteLine("Update Price of Item :");
                        double newPrice = double.Parse(Console.ReadLine());
                        itemToUpdate.itemPrice = newPrice;
                        Console.WriteLine("Price of Item updated successfully!");
                        break;
                    case 4:
                        Console.WriteLine("Update Quantity of Item :");
                        int newQuantity = int.Parse(Console.ReadLine());
                        itemToUpdate.itemQuantity = newQuantity;
                        Console.WriteLine("Quantity of Item updated successfully!");
                        break;

                    case 5:
                        return;

                    default:

                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                        continue; 
                }

                FindItemById(itemToUpdate.itemID);
                
            }
        }

        static void DeleteItem()
        {
            if (items.Count == 0)
            {
                Console.WriteLine("There are no items to delete.");
                return;
            }

            DisplayItems();

            Console.WriteLine("Enter the Item ID to delete:");
            int itemID= int.Parse(Console.ReadLine());
            Item itemDetail = FindItemById(itemID);
            if (itemDetail == null  )
            {
                Console.WriteLine("Invalid item ID.");
                return;
            }
            Console.WriteLine($"Are you sure you want to delete item '{itemDetail.itemName}' (ID: {itemID})? (y/n)");
            char confirmation = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (confirmation.ToString().ToLower() == "y")
            {
                items.RemoveAt(items.IndexOf(itemDetail));
                Console.WriteLine("Item deleted successfully!");
            }
            else
            {
                Console.WriteLine("Item deletion cancelled.");
            }

            

        }
    }
}

public class Item
{
    public string itemName { get; set; }
    public int itemID { get; set; }
    public double itemPrice { get; set; }
    public int itemQuantity { get; set; }

    public Item(string itemName, int itemID, double itemPrice, int itemQuantity)
    {
        this.itemName = itemName;
        this.itemID = itemID;
        this.itemPrice = itemPrice;
        this.itemQuantity = itemQuantity;
    }
}


