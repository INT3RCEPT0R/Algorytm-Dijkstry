using System;
using System.Collections.Generic;
namespace Algorytm_Dijkstry
{
    class Program
    {
        static bool debug = true;
        static void Main()
        {
            //Słownik przedstawiający wierzchołki grafu kosztem przejścia do sąsiadów
            Dictionary<string, List<Dictionary<string, int>>> Graph = new Dictionary<string, List<Dictionary<string, int>>>();
            //Słownik przedstawiający koszty dotarcia do kolejnych wierzchołków od startu
            Dictionary<string, int> Koszty = new Dictionary<string, int>();
            //Słownik przedstawiający kolejnych rodziców rozwiązania grafu 
            Dictionary<string, string>  Rodzice = new Dictionary<string, string>();            
 
            //Lista sprawdzonych wierzchołków 
            List<string> Checked = new List<string>();
            //Kolejka wierzchołków do sprawdzenia
            Queue<string> ToCheck = new Queue<string>();
 
            //Wypełnienie grafu
            Graph.Add("Start", new List<Dictionary<string, int>>{
                new Dictionary<string, int>{{"A",5}},
                new Dictionary<string, int>{{"B",0}}
            });
            Graph.Add("A", new List<Dictionary<string, int>>{
                new Dictionary<string, int>{{"C",15}},
                new Dictionary<string, int>{{"D",20}}
            });
            Graph.Add("B", new List<Dictionary<string, int>>{
                new Dictionary<string, int>{{"C",30}},
                new Dictionary<string, int>{{"D",35}}
            });
            Graph.Add("C", new List<Dictionary<string, int>>{
                new Dictionary<string, int>{{"Meta",20}}
            });
            Graph.Add("D", new List<Dictionary<string, int>>{
                new Dictionary<string, int>{{"Meta",10}}
            });
            Graph.Add("Meta", new List<Dictionary<string, int>>{});
 
            //Wypełnienie kosztów początkowych wierzchołków
            Koszty.Add("A",5);
            Koszty.Add("B",0);
            Koszty.Add("C",100);
            Koszty.Add("D",100);
            Koszty.Add("Meta", 100);
 
            //Wypełnienie początkowe rodziców.
            Rodzice.Add("A","Start");
            Rodzice.Add("B","Start");
            Rodzice.Add("C",null);
            Rodzice.Add("D",null);
            Rodzice.Add("Meta",null);            
 
            //Dodanie pierwszego wierzchołka do sprawdzenia
            string node = FindLowest(Koszty, Checked);
            ToCheck.Enqueue(node);
 
            //Dopóki są wierzchołkiw kolejce:
            do
            {   
                //Pobranie listy wierzchołków pochodzących od pierwszego wierzchołka z listy
                if((ToCheck.Peek()).Equals("Meta"))
                    break;
                foreach (Dictionary<string, int> dict in Graph[ToCheck.Peek()])
                {
                    //Kolejne wierzchołki z listy 
                    foreach (KeyValuePair<string, int> pair in dict)
                    {
                        //Wyświetlenie info o aktualnie sprawdzanym wierzchołku 
                        if(debug==true)
                            System.Console.WriteLine("from = {0}, to = {1}, cost = {2}", ToCheck.Peek(), pair.Key, pair.Value);
 
                        //Jeśli ścieżka do danego węzła jest najtańsza, ustaw aktualną konfigurację
                        int SumarycznyKoszt = Koszty.GetValueOrDefault(ToCheck.Peek())+pair.Value;
                        if( Koszty.GetValueOrDefault(pair.Key) > SumarycznyKoszt)
                        {
                            Koszty.Remove(pair.Key);
                            Koszty.Add(pair.Key,SumarycznyKoszt);
                            Rodzice.Remove(pair.Key);
                            Rodzice.Add(pair.Key,ToCheck.Peek());
                        }
                    }                    
                }           
 
                //Usunięcie aktualnie sprawdzanego wierzchołka z listy wierzchołków do sprawdzenia i dodanie do sprawdzonych            
                Checked.Add(node);
                ToCheck.Dequeue();
 
                //Znalezienie nowego najtanszego wezla i dodanie do kolejki
                node = FindLowest(Koszty, Checked);
                ToCheck.Enqueue(node);
 
            }while(ToCheck.Count != 0);
 
            //Prezentacja tablicy kosztów i rodziców
            System.Console.WriteLine("\nTablica kosztów:");            
            foreach (KeyValuePair<string, int> pair in Koszty)
            {
                System.Console.WriteLine("to = {0}, cost = {1}", pair.Key, pair.Value);
            }
            System.Console.WriteLine("\nTablica rodziców:");            
            foreach (KeyValuePair<string, string> pair in Rodzice)
            {
                System.Console.WriteLine("to = {0}, rodzic = {1}", pair.Key, pair.Value);
            }
 
            //Wyznaczenie ścieżki końcowej wraz z kosztami 
            System.Console.WriteLine("\nKońcowa Ścieżka:");
            string[] Patch = new string[Graph.Count];
            int[] PatchCost = new int[Graph.Count];  
            Patch[0]=Rodzice["Meta"];
            List<Dictionary<string, int>> lista = (Graph[Rodzice["Meta"]]);
            foreach (Dictionary<string, int> dict in lista)
            {
                PatchCost[0]=dict["Meta"];
            }           
            for (int i = 1; i < Graph.Count-1; i++)
            {                             
                Patch[i]=Rodzice[(Patch[i-1])];                                
                foreach (Dictionary<string, int> dict in Graph[Rodzice[(Patch[i-1])]])
                {
                    if(dict.ContainsKey(Patch[i-1]))
                        PatchCost[i]=dict[Patch[i-1]];
                }       
                if(Rodzice[(Patch[i-1])].Equals("Start"))
                    break;          
            }
 
            //Prezentacja wyniku
            System.Console.Write("Meta");
            for (int i = 0; i < Patch.Length-1; i++)
            {                
                System.Console.Write(" <-{1}- {0}",Patch[i],PatchCost[i]);
                if(Patch[i].Equals("Start"))
                    break;
            }
        }
 
        //Funkcja do zwracania najtańszego węzła do sprawdzenia
        static string FindLowest(Dictionary<string, int> Price, List<string> Sprawdzone)
        {
            int lowest = 99;
            string node = null;
            foreach (KeyValuePair<string, int> pair in Price)
            {        
                if(pair.Value<lowest && !Sprawdzone.Contains(pair.Key) && pair.Value<99)
                {
                    lowest = pair.Value;
                    node = pair.Key;
                }
            }
            if(debug==true)
                System.Console.WriteLine("do = {0}, koszt = {1}", node, lowest);
            return node;
        }
    }
}