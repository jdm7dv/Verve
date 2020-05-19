using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicTest
{
    class Cat
    {
        public void Speak() { Console.WriteLine("Meow");}
    }

    class Dog
    {
        public void Speak() { Console.WriteLine("Bark");}
    }

    class Program
    {
        static void Main(string[] args)
        {
            CallSpeak(new Cat());
            CallSpeak(new Dog());

            CallSpeak2(new Cat());
            CallSpeak2(new Dog());
        }

        public static void CallSpeak(object animal)
        {
            var methodInfo = animal.GetType().GetMethod("Speak");
            methodInfo.Invoke(animal, null);
        }

        public static void CallSpeak2(dynamic animal)
        {
            animal.Speak();
        }
    }
}
