using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavnsparkering
{
    class CarFactory
    {
        private Random Rand { get; }

        public CarFactory(Random rand)
        {
            Rand = rand;
        }

        /// <summary>
        /// Gernerates a license plate based on the Random object, in the format of [AAAA0000]
        /// where the A's are substituted for random alphabetic characters and
        /// the 0's are substituted for random numeric characters
        /// </summary>
        /// <returns></returns>
        public string GenerateLicensePlate()
        {
            int numAlphaChars = 4;
            int numNumChars = 4;
            char[] plateChars = new char[numAlphaChars + numNumChars];
            for (int i = 0; i < numAlphaChars; i++)
            {
                // generate a char based on the random
                plateChars[i] = (char)Rand.Next('A', 'Z' + 1);
            }
            for (int i = 0; i < numNumChars; i++)
            {
                plateChars[i + numAlphaChars] = (char)Rand.Next('0', '9' + 1);
            }
            return plateChars.ToString();
        }

        /// <summary>
        /// Picks a random color from a hardcoded array
        /// </summary>
        /// <returns></returns>
        public string GenerateRandomColor()
        {
            string[] colors =
            {
                "RED",
                "BLACK",
                "SILVERGREY",
                "YELLOW",
                "GREEN",
                "BLUE"
            };
            // pick a random color from the array
            return colors[Rand.Next(0, colors.Length)];
        }
        public Car GenerateNewCar()
        {
            return new Car(GenerateRandomColor(), GenerateLicensePlate());
        }
    }
}
