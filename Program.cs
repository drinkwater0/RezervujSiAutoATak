using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VozovyPark
{
    class Program
    {
        static void Main(string[] args)
        {
            MujSystem.Start();
            do
            {
                do
                {
                    MujSystem.Prihlaseni();
                } while (MujSystem.currentUser == null);

                MujSystem.HlavniStranka();
            } while (true);
        }
    }
}