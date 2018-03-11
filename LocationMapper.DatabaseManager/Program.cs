using LocationMapper.Entities;
using LocationMapper.Repository;
using LocationMapper.WebScrapers;
using LocationMapper.WebScrapers.Entities;
using LocationMapper.WebScrapers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LocationMapper.DatabaseManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseManager = new UkcDatabaseManager();
            var action = GetActionFromUser();

            switch (action)
            {
                case Actions.DeleteAllCragsFromDatabase:
                    databaseManager.DeleteAllCragsFromDatabase();
                    break;
                case Actions.AddAllCragsToDatabase:
                    databaseManager.ReadAllCragsFromUkcAndAddThemToDatabase(23);
                    break;
                case Actions.UpdateLocationsOfCragsWithoutALocation:
                    databaseManager.FindLocationsOfCraggsWithoutALocation();
                    break;
                default:
                    break;
            }

            Console.WriteLine("Press enter to exit:");
            Console.ReadLine();
        }

        private static Actions GetActionFromUser()
        {
            AskUserForInput();

            var result = Console.ReadLine();
            int value;

            while (!(int.TryParse(result, out value) && Enum.IsDefined(typeof(Actions), value)))
            {
                Console.WriteLine($"Invalid input: {result}.");
                AskUserForInput();
                result = Console.ReadLine();
            }

            var action = (Actions)value;

            return action;
        }

        private static void AskUserForInput()
        {
            foreach (var action in Enum.GetValues(typeof(Actions)))
            {
                Console.WriteLine($"Press {(int)action} to perform action '{((Actions)action).ToString()}'.");
            }
        }
    }
}
