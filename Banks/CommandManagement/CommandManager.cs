using System;
using System.Collections.Generic;
using System.Linq;
using Banks.BanksManagement;
using Banks.Tools;
using Banks.UserInteraction;

namespace Banks.CommandManagement
{
    public class CommandManager
    {
        private List<ICommand> _commands;

        public CommandManager()
        {
            _commands = new List<ICommand>();
        }

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }

        public void HandleCommand(string name, BanksManager banksManager)
        {
            ICommand command = _commands.FirstOrDefault(command => command.Text == name);
            if (command is null)
                throw new BanksException("Command not found");
            command.Execute(banksManager);
        }

        public void DisplayCommands()
        {
            int counter = 1;
            _commands.ForEach(command =>
            {
                Console.WriteLine($"{counter}) {command.Text}");
                counter++;
            });
            Console.WriteLine($"{counter}) help");
        }
    }
}