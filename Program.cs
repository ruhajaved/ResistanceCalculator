using System;
using System.Collections.Generic;

namespace resistance_cal
{
    class Utilities
    {
        //assigned if the color doesn't make sense in reference to the band number, treated as error value 
         public static int illogical_color = -3; 
         
         //enum for the color band number 
        public enum BandNumber 
        {
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4,
            Fifth = 5,
            Sixth = 6
        };

     //enum for color code given to user 
        public enum ColorCode 
        {
            Black = 0,
            Brown = 1,
            Red = 2,
            Orange = 3,
            Yellow = 4, 
            Green = 5, 
            Blue = 6,
            Violet = 7,
            Grey = 8,
            White = 9,
            Gold = 10,
            Silver = 11,
            None = 12
        };

          //resistor color code table as a 2d array 
        public static int[][] resistor_color_code_table = new int [13][]
        {
            //-3 is used as a code for null - look at code sheet for more info. 
            //information given in rows: digit, multiplier, tolerance in percent, and temperature coefficient in ppm/°C
            new int[] {0, 0, -3, -3},
            new int[] {1, 1, 1, 100},
            new int[] {2, 2, 2, 50},
            new int[] {3, 3, -3, 15},
            new int[] {4, 4, -3, 25},
            new int[] {5, 5, 50, -3}, //here need to divide the 3rd element by 100, when calculating tolerance. Work around for integer array. 
            new int[] {6, 6, 25, -3}, //here need to divide the 3rd by 100
            new int[] {7, 7, 10, -3}, //here need to divide the 3rd by 100
            new int[] {8, -3, 5, -3}, //here need to divide the 3rd by 100
            new int[] {9, -3, -3, -3},
            new int[] {-3, -1, 5, -3},
            new int[] {-3, -2, 10, -3},
            new int[] {-3, -3, 20, -3}
        };

        //used to print the legend for the user 
         public static void print_color_codes()
        {
                foreach (string color_name in Enum.GetNames(typeof(Utilities.ColorCode)))
                {
                    //display everything but None since that's reserved for 3 band resistors 
                    if (color_name != "None")
                    {
                        int color_value = (int) Enum.Parse(typeof(Utilities.ColorCode), color_name);
                        Console.WriteLine(color_name + ": " + color_value);
                    }
                }
                Console.WriteLine();
        }

        //used to concantenate two digits together 
         public static int digit_concantenate(int first_digit, int second_digit) 
        {
            int base_number; //what will be returned - the digits concantenated together 
            string _first_digit, _second_digit, _base_number; // need so that we can concantenate the digits together 
            //convert the 1st and 2nd band digits into strings 
            _first_digit = first_digit.ToString();
            _second_digit = second_digit.ToString();
            //then concantenate them 
            _base_number = _first_digit + _second_digit;
            //convert the base number back into an integer and return  
            base_number = Int32.Parse(_base_number);
            return base_number;
        }

         //display result 
        public static void display_result(Resistor _resistor)
        {
            string units;
            //displays number of bands selected by user 
            Console.Write("\nYou entered " + _resistor.num_of_bands + " bands with colors: ");

            //displays the colors selected by the user 
            foreach (int color_code in _resistor.band_color_codes)
            {
                //get the string rep of the color_code
                string color_string = Enum.GetName(typeof(Utilities.ColorCode), color_code);

                //get the color rep of the color_string to write in that color - if white write in black 
                //if (color_string == "White")
                //    Console.ForegroundColor = ConsoleColor.Black;
                //else 
                //{
                    //Color color_of_band = Color.FromName(color_string);
                    //Color color_of_band = (Color) Enum.Parse(typeof(Colors), color_string);
                    //Console.ForegroundColor = ConsoleColor.(color_of_band);
                //}
                Console.Write(color_string + " ");
            }

            //determine units based on magnitude of resistance 
            if (_resistor.resistance >= 1000)
            {
                _resistor.resistance /= 1000;
                units = "kOhms";
            }
            else 
                units = "Ohms";
                
            //display the resistance and the tolerance, with the correct units - format it using String.Format
            Console.WriteLine(String.Format("\nThe resistance is: {0} {1}, with a tolerance of: {2}%.", _resistor.resistance, units, _resistor.tolerance));
            
            //if there are six bands set the temp coefficent and display it
            if (_resistor.num_of_bands == 6)
            {
                //_resistor.set_temperature_coefficient();
                Console.Write("The temperature coefficent is: " + _resistor.temperature_coefficient + " ppm/°C.\n");
            }
        }
    }
    
    class Resistor 
    {
        //attributes 
        public int num_of_bands; 
        public List<int> band_color_codes = new List<int> ();
        public float resistance;
        public float tolerance;
        public int temperature_coefficient;

        // class methods 
        public void get_num_of_bands()
        {
            //need to have input as string in order to give message when exception raised. 
            string input_num_of_bands = null; 

            while (true)
            {
                // try to read number of colour bands. handle if invalid input given. 
                try
                {
                    Console.WriteLine("\nHow many color bands does the resistor have? Please enter 3, 4, 5, or 6.\n");

                    //convert number read from a string to an integer. This is wehere an exception may be raised. 
                    input_num_of_bands = Console.ReadLine();
                    num_of_bands = Int32.Parse(input_num_of_bands);

                    //handle illogical input. Resistors only have 4, 5, or 6 bands.
                    if (num_of_bands < 3 || num_of_bands > 6) //could call on a function here to do this checking for you 
                        Console.WriteLine("\nUh Oh! You entered " + num_of_bands + ". There are no resistors with that many bands; try again!\n");
                    else
                     break;
                }
                catch (FormatException) //exception rasied if anything other than an integer entered. 
                {
                    Console.Write("\nYou entered " + input_num_of_bands + ". This is not even an integer! Try again!\n");
                }
            }
        }

        public void get_band_colors()
        {
            Console.WriteLine("\nNow it's time to put in the colors! Use the following legend:\n");
            Utilities.print_color_codes();
            
            string input = null;
            int color_code;

            for (int i = 1; i <= num_of_bands; i++)
            {
                //use a while loop to keep asking for a band color until valid input given 
                while (true)
                {
                    //use a switch statement to ask the user for input 
                    switch(i)
                    {
                        case (int)Utilities.BandNumber.First:
                            Console.WriteLine("The color of the 1st band: ");
                            break;
                        case (int)Utilities.BandNumber.Second:
                            Console.WriteLine("The color of the 2nd band: ");
                            break;
                        case (int)Utilities.BandNumber.Third:
                            Console.WriteLine("The color of the 3rd band: ");
                            break;
                        case (int)Utilities.BandNumber.Fourth:
                            Console.WriteLine("The color of the 4th band: ");
                            break;
                        case (int)Utilities.BandNumber.Fifth:
                            Console.WriteLine("The color of the 5th band: ");
                            break;
                        case (int)Utilities.BandNumber.Sixth:
                            Console.WriteLine("The color of the 6th band: ");
                            break;
                    }

                    // get info from user about the color code for the band 
                    input = Console.ReadLine();
                    Console.WriteLine();

                    try
                    {
                        //convert input into an integer 
                        color_code = Int32.Parse(input);
                        //invalid color code 
                        if (color_code < 0 || color_code > 11) 
                            Console.WriteLine("\nUh Oh! You entered the color code: " + color_code + ". This is out of bounds; try again!\n");
                        else //valid color code, now check for logic errors 
                        { 
                            if ((i == 1 || i == 2) && Utilities.resistor_color_code_table[color_code][0] == Utilities.illogical_color)
                                Console.WriteLine("The 1st and 2nd band of any resistor cannot be of this color. Try again!\n");
                            else if (i == 3 && (num_of_bands == 3 || num_of_bands == 4) && Utilities.resistor_color_code_table[color_code][1] == Utilities.illogical_color)
                                Console.WriteLine("The 3rd band of a resistor with 3 or 4 bands cannot be of this color. Try again!\n");
                            else if (i == 3 && (num_of_bands == 5 || num_of_bands == 6) && Utilities.resistor_color_code_table[color_code][0] == Utilities.illogical_color)
                                Console.WriteLine("The 3rd band of a resistor with 5 or 6 bands cannot be of this color. Try again!\n");
                            else if (i == 4 && num_of_bands == 4 && Utilities.resistor_color_code_table[color_code][2] == Utilities.illogical_color)
                                Console.WriteLine("The 4th band of a resistor with 4 bands cannot be of this color. Try again!\n");
                            else if (i == 4 && (num_of_bands == 5 || num_of_bands == 6) && Utilities.resistor_color_code_table[color_code][1] == Utilities.illogical_color)
                                Console.WriteLine("The 4th band of a resistor with 5 bands cannot be of this color. Try again!\n");
                            else if (i == 5 && (num_of_bands == 5 || num_of_bands == 6) && Utilities.resistor_color_code_table[color_code][2] == Utilities.illogical_color)
                                Console.WriteLine("The 5th band of a resistor with 5 bands cannot be of this color. Try again!\n");
                            else if (i == 6 && num_of_bands == 6 && Utilities.resistor_color_code_table[color_code][3] == Utilities.illogical_color)
                                Console.WriteLine("The 6th band of a resistor with 6 bands cannot be of this color. Try again!\n");
                            else //valid code, add to list of color codes
                            {
                                band_color_codes.Add(color_code);
                                break;
                            }
                        }    
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nUh Oh! You entered: " + input + ". This is not even an integer; try again!\n");
                    }
                }
            }
        }

        public void calc_resistance()
        {
            int first_digit, second_digit, third_digit, base_number, multiplier;
            //get the first digit for the calculation, using the first color stored in band_color_codes,applies to all resistors 
            first_digit = Utilities.resistor_color_code_table[band_color_codes[0]][0]; 
             //get the second digit for the calculation, using the first color stored in band_color_codes, applies to all resistors 
            second_digit = Utilities.resistor_color_code_table[band_color_codes[1]][0];

            //get the 1st and 2nd digit concantenated together
            base_number = Utilities.digit_concantenate(first_digit, second_digit);

            //3rd colour represents multiplier in resistors for 3/4 bands, so do calc with that assumption 
            //set the 3rd digit for calc, using the 3rd colour in band_color_codes and the column with multipliers 
            if (num_of_bands == 3 || num_of_bands == 4)
                multiplier = Utilities.resistor_color_code_table[band_color_codes[2]][1];
            else //num_of_bands has to be 5/6, so the 3rd color actually represents a digit, and then the 4th color is the multiplier 
            {
                third_digit = Utilities.resistor_color_code_table[band_color_codes[2]][0];
                base_number = Utilities.digit_concantenate(base_number, third_digit); //add the 3rd digit to the base number 
                multiplier = Utilities.resistor_color_code_table[band_color_codes[3]][1];
            }
            //finally, calculate the resistance and do a type cast since Math returns a double 
            resistance = base_number * (float) Math.Pow(10, multiplier); //base x 10^(n)
        }

       public void set_tolerance()
       {
            if (num_of_bands == 3)
                tolerance = Utilities.resistor_color_code_table[12][2]; //uses None to set tolerance 
            else if (num_of_bands == 4)
            {
                tolerance = Utilities.resistor_color_code_table[band_color_codes[3]][2];
                //check to see if tolerance color is one of the tolerances manipulated in the resistor_color_codes array 
                if (band_color_codes[3] >= 5 && band_color_codes[3] <= 8)
                    tolerance = tolerance/100;
            }
            else // num_of_bands has to be 5 or 6
            {
                tolerance = Utilities.resistor_color_code_table[band_color_codes[4]][2];
                //check to see if tolerance color is one of the tolerances manipulated in the resistor_color_codes array 
                if (band_color_codes[4] >= 5 && band_color_codes[4] <= 8)
                    tolerance = tolerance/100;
            }
       }

       public void set_temperature_coefficient ()
       {
           temperature_coefficient = Utilities.resistor_color_code_table[band_color_codes[5]][3];
       }

    }
    
    class MainProgram
    {
        public static void Main()
        {
            string input_key;
            int input = 1; //used to keep track of wether the program should run again or quit 

            Console.WriteLine("\nHello there! Welcome to the Resistance Calculator!\n");

            //wait for user to be ready.
            Console.Write("Press enter to get started!");
            Console.ReadKey();

            while (input == 1)
            {
                //create a resistor object 
                Resistor _resistor = new Resistor();

                //get number of bands from the user
                _resistor.get_num_of_bands();

                //get band colors from the user 
                _resistor.get_band_colors();

                //calculate the resistance 
                _resistor.calc_resistance();
                _resistor.set_tolerance();

                //if 6 band resistor set the temperature coefficient 
                if (_resistor.num_of_bands == 6)
                    _resistor.set_temperature_coefficient();

                Utilities.display_result(_resistor);

                while (true)
                {
                    Console.WriteLine("\nPlease enter 0 to quit or 1 to calculate another resistance.");
                    input_key = Console.ReadLine();
                    try
                    {
                        input = Int32.Parse(input_key);
                        if (input == 0 || input == 1)
                            break;
                        else  
                            Console.WriteLine("\nUh oh! That wasn't a 1 or 0. Try again!");

                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("\nUh oh! That wasn't a 1 or 0. Try again!");
                    }
                }
            }
            Console.WriteLine("\nQuitting!\n");
        }
    }
}
