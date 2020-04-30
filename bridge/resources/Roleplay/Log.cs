using System;
using System.Collections.Generic;
using System.Text;

namespace Roleplay
{
    class Log
    {
        //Mein Gedanke dahinter war ganz einfacher in der Konsole einfacherer zu erkennen was der Server gerade macht.
        //Ob der Server gerade etwas mit MySQL erledigt oder einfach eine Fehlermeldung ausspuckt, hiermit soll es übersichtlicher gestaltet werden und den Code für uns verkürzen.
        //Anstatt Console.WriteLine("DB Fehler: " + Fehler + " " + DateTime.Now); (ca. 54 Zeichen) schreiben wir nur noch folgendes: Log.WriteDError(Fehler); (24 Zeichen) und wir erhalten
        //folgende Informationen: [Vorher: (DB Error: XXXX 08.07.2019 13:30) JETZT: ([14:35] [DB FEHLER] :: XXXX)]

        private static void WriteLine(ConsoleColor _c, string _prefix, string _text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(DateTime.Now.ToShortTimeString());
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] [");
            Console.ForegroundColor = _c;
            Console.Write(_prefix);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] :: ");
            Console.WriteLine(_text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Informationen
        public static void WriteI(string _text)
        {
            WriteLine(ConsoleColor.Yellow, "INFO", _text);
        }

        //Datenbank Fehlermeldung
        public static void WriteDError(string _text)
        {
            WriteLine(ConsoleColor.Red, "DB FEHLER", _text);
        }

        //MySQL Fehlermeldung
        public static void WriteMError(string _text)
        {
            WriteLine(ConsoleColor.Red, "MYSQL FEHLER", _text);
        }

        //Reader Fehlermeldung
        public static void WriteRError(string _text)
        {
            WriteLine(ConsoleColor.Red, "READER FEHLER", _text);
        }

        //MySQL
        public static void WriteM(string _text)
        {
            WriteLine(ConsoleColor.Cyan, "MYSQL", _text);
        }

        //Server
        public static void WriteS(string _text)
        {
            WriteLine(ConsoleColor.Green, "SERVER", _text);
        }

        //Hiermit kannst Du selber einen Prefix angeben so das es z.B. folgenderweise aussieht: [13:30] [PREFIX] :: TEXT
        public static void Write(ConsoleColor _c, string _prefix, string _text)
        {
            WriteLine(_c, _prefix, _text);
        }
    }
}
