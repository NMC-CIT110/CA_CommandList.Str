﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinchAPI;

namespace CA_CommandList
{
    class Program
    {
        //
        // list of all commands as an enum
        //
        private enum FinchCommand
        {
            DONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            DELAY,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF
        }

        static void Main(string[] args)
        {
            //
            // Note: initial Finch configuration values taken from a static
            //       config file
            int numberOfCommands = DefaultFinchConfig.NUMBER_OF_COMMNANDS;
            int delayDuration = DefaultFinchConfig.DELAY_DURATION;
            int motorSpeed = DefaultFinchConfig.MOTOR_SPEED;
            int ledBrightness = DefaultFinchConfig.MOTOR_SPEED;

            //
            // TODO - 1) Comment out FinchCommand array
            //
            FinchCommand[] commands = new FinchCommand[numberOfCommands];
            Finch myFinch = new Finch();

            DisplayWelcomeScreen();

            InitializeFinch(myFinch);

            DisplayGetFinchCommands(commands, numberOfCommands);

            ProcessFinchCommands(myFinch, commands, motorSpeed, ledBrightness, delayDuration);

            TerminateFinch(myFinch);

            DisplayClosingScreen();
        }

        /// <summary>
        /// Turn the cursor off and display a continue prompt to the user
        /// </summary>
        private static void DisplayContinuePrompt()
        {
            Console.WriteLine();

            //
            // turn cursor off
            //
            Console.CursorVisible = false;

            Console.Write("Press any key to continue.");
            Console.ReadKey();

            //
            // turn cursor on
            //
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Display a welcome screen including the purpose of the application
        /// </summary>
        private static void DisplayWelcomeScreen()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Welcome to the Command Array Application");
            Console.WriteLine("Author: John E Velis");
            Console.WriteLine();
            Console.WriteLine("You will be prompted to enter a series of commands for the Finch robot.");
            Console.WriteLine("The Finch robot will then execute each of the commands in the order they were entered.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Display a closing screen
        /// </summary>
        private static void DisplayClosingScreen()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Thank you for using the Command Array Application");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Initialize the Finch robot
        /// </summary>
        private static void InitializeFinch(Finch myFinch)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Attempting to connect to the Finch robot.");
            Console.WriteLine();

            myFinch.connect();

            //
            // Audio/visual feedback to user
            //
            for (int increment = 0; increment < 255; increment += 10)
            {
                myFinch.setLED(0, increment, 0);
                //myFinch.noteOn(increment * 100);
                myFinch.wait(200);
            }
            myFinch.setLED(0, 0, 0);
            myFinch.noteOff();

            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("The Finch robot is now connected.");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Shut down the Finch robot
        /// </summary>
        /// <param name="myFinch"></param>
        private static void TerminateFinch(Finch myFinch)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Attempting to disconnect from the Finch robot.");
            Console.WriteLine();

            //
            // Audio/visual feedback to user
            //
            for (int increment = 255; increment > 0; increment -= 10)
            {
                myFinch.setLED(0, increment, 0);
                myFinch.noteOn(increment * 100);
                myFinch.wait(200);
            }
            myFinch.setLED(0, 0, 0);
            myFinch.noteOff();

            myFinch.disConnect();

            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("The Finch robot is now disconnected.");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        //
        // TODO - 2) Change the type in the parameter list to a list of FinchCommand
        //
        /// <summary>
        /// Add the user command sequence to the command array
        /// </summary>
        /// <param name="commands">array of FinchCommand</param>
        private static void DisplayGetFinchCommands(FinchCommand[] commands, int numberOfCommands)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Enter Command Sequence");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("You will be prompted for each command.");

            //
            // Display all of the enum command options
            //
            Console.WriteLine("Command Options: ");
            Console.WriteLine();

            //
            // List all available commands form the FinchCommand enum
            //
            foreach (var command in Enum.GetValues(typeof(FinchCommand)))
            {
                Console.WriteLine("\t| " + command);
            }
            Console.WriteLine();

            //
            // TODO - 4) Use the Add method to add the command to the FinchCommand list
            //
            // Get individual commands from the user and add each to the array of commands
            //
            for (int index = 0; index < numberOfCommands; index++)
            {
                commands[index] = GetFinchCommandValue();
            }

            //
            // Confirm and echo the command sequence to the user
            //
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("You have completed the command entry.");
            Console.WriteLine("The command sequence you have selected is listed below.");
            foreach (FinchCommand command in commands)
            {
                Console.WriteLine("Command: " + command);
            }

            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Prompt user for a FinchCommand and validate the response
        /// </summary>
        /// <returns>FinchCommand</returns>
        private static FinchCommand GetFinchCommandValue()
        {
            FinchCommand userCommand = FinchCommand.DONE;
            string userResponse;
            bool userResponseValid = false;

            while (!userResponseValid)
            {
                Console.Write("Command: ");
                userResponse = Console.ReadLine().Trim().ToUpper();

                if (Enum.TryParse<FinchCommand>(userResponse, out userCommand))
                {
                    userResponseValid = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("It appears that you entered and invalid command.");
                    Console.WriteLine("Please reenter your command.");
                    Console.WriteLine();

                    //
                    // Display all of the enum command options
                    //
                    Console.WriteLine("Command Options: ");
                    Console.Write("| ");
                    foreach (var command in Enum.GetValues(typeof(FinchCommand)))
                    {
                        Console.Write(command + " | ");
                    }
                    Console.WriteLine();
                }
            }

            return userCommand;
        }

        //
        // TODO - 3) Change the type in the parameter list to a list of FinchCommand
        //
        /// <summary>
        /// Process each command 
        /// </summary>
        /// <param name="myFinch">Finch robot object</param>
        /// <param name="commands">FinchCommand</param>
        private static void ProcessFinchCommands(Finch myFinch, FinchCommand[] commands, int motorSpeed, int ledBrightness, int delayDuration)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("The application will now process your command sequence.");
            Console.WriteLine();

            foreach (FinchCommand command in commands)
            {
                Console.WriteLine("Command Currently Executing: " + command.ToString());

                switch (command)
                {
                    case FinchCommand.DONE:
                        break;
                    case FinchCommand.MOVEFORWARD:
                        myFinch.setMotors(motorSpeed, motorSpeed);
                        break;
                    case FinchCommand.MOVEBACKWARD:
                        myFinch.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case FinchCommand.STOPMOTORS:
                        myFinch.setMotors(0, 0);
                        break;
                    case FinchCommand.DELAY:
                        myFinch.wait(delayDuration);
                        break;
                    case FinchCommand.TURNRIGHT:
                        myFinch.setMotors(motorSpeed, -motorSpeed);
                        break;
                    case FinchCommand.TURNLEFT:
                        myFinch.setMotors(-motorSpeed, motorSpeed);
                        break;
                    case FinchCommand.LEDON:
                        myFinch.setLED(ledBrightness, ledBrightness, ledBrightness);
                        break;
                    case FinchCommand.LEDOFF:
                        myFinch.setLED(0, 0, 0);
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
