using Microsoft.EntityFrameworkCore;

namespace ADO_Mod6;

internal class Program
{
    static void Main(string[] args)
    {
        using (var context = new FootballLeagueContext())
        {
            context.Database.EnsureCreated();

            if (context.Teams.CountAsync().Result == 0)
            {
                context.Teams.AddRange(
                    new Team
                    {
                        Name = "Barcelona",
                        City = "Barcelona",
                        Wins = 20,
                        Losses = 5,
                        Draws = 3,
                        GoalsScored = 60,
                        GoalsConceded = 25
                    },
                    new Team
                    {
                        Name = "Real Madrid",
                        City = "Madrid",
                        Wins = 22,
                        Losses = 4,
                        Draws = 2,
                        GoalsScored = 65,
                        GoalsConceded = 20
                    },
                    new Team
                    {
                        Name = "Atletico Madrid",
                        City = "Madrid",
                        Wins = 18,
                        Losses = 6,
                        Draws = 4,
                        GoalsScored = 55,
                        GoalsConceded = 30
                    }
                );
                context.SaveChanges();
            }

            Console.WriteLine("Tournament table:");
            foreach (var team in context.Teams)
            {
                Console.WriteLine($"\nName: {team.Name}, " +
                    $"\nCity: {team.City}, " +
                    $"\nWins: {team.Wins}, " +
                    $"\nLosses: {team.Losses}, " +
                    $"\nDraws: {team.Draws}, " +
                    $"\nGoals Scored: {team.GoalsScored}, " +
                    $"\nGoals Conceded: {team.GoalsConceded}\n");
            }

            Console.WriteLine("1. Search for a team by name:");
            SearchByTeamName(context, "Barcelona");

            Console.WriteLine("\n2. Search for teams outside the city:");
            SearchByCity(context, "Madrid");

            Console.WriteLine("\n3. Search for a team by name and city:");
            SearchByTeamAndCity(context, "Barcelona", "Barcelona");

            Console.WriteLine("\n4. Display of teams with maximum indicators:");
            DisplayTopTeams(context);

            Console.WriteLine("\n5.Adding a new team:");
            AddTeam(context, new Team { Name = "Sevilla", City = "Seville", Wins = 15, Losses = 10, Draws = 5, GoalsScored = 45, GoalsConceded = 35 });

            Console.WriteLine("\n6. Update team data:");
            UpdateTeam(context, "Barcelona", "Barcelona", team => team.Wins = 25);

            Console.WriteLine("\n7. Deleting a team:");
            DeleteTeam(context, "Sevilla", "Seville");

        }
    }


    //task1
    static void SearchByTeamName(FootballLeagueContext context, string teamName)
    {
        var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
        if (team != null)
        {
            Console.WriteLine($"Team: {team.Name}, \n" +
                $"City: {team.City}, \n" +
                $"Wins: {team.Wins}, \n" +
                $"Losses: {team.Losses}, \n" +
                $"Draws: {team.Draws}\n");
        }
        else
        {
            Console.WriteLine("No command found.");
        }
    }

    static void SearchByCity(FootballLeagueContext context, string city)
    {
        var teams = context.Teams.Where(t => t.City == city).ToList();
        if (teams.Any())
        {
            Console.WriteLine($"Teams from the city {city}:");
            foreach (var team in teams)
            {
                Console.WriteLine($"- {team.Name}");
            }
        }
        else
        {
            Console.WriteLine($"There are no teams in {city}.");
        }
    }

    static void SearchByTeamAndCity(FootballLeagueContext context, string teamName, string city)
    {
        var team = context.Teams.FirstOrDefault(t => t.Name == teamName && t.City == city);
        if (team != null)
        {
            Console.WriteLine($"Team: {team.Name}, " +
                $"City: {team.City}, " +
                $"Wins: {team.Wins}, " +
                $"Losses: {team.Losses}, " +
                $"Draws: {team.Draws}");
        }
        else
        {
            Console.WriteLine("No command found.");
        }
    }

    //task2
    static void DisplayTopTeams(FootballLeagueContext context)
    {
        var mostWins = context.Teams.OrderByDescending(t => t.Wins).FirstOrDefault();
        var mostLosses = context.Teams.OrderByDescending(t => t.Losses).FirstOrDefault();
        var mostDraws = context.Teams.OrderByDescending(t => t.Draws).FirstOrDefault();
        var mostGoalsScored = context.Teams.OrderByDescending(t => t.GoalsScored).FirstOrDefault();
        var mostGoalsConceded = context.Teams.OrderByDescending(t => t.GoalsConceded).FirstOrDefault();

        Console.WriteLine("\nThe team with the most wins:");
        Console.WriteLine($"- {mostWins?.Name ?? "No data available"}");

        Console.WriteLine("\nThe team with the most defeats:");
        Console.WriteLine($"- {mostLosses?.Name ?? "No data available"}");

        Console.WriteLine("\nThe team with the most draws:");
        Console.WriteLine($"- {mostDraws?.Name ?? "No data available"}");

        Console.WriteLine("\nThe team with the most goals scored:");
        Console.WriteLine($"- {mostGoalsScored?.Name ?? "No data available"}");

        Console.WriteLine("\nThe team with the most goals conceded:");
        Console.WriteLine($"- {mostGoalsConceded?.Name ?? "No data available"}");
    }

    //task3
    static void AddTeam(FootballLeagueContext context, Team newTeam)
    {
        if (context.Teams.Any(t => t.Name == newTeam.Name && t.City == newTeam.City))
        {
            Console.WriteLine("The team already exists.");
            return;
        }

        context.Teams.Add(newTeam);
        context.SaveChanges();
        Console.WriteLine("Team added successfully.");
    }

    static void UpdateTeam(FootballLeagueContext context, string teamName, string city, Action<Team> updateAction)
    {
        var team = context.Teams.FirstOrDefault(t => t.Name == teamName && t.City == city);
        if (team == null)
        {
            Console.WriteLine("No command found.");
            return;
        }

        updateAction(team);
        context.SaveChanges();
        Console.WriteLine("Team data updated.");
    }

    static void DeleteTeam(FootballLeagueContext context, string teamName, string city)
    {
        var team = context.Teams.FirstOrDefault(t => t.Name == teamName && t.City == city);
        if (team == null)
        {
            Console.WriteLine("No command found.");
            return;
        }

        Console.WriteLine($"Are you sure you want to remove {teamName} from {city}? (y/n)\"");
        var response = Console.ReadLine();
        if (response?.ToLower() == "y")
        {
            context.Teams.Remove(team);
            context.SaveChanges();
            Console.WriteLine("The team has been deleted.");
        }
        else
        {
            Console.WriteLine("The deletion has been cancelled.");
        }
    }
}
